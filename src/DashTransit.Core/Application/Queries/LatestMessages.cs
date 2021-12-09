namespace DashTransit.Core.Application.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using DashTransit.Core.Domain;
using MediatR;

public record LatestMessages(MessageId Id, MessageType MessageType, DateTime Sent);

public class LatestMessagesQuery : IRequest<IEnumerable<LatestMessages>>
{
    public class Handler : IRequestHandler<LatestMessagesQuery, IEnumerable<LatestMessages>>
    {
        private readonly IReadRepositoryBase<IRawAuditData> database;

        public Handler(IReadRepositoryBase<IRawAuditData> database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<LatestMessages>> Handle(LatestMessagesQuery request, CancellationToken cancellationToken)
            => await this.database.ListAsync(new Query(), cancellationToken);

        public class Query : Specification<IRawAuditData, LatestMessages>
        {
            public Query()
            {
                Query.Where(x => x.MessageId.HasValue && (x.ContextType == "Send" || x.ContextType == "Publish" && x.SentTime.HasValue));

                Query.OrderByDescending(x => x.SentTime);

                Query.Take(50);

                Query.Select(raw => new LatestMessages(MessageId.From(raw.MessageId.GetValueOrDefault()), MessageType.From(raw.MessageType), raw.SentTime!.Value));
            }
        }
    }
}