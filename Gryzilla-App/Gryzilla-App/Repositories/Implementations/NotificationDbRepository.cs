using Gryzilla_App.DTOs.Requests.Notification;
using Gryzilla_App.DTOs.Responses.Notification;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class NotificationDbRepository : INotificationDbRepository
{
    private readonly GryzillaContext _context;
    private INotificationDbRepository _notificationDbRepository;

    public NotificationDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task <NotificationDto?> AddNotificationFromDb(NewNotificationDto newNotificationDto)
    {
        var user = await _context
            .UserData
            .Where(x => x.IdUser == newNotificationDto.IdUser)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        var notification = new Notification
        {
            IdUser        = newNotificationDto.IdUser,
            Date          = DateTime.Now,
            Content       = newNotificationDto.Content
        };

        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        
        var newIdNotification = _context.Notifications
            .Max(x => x.IdNotification);
        
        return new NotificationDto
        {
            IdNotification = newIdNotification,
            IdUser         = newNotificationDto.IdUser,
            Date           = DateTime.Now,
            Content        = newNotificationDto.Content
        };
    }

    public async Task<NotificationDto?> DeleteNotificationFromDb(int idNotification)
    {
        var notification =
            await _context
                .Notifications
                .SingleOrDefaultAsync(x => x.IdNotification == idNotification);

        if (notification is null)
        {
            return null;
        }

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
        
        return new NotificationDto
        {
            IdNotification = idNotification,
            IdUser         = notification.IdUser,
            Date           = notification.Date,
            Content        = notification.Content
        };
    }

    public async Task<NotificationDto?> ModifyNotificationFromDb(int idNotification, ModifyNotificationDto modifyNotification)
    {
        var notification =
            await _context
                .Notifications
                .SingleOrDefaultAsync(x => x.IdNotification == idNotification);

        if (notification is null)
        {
            return null;
        }

        notification.Content = modifyNotification.Content;
        notification.Date    = DateTime.Now;
        
        await _context.SaveChangesAsync();
        
        return new NotificationDto
        {
            IdNotification = idNotification,
            IdUser         = notification.IdUser,
            Date           = notification.Date,
            Content        = notification.Content
        };
        
    }
}