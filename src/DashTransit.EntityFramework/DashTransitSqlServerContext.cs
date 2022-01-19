// <copyright file="DashTransitSqlServerContext.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DashTransitSqlServerContext : DashTransitContext
{
    public DashTransitSqlServerContext(DbContextOptions<DashTransitContext> options)
        : base(options)
    {
    }
}

public class DashTransitSqlServerContextFactory : IDesignTimeDbContextFactory<DashTransitSqlServerContext>
{
    public DashTransitSqlServerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DashTransitContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;User Id=sa;Password=P@ssword123;Initial Catalog=DashTransit");

        return new DashTransitSqlServerContext(optionsBuilder.Options);
    }
}
