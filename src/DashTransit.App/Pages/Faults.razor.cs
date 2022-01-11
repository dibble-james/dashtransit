namespace DashTransit.App.Pages;

using DashTransit.Core.Application.Queries;
using Fluxor;
using MediatR;

public partial class Faults
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Dispatcher.Dispatch(new FetchData());
    }

    [ReducerMethod]
    public static FaultsState OnFetch(FaultsState state, FetchData action) => state with { Loading = true, Page = action.Page };

    [ReducerMethod]
    public static FaultsState OnFetched(FaultsState state, Fetched action) => state with { Loading = false, Faults = action.Faults.ToList() };

    [FeatureState]
    public record FaultsState
    {
        public bool Loading { get; init; } = true;
        public List<LatestFault> Faults { get; init; } = new List<LatestFault>();
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
            var messages = await this.mediator.Send(new LatestFaultsQuery(action.Page));
            dispatcher.Dispatch(new Fetched(messages));
        }
    }

    public record FetchData(int Page = 1);

    public record Fetched(IEnumerable<LatestFault> Faults);
}