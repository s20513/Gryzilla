using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Requests.Rank;

public class AddRankDto
{
    [Required]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    [MinLength(2, ErrorMessage = "Min lenght : 2")]
    public string Name { get; set; }
    [Required]
    public int RankLevel { get; set; }
}