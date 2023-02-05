using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Gryzilla_App.DTOs.Requests.BlockedUser;

public class BlockedUserRequestDto
{
    [Required]
    public int IdUserBlocking { get; set; }
    
    [Required]
    public int IdUserBlocked { get; set; }
    
    public string? Comment { get; set; }
}