using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Requests.Rank;

public class PutRankDto
{
    [Required]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    public string Name { get; set; }
    [Required]
    public int RankLevel { get; set; }
}