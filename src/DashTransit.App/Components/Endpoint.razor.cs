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

    [ReducerMethod]
    public static EndpointState OnFetch(EndpointState state, Fetch action)
    {
        var e = new Dictionary<EndpointId, (bool, double)>(state.Endpoints);
        e[action.Endpoint] = (true, 0);
        return state with { Endpoints = e };
    }

    [ReducerMethod]
    public static EndpointState OnFetched(EndpointState state, Fetched action)
    {
        var e = new Dictionary<EndpointId, (bool, double)>(state.Endpoints);
        e[action.Endpoint] = (false, action.Rate);
        return state with { Endpoints = e };
    }

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod]
        public async Task HandleFetchConversationAction(Fetch action, IDispatcher dispatcher)
        {
            var rate = await this.mediator.Send(new CalculateMessageRate(action.Endpoint));
            dispatcher.Dispatch(new Fetched(action.Endpoint, rate));
        }
    }

    [FeatureState]
    public record EndpointState
    {
        public IReadOnlyDictionary<EndpointId, (bool Loading, double Rate)> Endpoints { get; init; } = new Dictionary<EndpointId, (bool Loading, double Rate)>();

        public bool IsLoading(EndpointId endpoint) => this.Endpoints.TryGetValue(endpoint, out var val) ? val.Loading : false;

        public double GetRate(EndpointId endpoint) => this.Endpoints.TryGetValue(endpoint, out var val) ? val.Rate : 0;
    }

    public record Fetch(EndpointId Endpoint);

    public record Fetched(EndpointId Endpoint, double Rate);
}