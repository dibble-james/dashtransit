namespace DashTransit.App.Pages;

using DashTransit.Core.Application.Queries;
using DashTransit.Core.Domain;
using Fluxor;
using MediatR;

public partial class Message
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Dispatcher.Dispatch(new FetchData(Core.Domain.AuditId.From(this.AuditId)));
    }

    [ReducerMethod(typeof(FetchData))]
    public static MessageState OnFetch(MessageState state) => state with { Loading = true };

    [ReducerMethod]
    public static MessageState OnFetched(MessageState state, Fetched action) => state with { Loading = false, Message = action.Message };

    [FeatureState]
    public record MessageState
    {
        public bool Loading { get; init; } = true;
        public IRawAuditData? Message { get; init; }
    }

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [EffectMethod]
        public async Task HandleFetchDataAction(FetchData action, IDispatcher dispatcher)
        {
            var message = await this.mediator.Send(new MessageByAuditId(action.AuditId));
            dispatcher.Dispatch(new Fetched(message));
        }
    }

    public record FetchData(AuditId AuditId);

    public record Fetched(IRawAuditData? Message);
}