// <copyright file="OrderService.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace FaultGenerator
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using static Failure;

    public class OrderService : IConsumer<PlaceOrder>, IConsumer<ShippingService.OrderShipped>
    {
        public record OrderPlaced(Guid Id);

        public Task Consume(ConsumeContext<PlaceOrder> context) => WithFailureRate(0.1)(() => context.Publish(new OrderPlaced(context.Message.Id)));

        public Task Consume(ConsumeContext<ShippingService.OrderShipped> context) => WithFailureRate(0.2)(() => Task.CompletedTask);
    }
}
