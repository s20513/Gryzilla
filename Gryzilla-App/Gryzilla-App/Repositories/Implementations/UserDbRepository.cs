using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
                RankName    = x.IdRankNavigation.Name,
                LinkDiscord = x.DiscordLink,
                LinkSteam   = x.SteamLink,
                LinkEpic    = x.EpicLink,
                LinkXbox    = x.XboxLink,
                LinkPs      = x.PsLink
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
                RankName    = x.IdRankNavigation.Name,
                LinkDiscord = x.DiscordLink,
                LinkSteam   = x.SteamLink,
                LinkEpic    = x.EpicLink,
                LinkXbox    = x.XboxLink,
                LinkPs      = x.PsLink
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
                RankName     = user.IdRankNavigation.Name,
                LinkDiscord  = user.DiscordLink,
                LinkSteam    = user.SteamLink,
                LinkEpic     = user.EpicLink,
                LinkXbox     = user.XboxLink,
                LinkPs       = user.PsLink
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

        var idRank = await _context.Ranks
            .Where(e => e.Name == "User")
            .Select(e => e.IdRank)
            .SingleOrDefaultAsync();

        if (idRank == 0)
        {
            return null;
        }
        
        var newUser = new UserDatum
        {
            IdRank      = idRank, //Standard user
            Nick        = addUserDto.Nick,
            Password    = HashPassword(addUserDto.Password),
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

    public async Task<TokenResponseDto?> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _context.UserData
            .Include(e => e.IdRankNavigation)
            .SingleOrDefaultAsync(e => e.Nick == loginRequestDto.Nick);

        if (user is null)
        {
            return null;
        }

        if (!CheckPassword(loginRequestDto.Password, user.Password))
        {
            return null;
        }

        var refreshToken = Guid.NewGuid();
        user.RefreshToken = refreshToken;
        user.TokenExpire = DateTime.Now.AddMinutes(30);

        await _context.SaveChangesAsync();

        var token = GenerateToken(user);

        return new TokenResponseDto
        {
            Token = token,
            RefreshToken = refreshToken.ToString()
        };
    }

    public async Task<TokenResponseDto?> RefreshToken(RefreshTokenDto refreshToken)
    {
        var user = await _context.UserData
            .Include(e => e.IdRankNavigation)
            .Where(e => e.RefreshToken.ToString() == refreshToken.RefreshToken)
            .SingleOrDefaultAsync();
        
        if (user is null || 
            user.TokenExpire < DateTime.Now)
        {
            return null;
        }
        
        var newRefreshToken = Guid.NewGuid();
        user.RefreshToken = newRefreshToken;
        user.TokenExpire = DateTime.Now.AddMinutes(30);

        await _context.SaveChangesAsync();

        var token = GenerateToken(user);

        return new TokenResponseDto
        {
            Token = token,
            RefreshToken = newRefreshToken.ToString()
        };
        
    }

    public async Task<UserDto?> ChangeUserRank(UserRank userRank)
    {
        var rank = await _context.Ranks.SingleOrDefaultAsync(x => x.IdRank == userRank.IdRank);

        if (rank is null)
        {
            return null;
        }

        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == userRank.IdUser);

        if (user is null)
        {
            return null;
        }

        user.IdRank = rank.IdRank;
        await _context.SaveChangesAsync();

        return new UserDto
        {
            IdUser = user.IdUser,
            Nick = user.Nick,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IdRank = rank.IdRank,
            RankName = rank.Name,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<UserDto?> SetUserPhoto(IFormFile photo, int idUser)
    {
        var user = await _context.UserData
            .Include(x => x.IdRankNavigation)
            .Where(x => x.IdUser == idUser)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        byte[]? photoBytes = null;
        if (photo.Length > 0)
        {
            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);
            photoBytes = ms.ToArray();
            
            ms.Close();
            await ms.DisposeAsync();
        }

        if (photoBytes is null)
        {
            return null;
        }
        user.Photo = photoBytes;
        user.PhotoType = Path.GetExtension(photo.FileName).Remove(0,1);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            IdUser = user.IdUser,
            Nick = user.Nick,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            RankName = user.IdRankNavigation.Name,
            IdRank = user.IdRank
        };
    }

    public async Task<UserPhotoResponseDto?> GetUserPhoto(int idUser)
    {
        var userPhotoData = await _context.UserData
            .Where(x => x.IdUser == idUser)
            .Select(e => new
            {
                e.Photo,
                e.PhotoType
            })
            .SingleOrDefaultAsync();

        if (userPhotoData?.PhotoType is null || userPhotoData.Photo is null)
        {
            return null;
        }
        
        return new UserPhotoResponseDto
        {
            Type = userPhotoData.PhotoType,
            base64PhotoData = Convert.ToBase64String(userPhotoData.Photo)
        };
    }

    public async Task<bool?> ChangePassword(ChangePasswordDto changePasswordDto, int idUser)
    {
        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == idUser);
        
        if (user is null)
        {
            return null;
        }

        if (!CheckPassword(changePasswordDto.OldPassword, user.Password))
        {
            return false;
        }

        user.Password = HashPassword(changePasswordDto.NewPassword);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ChangePassword(ChangePasswordShortDto changePasswordShortDto, int idUser)
    {
        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == idUser);
        
        if (user is null)
        {
            return false;
        }
        
        user.Password = HashPassword(changePasswordShortDto.NewPassword);
        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerateToken(UserDatum user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

        var description = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                    new(ClaimTypes.Name, user.Nick),
                    new(ClaimTypes.Role, user.IdRankNavigation.Name),
                }),
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(description);

        return handler.WriteToken(token);
    }
    
    private string HashPassword(string password)
    {
        var hash = SHA256.Create();

        var passwordBytes = Encoding.Default.GetBytes(password + _configuration["JWT:Salt"]);

        var hashedPassword = hash.ComputeHash(passwordBytes);

        return Convert.ToHexString(hashedPassword);
    }

    private bool CheckPassword(string givenPassword, string dbHashedPassword)
    {
        var givenPasswordHash = HashPassword(givenPassword);

        return givenPasswordHash == dbHashedPassword;
    }
}