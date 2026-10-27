using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingApp.Application.Data;
using TicketingApp.Application.Repositories;
using TicketingApp.Infrastructure.Data;
using TicketingApp.Infrastructure.Database;
using TicketingApp.Infrastructure.Repositories;

namespace TicketingApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") 
            ?? "Host=localhost;Port=5432;Database=ticketing_db;Username=postgres;Password=postgres;";

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<TicketingApp.Application.Abstractions.Idempotency.IIdempotencyService, TicketingApp.Infrastructure.Idempotency.IdempotencyService>();

        return services;
    }
}
