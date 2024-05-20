using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using System.ComponentModel.DataAnnotations;

namespace StargateAPI.Business.Commands
{
    public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>
    {
        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(32)]
        public required string Rank { get; set; }

        [Required]
        [MaxLength(255)]
        public required string DutyTitle { get; set; }

        [Required]
        public DateTime DutyStartDate { get; set; }
    }

    public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyPreProcessor(StargateContext context)
        {
            _context = context;
        }

        public async Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var person = await _context.People
                .AsNoTracking()
                .FirstOrDefaultAsync(z => z.Name == request.Name);

            if (person is null) throw new BadHttpRequestException($"Person with name \"{request.Name}\" not found.");

            // Update to check for that specific person.
            var verifyNoPreviousDuty = await _context.AstronautDuties
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.PersonId == person.Id &&
                    x.DutyTitle == request.DutyTitle && 
                    x.DutyStartDate == request.DutyStartDate);

            if (verifyNoPreviousDuty is not null)
            {
                throw new BadHttpRequestException("Duty, {request.DutyTitle}, starting at {request.DutyStartDate} already exists.");
            }

            // Verify that new starting date is greater than or equal to their last duty start date.
            var lastDuty = await _context.AstronautDuties
                .AsNoTracking()
                .OrderByDescending(x => x.DutyStartDate)
                .FirstOrDefaultAsync(x => x.PersonId == person.Id);

            if (lastDuty is not null && request.DutyStartDate < lastDuty.DutyStartDate)
            {                
                throw new BadHttpRequestException("New duties must have a start date greater than or equal to the most recent duty.");                
            }

            await Task.CompletedTask;
        }
    }

    public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
        {

           // Removed interpolated string that allowed for SQL injection attacks.
           var query = "SELECT Id, Name FROM [Person] WHERE Name = @Name";           
           var person = await _context.Connection.QueryFirstOrDefaultAsync<Person>(query, new { request.Name });

            if (person is null)
            {
                throw new BadHttpRequestException($"Person with name \"{request.Name}\" not found.");
            }

            query = "SELECT Id, PersonId, CurrentRank, CurrentDutyTitle, CareerStartDate, CareerEndDate FROM [AstronautDetail] WHERE PersonId = @Id";
            var astronautDetail = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDetail>(query, new { person.Id });
            
            if (astronautDetail is null)
            {
                astronautDetail = new AstronautDetail
                {
                    PersonId = person.Id,
                    CurrentDutyTitle = request.DutyTitle,
                    CurrentRank = request.Rank,
                    CareerStartDate = request.DutyStartDate.Date
                };

                if (request.DutyTitle == SpecialDutyTitles.Retired)
                {
                    // TODO: If a person goes straight into retirement, should they even be considered an astronaut?
                    // Should their career end date still be one day before the retired duty start date?
                    astronautDetail.CareerEndDate = request.DutyStartDate.Date;
                }

                _context.AstronautDetails.Add(astronautDetail);
            }
            else
            {
                astronautDetail.CurrentDutyTitle = request.DutyTitle;
                astronautDetail.CurrentRank = request.Rank;
                if (request.DutyTitle == SpecialDutyTitles.Retired)
                {
                    astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                }
                _context.AstronautDetails.Update(astronautDetail);
            }

            query = $"""
                SELECT 
                    Id, 
                    PersonId, 
                    Rank, 
                    DutyTitle, 
                    DutyStartDate, 
                    DutyEndDate 
                FROM [AstronautDuty] 
                WHERE PersonId = @Id
                    AND DutyEndDate IS NULL
                ORDER BY DutyStartDate DESC
                LIMIT 1;
                """;

            var astronautDuty = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDuty>(query, new { person.Id });

            if (astronautDuty != null)
            {
                astronautDuty.DutyEndDate = request.DutyStartDate.AddDays(-1).Date;
                _context.AstronautDuties.Update(astronautDuty);
            }

            var newAstronautDuty = new AstronautDuty()
            {
                PersonId = person.Id,
                Rank = request.Rank,
                DutyTitle = request.DutyTitle,
                DutyStartDate = request.DutyStartDate.Date,
                DutyEndDate = null
            };

            _context.AstronautDuties.Add(newAstronautDuty);
            await _context.SaveChangesAsync();

            return new CreateAstronautDutyResult()
            {
                Id = newAstronautDuty.Id
            };
        }
    }

    public class CreateAstronautDutyResult : BaseResponse
    {
        public int? Id { get; set; }
    }
}
