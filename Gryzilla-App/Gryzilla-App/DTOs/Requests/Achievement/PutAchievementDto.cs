using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Requests.Rank;

public class PutAchievementDto
{
    [Required]
    public int idAchievement { get; set; }
    [Required]
    //od 0 do 100.00 punktów
    [Range(0,100)]
    public decimal points { get; set; }
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    public string description { get; set; }
    [Required]
    [MaxLength(50,ErrorMessage = "Max length : 50")]
    public string achievementName { get; set; }
}