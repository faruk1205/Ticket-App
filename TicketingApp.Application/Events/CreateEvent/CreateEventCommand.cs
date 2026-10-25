using System;
using MediatR;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Events.CreateEvent;

public record CreateEventCommand(
    string Name, 
    string Location, 
    DateTime StartDate, 
    DateTime EndDate) : IRequest<Result<Guid>>;     
//IRequest<Result<Guid>>: MediatR'a bu isteğin Result<Guid> türünde bir cevap döndüreceğini söyler.
