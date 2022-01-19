namespace DashTransit.EntityFramework;

using System;
using Ardalis.Specification;
using DashTransit.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;

public static class Bootstrap
{
    public static void UseDashTransitEntityFramework(
        this IServiceCollection services,
        string provider,
        string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<DashTransitContext>(opt => Builder(opt, provider, connectionString), ServiceLifetime.Transient);
        services.AddTransient<IRepositoryBase<Fault>, FaultRepository>();
        services.AddTransient<IReadRepositoryBase<Fault>, FaultRepository>();
        services.AddTransient<IReadRepositoryBase<IRawAuditData>, AuditRepository>();
        services.AddTransient<IEndpointRepository, EndpointsRepository>();
        services.AddTransient<ICalculateMessageRate, StatisticsRepository>();
    }

    private static DbContextOptionsBuilder Builder(
        DbContextOptionsBuilder builder,
        string provider,
        string connectionString) => provider.ToLowerInvariant() switch
        {
            "postgres" => builder.UseNpgsql(connectionString),
            "sqlserver" => builder.UseSqlServer(connectionString),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, "Invalid storage provider"),
        };
}