using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Requests.Rank;

public class PutRankDto
{
    [Required]
    public int IdRank { get; set; }

    [Required]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    [MinLength(2,ErrorMessage = "Max length : 2")]
    public string Name { get; set; } = null!;

    [Required]
    public int RankLevel { get; set; }
}