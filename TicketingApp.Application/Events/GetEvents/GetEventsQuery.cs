using System.Collections.Generic;
using MediatR;
using TicketingApp.Application.Events.GetEvent;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Events.GetEvents;

public record GetEventsQuery() : IRequest<Result<IReadOnlyList<EventResponse>>>;  //: IRequest<...>: MediatR'a bu sorgunun hangi türde cevap döndüreceğini söyler.
