using Gryzilla_App.DTOs.Requests.Notification;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/notification")]
public class NotificationController : Controller
{
    private readonly INotificationDbRepository _notificationDbRepository;

    public NotificationController(INotificationDbRepository notificationDbRepository)
    {
        _notificationDbRepository = notificationDbRepository;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> AddNotification([FromBody] NewNotificationDto newNotificationDto)
    { 
       var notification = await _notificationDbRepository.AddNotificationToDb(newNotificationDto);
       if (notification is null)
       {
           return NotFound("No user with given id found");
       }
        
        return Ok(notification);
    }
    
    
    [HttpDelete("{idNotification:int}")]
    public async Task<IActionResult> DeleteNotification([FromRoute] int idNotification)
    {
        var notification = await _notificationDbRepository.DeleteNotificationFromDb(idNotification);
        if (notification is null)
        {
            return NotFound("No notification with given id found");
        }
        
        return Ok(notification);
    }
    
    [HttpPut("{idNotification:int}")]
    public async Task<IActionResult> ModifyNotification([FromRoute] int idNotification, [FromBody] ModifyNotificationDto modifyNotificationDto)
    {
        var notification = await _notificationDbRepository.ModifyNotificationFromDb(idNotification, modifyNotificationDto);
        if (notification is null)
        {
            return NotFound("No notification with given id found");
        }
        return Ok(notification);
    }
}