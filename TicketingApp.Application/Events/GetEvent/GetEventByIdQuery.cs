using System;
using MediatR;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Events.GetEvent;

public record GetEventByIdQuery(Guid EventId) : IRequest<Result<EventResponse>>;

public record EventResponse(
    Guid Id, 
    string Name, 
    string Location, 
    DateTime StartDate, 
    DateTime EndDate, 
    bool IsActive);
