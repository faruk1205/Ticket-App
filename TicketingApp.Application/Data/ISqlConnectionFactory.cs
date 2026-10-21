using System.Data;

namespace TicketingApp.Application.Data;

// Altyapıdan (Infrastructure) bize ham bir SQL bağlantısı vermesini istiyoruz. 
// Dapper doğrudan IDbConnection üzerinde çalışır.
public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}