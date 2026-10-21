using Dapper;
using MediatR;
using TicketingApp.Application.Data;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Tickets.GetTicket;

public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, Result<TicketResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    // Burada Repository YOK! Çünkü Repository yazma/domain işlemleri içindir.
    public GetTicketByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<TicketResponse>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Veritabanı bağlantısını aç
        using var connection = _sqlConnectionFactory.CreateConnection();

        // 2. Doğrudan SQL yaz (Dapper'ın gücü)
        const string sql = """
            SELECT 
                Id, 
                EventId, 
                SeatNumber, 
                IsReserved
            FROM Tickets
            WHERE Id = @TicketId
            """;

        // 3. Veriyi doğrudan DTO'ya eşleştir (Mapping kodu yok, automapper yok)
        var ticket = await connection.QueryFirstOrDefaultAsync<TicketResponse>(
            sql, 
            new { request.TicketId });

        if (ticket is null)
            return Result.Failure<TicketResponse>(new Error("Ticket.NotFound", "Bilet bulunamadı."));

        return Result.Success(ticket);
    }
}   