namespace Gryzilla_App.DTOs.Responses.Articles;

public class ArticleQtyDto
{
    public IEnumerable<ArticleDto>? articles { get; set; }
    public bool isNext { get; set; }
}