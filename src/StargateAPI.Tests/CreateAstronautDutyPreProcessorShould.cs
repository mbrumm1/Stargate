using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests;
public class CreateAstronautDutyPreProcessorShould : IClassFixture<StargateContextFixture>
{
    private readonly DbContextOptions<StargateContext> _options;    

    public CreateAstronautDutyPreProcessorShould(StargateContextFixture fixture)
    {
        _options = fixture.Options;        
    }

    [Fact]
    public async Task ThrowBadRequestWhenPersonNotFound()
    {
        // Arrange        
        var request = new CreateAstronautDuty
        {
            Name = string.Empty,
            Rank = "Testing",
            DutyTitle = "Testing",
            DutyStartDate = DateTime.Now.Date,
        };
        var context = new StargateContext(_options);
        var preProcessor = new CreateAstronautDutyPreProcessor(context);
        // Act and Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(async () => await preProcessor.Process(request, new CancellationToken()));
    }

    [Fact]
    public async Task ThrowBadRequestIfPreviousDutyWithSameTitleAndStartDate()
    {
        // Arrange
        var request = new CreateAstronautDuty
        {
            Name = "John Doe",
            Rank = "1LT",
            DutyTitle = "Commander",
            DutyStartDate = new DateTime(2024, 5, 16, 13, 27, 2, 322, DateTimeKind.Local).AddTicks(2785),
        };
        var context = new StargateContext(_options);
        var preProcessor = new CreateAstronautDutyPreProcessor(context);
        // Act and Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(async () => await preProcessor.Process(request, new CancellationToken()));
    }

    [Fact]
    public async Task ThrowBadRequestIfNewDutyStartDateLessThanLastDutyStartDate()
    {
        // Arrange
        var request = new CreateAstronautDuty
        {
            Name = "John Doe",
            Rank = "1LT",
            DutyTitle = "New Title",
            DutyStartDate = new DateTime(2024, 5, 16).AddDays(-1).Date,
        };
        var context = new StargateContext(_options);
        var preProcessor = new CreateAstronautDutyPreProcessor(context);
        // Act and Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(async () => await preProcessor.Process(request, new CancellationToken()));
    }

    [Fact]
    public async Task NotThrowIfNewDutyStartDateEqualToLastDutyStartDate()
    {
        // Arrange
        var request = new CreateAstronautDuty
        {
            Name = "John Doe",
            Rank = "1LT",
            DutyTitle = "New Title",
            DutyStartDate = new DateTime(2024, 5, 16, 13, 27, 2, 322, DateTimeKind.Local).AddTicks(2785),
        };
        var context = new StargateContext(_options);
        var preProcessor = new CreateAstronautDutyPreProcessor(context);
        // Act 
        var exception = await Record.ExceptionAsync(async () => await preProcessor.Process(request, new CancellationToken()));
        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task NotThrowIfNewDutyStartDateGreaterThanLastDutyStartDate()
    {
        // Arrange
        var request = new CreateAstronautDuty
        {
            Name = "John Doe",
            Rank = "1LT",
            DutyTitle = "New Title",
            DutyStartDate = DateTime.Now.AddDays(50).Date,
        };
        var context = new StargateContext(_options);
        var preProcessor = new CreateAstronautDutyPreProcessor(context);
        // Act 
        var exception = await Record.ExceptionAsync(async () => await preProcessor.Process(request, new CancellationToken()));
        // Assert
        Assert.Null(exception);
    }
}
