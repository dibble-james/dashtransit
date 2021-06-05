// <copyright file="Bootstrap.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core
{
    using System;
    using DashTransit.Core.Application;
    using DashTransit.Core.Domain.Common;
    using DashTransit.Core.Infrastructure;
    using GreenPipes;
    using MassTransit;
    using MassTransit.ExtensionsDependencyInjectionIntegration;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class Bootstrap
    {
        public static void AddDashTransit(this IServiceCollection services, string storageConnectionString)
        {
            services.AddMediatR(typeof(Hook).Assembly);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(DbConnectionFactory.Open(storageConnectionString));
            services.Scan(scanner => scanner.FromAssemblyOf<Hook>()
                .AddClasses(c => c.InNamespaces("DashTransit.Core.Infrastructure", "DashTransit.Core.Domain.Services"))
                .AsSelf()
                .AsImplementedInterfaces());
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
                endpoint.UseMessageRetry(retry => retry.Incremental(5, TimeSpan.FromSeconds(0.1), TimeSpan.FromMilliseconds(0.5)));
                endpoint.ConfigureConsumer<FaultHandler>(context);
                endpoint.UseConsumeFilter(typeof(TransactionFilter<>), context);
            });
        }
    }
}
