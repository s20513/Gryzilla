using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Reason;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class ReasonDbRepository: IReasonDbRepository
{
    private readonly GryzillaContext _context;

    public ReasonDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task<List<FullTagDto>> GetReasonsFromDb()
    {
        var reasons = await _context.Reasons
            .Select(e => new FullTagDto
            {
                Id = e.IdReason,
                Name = e.ReasonName
            }).ToListAsync();

        return reasons;
    }

    public async Task<FullTagDto?> GetReasonFromDb(int idReason)
    {
        var reason = await _context.Reasons
            .Where(e => e.IdReason == idReason)
            .Select(e => new FullTagDto
            {
                Id = e.IdReason,
                Name = e.ReasonName
            }).SingleOrDefaultAsync();

        return reason;
    }

    public async Task<FullTagDto> AddReasonToDb(NewReasonDto newReasonDto)
    {
        var sameNameReason = await _context.Reasons
            .SingleOrDefaultAsync(e => e.ReasonName == newReasonDto.Name);
        
        if (sameNameReason is not null)
        {
            throw new SameNameException("Reason with given name already exists!");
        }

        var newReason = new Reason
        {
            ReasonName = newReasonDto.Name
        };

        await _context.Reasons.AddAsync(newReason);
        await _context.SaveChangesAsync();

        var id = _context.Reasons.Max(e => e.IdReason);

        return new FullTagDto
        {
            Id = id,
            Name = newReasonDto.Name
        };
    }

    public async Task<FullTagDto?> DeleteReasonFromDb(int id)
    {
        var reason = await _context.Reasons.SingleOrDefaultAsync(e => e.IdReason == id);

        if (reason is null)
        {
            return null;
        }

        _context.Reasons.Remove(reason);
        await _context.SaveChangesAsync();

        return new FullTagDto
        {
            Id = reason.IdReason,
            Name = reason.ReasonName
        };
    }
}