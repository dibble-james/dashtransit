namespace DashTransit.Core.Application.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using DashTransit.Core.Domain;
using MediatR;

public record LatestMessages(AuditId Id, MessageType MessageType, DateTime Sent, EndpointId Sender);

public record LatestMessagesQuery(int Page = 1) : IRequest<IEnumerable<LatestMessages>>
{
    public class Handler : IRequestHandler<LatestMessagesQuery, IEnumerable<LatestMessages>>
    {
        private readonly IReadRepositoryBase<IRawAuditData> database;

        public Handler(IReadRepositoryBase<IRawAuditData> database) => this.database = database;

        public async Task<IEnumerable<LatestMessages>> Handle(LatestMessagesQuery request, CancellationToken cancellationToken)
        {
            return await this.database.ListAsync(new Query(request.Page), cancellationToken);
        }

        public class Query : Specification<IRawAuditData, LatestMessages>
        {
            public Query(int page = 1)
            {
                this.Query.Where(x => !x.MessageType.StartsWith("MassTransit.Fault") && x.MessageId != null && (x.ContextType == "Send" || x.ContextType == "Publish" && x.SentTime.HasValue));
                this.Query.OrderByDescending(x => x.SentTime);
                this.Query.Skip((page - 1) * 25);
                this.Query.Take(25);
                this.Query.Select(raw => new LatestMessages(AuditId.From(raw.AuditRecordId), MessageType.From(raw.MessageType), raw.SentTime!.Value, EndpointId.From(new Uri(raw.SourceAddress))));
            }
        }
    }
}