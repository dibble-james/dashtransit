// <copyright file="Actors.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Components;

using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Core.Application.Queries;
using Core.Domain;
using Fluxor;
using MediatR;

public partial class Actors
{
    private void LoadActors()
    {
        this.Dispatcher.Dispatch(new Fetch(this.MessageId!));
    }

    [ReducerMethod(typeof(Fetch))]
    public static ActorState OnFetch(ActorState state) => state with { Loading = true };

    [ReducerMethod]
    public static ActorState OnFetched(ActorState state, Fetched action) => state with { Loading = false, Actors = action.Actors };


    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod]
        public async Task HandleFetchActorsAction(Fetch action, IDispatcher dispatcher)
        {
            var actors = await this.mediator.Send(new MessageActors(action.Id));
            var diagram = new Diagram();

            var initiator = actors.First(x => x is Sender or Publisher);
            var initiatorNode = new NodeModel { Title = initiator.Endpoint.ToString() };
            initiatorNode.AddPort(PortAlignment.Right);
            diagram.Nodes.Add(initiatorNode);

            foreach (var consumer in actors.Where(x => x is Consumer))
            {
                var node = new NodeModel { Title = consumer.Endpoint.ToString() };
                node.AddPort(PortAlignment.Left);

                diagram.Nodes.Add(node);
                diagram.Links.Add(new LinkModel(initiatorNode.GetPort(PortAlignment.Right), node.GetPort(PortAlignment.Left)));
            }

            diagram.AutoArrange();

            dispatcher.Dispatch(new Fetched(diagram));
        }
    }

    [FeatureState]
    public record ActorState
    {
        public bool Loading { get; init; } = true;
        public Diagram? Actors { get; init; }
    }

    public record Fetch(MessageId Id);

    public record Fetched(Diagram Actors);
}