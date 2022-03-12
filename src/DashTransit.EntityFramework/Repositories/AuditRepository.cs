namespace DashTransit.EntityFramework.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using DashTransit.Core.Domain;
using DashTransit.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Fault = Entities.Fault;
using DomainFault = Core.Domain.Fault;

public class AuditRepository : IReadRepositoryBase<IRawAuditData>
{
    private readonly DashTransitContext context;

    public AuditRepository(DashTransitContext context) => this.context = context;

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
        => this.context.Set<RawAudit>().CountAsync(cancellationToken);

    public Task<int> CountAsync(ISpecification<IRawAuditData> specification,
        CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .CountAsync(cancellationToken);

    public Task<TResult> GetBySpecAsync<TResult>(
        ISpecification<IRawAuditData, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .FirstAsync(cancellationToken);

    public Task<List<IRawAuditData>> ListAsync(ISpecification<IRawAuditData> specification,
        CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .ToListAsync(cancellationToken);

    Task<List<TResult>> IReadRepositoryBase<IRawAuditData>.ListAsync<TResult>(
        ISpecification<IRawAuditData, TResult> specification, CancellationToken cancellationToken)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .ToListAsync(cancellationToken);

    async Task<IRawAuditData?> IReadRepositoryBase<IRawAuditData>.GetByIdAsync<TId>(TId id, CancellationToken cancellationToken)
        => await this.context.Set<RawAudit>().FindAsync(new object[] { id! }, cancellationToken);

    public Task<IRawAuditData?> GetBySpecAsync<Spec>(Spec specification,
        CancellationToken cancellationToken) where Spec : ISingleResultSpecification, ISpecification<IRawAuditData>
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<List<IRawAuditData>> ListAsync(CancellationToken cancellationToken)
        => (await this.context.Set<RawAudit>().ToListAsync(cancellationToken)).Cast<IRawAuditData>().ToList();
}