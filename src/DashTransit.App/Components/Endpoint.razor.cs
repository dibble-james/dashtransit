// <copyright file="Endpoint.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Components;

using Core.Application.Queries;
using DashTransit.Core.Domain;
using Fluxor;
using MediatR;

public partial class Endpoint
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Dispatcher.Dispatch(new Fetch(this.EndpointId!));
    }

    [ReducerMethod(typeof(Fetch))]
    public static EndpointState OnFetch(EndpointState state) => state with { Loading = true };

    [ReducerMethod]
    public static EndpointState OnFetched(EndpointState state, Fetched action) => state with { Loading = false, Rate = action.Rate };

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod]
        public async Task HandleFetchConversationAction(Fetch action, IDispatcher dispatcher)
        {
            var rate = await this.mediator.Send(new CalculateMessageRate(action.Endpoint));
            dispatcher.Dispatch(new Fetched(rate));
        }
    }

    [FeatureState]
    public record EndpointState
    {
        public bool Loading { get; init; } = true;
        public double Rate { get; init; }
    }

    public record Fetch(EndpointId Endpoint);

    public record Fetched(double Rate);
}