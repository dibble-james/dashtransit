// <copyright file="Conversation.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Components;

using Core.Application.Queries;
using Core.Domain;
using Fluxor;
using MediatR;

public partial class Conversation
{
    private void LoadConversation()
    {
        this.Dispatcher.Dispatch(new Fetch(this.ConversationId));
    }

    [ReducerMethod(typeof(Fetch))]
    public static ConversationState OnFetch(ConversationState state) => state with { Loading = true };

    [ReducerMethod]
    public static ConversationState OnFetched(ConversationState state, Fetched action) => state with { Loading = false, Conversation = action.Conversation.ToList() };

    public class Handlers
    {
        private readonly IMediator mediator;

        public Handlers(IMediator mediator) => this.mediator = mediator;

        [EffectMethod]
        public async Task HandleFetchConversationAction(Fetch action, IDispatcher dispatcher)
        {
            var conversation = await this.mediator.Send(new ConversationById(action.Id));
            dispatcher.Dispatch(new Fetched(conversation));
        }
    }

    [FeatureState]
    public record ConversationState
    {
        public bool Loading { get; init; } = true;
        public List<IRawAuditData> Conversation { get; init; }
    }

    public record Fetch(Guid Id);

    public record Fetched(IEnumerable<IRawAuditData> Conversation);
}