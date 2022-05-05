using Gryzilla_App.DTO.Requests.User;
using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Posts;
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

    public async Task<UserDto?> GetUserFromDb( int idUser)
    {
        var user = await _context.UserData
            .Include(x => x.IdRankNavigation)
            .Where(x => x.IdUser == idUser)
            .Select(x => new UserDto 
            {
                IdUser = x.IdUser,
                IdRank = x.IdRank,
                Nick = x.Nick,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreatedAt = x.CreatedAt,
                RankName = x.IdRankNavigation.Name
        }).SingleOrDefaultAsync();
        
        return user;
    }

    public async Task<IEnumerable<UserDto?>> GetUsersFromDb()
    {
        var users = await _context.UserData
            .Include(x => x.IdRankNavigation)
            .Select(x => new UserDto 
            {
                IdUser = x.IdUser,
                IdRank = x.IdRank,
                Nick = x.Nick,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreatedAt = x.CreatedAt,
                RankName = x.IdRankNavigation.Name
        }).ToArrayAsync();

        return users;
    }
    
    // public async Task<string?> ModifyUserFromDb( PutUserDto putUserDto)
    // {
    //     _context.UserData
    //         .Where(x => x.IdUser == putUserDto.IdUser)
    // }
    
    
}