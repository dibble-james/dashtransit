// <copyright file="ConversationDetails.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DashTransit.Core.Application.Common;
    using DashTransit.Core.Domain;
    using MediatR;
    using SqlKata.Execution;

    public record ConversationDetails(CorrelationId ConversationId) : IRequest<ConversationDetailsResponse>;

    public record ConversationDetailsResponse(CorrelationId ConversationId);

    public class ConversationDetailsHandler : IRequestHandler<ConversationDetails, ConversationDetailsResponse>
    {
        private readonly Task<QueryFactory> queryFactory;

        public ConversationDetailsHandler(Task<QueryFactory> queryFactory) => this.queryFactory = queryFactory;

        public Task<ConversationDetailsResponse> Handle(ConversationDetails request, CancellationToken cancellationToken) => With.Db<ConversationDetailsResponse>(this.queryFactory)(async db =>
        {
            var messageQuery = db.Query()
                .Select("Message.{MessageId,Timestamp,MessageTypeId}", "InitiatedBy.{MessageId as InitiatorId,Timestamp as InitiatorTimestamp,MessageTypeId as InitiatorMessageTypeId}")
                .From("Messages as Message, Initiator as Initiator, Messages as InitiatedBy")
                .WhereRaw("MATCH(Message-(Initiator)->InitiatedBy)")
                .Where("Initiator.ConversationId", request.ConversationId.Id);

            var query = await db.Query()
                .Select("Messages.{MessageId,Timestamp,MessageTypeId}", "MessageTypes.{Id,Name}", "InitiatedType.{Id,Name}")
                .From(messageQuery, "Messages")
                .Join("MessageTypes", j => j.On("Messages.MessageTypeId", "MessageTypes.Idx"))
                .Join("MessageTypes as InitiatedType", j => j.On("Messages.InitiatorMessageTypeId", "InitiatedType.Idx"))
                .GetAsync();

            return new ConversationDetailsResponse(request.ConversationId);
        });
    }
}
