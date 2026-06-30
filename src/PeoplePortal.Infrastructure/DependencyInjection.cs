using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client.Core;
using PeoplePortal.Application.Common.Interfaces;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Infrastructure.Messaging;
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
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
        services.AddScoped<IBenefitRepository, BenefitRepository>();

        var natsUrl = configuration.GetSection("Nats")["Url"] ?? "nats://localhost:4222";

        services.AddSingleton(_ => new NatsConnection(new NatsOpts { Url = natsUrl }));
        services.AddSingleton<IEventBus>(_ => new NatsEventBus(natsUrl));
        services.AddHostedService<EventConsumerService>();

        return services;
    }
}
