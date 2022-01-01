namespace DashTransit.App.Pages;

using DashTransit.Core.Application.Queries;
using Fluxor;
using MediatR;

public partial class Messages
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Dispatcher.Dispatch(new FetchData());
    }

    [ReducerMethod(typeof(FetchData))]
    public static MessagesState OnFetch(MessagesState state) => state with { Loading = true };

    [ReducerMethod]
    public static MessagesState OnFetched(MessagesState state, Fetched action) => state with { Loading = false, Messages = action.Messages.ToList() };

    [FeatureState]
    public record MessagesState
    {
        public bool Loading { get; init; } = true;
        public List<LatestMessages> Messages { get; init; } = new List<LatestMessages>();
    }

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [EffectMethod(typeof(FetchData))]
        public async Task HandleFetchDataAction(IDispatcher dispatcher)
        {
            var messages = await this.mediator.Send(new LatestMessagesQuery());
            dispatcher.Dispatch(new Fetched(messages));
        }
    }

    public record FetchData();

    public record Fetched(IEnumerable<LatestMessages> Messages);
}