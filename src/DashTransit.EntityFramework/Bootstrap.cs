namespace DashTransit.EntityFramework;

using Ardalis.Specification;
using DashTransit.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Queries;

public static class Bootstrap
{
    public static void UseDashTransitEntityFramework(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DashTransitContext>(opt => opt.UseSqlServer(connectionString));
        services.AddTransient(typeof(IReadRepositoryBase<>), typeof(Repository<>));
        services.AddTransient(typeof(IRepositoryBase<>), typeof(Repository<>));
        services.AddTransient<IReadRepositoryBase<IRawAuditData>, Repository<IRawAuditData>>();
        services.AddTransient<IEndpointRepository, EndpointsRepository>();
    }
}