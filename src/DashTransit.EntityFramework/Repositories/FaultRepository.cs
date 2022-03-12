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

public class FaultRepository : RepositoryBase<DomainFault>
{
    private readonly DashTransitContext context;

    public FaultRepository(DashTransitContext context)
        : base(context) =>
        this.context = context;

    public override Task<int> CountAsync(ISpecification<DomainFault> specification,
        CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<Fault>(), specification)
            .CountAsync(cancellationToken);

    public override Task<TResult> GetBySpecAsync<TResult>(
        ISpecification<DomainFault, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<Fault>(), specification)
            .FirstAsync(cancellationToken);

    public async override Task<List<DomainFault>> ListAsync(ISpecification<DomainFault> specification,
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

    public override Task<List<TResult>> ListAsync<TResult>(
        ISpecification<DomainFault, TResult> specification, CancellationToken cancellationToken = default)
        => SpecificationEvaluator.Default.GetQuery(this.context.Set<Fault>(), specification)
            .ToListAsync(cancellationToken);

    public async override Task<DomainFault?> GetByIdAsync<TId>(TId id,
        CancellationToken cancellationToken)
        => await this.context.Set<Fault>().FindAsync(new object[] {id}, cancellationToken: cancellationToken);

    public async override Task<DomainFault?> GetBySpecAsync<Spec>(Spec specification,
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

    public async override Task<List<DomainFault>> ListAsync(CancellationToken cancellationToken)
    {
        return (await this.context.Set<Fault>().ToListAsync(cancellationToken)).Cast<DomainFault>().ToList();
    }

    public async override Task<DomainFault> AddAsync(DomainFault entity,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var fault = await this.context.Set<Fault>()
            .AddAsync(
                new Fault(entity.MessageId, entity.Exceptions, entity.Produced, entity.ProducedBy,
                    Array.Empty<IRawAuditData>()), cancellationToken);

        await this.context.SaveChangesAsync(cancellationToken);

        return fault.Entity;
    }

    public override Task UpdateAsync(DomainFault entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public override Task DeleteAsync(DomainFault entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public override Task DeleteRangeAsync(IEnumerable<DomainFault> entities,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }
}