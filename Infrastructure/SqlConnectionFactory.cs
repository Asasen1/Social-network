using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure;

public class SqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection() =>
        new NpgsqlConnection(_configuration.GetConnectionString("SocialNetwork"));
}