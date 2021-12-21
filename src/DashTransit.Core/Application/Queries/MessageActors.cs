namespace DashTransit.Core.Application.Queries;

public record MessageActors(MessageId Id) : IRequest<IEnumerable<Actor>>
{
    public class Handler : IRequestHandler<MessageActors, IEnumerable<Actor>>
    {
        private readonly IReadRepositoryBase<IRawAuditData> database;

        public Handler(IReadRepositoryBase<IRawAuditData> database) => this.database = database;

        public async Task<IEnumerable<Actor>> Handle(MessageActors request, CancellationToken cancellationToken)
        {
            var audits = await this.database.ListAsync(new Query(request.Id), cancellationToken);

            return audits.Select(Actor.FromRawAuditData);
        }

        public class Query : Specification<IRawAuditData>
        {
            public Query(MessageId id) => this.Query.Where(x => x.MessageId == id);
        }
    }
}