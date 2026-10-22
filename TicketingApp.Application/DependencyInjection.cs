using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TicketingApp.Application.Behaviors;

namespace TicketingApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)     //Program.cs'te çağrılıyor -> builder.Services.AddApplication(); şeklinde
    {
        var assembly = typeof(DependencyInjection).Assembly;     //Assembly, derlenmiş .NET kodunun bulunduğu bir birimdir; genellikle bir .dll dosyasına karşılık gelir.

        services.AddValidatorsFromAssembly(assembly);           //Bu satır, belirtilen assembly'deki FluentValidation validator sınıflarını keşfeder ve DI sistemine kaydeder. CreateTicketCommandValidator.cs gibi

        services.AddMediatR(cfg =>      //AddMediatR(...), MediatR servislerini Dependency Injection sistemine ekler.
        {
            cfg.RegisterServicesFromAssembly(assembly);        //RegisterServicesFromAssembly(assembly), belirtilen assembly'deki MediatR Handler'larını keşfedip kaydeder.
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}

/*Önemli ayrım: Assembly tek başına Reflection değildir. Assembly, incelenecek derlenmiş kod birimidir;
 Reflection ise o kodun tür bilgilerini çalışma zamanında incelemek için kullanılabilen mekanizmadır.*/