namespace DashTransit.Core.Application.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using DashTransit.Core.Domain;
using MediatR;

public record LatestFault(FaultId Id, string? Exception, DateTime Produced, EndpointId ProducedBy, MessageType Type);

public record LatestFaultsQuery(int Skip = 0, int Take = 0) : IRequest<Page<LatestFault>>
{
    public class Handler : IRequestHandler<LatestFaultsQuery, Page<LatestFault>>
    {
        private readonly IReadRepositoryBase<Fault> database;

        public Handler(IReadRepositoryBase<Fault> database) => this.database = database;

        public async Task<Page<LatestFault>> Handle(LatestFaultsQuery request, CancellationToken cancellationToken)
        {
            var faults = await this.database.ListAsync(new Query(request.Skip, request.Take), cancellationToken);
            var count = await this.database.CountAsync(new Query(), cancellationToken);

            var items = faults.Select(x => new LatestFault(x.Id, x.Exceptions.FirstOrDefault()?.Message, x.Produced, x.ProducedBy,
                MessageType.From(x.Message?.MessageType ?? "Unknown")));

            return new Page<LatestFault>(items, count);
        }

        public class Query : Specification<Fault>
        {
            public Query()
            {
            }

            public Query(int skip, int take)
            {
                this.Query.Skip(skip);
                this.Query.Take(take);
                this.Query.OrderByDescending(x => x.Produced);
            }
        }
    }
}