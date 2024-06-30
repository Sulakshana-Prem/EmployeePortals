using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DBConfig
{
    private readonly IConfiguration _configuration; 

    public DBConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

  
    public IDbConnection CreateSQLServerConnection()
    {
        var connectionString = _configuration.GetConnectionString("SQLServerConnection");
        return new SqlConnection(connectionString);
    }
}