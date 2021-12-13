namespace DashTransit.App.Pages;

using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
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

    private void LoadActors()
    {
        this.Dispatcher.Dispatch(new FetchActors(MessageId.From(this.State.Value.Message!.MessageId!.Value)));
    }

    [ReducerMethod(typeof(FetchData))]
    public static MessageState OnFetch(MessageState state) => state with { Loading = true };

    [ReducerMethod]
    public static MessageState OnFetched(MessageState state, Fetched action) => state with { Loading = false, Message = action.Message };

    [ReducerMethod(typeof(FetchActors))]
    public static MessageState OnFetchActors(MessageState state) => state with { LoadingActors = true, Actors = null };

    [ReducerMethod]
    public static MessageState OnActorsFetched(MessageState state, ActorsFetched action) => state with { ActorsLoaded = true, LoadingActors = false, Actors = action.Actors };

    [FeatureState]
    public record MessageState
    {
        public bool Loading { get; init; } = true;
        public IRawAuditData? Message { get; init; }
        public bool LoadingActors { get; init; } = true;
        public bool ActorsLoaded { get; init; } = false;
        public Diagram? Actors { get; init; }
    }

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod]
        public async Task HandleFetchDataAction(FetchData action, IDispatcher dispatcher)
        {
            var message = await this.mediator.Send(new MessageByAuditId(action.AuditId));
            dispatcher.Dispatch(new Fetched(message));
        }

        [EffectMethod]
        public async Task HandleFetchActorsAction(FetchActors action, IDispatcher dispatcher)
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

            dispatcher.Dispatch(new ActorsFetched(diagram));
        }
    }

    public record FetchData(AuditId AuditId);

    public record Fetched(IRawAuditData? Message);

    public record FetchActors(MessageId Id);

    public record ActorsFetched(Diagram Actors);
}