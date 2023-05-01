namespace Gryzilla_App.DTOs.Responses.Articles;

public class ArticleQtySearchDto
{
    public IEnumerable<ArticleSearchDto>? Articles { get; set; }
    public bool IsNext { get; set; }
}