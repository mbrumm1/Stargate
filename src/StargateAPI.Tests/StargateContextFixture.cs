using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests;
public class StargateContextFixture : IDisposable
{    
    public DbContextOptions<StargateContext> Options { get; }

    public StargateContextFixture()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        Options = new DbContextOptionsBuilder<StargateContext>()
            .UseSqlite(connection)
            .Options;        
        using var context = new StargateContext(Options);
        context.Database.Migrate();        
    }

    public void Dispose()
    {        
    }
}
