using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using TicketingApp.Application.Data;
using TicketingApp.Application.Events.GetEvent;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Events.GetEvents;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, Result<IReadOnlyList<EventResponse>>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetEventsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<EventResponse>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
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
            ORDER BY StartDate ASC
            """;

        var events = await connection.QueryAsync<EventResponse>(sql);

        return Result.Success<IReadOnlyList<EventResponse>>(events.ToList());
    }
}
