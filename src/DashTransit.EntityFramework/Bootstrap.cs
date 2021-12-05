namespace DashTransit.EntityFramework;

using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class Bootstrap
{
    public static void UseDashTransitEntityFramework(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DashTransitContext>(opt => opt.UseSqlServer(connectionString));
        services.AddTransient(typeof(IReadRepositoryBase<>), typeof(Repository<>));
    }
}