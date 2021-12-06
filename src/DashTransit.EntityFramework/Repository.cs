namespace DashTransit.EntityFramework;

using System.Collections.Generic;
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
    {
        throw new System.NotImplementedException();
    }

    public Task<TResult> GetBySpecAsync<TResult>(ISpecification<IRawAuditData, TResult> specification, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<IRawAuditData>> ListAsync(ISpecification<IRawAuditData> specification, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<TResult>> ListAsync<TResult>(ISpecification<IRawAuditData, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification).ToListAsync(cancellationToken);

    Task<IRawAuditData> IReadRepositoryBase<IRawAuditData>.GetByIdAsync<TId>(TId id, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    Task<IRawAuditData> IReadRepositoryBase<IRawAuditData>.GetBySpecAsync<Spec>(Spec specification, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    Task<List<IRawAuditData>> IReadRepositoryBase<IRawAuditData>.ListAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}