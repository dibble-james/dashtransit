// <copyright file="Program.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace FaultGenerator
{
    using System.Threading.Tasks;
    using MassTransit;
    using MassTransit.EntityFrameworkCoreIntegration.Audit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public static class Program
    {
        public static Task Main(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Debug))
                .ConfigureServices((host, services) =>
                {

                    services.AddMassTransit(bus =>
                    {
                        bus.AddConsumer<BillingService>();
                        bus.AddConsumer<ShippingService>();
                        bus.AddConsumer<OrderService>();

                        bus.UsingRabbitMq((context, r) =>
                        {
                            r.UseEntityFrameworkCoreAuditStore(
                                new DbContextOptionsBuilder()
                                    .UseSqlServer("Data Source=localhost;User Id=sa;Password=P@ssword123;Initial Catalog=DashTransit"),
                                "__audit");
                            r.Host("amqp://guest:guest@localhost:5672");
                            r.ReceiveEndpoint("demo-billing", e => e.ConfigureConsumer<BillingService>(context));
                            r.ReceiveEndpoint("demo-shipping", e => e.ConfigureConsumer<ShippingService>(context));
                            r.ReceiveEndpoint("demo-orders", e => e.ConfigureConsumer<OrderService>(context));
                        });
                    });

                    services.AddMassTransitHostedService();
                    ////services.AddHostedService<OrderPlacer>();
                })
                .RunConsoleAsync();
    }
}
