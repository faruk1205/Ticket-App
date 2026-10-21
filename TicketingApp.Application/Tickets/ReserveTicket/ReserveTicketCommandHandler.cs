using MediatR;
using TicketingApp.Application.Repositories;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Tickets.ReserveTicket;

// IRequestHandler ile MediatR'a diyoruz ki: "ReserveTicketCommand gelirse bunu ben karşılarım."
public class ReserveTicketCommandHandler : IRequestHandler<ReserveTicketCommand, Result>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReserveTicketCommandHandler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReserveTicketCommand request, CancellationToken cancellationToken)
    {
        // İş mantığımız gram değişmedi! Sadece metoda gelen parametre "request.TicketId" oldu.
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);
        
        if (ticket is null)
            return Result.Failure(new Error("Ticket.NotFound", "Bilet bulunamadı."));

        var reserveResult = ticket.Reserve();

        if (reserveResult.IsFailure)
            return reserveResult;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

/*
using TicketingApp.Application.Repositories;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Tickets.ReserveTicket;

public class ReserveTicketHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    // Interface'leri (sözleşmeleri) istiyoruz. Gerçekte EF Core mu gelecek, Redis mi, bilmiyoruz ve umrumuzda değil!
    public ReserveTicketHandler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        // 1. Veritabanından bileti getir (Repository sözleşmesi ile)
        var ticket = await _ticketRepository.GetByIdAsync(ticketId, cancellationToken);
        
        if (ticket is null)
            return Result.Failure(new Error("Ticket.NotFound", "Bilet bulunamadı."));

        // 2. DOMAIN katmanındaki iş kuralını çalıştır (Bileti rezerve et)
        var reserveResult = ticket.Reserve();

        // Kural ihlal edildiyse işlemi durdur ve hatayı dön (Örn: Zaten rezerveyse)
        if (reserveResult.IsFailure)
            return reserveResult;

        // 3. Değişiklikleri kaydet (UnitOfWork sözleşmesi ile)
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
*/
