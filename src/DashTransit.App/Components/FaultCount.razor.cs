// <copyright file="FaultCount.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Components;

using Core.Application.Queries;
using Fluxor;
using MediatR;

public partial class FaultCount
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Dispatcher.Dispatch(new Fetch());
    }

    [ReducerMethod(typeof(Fetch))]
    public static FaultCountState OnFetch(FaultCountState state) => state with { Loading = true };

    [ReducerMethod]
    public static FaultCountState OnFetched(FaultCountState state, Fetched action) => state with { Loading = false, Rate = action.Rate };

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod]
        public async Task HandleFetchFaultCountAction(Fetch action, IDispatcher dispatcher)
        {
            var rate = await this.mediator.Send(new CountFaults());
            dispatcher.Dispatch(new Fetched(rate));
        }
    }

    [FeatureState]
    public record FaultCountState
    {
        public bool Loading { get; init; } = true;
        public int Rate { get; init; }
    }

    public record Fetch();

    public record Fetched(int Rate);
}