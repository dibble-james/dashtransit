// <copyright file="Endpoints.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Pages;

using Core.Domain;
using Fluxor;
using MediatR;

public partial class Endpoints
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Dispatcher.Dispatch(new FetchData());
    }

    [ReducerMethod(typeof(FetchData))]
    public static EndpointsState OnFetch(EndpointsState state) => state with { Loading = true };

    [ReducerMethod]
    public static EndpointsState OnFetched(EndpointsState state, Fetched action) => state with { Loading = false, Endpoints = action.Endpoints.ToList() };

    [FeatureState]
    public record EndpointsState
    {
        public bool Loading { get; init; } = true;
        public List<EndpointId> Endpoints { get; init; } = new List<EndpointId>();
    }

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod(typeof(FetchData))]
        public async Task HandleFetchDataAction(IDispatcher dispatcher)
        {
            var endpoints = await this.mediator.Send(new Core.Application.Queries.Endpoints());
            dispatcher.Dispatch(new Fetched(endpoints));
        }
    }

    public record FetchData;

    public record Fetched(IEnumerable<EndpointId> Endpoints);
}