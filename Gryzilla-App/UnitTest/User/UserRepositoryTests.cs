using System.Drawing;
using System.Security.Claims;
using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTest.User;

public class UserRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly UserDbRepository _repository;
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public UserRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        var configuration = new ConfigurationManager();
        configuration["JWT:Key"] = "9fXc7R2GhJ1tLz3Q";

        _context = new GryzillaContext(options, true);
        _repository = new UserDbRepository(_context, configuration);
        
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Role, "User"),
        };
        _mockClaimsPrincipal.Setup(x => x.Claims).Returns(claims);
        _mockClaimsPrincipal
            .Setup(x => x.FindFirst(It.IsAny<string>()))
            .Returns<string>(claimType => claims.FirstOrDefault(c => c.Type == claimType));
    }

    private async Task AddTestDataWithOneUser()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "Rank1",
            RankLevel = 1
        });
        await _context.SaveChangesAsync();
        
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "Moderator",
            RankLevel = 2
        });
        await _context.SaveChangesAsync();

        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "Admin",
            RankLevel = 3
        });
        await _context.SaveChangesAsync();
        
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "User",
            RankLevel = 4
        });
        await _context.SaveChangesAsync();
        
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick1",
            Password = "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3",
            Email = "email1",
            CreatedAt = DateTime.Today,
            RefreshToken = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            TokenExpire = new DateTime(3000, 1, 1)
        });
        await _context.SaveChangesAsync();
    }
    
    private async Task AddTestDataWithManyUser()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "Rank1",
            RankLevel = 1
        });
        await _context.SaveChangesAsync();

        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick1",
            Password = "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
        
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
            Password = "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
    }

    private async Task AddPhotoToUser(int idUser)
    {
        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == idUser);

        var desktopPath = Directory.GetCurrentDirectory();
        var imagePath = Path.Combine(desktopPath, @"..\..\..\Images\test.png");
        var image = Image.FromFile(imagePath);
        
        byte[] byteArray;
        using MemoryStream stream = new MemoryStream();
        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        byteArray = stream.ToArray();

        user.Photo = byteArray;
        user.PhotoType = "png";
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task DeleteUserFromDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteUserFromDb(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);

        var users = _context.UserData.ToList();
        Assert.False(users.Exists(e => e.IdUser== res.IdUser));
    }
    
    [Fact]
    public async Task DeleteUserFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteUserFromDb(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    
    [Fact]
    public async Task ModifyUserFromDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;

        var putUserFromDb = new PutUserDto
        {
            IdUser = 1,
            Nick = "Poziomka",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        //Act
        var res = await _repository.ModifyUserFromDb(idUser, putUserFromDb, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        var users = _context.UserData.ToList();
        Assert.True(users.Exists(e => e.Nick == res.Nick));
    }
    
    [Fact]
    public async Task ModifyUserFromDb_Returns_SameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyUser();

        var idUser = 1;

        var putUserFromDb = new PutUserDto
        {
            IdUser = 1,
            Nick = "Nick2",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        
        //Act
        //Assert
        await Assert.ThrowsAsync<SameNameException>(() => _repository.ModifyUserFromDb(idUser, putUserFromDb, _mockClaimsPrincipal.Object));
    }
    
    [Fact]
    public async Task ModifyUserFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        var idUser = 1;
        
        var putUserFromDb = new PutUserDto
        {
            IdUser = 1,
            Nick = "Poziomka",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        
        //Act
        var res = await _repository.ModifyUserFromDb(idUser, putUserFromDb, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddUserToDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;

        var addUserToDb = new AddUserDto
        {
            Nick = "Poziomka",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        //Act
        var res = await _repository.AddUserToDb(addUserToDb);
        
        //Assert
        Assert.NotNull(res);
        var users = _context.UserData.ToList();
        Assert.True(users.Exists(e => e.Nick == res.Nick));
    }
    
    [Fact]
    public async Task AddUserToDb_Returns_SameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var addUserToDb = new AddUserDto
        {
            Nick = "Nick1",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        
        //Act
        //Assert
        await Assert.ThrowsAsync<SameNameException>(() => _repository.AddUserToDb(addUserToDb));
    }
    
    [Fact]
    public async Task GetUserFromDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;

        //Act
        var res = await _repository.GetUserFromDb(idUser);
        
        //Assert
        Assert.NotNull(res);
        
        var users = _context.UserData.ToList();
        Assert.Single(users);
        
        var user = users.SingleOrDefault(e => e.IdUser == res.IdUser);
        Assert.NotNull(user);
    }
    
    [Fact]
    public async Task GetUserFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        var idUser = 1;

        //Act
        var res = await _repository.GetUserFromDb(idUser);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetUsersFromDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;

        //Act
        var res = await _repository.GetUsersFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var users = await _context.UserData.Select(e => e.IdUser).ToListAsync();
        Assert.Equal(users, res.Select(e => e.IdUser));
    }
    
    [Fact]
    public async Task GetUsersFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        //Act
        var res = await _repository.GetUsersFromDb();
        
        //Assert
        Assert.Empty(res);
    }
    
    [Fact]
    public async Task ChangeUserRank_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var userRank = new UserRank
        {
            IdUser = 1,
            IdRank = 2
        };

        //Act
        var res = await _repository.ChangeUserRank(userRank);
        
        //Assert
        Assert.NotNull(res);

        var userRankId = _context.UserData
            .Where(e => e.IdUser == userRank.IdUser)
            .Select(e => e.IdRank)
            .SingleOrDefault();
        
        Assert.True(userRankId == userRank.IdRank);
    }
    
    [Fact]
    public async Task ChangeUserRank_WithNoExistingRankWithGivenId_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var userRank = new UserRank
        {
            IdUser = 1,
            IdRank = 20
        };

        //Act
        var res = await _repository.ChangeUserRank(userRank);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ChangeUserRank_WithNoExistingUserWithGivenId_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var userRank = new UserRank
        {
            IdUser = 100,
            IdRank = 2
        };

        //Act
        var res = await _repository.ChangeUserRank(userRank);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ChangePassword_WithCheckingOldPassword_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 100;
        var changePasswordDto = new ChangePasswordDto();

        //Act
        var res = await _repository.ChangePassword(changePasswordDto, idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ChangePassword_WithCheckingOldPassword_Returns_false()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        var changePasswordDto = new ChangePasswordDto
        {
            OldPassword = "1234"
        };

        //Act
        var res = await _repository.ChangePassword(changePasswordDto, idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.False(res);
    }
    
    [Fact]
    public async Task ChangePassword_WithCheckingOldPassword_Returns_true()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        var changePasswordDto = new ChangePasswordDto
        {
            OldPassword = "123",
            NewPassword = "321"
        };

        //Act
        var res = await _repository.ChangePassword(changePasswordDto, idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.True(res);
    }
    
    [Fact]
    public async Task ChangePassword_WithoutCheckingOldPassword_Returns_false()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 100;
        var changePasswordShortDto = new ChangePasswordShortDto();

        //Act
        var res = await _repository.ChangePassword(changePasswordShortDto, idUser);
        
        //Assert
        Assert.False(res);
    }
    
    [Fact]
    public async Task ChangePassword_WithoutCheckingOldPassword_Returns_true()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        var changePasswordShortDto = new ChangePasswordShortDto
        {
            NewPassword = "321"
        };

        //Act
        var res = await _repository.ChangePassword(changePasswordShortDto, idUser);
        
        //Assert
        Assert.True(res);
    }
    
    [Fact]
    public async Task GetUserPhoto_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;

        //Act
        var res = await _repository.GetUserPhoto(idUser);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetUserPhoto_Returns_UserPhotoResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var idUser = 1;

        await AddPhotoToUser(idUser);
        
        //Act
        var res = await _repository.GetUserPhoto(idUser);
        
        //Assert
        Assert.NotNull(res);
    }
    
    [Fact]
    public async Task SetUserPhoto_WithNoExistingUser_Returns_null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var idUser = 100;

        var fileMock = new Mock<IFormFile>();

        //Act
        var res = await _repository.SetUserPhoto(fileMock.Object,idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task SetUserPhoto_WithNullPhoto_Returns_null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var idUser = 1;

        var fileMock = new Mock<IFormFile>();

        //Act
        var res = await _repository.SetUserPhoto(fileMock.Object,idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task SetUserPhoto_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var idUser = 1;
        
        var currentDirectory = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(currentDirectory, @"..\..\..\Images\test.png");
        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        
        IFormFile formFile = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(filePath));

        //Act
        var res = await _repository.SetUserPhoto(formFile,idUser, _mockClaimsPrincipal.Object);
        
        await fileStream.DisposeAsync();
        
        //Assert
        Assert.NotNull(res);
    }
    
    [Fact]
    public async Task CheckNickExist_Returns_ExistNickDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var checkNickDto = new CheckNickDto
        {
            Nick = "Nick1"
        };

        //Act
        var res = await _repository.ExistUserNick(checkNickDto.Nick);
        
        //Assert
        Assert.NotNull(res);
    }
    
    [Fact]
    public async Task Login_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var loginRequestDto = new LoginRequestDto
        {
            Nick = "IDontExist"
        };

        //Act
        var res = await _repository.Login(loginRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task Login_WithIncorrectPassword_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var loginRequestDto = new LoginRequestDto
        {
            Nick = "Nick1",
            Password = "WrongPassword"
        };

        //Act
        var res = await _repository.Login(loginRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task Login_Returns_TokenResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var loginRequestDto = new LoginRequestDto
        {
            Nick = "Nick1",
            Password = "123"
        };

        //Act
        var res = await _repository.Login(loginRequestDto);
        
        //Assert
        Assert.NotNull(res);
    }
    
    [Fact]
    public async Task RefreshToken_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var refreshTokenDto = new RefreshTokenDto
        {
            RefreshToken = "NotExistingRefreshToken"
        };

        //Act
        var res = await _repository.RefreshToken(refreshTokenDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task RefreshToken_Returns_TokenResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        var refreshTokenDto = new RefreshTokenDto
        {
            RefreshToken = "11111111-1111-1111-1111-111111111111"
        };

        //Act
        var res = await _repository.RefreshToken(refreshTokenDto);
        
        //Assert
        Assert.NotNull(res);
    }
}