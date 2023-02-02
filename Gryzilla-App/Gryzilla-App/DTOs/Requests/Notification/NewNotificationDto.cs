namespace Gryzilla_App.DTOs.Requests.Notification;

public class NewNotificationDto
{
    public int IdUser { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
}