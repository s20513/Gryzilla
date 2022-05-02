﻿namespace Gryzilla_App.DTO.Responses.Posts;

public class OnePostDto
{
    public string Nick { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public TagDto? [] Tags { get; set; }
    public CommentDto? [] Comments { get; set; }
    public int? Likes{ get; set; }
}