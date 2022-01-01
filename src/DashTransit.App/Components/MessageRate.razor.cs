// <copyright file="MessageRate.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Components;

using Core.Application.Queries;
using Fluxor;
using MediatR;

public partial class MessageRate
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Dispatcher.Dispatch(new Fetch());
    }

    [ReducerMethod(typeof(Fetch))]
    public static MessageRateState OnFetch(MessageRateState state) => state with { Loading = true };

    [ReducerMethod]
    public static MessageRateState OnFetched(MessageRateState state, Fetched action) => state with { Loading = false, Rate = action.Rate };

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod(typeof(Fetch))]
        public async Task HandleFetchConversationAction(IDispatcher dispatcher)
        {
            var rate = await this.mediator.Send(new CalculateMessageRate());
            dispatcher.Dispatch(new Fetched(rate));
        }
    }

    [FeatureState]
    public record MessageRateState
    {
        public bool Loading { get; init; } = true;
        public double Rate { get; init; }
    }

    public record Fetch();

    public record Fetched(double Rate);
}