// <copyright file="MessageDetails.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DashTransit.Core.Application.Common;
    using DashTransit.Core.Domain;
    using MediatR;
    using OneOf;
    using OneOf.Types;
    using SqlKata.Execution;

    public record MessageDetails(MessageId Id) : IRequest<OneOf<MessageDetailsResponse, None>>;

    public record MessageDetailsResponse(
        MessageId Id,
        CorrelationId ConversationId,
        DateTimeOffset Timestamp,
        string MessageType,
        string Content,
        string Source,
        string Destination);

    public class MessageDetailsHandler : IRequestHandler<MessageDetails, OneOf<MessageDetailsResponse, None>>
    {
        private readonly Task<QueryFactory> queryFactory;

        public MessageDetailsHandler(Task<QueryFactory> queryFactory) => this.queryFactory = queryFactory;

        public Task<OneOf<MessageDetailsResponse, None>> Handle(MessageDetails request, CancellationToken cancellationToken) => With.Db<OneOf<MessageDetailsResponse, None>>(this.queryFactory)(async db =>
        {
            var query = await db.Query()
                .From("Messages")
                .Join("MessageTypes", j => j.On("Messages.MessageTypeId", "MessageTypes.Idx"))
                .Join("Endpoints as Source", j => j.On("Messages.SourceEndpointId", "Source.Idx"))
                .Join("Endpoints as Destination", j => j.On("Messages.SourceEndpointId", "Destination.Idx"))
                .Where("Messages.MessageId", request.Id.Id)
                .FirstOrDefaultAsync();

            if (query is null)
            {
                return new None();
            }

            return new MessageDetailsResponse(
                new MessageId(query.MessageId),
                new CorrelationId(query.ConversationId),
                query.Timestamp,
                query.Name,
                query.Content,
                null,
                null);
        });
    }
}
