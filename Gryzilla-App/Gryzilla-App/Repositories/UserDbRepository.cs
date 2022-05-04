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

    public async Task<UserDto?> GetUserFromDb( int idUser)
    {
        var user = await _context.UserData
            .Join(_context.Ranks,
                sc => sc.IdRank,
                soc => soc.IdRank,
                (sc,soc) => new {sc, soc})
            .Where(x => x.sc.IdUser == idUser)
            .Select(x => new UserDto 
            {
                IdUser = x.sc.IdUser,
                IdRank = x.sc.IdRank,
                Nick = x.sc.Nick,
                Email = x.sc.Email,
                PhoneNumber = x.sc.PhoneNumber,
                CreatedAt = x.sc.CreatedAt,
                RankName = x.soc.Name
        }).SingleOrDefaultAsync();
        
        return user;
    }

    public async Task<IEnumerable<UserDto?>> GetUsersFromDb()
    {
        var users = await _context.UserData
            .Join(_context.Ranks,
                sc => sc.IdRank,
                soc => soc.IdRank,
                (sc,soc) => new {sc, soc})
            .Select(x => new UserDto() 
            {
                IdUser = x.sc.IdUser,
                IdRank = x.sc.IdRank,
                Nick = x.sc.Nick,
                Email = x.sc.Email,
                PhoneNumber = x.sc.PhoneNumber,
                CreatedAt = x.sc.CreatedAt,
                RankName = x.soc.Name
        }).ToArrayAsync();

        return users;

    }
    
    
}