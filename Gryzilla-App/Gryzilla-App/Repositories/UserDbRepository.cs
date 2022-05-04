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
        // var singleUser = await (from user in _context.UserData
        //     where user.IdUser == idUser
        //     select new UserDto()
        //     {
        //         IdUser = user.IdUser,
        //         IdRank = user.IdRank,
        //         Nick = user.Nick,
        //         Email = user.Email,
        //         PhoneNumber = user.PhoneNumber,
        //         CreatedAt = user.CreatedAt
        //     }).FirstOrDefaultAsync();
        //
        // return singleUser;
        var user = await _context.UserData
            // .Join(Rank,
            //     sc => sc.IdRank,
            //     soc => soc.IdRank,
            //     (sc,soc) => new {sc, soc})
            // .Where(x => x.IdUser == idUser)
            .Select(x => new UserDto
        {
            IdUser = x.IdUser,
            IdRank = x.IdRank,
            Nick = x.Nick,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            CreatedAt = x.CreatedAt
        }).SingleOrDefaultAsync();
        
        return user;
    }

    public async Task<IEnumerable<UserDto?>> GetUsersFromDb()
    {
        var users = await _context.UserData.Select(x => new UserDto() {
            IdUser = x.IdUser,
            IdRank = x.IdRank,
            Nick = x.Nick,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            CreatedAt = x.CreatedAt
        }).ToArrayAsync();

        return users;

    }
    
    
}