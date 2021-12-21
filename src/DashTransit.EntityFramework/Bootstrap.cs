namespace DashTransit.EntityFramework;

using Ardalis.Specification;
using DashTransit.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;

public static class Bootstrap
{
    public static void UseDashTransitEntityFramework(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DashTransitContext>(opt => opt.UseSqlServer(connectionString));
        services.AddTransient<IRepositoryBase<Fault>, FaultRepository>();
        services.AddTransient<IReadRepositoryBase<Fault>, FaultRepository>();
        services.AddTransient<IReadRepositoryBase<IRawAuditData>, AuditRepository>();
        services.AddTransient<IEndpointRepository, EndpointsRepository>();
    }
}