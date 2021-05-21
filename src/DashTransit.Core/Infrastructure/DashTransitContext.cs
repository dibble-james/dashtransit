// <copyright file="DashTransitContext.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Infrastructure
{
    using System.Linq;
    using System.Threading.Tasks;
    using DashTransit.Core.Application.Common;
    using DashTransit.Core.Domain;
    using DashTransit.Core.Domain.Common;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public class DashTransitContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction? transaction;

        public DbSet<Endpoint> Endpoints => this.Set<Endpoint>();

        public DbSet<Message> Messages => this.Set<Message>();

        public DbSet<Fault> Faults => this.Set<Fault>();

        public async Task Save<T, TId>(T entity)
            where T : Aggregate<TId>
            where TId : class
        {
            this.transaction ??= this.Database.BeginTransaction();

            var dbSet = this.Set<T>();

            if (!dbSet.Local.Any(a => a == entity))
            {
                await dbSet.AddAsync(entity);
            }

            await this.SaveChangesAsync();
        }

        public async Task Commit()
        {
            if (this.transaction is null)
            {
                return;
            }

            try
            {
                await this.transaction.CommitAsync();
            }
            catch
            {
                await this.transaction.RollbackAsync();
                throw;
            }
            finally
            {
                this.transaction = null;
            };
        }

        public async override ValueTask DisposeAsync()
        {
            if (this.transaction is object)
            {
                await this.transaction.RollbackAsync();
            }

            await base.DisposeAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Hook).Assembly);
        }
    }
}
