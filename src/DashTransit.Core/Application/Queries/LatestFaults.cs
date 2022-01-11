namespace DashTransit.Core.Application.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using DashTransit.Core.Domain;
using MediatR;

public record LatestFault(FaultId Id, string? Exception, DateTime Produced, EndpointId ProducedBy, MessageType Type);

public record LatestFaultsQuery(int Page = 1) : IRequest<IEnumerable<LatestFault>>
{
    public class Handler : IRequestHandler<LatestFaultsQuery, IEnumerable<LatestFault>>
    {
        private readonly IReadRepositoryBase<Fault> database;

        public Handler(IReadRepositoryBase<Fault> database) => this.database = database;

        public async Task<IEnumerable<LatestFault>> Handle(LatestFaultsQuery request, CancellationToken cancellationToken)
        {
            var faults = await this.database.ListAsync(new Query(request.Page), cancellationToken);
            return faults.Select(x => new LatestFault(x.Id, x.Exceptions.FirstOrDefault()?.Message, x.Produced, x.ProducedBy,
                MessageType.From(x.Message?.MessageType ?? "Unknown")));
        }

        public class Query : Specification<Fault>
        {
            public Query(int page = 1)
            {
                this.Query.Skip((page - 1) * 25);
                this.Query.Take(25);
                this.Query.OrderByDescending(x => x.Produced);
            }
        }
    }
}