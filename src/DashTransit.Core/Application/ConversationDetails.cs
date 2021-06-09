// <copyright file="ConversationDetails.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DashTransit.Core.Application.Common;
    using DashTransit.Core.Domain;
    using MediatR;
    using SqlKata.Execution;

    public record ConversationDetails(CorrelationId ConversationId) : IRequest<ConversationDetailsResponse>;

    public record ConversationDetailsResponse(CorrelationId ConversationId, IEnumerable<ConversationDetailsResponse.Message> Messages)
    {
        public record Message(MessageId MessageId, string MessageType, DateTimeOffset Timestamp, Message? Parent = null);
    }

    public class ConversationDetailsHandler : IRequestHandler<ConversationDetails, ConversationDetailsResponse>
    {
        private readonly Task<QueryFactory> queryFactory;

        public ConversationDetailsHandler(Task<QueryFactory> queryFactory) => this.queryFactory = queryFactory;

        public Task<ConversationDetailsResponse> Handle(ConversationDetails request, CancellationToken cancellationToken) => With.Db<ConversationDetailsResponse>(this.queryFactory)(async db =>
        {
            var messageQuery = db.Query()
                .Select(
                    "Message.{MessageId,Timestamp,MessageTypeId}",
                    "InitiatedBy.MessageId as InitiatedId",
                    "InitiatedBy.Timestamp as InitiatedTimestamp",
                    "InitiatedBy.MessageTypeId as InitiatedTypeId")
                .FromRaw("Messages as Message, Initiator as Initiator, Messages as InitiatedBy")
                .WhereRaw("MATCH(Message-(Initiator)->InitiatedBy)")
                .Where("Initiator.ConversationId", request.ConversationId.Id);

            var cte = db.Query()
                .Select("Messages.{MessageId,Timestamp,MessageTypeId,InitiatedId,InitiatedTimestamp,InitiatedTypeId}", "MessageTypes.Name as MessageType", "InitiatedType.Name as InitiatedType")
                .From(messageQuery, "Messages")
                .Join("MessageTypes", j => j.On("Messages.MessageTypeId", "MessageTypes.Idx"))
                .Join("MessageTypes as InitiatedType", j => j.On("Messages.InitiatedTypeId", "InitiatedType.Idx"));

            var initiator = db.Query("m")
                .SelectRaw("[m].[InitiatedId], [m].[InitiatedTimestamp], [m].[InitiatedTypeId], null,null,null, [m].[InitiatedType], NULL")
                .LeftJoin("m as m2", "m.MessageId", "m2.InitiatedId")
                .WhereNull("m2.MessageId");

            var query = await db.Query()
                .With("m", cte)
                .From("m")
                .UnionAll(initiator)
                .GetAsync();

            static ConversationDetailsResponse.Message? MapInitiator(dynamic result)
            {
                if (result.InitiatedId is null)
                {
                    return null;
                }

                return new ConversationDetailsResponse.Message(new MessageId(result.InitiatedId), result.InitiatedType, result.InitiatedTimestamp);
            }

            var messages = query.Select(m =>
                new ConversationDetailsResponse.Message(
                    new MessageId(m.MessageId),
                    m.MessageType,
                    m.Timestamp,
                    MapInitiator(m)));

            return new ConversationDetailsResponse(request.ConversationId, messages.ToArray());
        });
    }
}
