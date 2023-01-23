using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.User;

public class UserRank
{
    [Required]
    public int IdUser {get; set;}
    
    [Required]
    public int IdRank {get; set;}
}