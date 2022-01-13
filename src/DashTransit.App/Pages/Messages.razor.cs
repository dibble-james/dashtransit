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

    [ReducerMethod]
    public static MessagesState OnFetch(MessagesState state, FetchData action) => state with { Loading = true, Page = action.Page, Messages = null };

    [ReducerMethod]
    public static MessagesState OnFetched(MessagesState state, Fetched action) => state with { Loading = false, Messages = action.Messages.ToList() };

    [FeatureState]
    public record MessagesState
    {
        public bool Loading { get; init; } = true;
        public List<LatestMessages>? Messages { get; init; }
        public int Page { get; init; } = 1;
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
            var messages = await this.mediator.Send(new LatestMessagesQuery(action.Page));
            dispatcher.Dispatch(new Fetched(messages));
        }
    }

    public record FetchData(int Page = 1);

    public record Fetched(IEnumerable<LatestMessages> Messages);
}