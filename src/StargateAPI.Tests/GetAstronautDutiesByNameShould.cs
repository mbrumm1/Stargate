using Microsoft.AspNetCore.Http;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;

namespace StargateAPI.Tests;
public class GetAstronautDutiesByNameShould : IClassFixture<StargateContextFixture>
{
    private readonly StargateContext _context;
    private readonly GetAstronautDutiesByNameHandler _handler;

    public GetAstronautDutiesByNameShould(StargateContextFixture fixture)
    {
        _context = fixture.Context;
        _handler = new GetAstronautDutiesByNameHandler(_context);
    }


    [Fact]
    public async Task ThrowBadRequestWhenNotFound()
    {
        // Arrange                
        var request = new GetAstronautDutiesByName() { Name = string.Empty };
        // Act and Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(
            async () => await _handler.Handle(request, new CancellationToken()));
    }

    [Fact]
    public async Task ReturnOkWithCorrectPerson()
    {
        // Arrange
        string name = "John Doe";
        var request = new GetAstronautDutiesByName() { Name = name };
        // Act
        var result = await _handler.Handle(request, new CancellationToken());
        // Assert
        Assert.Equal(200, result.ResponseCode);
        Assert.Equal(name, result.Person.Name);
    }
}
