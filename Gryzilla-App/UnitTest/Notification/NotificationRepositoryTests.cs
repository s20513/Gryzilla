using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.Notification;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
namespace UnitTest.Notification;

public class NotificationRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly NotificationDbRepository _repository;

    public NotificationRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new NotificationDbRepository(_context);
    }

    private async Task AddTestDataWithManyUser()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Rank
        {
            Name = "User",
            RankLevel = 4
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

        await _context.Notifications.AddAsync(new Gryzilla_App.Notification
        {
            IdUser = 1,
            Content = "Content",
            Date    = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task AddNotificationToDb_Returns_NotificationDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyUser();

        var newNotificationRequestDto = new NewNotificationDto
        {
            IdUser = 1,
            Content = "Content",
            Date = DateTime.Now
        };
        
        //Act
        var res = await _repository.AddNotificationFromDb(newNotificationRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var notifaction = _context.Notifications.ToList();
        Assert.True(notifaction.Exists(e => e.IdNotification == res.IdNotification));
    }
    
    [Fact]
    public async Task AddNotificationToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var newNotificationRequestDto = new NewNotificationDto
        {
            IdUser = 1,
            Content = "Content",
            Date = DateTime.Now
        };
        
        //Act
        var res = await _repository.AddNotificationFromDb(newNotificationRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task DeleteNotificationToDb_Returns_NotificationDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyUser();

        var idNotification = 1;
        
        //Act
        var res = await _repository.DeleteNotificationFromDb(idNotification);
        
        //Assert
        Assert.NotNull(res);

        var notifications = _context.Notifications.ToList();
        Assert.False(notifications.Exists(e => e.IdNotification == res.IdNotification));
    }
    
    [Fact]
    public async Task DeleteNotificationDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idNotification = 1;
        
        //Act
        var res = await _repository.DeleteNotificationFromDb(idNotification);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyNotificationToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idNotification = 1;
        
        var modifyNotificationRequestDto = new ModifyNotificationDto
        {
            Content = "Content"
        };
        
        //Act
        var res = await _repository.ModifyNotificationFromDb(idNotification, modifyNotificationRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyNotificationToDb_Returns_NotificationDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyUser();
        
        var idNotification = 1;
        
        var modifyNotificationRequestDto = new ModifyNotificationDto
        {
            Content = "Content"
        };
        
        //Act
        var res = await _repository.ModifyNotificationFromDb(idNotification, modifyNotificationRequestDto);

        //Assert
        Assert.NotNull(res);
        
        var notification = await _context.Notifications.SingleOrDefaultAsync(e =>
            e.IdNotification == idNotification
            && e.Content   == modifyNotificationRequestDto.Content);
        
        Assert.NotNull(notification);
    }
}