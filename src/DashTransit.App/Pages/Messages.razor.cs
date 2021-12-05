namespace DashTransit.App.Pages;

using DashTransit.Core.Application.Queries;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;

public partial class Messages
{
    [Inject]
    public IState<MessagesState> State { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    [Inject]
    public IMediator Mediator { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Dispatcher.Dispatch(new FetchData());
    }

    [EffectMethod(typeof(FetchData))]
    public async Task HandleFetchDataAction(IDispatcher dispatcher)
    {
        var messages = await Mediator.Send(new LatestMessagesQuery());
        dispatcher.Dispatch(new Fetched(messages));
    }

    [ReducerMethod(typeof(FetchData))]
    public static MessagesState OnFetch(MessagesState state) => state with { Loading = true, Messages = Enumerable.Empty<LatestMessages>() };

    [ReducerMethod]
    public static MessagesState OnFetched(MessagesState state, Fetched action) => state with { Loading = false, Messages = action.Messages };

    [FeatureState]
    public record MessagesState
    {
        public bool Loading { get; init; } = true;
        public IEnumerable<LatestMessages> Messages { get; init; } = Enumerable.Empty<LatestMessages>();
    }

    public record FetchData();

    public record Fetched(IEnumerable<LatestMessages> Messages);
}