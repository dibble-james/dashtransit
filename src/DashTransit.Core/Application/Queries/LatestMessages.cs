namespace DashTransit.Core.Application.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using DashTransit.Core.Domain;
using MediatR;

public record LatestMessages(AuditId Id, MessageType MessageType, DateTime Sent, EndpointId Sender);

public record LatestMessagesQuery(int Skip = 0, int Take = 25) : IRequest<Page<LatestMessages>>
{
    public class Handler : IRequestHandler<LatestMessagesQuery, Page<LatestMessages>>
    {
        private readonly IReadRepositoryBase<IRawAuditData> database;

        public Handler(IReadRepositoryBase<IRawAuditData> database) => this.database = database;

        public async Task<Page<LatestMessages>> Handle(LatestMessagesQuery request, CancellationToken cancellationToken)
        {
            var items = await this.database.ListAsync(new Query(request.Skip, request.Take), cancellationToken);
            var total = await this.database.CountAsync(new Query(), cancellationToken);

            return new Page<LatestMessages>(items, total);
        }

        public class Query : Specification<IRawAuditData, LatestMessages>
        {
            public Query(int skip, int take)
                : this()
            {
                this.Query.OrderByDescending(x => x.SentTime);
                this.Query.Skip(skip);
                this.Query.Take(take);
                this.Query.Select(raw => new LatestMessages(AuditId.From(raw.AuditRecordId), MessageType.From(raw.MessageType), raw.SentTime!.Value, EndpointId.From(new Uri(raw.SourceAddress))));
            }

            public Query()
            {
                this.Query.Where(x => !x.MessageType.StartsWith("MassTransit.Fault") && x.MessageId != null && (x.ContextType == "Send" || x.ContextType == "Publish" && x.SentTime.HasValue));
            }
        }
    }
}