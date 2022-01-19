// <copyright file="DashTransitPostgresContext.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DashTransitPostgresContext : DashTransitContext
{
    public DashTransitPostgresContext(DbContextOptions<DashTransitContext> options)
        : base(options)
    {
    }
}

public class DashTransitContextFactory : IDesignTimeDbContextFactory<DashTransitPostgresContext>
{
    public DashTransitPostgresContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DashTransitContext>();
        optionsBuilder.UseNpgsql("User ID=sa;Password=P@ssword123;Host=localhost;Port=5432;Database=dashtransit;");

        return new DashTransitPostgresContext(optionsBuilder.Options);
    }
}
