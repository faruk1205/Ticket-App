using System;
using MediatR;
using TicketingApp.Application.Repositories;
using TicketingApp.Domain.Entities;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Events.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Result<Guid>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var eventId = Guid.NewGuid();

        var eventCreateResult = Event.Create(
            eventId,
            request.Name,
            request.Location,
            request.StartDate,
            request.EndDate);

        if (eventCreateResult.IsFailure)
        {
            return Result.Failure<Guid>(eventCreateResult.Error);
        }

        _eventRepository.Add(eventCreateResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(eventId);
    }
}
