using MediatR;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Tickets.GetTicket;

// DİKKAT: Dönüş tipi "Ticket" (Entity) değil, "TicketResponse" (DTO).
public record GetTicketByIdQuery(Guid TicketId) : IRequest<Result<TicketResponse>>;

public record TicketResponse(Guid Id, Guid EventId, string SeatNumber, bool IsReserved);