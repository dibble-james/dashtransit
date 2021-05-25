// <copyright file="DashTransitContext.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Infrastructure
{
    using System.Linq;
    using System.Threading.Tasks;
    using DashTransit.Core.Domain;
    using DashTransit.Core.Domain.Common;
    using Microsoft.EntityFrameworkCore;

    public class DashTransitContext : DbContext, IUnitOfWork
    {
        public DbSet<Endpoint> Endpoints => this.Set<Endpoint>();

        public DbSet<Message> Messages => this.Set<Message>();

        public DbSet<Fault> Faults => this.Set<Fault>();

        public async Task Save<T, TId>(T entity)
            where T : Aggregate<TId>
            where TId : class
        {
            var dbSet = this.Set<T>();

            if (!dbSet.Local.Any(a => a == entity))
            {
                await dbSet.AddAsync(entity);
            }

            await this.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Hook).Assembly);
        }
    }
}
