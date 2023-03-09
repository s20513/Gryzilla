using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.Notification;

public class NewNotificationDto
{
    [Required]
    public int IdUser { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Min Lenght : 2")]
    [MaxLength(50, ErrorMessage = "Max length : 50")]
    public string Content { get; set; }
    public DateTime Date { get; set; }
}