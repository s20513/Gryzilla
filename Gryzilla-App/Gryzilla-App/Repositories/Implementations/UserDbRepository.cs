using Gryzilla_App.DTOs.Requests.User;
using Gryzilla_App.DTOs.Responses.User;
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
    public async Task<UserDto?> GetUserFromDb(int idUser)
    {
        var user = await _context.UserData
            .Include(x => x.IdRankNavigation)
            .Where(x => x.IdUser == idUser)
            .Select(x => new UserDto 
            {
                IdUser      = x.IdUser,
                IdRank      = x.IdRank,
                Nick        = x.Nick,
                Email       = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreatedAt   = x.CreatedAt,
                RankName    = x.IdRankNavigation.Name
            })
            .SingleOrDefaultAsync();
        
        return user;
    }

    public async Task<IEnumerable<UserDto?>> GetUsersFromDb()
    {
        var users = await _context.UserData
            .Include(x => x.IdRankNavigation)
            .Select(x => new UserDto 
            {
                IdUser      = x.IdUser,
                Nick        = x.Nick,
                Email       = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreatedAt   = x.CreatedAt,
                IdRank      = x.IdRank,
                RankName    = x.IdRankNavigation.Name
            })
            .ToArrayAsync();

        return users;
    }
    
    public async Task<UserDto?> ModifyUserFromDb(int idUser, PutUserDto putUserDto)
    {
        var user = await _context.UserData
            .Where(x => x.IdUser == idUser)
            .Include(x => x.IdRankNavigation)
            .SingleOrDefaultAsync();
        
        if (user is not null)
        {
            user.Email       = putUserDto.Email;
            user.Password    = putUserDto.Password;
            user.Nick        = putUserDto.Nick;
            user.PhoneNumber = putUserDto.PhoneNumber;
            
            await _context.SaveChangesAsync();
            
            return new UserDto() {
                IdUser       = user.IdUser,
                Nick         = user.Nick,
                Email        = user.Email,
                PhoneNumber  = user.PhoneNumber,
                CreatedAt    = user.CreatedAt,
                IdRank       = user.IdRank,
                RankName     = user.IdRankNavigation.Name
            };
        }
        else
        {
            return null;
        }
    }

    private Exception? NameTakenException { get; }
    public async Task<UserDto?> AddUserToDb(AddUserDto addUserDto)
    {
        var nick = await _context.UserData
            .Where(x => x.Nick == addUserDto.Nick)
            .SingleOrDefaultAsync();
        
        if (nick is not null)
        {
            throw NameTakenException!;
        }
        
        var newUser = new UserDatum
        {
            IdRank      = 4, //Standard user
            Nick        = addUserDto.Nick,
            Password    = addUserDto.Password,
            Email       = addUserDto.Email,
            PhoneNumber = addUserDto.PhoneNumber,
            CreatedAt   = DateTime.Now
        };
        
        await _context.UserData.AddAsync(newUser);
        await _context.SaveChangesAsync();

        var newIdUser = _context.UserData
            .Max(x => x.IdUser);
       
        var newUserDto = await _context.UserData
           .Include(x => x.IdRankNavigation)
           .Where(x => x.IdUser == newIdUser)
           .Select(x => new UserDto 
           {
               IdUser      = x.IdUser,
               IdRank      = x.IdRank,
               Nick        = x.Nick,
               Email       = x.Email,
               PhoneNumber = x.PhoneNumber,
               CreatedAt   = x.CreatedAt,
               RankName    = x.IdRankNavigation.Name
           })
           .SingleOrDefaultAsync();

       return newUserDto;
    }
    
    public async Task<UserDto?> DeleteUserFromDb(int idUser)
    {
        var user = await GetUserFromDb(idUser);

        if (user is null)
        {
            return null;
        }
        else
        {
            var deleteUserById = await _context.UserData
                .Where(x => x.IdUser == idUser)
                .SingleOrDefaultAsync();

            if (deleteUserById != null)
            {
                _context.UserData.Remove(deleteUserById);
                await _context.SaveChangesAsync();
            }
            
            return user;
        }
    }
}