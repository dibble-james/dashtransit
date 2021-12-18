namespace DashTransit.EntityFramework;

using System.Linq;
using DashTransit.Core.Domain;
using DashTransit.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DashTransitContext : DbContext
{
    public DashTransitContext(DbContextOptions<DashTransitContext> options)
        : base(options)
    {
    }

    public IQueryable<IRawAuditData> Audit => this.Set<RawAudit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DashTransitContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

public class DashTransitContextFactory : IDesignTimeDbContextFactory<DashTransitContext>
{
    public DashTransitContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DashTransitContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;User Id=sa;Password=P@ssword123;Initial Catalog=DashTransit");

        return new DashTransitContext(optionsBuilder.Options);
    }
}