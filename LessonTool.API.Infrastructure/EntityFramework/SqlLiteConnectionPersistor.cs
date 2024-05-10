using Microsoft.Data.Sqlite;

namespace LessonTool.API.Infrastructure.EntityFramework;

/// <summary>
/// Holds a connection open with the in memory database to prevent data wiping between requests
/// </summary>
public class SqlLiteConnectionPersistor : IDisposable
{
    private readonly SqliteConnection _connection;

    public SqlLiteConnectionPersistor()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
