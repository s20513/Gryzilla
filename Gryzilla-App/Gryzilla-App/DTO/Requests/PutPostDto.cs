﻿using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Responses.Posts;

public class PutPostDto
{
    [Required]
    public int idPost { get; set; }
    [Required]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    public string title { get; set; }
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    public string content { get; set; }
    public TagByIdDto [] tags { get; set; }
}