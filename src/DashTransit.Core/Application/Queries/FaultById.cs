// <copyright file="FaultById.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Queries;

using Fault = DashTransit.Core.Domain.Fault;

public record FaultById(FaultId FaultId) : IRequest<Fault?>
{
    public class Handler : IRequestHandler<FaultById, Fault?>
    {
        private readonly IReadRepositoryBase<Fault> database;

        public Handler(IReadRepositoryBase<Fault> database) => this.database = database;

        public async Task<Fault?> Handle(FaultById request, CancellationToken cancellationToken)
        {
            var fault = await this.database.GetBySpecAsync(new Query(request.FaultId), cancellationToken);
            return fault;
        }

        private class Query : Specification<Fault>, ISingleResultSpecification
        {
            public Query(FaultId id)
            {
                this.Query.Where(x => x.Id == id);
                this.Query.Include(x => x.Message);
            }
        }
    }
}