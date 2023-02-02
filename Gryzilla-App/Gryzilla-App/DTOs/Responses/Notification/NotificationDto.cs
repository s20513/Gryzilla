namespace Gryzilla_App.DTOs.Responses.Notification;

public class NotificationDto
{
    public int IdNotification { get; set; }
    public int IdUser { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
}