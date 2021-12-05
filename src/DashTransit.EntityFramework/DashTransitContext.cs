using MassTransit.EntityFrameworkCoreIntegration.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DashTransit.EntityFramework;

public class DashTransitContext : DbContext
{
    public DashTransitContext(DbContextOptions<DashTransitContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("mt");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DashTransitContext).Assembly);
        modelBuilder.ApplyConfiguration(new AuditMapping("__audit"));

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