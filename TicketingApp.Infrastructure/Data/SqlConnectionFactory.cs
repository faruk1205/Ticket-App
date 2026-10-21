using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql; // Veritabanı detayı (PostgreSQL) sadece Infrastructure'da!
using TicketingApp.Application.Data;

namespace TicketingApp.Infrastructure.Data;

// Application katmanındaki sözleşmeyi (interface) yerine getiriyoruz.
public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    // API'nin appsettings.json dosyasından connection string'i okumak için IConfiguration kullanıyoruz
    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Database") 
            ?? throw new ArgumentNullException("Connection string bulunamadı!");
    }

    public IDbConnection CreateConnection()
    {
        // PostgreSQL bağlantısını oluştur ve dön. 
        // Application katmanı bunu sadece genel bir IDbConnection olarak görecek.
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        
        return connection;
    }
}