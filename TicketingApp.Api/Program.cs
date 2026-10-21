using FluentValidation;
using TicketingApp.Application.Behaviors;
using TicketingApp.Application.Data;
using TicketingApp.Application.Repositories;
using TicketingApp.Application.Tickets.ReserveTicket;
using TicketingApp.Infrastructure.Database;
using TicketingApp.Infrastructure.Data;
using TicketingApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// --- 1. ALTYAPI (INFRASTRUCTURE) KAYITLARI ---
// Yazma işlemleri için EF Core ve Repository (Aşama 6)
builder.Services.AddScoped<IUnitOfWork, ApplicationDbContext>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// Okuma işlemleri için Dapper Fabrikası (Aşama 9)
builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();


// --- 2. UYGULAMA (APPLICATION) KAYITLARI ---
// FluentValidation: Application projesindeki tüm IValidator arayüzlerini bulur ve ekler (Aşama 8)
builder.Services.AddValidatorsFromAssembly(typeof(ReserveTicketCommand).Assembly);

// MediatR: Command/Query Handler'ları ve Pipeline Behavior'ları bulur (Aşama 8)
builder.Services.AddMediatR(cfg => 
{
    // Handler'ları otomatik kaydet
    cfg.RegisterServicesFromAssembly(typeof(ReserveTicketCommand).Assembly);
    
    // Loglama boru hattı (Sistemdeki tüm Request ve Response'ları sarar)
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var app = builder.Build();

app.MapControllers();
app.Run();