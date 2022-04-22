using Gryzilla_App.DTO.Responses;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class UserDbRepository : IUserDbRepository
{
    private readonly GryzillaContext _context;
    public UserDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    public async Task<UserDto> GetUserFromDb( int idUser)
    {
        var user = await _context.UserData.Where(x => x.IdUser == idUser).Select(x => new UserDto
        {
            IdUser = x.IdUser,
            IdRank = x.IdRank,
            Nick = x.Nick,
            Password = x.Password,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            CreatedAt = x.CreatedAt
        }).SingleOrDefaultAsync();
        
        return user;
    }
    
}