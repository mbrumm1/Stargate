using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests;
public class CreateAstronautDutyShould : IClassFixture<StargateContextFixture>
{
    private readonly DbContextOptions<StargateContext> _options;    

    public CreateAstronautDutyShould(StargateContextFixture fixture)
    {
        _options = fixture.Options;        
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
        var context = new StargateContext(_options);
        var handler = new CreateAstronautDutyHandler(context);
        // Act and Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(
            async () => await handler.Handle(request, new CancellationToken()));
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
        var context = new StargateContext(_options);
        var handler = new CreateAstronautDutyHandler(context);
        // Act
        var result = await handler.Handle(request, new CancellationToken());
        var newDuty = await context.AstronautDuties
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == result.Id);
        Assert.NotNull(newDuty);
        Assert.Equal(request.DutyTitle, newDuty.DutyTitle);
        Assert.Equal(request.Rank, newDuty.Rank);
        Assert.Equal(request.DutyStartDate, newDuty.DutyStartDate);
        Assert.Null(newDuty.DutyEndDate);
    }

    [Fact]
    public async Task HaveValidCareerAfterRetirement()
    {
        // Arrange
        var request = new CreateAstronautDuty()
        {
            Name = "John Doe",
            DutyTitle = "RETIRED",
            Rank = "Testing",
            DutyStartDate = DateTime.Now.AddDays(1).Date
        };
        var context = new StargateContext(_options);
        var handler = new CreateAstronautDutyHandler(context);
        // Act
        var result = await handler.Handle(request, new CancellationToken());
        var astronautDetail = await context.AstronautDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Person.Name == request.Name);
        Assert.NotNull(astronautDetail);
        Assert.Equal(request.DutyStartDate.AddDays(-1), astronautDetail.CareerEndDate);
        Assert.Equal(SpecialDutyTitles.Retired, request.DutyTitle);
    }
}
