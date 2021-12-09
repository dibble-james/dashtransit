namespace DashTransit.EntityFramework;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using DashTransit.Core.Domain;
using DashTransit.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

public class Repository<T> : RepositoryBase<T>, IReadRepositoryBase<IRawAuditData>
    where T : class
{
    private readonly DashTransitContext context;

    public Repository(DashTransitContext context)
        : base(context)
    {
        this.context = context;       
    }

    public Task<int> CountAsync(ISpecification<IRawAuditData> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification).CountAsync(cancellationToken);

    public Task<TResult> GetBySpecAsync<TResult>(ISpecification<IRawAuditData, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification).FirstOrDefaultAsync(cancellationToken);

    public Task<List<IRawAuditData>> ListAsync(ISpecification<IRawAuditData> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification).ToListAsync(cancellationToken);
    

    public Task<List<TResult>> ListAsync<TResult>(ISpecification<IRawAuditData, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification).ToListAsync(cancellationToken);

    async Task<IRawAuditData> IReadRepositoryBase<IRawAuditData>.GetByIdAsync<TId>(TId id, CancellationToken cancellationToken)
        => await this.context.Set<RawAudit>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);

    Task<IRawAuditData> IReadRepositoryBase<IRawAuditData>.GetBySpecAsync<Spec>(Spec specification, CancellationToken cancellationToken)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification).FirstOrDefaultAsync(cancellationToken);

    async Task<List<IRawAuditData>> IReadRepositoryBase<IRawAuditData>.ListAsync(CancellationToken cancellationToken)
        => (await this.context.Set<RawAudit>().ToListAsync(cancellationToken)).Cast<IRawAuditData>().ToList();
}