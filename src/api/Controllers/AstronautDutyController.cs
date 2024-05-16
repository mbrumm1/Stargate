using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Xml.Linq;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AstronautDutyController> _logger;

        public AstronautDutyController(IMediator mediator, ILogger<AstronautDutyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<GetAstronautDutiesByNameResult>> GetAstronautDutiesByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetAstronautDutiesByName()
                {
                    Name = name
                });
                _logger.LogInformation("Successfully retrieved astronaut duties for {Name}", name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get astronaut duties by name: {Name}", name);
                return StatusCode(500, BaseResponse.InternalServerError(ex.Message));                
            }            
        }

        [HttpPost("")]
        public async Task<ActionResult<CreateAstronautDutyResult>> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
            try
            {
                var result = await _mediator.Send(request);
                _logger.LogInformation("Successfully created astronaut duty {Id}", result.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create astronaut duties for: {Name}", request.Name);
                return StatusCode(500, BaseResponse.InternalServerError(ex.Message));
            }
        }
    }
}