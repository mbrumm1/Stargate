using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests;
public class StargateContextFixture : IDisposable
{
    private SqliteConnection _connection;

    public DbContextOptions<StargateContext> Options { get; }

    public StargateContextFixture()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        Options = new DbContextOptionsBuilder<StargateContext>()
            .UseSqlite(_connection)
            .Options;        
        using var context = new StargateContext(Options);
        context.Database.Migrate();        
    }

    public void Dispose() => _connection.Dispose();
}
