// <copyright file="TransactionFilter.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using GreenPipes;
    using MassTransit;

    public class TransactionFilter<T> : IFilter<ConsumeContext<T>>
        where T : class
    {
        private readonly DashTransitContext context;

        public TransactionFilter(DashTransitContext context) => this.context = context;

        public void Probe(ProbeContext context)
        {
            context.CreateScope(nameof(TransactionFilter<T>));
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            using var transaction = await this.context.Database.BeginTransactionAsync(context.CancellationToken);

            try
            {
                await next.Send(context);
                await transaction.CommitAsync(context.CancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(context.CancellationToken);
                throw;
            }
        }
    }
}
