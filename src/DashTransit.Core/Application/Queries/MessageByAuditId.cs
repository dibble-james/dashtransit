namespace DashTransit.Core.Application.Queries;

using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using DashTransit.Core.Domain;
using MediatR;

public record MessageByAuditId(AuditId Id) : IRequest<IRawAuditData?>
{
    public class Handler : IRequestHandler<MessageByAuditId, IRawAuditData?>
    {
        private readonly IReadRepositoryBase<IRawAuditData> database;

        public Handler(IReadRepositoryBase<IRawAuditData> database) => this.database = database;

        public async Task<IRawAuditData?> Handle(MessageByAuditId request, CancellationToken cancellationToken)
        {
            var message = await this.database.GetBySpecAsync(new Query(request.Id), cancellationToken);

            return (message?.MessageId.HasValue).GetValueOrDefault() ? message : null;
        }

        public class Query : Specification<IRawAuditData>, ISingleResultSpecification
        {
            public Query(AuditId id) => this.Query.Where(x => x.AuditRecordId == id.Value);
        }
    }
}