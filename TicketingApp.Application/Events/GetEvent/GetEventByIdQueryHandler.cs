using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using TicketingApp.Application.Data;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Events.GetEvent;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Result<EventResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetEventByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<EventResponse>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT 
                Id, 
                Name, 
                Location, 
                StartDate, 
                EndDate, 
                IsActive
            FROM Events
            WHERE Id = @EventId
            """;

        var @event = await connection.QueryFirstOrDefaultAsync<EventResponse>(
            sql, 
            new { request.EventId });

        if (@event is null)
        {
            return Result.Failure<EventResponse>(new Error("Event.NotFound", "Etkinlik bulunamadı."));
        }

        return Result.Success(@event);
    }
}
