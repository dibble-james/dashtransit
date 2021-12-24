// <copyright file="OrderPlacer.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace FaultGenerator
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Hosting;

    public record PlaceOrder(Guid Id, DateTimeOffset Placed, double Amount);

    public class OrderPlacer : BackgroundService
    {
        private readonly IBus bus;

        public OrderPlacer(IBus bus) => this.bus = bus;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var random = new Random();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(random.Next(250, 5000), stoppingToken);

                await this.bus.Publish(new PlaceOrder(Guid.NewGuid(), DateTimeOffset.Now, random.NextDouble() * random.Next(10, 500)));
            }
        }
    }
}
