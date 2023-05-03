using System.Drawing;
using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTest.Groups;

public class GroupRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly GroupDbRepository _repository;

    public GroupRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new GroupDbRepository(_context);
    }

    private async Task AddPhotoToGroup(int idGroup)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(e => e.IdGroup == idGroup);

        var desktopPath = Directory.GetCurrentDirectory();
        var imagePath = Path.Combine(desktopPath, @"..\..\..\Images\test.png");
        var image = Image.FromFile(imagePath);
        
        byte[] byteArray;
        using MemoryStream stream = new MemoryStream();
        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        byteArray = stream.ToArray();

        group.Photo = byteArray;
        group.PhotoType = "png";
        await _context.SaveChangesAsync();
    }
    
    private async Task AddTestDataWithManyGroup()
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
            Password = "Pass1",
            Email = "email1",
            CreatedAt = DateTime.Today
        });

        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
            Password = "Pass1",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
        
        var user = await _context.UserData.FirstAsync();
        
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
            Password = "Pass2",
            Email = "email2",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();

        await _context.Groups.AddAsync(new Group
        {
            GroupName = "test",
            CreatedAt = DateTime.Now,
            IdUserCreator = 2,
            Description = "Nowa grupa"
        });
        await _context.SaveChangesAsync();
        
        var group = await _context.Groups.FirstAsync();


        await _context.GroupUsers.AddAsync(new GroupUser
        {
            IdGroup = group.IdGroup,
            IdUser = user.IdUser
        });
        await _context.SaveChangesAsync();


        await _context.Groups.AddAsync(new Group
        {
            GroupName = "test2",
            CreatedAt = DateTime.Now,
            IdUserCreator = 1,
            Description = "Nowa grupa"
        });
        await _context.SaveChangesAsync();
    }
    
    private async Task AddTestDataWithOneGroup()
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
            Password = "Pass1",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();

        await _context.Groups.AddAsync(new Group
        {
            GroupName = "test",
            CreatedAt = DateTime.Now,
            IdUserCreator = 1,
            Description = "Nowa grupa"
        });
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddNewGroupToDb_Returns_GroupDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyGroup();

        var idUser = 1;
        var newGroupRequestDto = new NewGroupRequestDto()
        {
            IdUser = 1,
            GroupName = "test1",
            Description = "Nowa grupa"
        };
        
        
        //Act
        var res = await _repository.AddNewGroup(idUser, newGroupRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var groups = _context.Groups.ToList();
        Assert.True(groups.Exists(e => e.IdGroup == res.IdGroup));
    }
    
    [Fact]
    public async Task AddNewGroupToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        var newGroupRequestDto = new NewGroupRequestDto()
        {
            IdUser = 1,
            GroupName = "test1",
            Description = "Nowa grupa"
        };
        
        
        //Act
        var res = await _repository.AddNewGroup(idUser, newGroupRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddNewGroupToDb_Returns_SameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyGroup();

        var idUser = 1;
        var newGroupRequestDto = new NewGroupRequestDto()
        {
            IdUser = 1,
            GroupName = "test",
            Description = "Nowa grupa"
        };
        
        //Act
        //Assert
        await Assert.ThrowsAsync<SameNameException>(() => _repository.AddNewGroup(idUser, newGroupRequestDto));
    }
    
    [Fact]
    public async Task DeleteGroupToDb_Returns_GroupDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyGroup();

        var idGroup = 1;
        
        //Act
        var res = await _repository.DeleteGroup(idGroup);
        
        //Assert
        Assert.NotNull(res);

        var groups = _context.Groups.ToList();
        Assert.False(groups.Exists(e => e.IdGroup == res.IdGroup));
    }
    
    [Fact]
    public async Task DeleteGroupToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idGroup = 1;
        
        //Act
        var res = await _repository.DeleteGroup(idGroup);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyGroupToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idGroup = 3;
        
        var modifyGroupRequestDto = new GroupRequestDto()
        {
            IdGroup = 3,
            GroupName = "test",
            Content = "Nowa grupa"
        };
        
        //Act
        var res = await _repository.ModifyGroup(idGroup, modifyGroupRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyGroupToDb_Returns_GroupDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyGroup();
        
        var idGroup = 2;
        
        var modifyGroupRequestDto = new GroupRequestDto()
        {
            IdGroup = 2,
            GroupName = "changedName",
            Content = "Nowa grupa"
        };
        
        //Act
        var res = await _repository.ModifyGroup(idGroup, modifyGroupRequestDto);
        
        //Assert
        Assert.NotNull(res);
        
        var group = await _context.Groups.SingleOrDefaultAsync(e =>
            e.IdGroup == idGroup
            && e.GroupName    == modifyGroupRequestDto.GroupName
            && e.Description   == modifyGroupRequestDto.Content);
        
        Assert.NotNull(group);
    }
    
    [Fact]
    public async Task ModifyGroupToDb_Returns_SameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyGroup();
        
        var idGroup = 2;
        
        var modifyGroupRequestDto = new GroupRequestDto()
        {
            IdGroup = 2,
            GroupName = "test",
            Content = "Nowa grupa"
        };
        
        //Act
        //Assert
        await Assert.ThrowsAsync<SameNameException>(() => _repository.ModifyGroup(idGroup, modifyGroupRequestDto));
    }
    
    
    [Fact]
    public async Task GetGroupFromDb_Returns_GroupDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();
        
        var idGroup = 1;
        
        //Act
        var res = await _repository.GetGroup(idGroup);
        
        //Assert
        Assert.NotNull(res);
        
        var groups = _context.Groups.ToList();
        Assert.Single(groups);
        
        var group = groups.SingleOrDefault(e => e.IdGroup == res.IdGroup);
        Assert.NotNull(group);
    }
    
    [Fact]
    public async Task GetGroupFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();
        
        var idGroup = 2;
        
        //Act
        var res = await _repository.GetGroup(idGroup);
        
        //Assert
        Assert.Null(res);
    }
    
    
    [Fact]
    public async Task GetGroupsFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyGroup();

        //Act
        var res = await _repository.GetGroups();
        
        //Assert
        Assert.NotNull(res);
        
        var groups = await _context.Groups.Select(e => e.IdGroup).ToListAsync();
        Assert.Equal(groups, res.Select(e => e.IdGroup));
    }
        
    [Fact]
    public async Task  RemoveUserFromGroup_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyGroup();

        var idGroup = 1;
        
        var userToGroupDto = new UserToGroupDto()
        {
            IdGroup = 1,
            IdUser = 1
        }; 
        //Act
        var res = await _repository.RemoveUserFromGroup(idGroup, userToGroupDto);
        
        //Assert
        Assert.NotNull(res);
        
        var groups = _context.Groups.ToList();
        Assert.True(groups.Exists(e => e.IdGroup == res.IdGroup));
    }
    
    [Fact]
    public async Task  RemoveUserFromGroup_GroupOrUserDoesNotExist_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithOneGroup();

        var idUser = 2;
        
        var userToGroupDto = new UserToGroupDto()
        {
            IdGroup = 1,
            IdUser = 2
        }; 
        //Act
        var res = await _repository.RemoveUserFromGroup(idUser, userToGroupDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task  RemoveUserFromGroup_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyGroup();

        var idUser = 2;
        
        var userToGroupDto = new UserToGroupDto()
        {
            IdGroup = 1,
            IdUser = 2
        }; 
        //Act
        var res = await _repository.RemoveUserFromGroup(idUser, userToGroupDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task RemoveUserFromGroup_Returns_UserCreatorException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyGroup();

        var idGroup = 1;
        
        var userToGroupDto = new UserToGroupDto()
        {
            IdGroup = 1,
            IdUser = 2
        }; 
        
        //Act
        //Assert
        await Assert.ThrowsAsync<UserCreatorException>(() => _repository.RemoveUserFromGroup(idGroup, userToGroupDto));
    }
    
    [Fact]
    public async Task ExistUserInTheGroup_Returns_True()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyGroup();

        var idGroup = 1;
        var idUser = 1;
        
        //Act
        var res = await _repository.UserIsInGroup(idGroup, idUser);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.Member);
    }
    [Fact]
    public async Task AddUserToGroup_Returns_GroupDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyGroup();

        var idGroup = 2;
        
        var userToGroupDto = new UserToGroupDto()
        {
            IdGroup = 2,
            IdUser = 2
        }; 
        //Act
        var res = await _repository.AddUserToGroup(idGroup, userToGroupDto);
        
        //Assert
        Assert.NotNull(res);
        var groups = _context.Groups.ToList();
        Assert.True(groups.Exists(e => e.IdGroup == res.IdGroup));
    }
    
    [Fact]
    public async Task AddUserToGroup_GroupOrUserDoesNotExist_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithOneGroup();

        var idGroup = 2;
        
        var userToGroupDto = new UserToGroupDto()
        {
            IdGroup = 2,
            IdUser = 3
        }; 
        //Act
        var res = await _repository.AddUserToGroup(idGroup, userToGroupDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddUserToGroup_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyGroup();

        var idGroup = 1;
        
        var userToGroupDto = new UserToGroupDto()
        {
            IdGroup = idGroup,
            IdUser = 1
        }; 
        //Act
        var res = await _repository.AddUserToGroup(idGroup, userToGroupDto);
        
        //Assert
        Assert.Null(res);
    }

    [Fact]
    public async Task GetGroups_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyGroup();

        //Act
        var res = await _repository.GetGroups();

        //Assert
        Assert.NotNull(res);
        
        var groups = await _context.Groups.Select(e => e.IdGroup).ToListAsync();
        Assert.Equal(groups, res.Select(e => e.IdGroup));
    }
    
    [Fact]
    public async Task GetGroups_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetGroups();

        //Assert
        Assert.Empty(res);
    }
    
    [Fact]
    public async Task GetUserGroups_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyGroup();

        var idUser = 1;
        //Act
        var res = await _repository.GetUserGroups(idUser);

        //Assert
        Assert.NotNull(res);
        Assert.NotEmpty(res);
    }
    
    [Fact]
    public async Task GetUserGroups_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        
        //Act
        var res = await _repository.GetUserGroups(idUser);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task SetGroupPhoto_WithNoExistingGroup_Returns_null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();
        
        var idGroup = 100;

        var fileMock = new Mock<IFormFile>();

        //Act
        var res = await _repository.SetGroupPhoto(fileMock.Object,idGroup);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task SetGroupPhoto_WithNullPhoto_Returns_null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();
        
        var idGroup = 1;

        var fileMock = new Mock<IFormFile>();

        //Act
        var res = await _repository.SetGroupPhoto(fileMock.Object, idGroup);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task SetGroupPhoto_Returns_GroupDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();
        
        var idGroup = 1;
        
        var currentDirectory = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(currentDirectory, @"..\..\..\Images\test.png");
        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        
        IFormFile formFile = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(filePath));

        //Act
        var res = await _repository.SetGroupPhoto(formFile,idGroup);
        
        await fileStream.DisposeAsync();
        
        //Assert
        Assert.NotNull(res);
    }
    
    [Fact]
    public async Task GetGroupPhoto_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();

        var idGroup = 1;

        //Act
        var res = await _repository.GetGroupPhoto(idGroup);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetGroupPhoto_Returns_GroupPhotoResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();

        var idGroup = 1;

        await AddPhotoToGroup(idGroup);
        
        //Act
        var res = await _repository.GetGroupPhoto(idGroup);
        
        //Assert
        Assert.NotNull(res);
    }
}