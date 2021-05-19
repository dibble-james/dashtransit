// <copyright file="DashTransitContext.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Infrastructure
{
    using DashTransit.Core.Domain;
    using Microsoft.EntityFrameworkCore;

    public class DashTransitContext : DbContext
    {
        public DbSet<Endpoint> Endpoints => this.Set<Endpoint>();

        public DbSet<Message> Messages => this.Set<Message>();

        public DbSet<Fault> Faults => this.Set<Fault>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Hook).Assembly);
        }
    }
}
