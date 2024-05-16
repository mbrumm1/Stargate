using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests;
public class CreateAstronautDutyShould : IClassFixture<StargateContextFixture>
{
    private readonly StargateContext _context;
    private readonly CreateAstronautDutyHandler _handler;

    public CreateAstronautDutyShould(StargateContextFixture fixture)
    {
        _context = fixture.Context;
        _handler = new CreateAstronautDutyHandler(_context);
    }

    [Fact]
    public async Task ThrowBadRequestWhenPersonNotFound()
    {
        // Arrange
        var request = new CreateAstronautDuty()
        {
            Name = string.Empty,
            DutyTitle = "Testing",
            Rank = "Testing",
            DutyStartDate = DateTime.Now.AddDays(1).Date
        };
        // Act and Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(
            async () => await _handler.Handle(request, new CancellationToken()));
    }

    [Fact]
    public async Task ReturnOkWithValidDuty()
    {
        // Arrange
        var request = new CreateAstronautDuty()
        {
            Name = "John Doe",
            DutyTitle = "Testing",
            Rank = "Testing",
            DutyStartDate = DateTime.Now.AddDays(1).Date
        };
        // Act
        var result = await _handler.Handle(request, new CancellationToken());
        var newDuty = await _context.AstronautDuties
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == result.Id);
        Assert.NotNull(newDuty);
        Assert.Equal(request.DutyTitle, newDuty.DutyTitle);
        Assert.Equal(request.Rank, newDuty.Rank);
        Assert.Equal(request.DutyStartDate, newDuty.DutyStartDate);
        Assert.Null(newDuty.DutyEndDate);
    }
}
