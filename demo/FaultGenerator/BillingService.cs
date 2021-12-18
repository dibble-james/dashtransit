// <copyright file="BillingService.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace FaultGenerator
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using static Failure;

    public class BillingService : IConsumer<OrderService.OrderPlaced>, IConsumer<BillingService.SendInvoice>
    {
        public record SendInvoice(Guid Id);

        public record InvoicePaid(Guid Id, DateTimeOffset At);

        public Task Consume(ConsumeContext<OrderService.OrderPlaced> context) => WithFailureRate(0.1)(async () =>
        {
            await Task.Delay(1000);

            await context.Send(new Uri("queue:demo-billing"), new SendInvoice(context.Message.Id));
        });

        public Task Consume(ConsumeContext<SendInvoice> context) => WithFailureRate(0.5)(async () =>
        {
            await Task.Delay(1000);

            await context.Publish(new InvoicePaid(context.Message.Id, DateTimeOffset.Now));
        });
    }
}
