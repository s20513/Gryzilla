using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gryzilla_App.DTOs.Requests.User;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Gryzilla_App.Repositories.Implementations;

public class UserDbRepository : IUserDbRepository
{
    private readonly GryzillaContext _context;
    private readonly IConfiguration _configuration;
    public UserDbRepository(GryzillaContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
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
        var nick =  _context
            .UserData
            .Where(x => x.IdUser != idUser)
            .Count(x => x.Nick == putUserDto.Nick);
        
        if (nick > 0)
        {
            throw new SameNameException("Nick with given name already exists!");
        }
        
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

        return null;
    }
    
    public async Task<UserDto?> AddUserToDb(AddUserDto addUserDto)
    {
        var nick = await _context.UserData
            .Where(x => x.Nick == addUserDto.Nick)
            .SingleOrDefaultAsync();
        
        if (nick is not null)
        {
            throw new SameNameException("Nick with given name already exists!");
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

    public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _context.UserData
            .SingleOrDefaultAsync(e => e.Nick == loginRequestDto.Nick 
                                       && e.Password == loginRequestDto.Password);

        if (user is null)
        {
            return null;
        }

        //dodać sprawdzanie hasła i sprawdzanie wygaśnięcia tokena6

        var token = CreateToken(user.Nick);
        user.Token = token;

        await _context.SaveChangesAsync();

        return new LoginResponseDto
        {
            Id = user.IdUser,
            Nick = user.Nick,
            Token = token
        };
    }

    private string CreateToken(string nick)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

        var description = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, nick),
                }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(description);

        return handler.WriteToken(token);
    }
}