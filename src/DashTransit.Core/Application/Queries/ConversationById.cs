// <copyright file="ConversationById.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Queries;

public record ConversationById(Guid ConversationId) : IRequest<IEnumerable<IRawAuditData>>
{
    public class Handler : IRequestHandler<ConversationById, IEnumerable<IRawAuditData>>
    {
        private readonly IReadRepositoryBase<IRawAuditData> database;

        public Handler(IReadRepositoryBase<IRawAuditData> database) => this.database = database;

        public async Task<IEnumerable<IRawAuditData>> Handle(ConversationById request, CancellationToken cancellationToken)
        {
            return await this.database.ListAsync(new Query(request.ConversationId), cancellationToken);
        }

        private sealed class Query : Specification<IRawAuditData>
        {
            public Query(Guid conversationId) => this.Query.Where(x => x.ConversationId == conversationId);
        }
    }
}