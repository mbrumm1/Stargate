using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Xml.Linq;

namespace StargateAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonController(IMediator mediator)
        {
            _mediator = mediator;            
        }

        [HttpGet("")]
        public async Task<ActionResult<GetPeopleResult>> GetPeople()
        {
            var result = await _mediator.Send(new GetPeople());
            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<GetPersonByNameResult>> GetPersonByName(string name)
        {
            var result = await _mediator.Send(new GetPersonByName() { Name = name });
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<ActionResult<CreatePersonResult>> CreatePerson([Required][FromBody] string name)
        {
            var result = await _mediator.Send(new CreatePerson() { Name = name });
            return Ok(result);
        }

        [HttpPut("{name}")]
        public async Task<ActionResult<UpdatePersonResult>> UpdatePerson(string name, [Required][FromBody] string newName)
        {

            var result = await _mediator.Send(new UpdatePerson()
            {
                Name = name,
                NewName = newName
            });
            return Ok(result);
        }
    }
}