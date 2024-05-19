using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;

namespace StargateAPI.Tests;
public class GetAstronautDutiesByNameShould : IClassFixture<StargateContextFixture>
{
    private readonly DbContextOptions<StargateContext> _options;

    public GetAstronautDutiesByNameShould(StargateContextFixture fixture)
    {
        _options = fixture.Options;        
    }


    [Fact]
    public async Task ThrowBadRequestWhenNotFound()
    {
        // Arrange                
        var request = new GetAstronautDutiesByName() { Name = string.Empty };
        var context = new StargateContext(_options);
        var handler = new GetAstronautDutiesByNameHandler(context);
        // Act and Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(
            async () => await handler.Handle(request, new CancellationToken()));
    }

    [Fact]
    public async Task ReturnOkWithCorrectPerson()
    {
        // Arrange
        string name = "John Doe";
        var request = new GetAstronautDutiesByName() { Name = name };
        var context = new StargateContext(_options);
        var handler = new GetAstronautDutiesByNameHandler(context);
        // Act
        var result = await handler.Handle(request, new CancellationToken());
        // Assert
        Assert.Equal(200, result.ResponseCode);
        Assert.Equal(name, result.Person.Name);
    }
}
