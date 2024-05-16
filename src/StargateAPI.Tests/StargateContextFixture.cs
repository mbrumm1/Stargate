using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests;
public class StargateContextFixture : IDisposable
{
    public StargateContext Context { get; }

    public StargateContextFixture()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        var optionsBuilder = new DbContextOptionsBuilder<StargateContext>().UseSqlite(connection);
        Context = new StargateContext(optionsBuilder.Options);
        Context.Database.Migrate();
    }

    public void Dispose()
    {
        Context.Dispose();        
    }
}
