using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AstronautDutyController(IMediator mediator)
        {
            _mediator = mediator;
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

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, BaseResponse.InternalServerError(ex.Message));                
            }            
        }

        [HttpPost("")]
        public async Task<ActionResult<CreateAstronautDutyResult>> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, BaseResponse.InternalServerError(ex.Message));
            }
        }
    }
}