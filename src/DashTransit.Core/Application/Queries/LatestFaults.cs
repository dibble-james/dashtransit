namespace DashTransit.Core.Application.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using DashTransit.Core.Domain;
using MediatR;

public record LatestFault(FaultId Id, string Exception, DateTime Produced, EndpointId ProducedBy, MessageType Type);

public class LatestFaultsQuery : IRequest<IEnumerable<LatestFault>>
{
    public class Handler : IRequestHandler<LatestFaultsQuery, IEnumerable<LatestFault>>
    {
        private readonly IReadRepositoryBase<Fault> database;

        public Handler(IReadRepositoryBase<Fault> database) => this.database = database;

        public async Task<IEnumerable<LatestFault>> Handle(LatestFaultsQuery request, CancellationToken cancellationToken)
        {
            return await this.database.ListAsync(new Query(), cancellationToken);
        }

        public class Query : Specification<Fault, LatestFault>
        {
            public Query()
            {
                this.Query.Take(50);
                this.Query.OrderByDescending(x => x.Produced);
                this.Query.Where(x => new[] {"Publish", "Send"}.Contains(x.Message!.ContextType));
                this.Query.Select(x => new LatestFault(x.Id, x.Exception, x.Produced, x.ProducedBy, MessageType.From(x.Message!.MessageType)));
            }
        }
    }
}