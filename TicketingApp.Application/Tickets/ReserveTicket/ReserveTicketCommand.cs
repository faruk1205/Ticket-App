using MediatR;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Tickets.ReserveTicket;

// IRequest<Result> diyerek MediatR'a diyoruz ki: "Bu bir istektir ve geriye Result döner."
// Command'lar (veri değiştiren işlemler) C#'ta record olarak tanımlanır, veri taşırlar.
public record ReserveTicketCommand(Guid TicketId) : IRequest<Result>;