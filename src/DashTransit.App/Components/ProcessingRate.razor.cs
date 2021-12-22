// <copyright file="ProcessingRate.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Components;

using Core.Application.Queries;
using Fluxor;
using MediatR;

public partial class ProcessingRate
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Dispatcher.Dispatch(new Fetch());
    }

    [ReducerMethod(typeof(Fetch))]
    public static ProcessingRateState OnFetch(ProcessingRateState state) => state with { Loading = true };

    [ReducerMethod]
    public static ProcessingRateState OnFetched(ProcessingRateState state, Fetched action) => state with { Loading = false, Rate = action.Rate };

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod]
        public async Task HandleFetchConversationAction(Fetch action, IDispatcher dispatcher)
        {
            var rate = await this.mediator.Send(new CalculateProcessingRate());
            dispatcher.Dispatch(new Fetched(rate));
        }
    }

    [FeatureState]
    public record ProcessingRateState
    {
        public bool Loading { get; init; } = true;
        public TimeSpan Rate { get; init; }
    }

    public record Fetch();

    public record Fetched(TimeSpan Rate);
}