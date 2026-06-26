using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Infrastructure.Persistence;
using PeoplePortal.Infrastructure.Persistence.Repositories;

namespace PeoplePortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<PeoplePortalDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IHrRequestRepository, HrRequestRepository>();

        return services;
    }
}