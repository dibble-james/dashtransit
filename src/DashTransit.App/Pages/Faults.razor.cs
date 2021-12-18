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

    [ReducerMethod(typeof(FetchData))]
    public static FaultsState OnFetch(FaultsState state) => state with { Loading = true };

    [ReducerMethod]
    public static FaultsState OnFetched(FaultsState state, Fetched action) => state with { Loading = false, Faults = action.Faults.ToList() };

    [FeatureState]
    public record FaultsState
    {
        public bool Loading { get; init; } = true;
        public List<LatestFault> Faults { get; init; } = new List<LatestFault>();
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
            var messages = await this.mediator.Send(new LatestFaultsQuery());
            dispatcher.Dispatch(new Fetched(messages));
        }
    }

    public record FetchData();

    public record Fetched(IEnumerable<LatestFault> Faults);
}