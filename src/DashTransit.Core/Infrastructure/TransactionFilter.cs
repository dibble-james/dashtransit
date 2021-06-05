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
        public void Probe(ProbeContext context)
        {
            context.CreateScope(nameof(TransactionFilter<T>));
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            try
            {
                await next.Send(context);
            }
            catch
            {
                throw;
            }
        }
    }
}
