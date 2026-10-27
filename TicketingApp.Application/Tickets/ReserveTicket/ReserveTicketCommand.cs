using System;
using MediatR;
using TicketingApp.Application.Abstractions.Idempotency;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Tickets.ReserveTicket;

// IIdempotentCommand<Result> ile bu komutun idempotency boru hattına girmesini sağlıyoruz.
// RequestId varsayılan olarak Guid.Empty gelebilir; bu durumda idempotency pas geçilir.
public record ReserveTicketCommand(Guid TicketId, Guid RequestId = default) : IIdempotentCommand<Result>;