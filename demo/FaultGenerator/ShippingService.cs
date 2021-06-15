// <copyright file="ShippingService.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace FaultGenerator
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using static Failure;

    public class ShippingService : IConsumer<OrderService.OrderPlaced>, IConsumer<BillingService.InvoicePaid>
    {
        public record OrderShipped(Guid Id);

        public Task Consume(ConsumeContext<OrderService.OrderPlaced> context) => WithFailureRate(0.2)(() => Task.CompletedTask);

        public Task Consume(ConsumeContext<BillingService.InvoicePaid> context) => WithFailureRate(0.5)(() => context.Publish(new OrderShipped(context.Message.Id)));
    }
}
