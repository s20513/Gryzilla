namespace Gryzilla_App.DTOs.Responses.Articles;

public class ArticleQtyDto
{
    public IEnumerable<ArticleDto>? Articles { get; set; }
    public bool IsNext { get; set; }
}