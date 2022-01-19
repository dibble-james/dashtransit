namespace DashTransit.EntityFramework;

using System.Linq;
using DashTransit.Core.Domain;
using DashTransit.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Fault = Entities.Fault;

public class DashTransitContext : DbContext
{
    public DashTransitContext(DbContextOptions<DashTransitContext> options)
        : base(options)
    {
    }

    public IQueryable<IRawAuditData> Audit => this.Set<RawAudit>();

    public IQueryable<Fault> Fault => this.Set<Fault>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DashTransitContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}