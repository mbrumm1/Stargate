using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Net;
using System.Xml.Linq;

namespace StargateAPI.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IMediator mediator, ILogger<PersonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<GetPeopleResult>> GetPeople()
        {
            try
            {
                var result = await _mediator.Send(new GetPeople());
                _logger.LogInformation("Successfully retrieved all people");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all people");
                return StatusCode(500, BaseResponse.InternalServerError(ex.Message));
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<GetPersonByNameResult>> GetPersonByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetPersonByName() { Name = name });
                _logger.LogInformation("Successfully retrieved person: {Name}", name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get person by name: {Name}", name);
                return StatusCode(500, BaseResponse.InternalServerError(ex.Message));
            }
        }

        [HttpPost("")]
        public async Task<ActionResult<CreatePersonResult>> CreatePerson([FromBody] string name)
        {
            try
            {
                var result = await _mediator.Send(new CreatePerson() { Name = name });
                _logger.LogInformation("Successfully created person named {Name} (ID: {Id})", name, result.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create person by name: {Name}", name);
                return BadRequest(BaseResponse.InternalServerError(ex.Message));
            }
        }

        [HttpPut("{name}")]
        public async Task<ActionResult<UpdatePersonResult>> UpdatePerson(string name, [FromBody] string newName)
        {
            try
            {
                var result = await _mediator.Send(new UpdatePerson()
                {
                    Name = name,
                    NewName = newName
                });
                _logger.LogInformation("Successfully updated person from namer {Name}, to {NewName}", name, newName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update person by name: from {Name}, to {NewName}", name, newName);
                return StatusCode(500, BaseResponse.InternalServerError(ex.Message));
            }
        }
    }
}