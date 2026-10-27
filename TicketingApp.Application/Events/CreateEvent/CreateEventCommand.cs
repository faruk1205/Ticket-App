using System;
using MediatR;
using TicketingApp.Application.Abstractions.Idempotency;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Events.CreateEvent;

public record CreateEventCommand(
    string Name, 
    string Location, 
    DateTime StartDate, 
    DateTime EndDate,
    Guid RequestId = default) : IIdempotentCommand<Result<Guid>>;
