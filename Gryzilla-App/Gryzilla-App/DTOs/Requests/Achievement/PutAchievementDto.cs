using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Requests.Rank;

public class PutAchievementDto
{
    [Required]
    public int IdAchievement { get; set; }
    
    [Required]
    [Range(0,100)]
    public decimal Points { get; set; }
    
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    public string Description { get; set; }
    
    [Required]
    [MaxLength(50,ErrorMessage = "Max length : 50")]
    public string AchievementName { get; set; }
}