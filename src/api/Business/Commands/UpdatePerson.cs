using MediatR;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Commands;

public class UpdatePerson : IRequest<UpdatePersonResult>
{
    public string Name { get; set; } = string.Empty;
    public string NewName { get; set; } = string.Empty;
}

public class UpdatePersonHandler : IRequestHandler<UpdatePerson, UpdatePersonResult>
{
    private readonly StargateContext _context;

    public UpdatePersonHandler(StargateContext context)
    {
        _context = context;
    }

    public async Task<UpdatePersonResult> Handle(UpdatePerson request, CancellationToken cancellationToken)
    {
        var person = await _context.People.FirstOrDefaultAsync(x => x.Name == request.Name);

        if (person is null)
        {
            throw new BadHttpRequestException("A person with that name does not exist.");
        }

        var newNamePerson = await _context.People.FirstOrDefaultAsync(x => x.Name == request.NewName);

        if (newNamePerson is not null)
        {
            throw new BadHttpRequestException("A person already has the new name provided.");
        }


        person.Name = request.NewName;
        await _context.SaveChangesAsync();
        return new UpdatePersonResult();
    }
}

public class UpdatePersonResult : BaseResponse { }