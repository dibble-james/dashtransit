// <copyright file="ListMessages.cs" company="James Dibble">
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

    public record ListMessages(int Page, int PageSize) : IRequest<ListMessageResponse>, IRequestPage;

    public record ListMessageResponse(int Total, IEnumerable<ListMessageResponse.Message> Messages) : IPage
    {
        public record Message(MessageId Id, DateTimeOffset Timestamp, string MessageType);
    }

    public class ListMessagesHandler : IRequestHandler<ListMessages, ListMessageResponse>
    {
        private readonly Task<QueryFactory> queryFactory;

        public ListMessagesHandler(Task<QueryFactory> queryFactory) => this.queryFactory = queryFactory;

        public Task<ListMessageResponse> Handle(ListMessages request, CancellationToken cancellationToken) => With.Db<ListMessageResponse>(this.queryFactory)(async db =>
        {
            try
            {
                var messageQuery = db.Query()
                        .From("Messages")
                        .Select("MessageId", "Timestamp", "MessageTypeId")
                        .OrderByDesc("Timestamp");

                var totalQuery = messageQuery.Clone().CountAsync<int>();

                var query = db.Query()
                    .Select("Messages.{MessageId,Timestamp,MessageTypeId}", "MessageTypes.{Id,Name}")
                    .From(messageQuery.ForPage(request.Page, request.PageSize), "Messages")
                    .Join("MessageTypes", j => j.On("Messages.MessageTypeId", "MessageTypes.Idx"));

                var results = await query.GetAsync();
                var total = await totalQuery;

                return new ListMessageResponse(total, results.Select(r => new ListMessageResponse.Message(new MessageId(r.MessageId), r.Timestamp, r.Name)));
            }
            catch
            {
                throw;
            }
        });
    }
}
