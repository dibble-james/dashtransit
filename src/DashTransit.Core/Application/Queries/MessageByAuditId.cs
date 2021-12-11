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
        private readonly IMediator mediator;

        public Handler(IReadRepositoryBase<IRawAuditData> database, IMediator mediator)
        {
            this.database = database;
            this.mediator = mediator;
        }

        public async Task<IRawAuditData?> Handle(MessageByAuditId request, CancellationToken cancellationToken)
        {
            var message = await this.database.GetBySpecAsync(new Query(request.Id), cancellationToken);

            if (message is null)
            {
                return null;
            }

            if (!message.MessageId.HasValue)
            {
                return message;
            }

            var actors = this.mediator.Send(new MessageActors(MessageId.From(message.MessageId.Value)));

            return message;
        }

        public class Query : Specification<IRawAuditData>, ISingleResultSpecification
        {
            public Query(AuditId id)
            {
                this.Query.Where(x => x.AuditRecordId == id.Value);
            }
        }
    }
}