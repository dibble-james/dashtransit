// <copyright file="Bootstrap.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core
{
    using System;
    using GreenPipes;
    using MassTransit;
    using MassTransit.ExtensionsDependencyInjectionIntegration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Bootstrap
    {
        public static void AddDashTransit(this IServiceCollection services, string storageConnectionString)
        {
        }

        public static void AddDashTransit(this IServiceCollectionBusConfigurator bus)
        {
            bus.AddConsumers(typeof(Hook).Assembly);
        }

        public static void UseDashTransit(this IBusFactoryConfigurator bus, IBusRegistrationContext context)
        {
            bus.ReceiveEndpoint("dashtransit-faults", endpoint =>
            {
                endpoint.UseInMemoryOutbox();
                endpoint.UseMessageRetry(retry => retry.Incremental(5, TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.5)));
            });
        }
    }
}
