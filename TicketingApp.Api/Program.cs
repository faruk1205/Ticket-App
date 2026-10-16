using TicketingApp.Application.Repositories;
using TicketingApp.Application.Tickets.ReserveTicket;
using TicketingApp.Infrastructure.Database;
using TicketingApp.Infrastructure.Repositories;
// EF Core DbContext ayarları için gerekli paketlerin API'ye eklendiğini varsayıyoruz.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// COMPOSITION ROOT: Arayüzler ile Gerçek sınıflar burada birbirine bağlanıyor!
// Artık biri Application'da ITicketRepository isterse, ona Infrastructure'daki TicketRepository'i verilecek.
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IUnitOfWork, ApplicationDbContext>();

// Handler'ımızı da DI (Dependency Injection) havuzuna ekliyoruz.
builder.Services.AddScoped<ReserveTicketHandler>();

var app = builder.Build();

app.MapControllers();
app.Run();