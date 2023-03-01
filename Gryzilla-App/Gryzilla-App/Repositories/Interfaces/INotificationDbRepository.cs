using Gryzilla_App.DTOs.Requests.Notification;
using Gryzilla_App.DTOs.Responses.Notification;

namespace Gryzilla_App.Repositories.Interfaces;

public interface INotificationDbRepository
{
    public Task<NotificationDto?> ModifyNotificationFromDb(int idNotification, ModifyNotificationDto modifyNotification);
    public Task<NotificationDto?> DeleteNotificationFromDb(int idNotification);
    public Task<NotificationDto?> AddNotificationToDb(NewNotificationDto newNotificationDto);

}