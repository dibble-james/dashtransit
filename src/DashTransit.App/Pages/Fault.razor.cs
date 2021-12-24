namespace DashTransit.App.Pages;

using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Core.Application.Commands;
using DashTransit.Core.Application.Queries;
using DashTransit.Core.Domain;
using Fluxor;
using MediatR;

public partial class Fault
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Dispatcher.Dispatch(new FetchData(Core.Domain.FaultId.From(this.FaultId)));
    }

    [ReducerMethod(typeof(FetchData))]
    public static FaultState OnFetch(FaultState state) => state with { Loading = true };

    [ReducerMethod]
    public static FaultState OnFetched(FaultState state, Fetched action) => state with { Loading = false, Fault = action.Fault };

    [ReducerMethod(typeof(TriggerResend))]
    public static FaultState OnTriggerResend(FaultState state) => state with { Resending = true };

    [ReducerMethod(typeof(Resent))]
    public static FaultState OnResent(FaultState state) => state with { Resending = false };

    [FeatureState]
    public record FaultState
    {
        public bool Loading { get; init; } = true;
        public Core.Domain.Fault? Fault { get; init; }
        public bool Resending { get; init; }
    }

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod]
        public async Task HandleFetchDataAction(FetchData action, IDispatcher dispatcher)
        {
            var message = await this.mediator.Send(new FaultById(action.FaultId));
            dispatcher.Dispatch(new Fetched(message));
        }

        [EffectMethod]
        public async Task HandleResend(TriggerResend action, IDispatcher dispatcher)
        {
            await this.mediator.Send(new ResendMessage(action.Message, action.Endpoint));
            dispatcher.Dispatch(new Resent());
        }
    }

    public record FetchData(FaultId FaultId);

    public record Fetched(Core.Domain.Fault? Fault);

    public record TriggerResend(AuditId Message, EndpointId Endpoint);

    public record Resent;
}