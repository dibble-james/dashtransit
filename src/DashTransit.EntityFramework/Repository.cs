namespace DashTransit.EntityFramework;

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

public class Repository<T> : RepositoryBase<T>, IReadRepositoryBase<IRawAuditData>, IRepositoryBase<DomainFault>
    where T : class
{
    private readonly DashTransitContext context;

    public Repository(DashTransitContext context)
        : base(context)
    {
        this.context = context;
    }

    Task<int> IReadRepositoryBase<IRawAuditData>.CountAsync(ISpecification<IRawAuditData> specification,
        CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .CountAsync(cancellationToken);

    Task<TResult> IReadRepositoryBase<IRawAuditData>.GetBySpecAsync<TResult>(
        ISpecification<IRawAuditData, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .FirstOrDefaultAsync(cancellationToken);

    Task<List<IRawAuditData>> IReadRepositoryBase<IRawAuditData>.ListAsync(ISpecification<IRawAuditData> specification,
        CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .ToListAsync(cancellationToken);

    Task<List<TResult>> IReadRepositoryBase<IRawAuditData>.ListAsync<TResult>(
        ISpecification<IRawAuditData, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .ToListAsync(cancellationToken);

    async Task<IRawAuditData> IReadRepositoryBase<IRawAuditData>.GetByIdAsync<TId>(TId id,
        CancellationToken cancellationToken)
        => await this.context.Set<RawAudit>().FindAsync(new object[] {id}, cancellationToken: cancellationToken);

    Task<IRawAuditData> IReadRepositoryBase<IRawAuditData>.GetBySpecAsync<Spec>(Spec specification,
        CancellationToken cancellationToken)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<RawAudit>(), specification)
            .FirstOrDefaultAsync(cancellationToken);

    async Task<List<IRawAuditData>> IReadRepositoryBase<IRawAuditData>.ListAsync(CancellationToken cancellationToken)
        => (await this.context.Set<RawAudit>().ToListAsync(cancellationToken)).Cast<IRawAuditData>().ToList();

    Task<int> IReadRepositoryBase<DomainFault>.CountAsync(ISpecification<DomainFault> specification,
        CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<Fault>(), specification)
            .CountAsync(cancellationToken);

    Task<TResult> IReadRepositoryBase<DomainFault>.GetBySpecAsync<TResult>(
        ISpecification<DomainFault, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<Fault>(), specification)
            .FirstOrDefaultAsync(cancellationToken);

    async Task<List<DomainFault>> IReadRepositoryBase<DomainFault>.ListAsync(ISpecification<DomainFault> specification,
        CancellationToken cancellationToken = default)
    {
        var query = await SpecificationEvaluator.Default.GetQuery(this.context.Set<Fault>(), specification)
            .Join(
                this.context.Set<RawAudit>(),
                x => x.MessageId,
                x => x.MessageId,
                (fault, messages) =>
                    new {fault, messages})
            .ToListAsync(cancellationToken);

        var result = query.GroupBy(x => x.fault).Select(x =>
            (DomainFault)new Fault(x.Key.Id, x.Key.MessageId, x.Key.Exceptions, x.Key.Produced, x.Key.ProducedBy, x.Select(y => y.messages).ToArray()));

        return result.ToList();
    }

    Task<List<TResult>> IReadRepositoryBase<DomainFault>.ListAsync<TResult>(
        ISpecification<DomainFault, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<Fault>(), specification)
            .ToListAsync(cancellationToken);

    async Task<DomainFault> IReadRepositoryBase<DomainFault>.GetByIdAsync<TId>(TId id,
        CancellationToken cancellationToken)
        => await this.context.Set<Fault>().FindAsync(new object[] {id}, cancellationToken: cancellationToken);

    async Task<DomainFault> IReadRepositoryBase<DomainFault>.GetBySpecAsync<Spec>(Spec specification,
        CancellationToken cancellationToken)
    {
        var query = await SpecificationEvaluator.Default.GetQuery(this.context.Set<Fault>(), specification)
            .Join(
                this.context.Set<RawAudit>(),
                x => x.MessageId,
                x => x.MessageId,
                (fault, messages) =>
                    new {fault, messages})
            .ToListAsync(cancellationToken);

        var result = query.GroupBy(x => x.fault).Select(x =>
            (DomainFault)new Fault(x.Key.Id, x.Key.MessageId, x.Key.Exceptions, x.Key.Produced, x.Key.ProducedBy, x.Select(y => y.messages).ToArray()));

        return result.First();
    }

    async Task<List<DomainFault>> IReadRepositoryBase<DomainFault>.ListAsync(CancellationToken cancellationToken)
    {
        return (await this.context.Set<Fault>().ToListAsync(cancellationToken)).Cast<DomainFault>().ToList();
    }

    public async Task<DomainFault> AddAsync(DomainFault entity,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var fault = await this.context.Set<Fault>()
            .AddAsync(
                new Fault(entity.MessageId, entity.Exceptions, entity.Produced, entity.ProducedBy,
                    Array.Empty<IRawAuditData>()), cancellationToken);

        await this.context.SaveChangesAsync(cancellationToken);

        return fault.Entity;
    }

    public Task UpdateAsync(DomainFault entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteAsync(DomainFault entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<DomainFault> entities,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }
}