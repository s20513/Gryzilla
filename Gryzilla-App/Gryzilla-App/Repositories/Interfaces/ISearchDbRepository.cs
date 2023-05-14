using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.DTOs.Responses.Posts;
using Gryzilla_App.DTOs.Responses.User;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ISearchDbRepository
{
    public Task<UsersQtyDto?> GetUsersByNameFromDb(int qtyUsers, DateTime time, string word);
    public Task<PostQtySearchDto?> GetPostByWordFromDb(int qtyPosts, DateTime time, string word);
    public Task<ArticleQtySearchDto?> GetArticleByWordFromDb(int qtyArticles, DateTime time, string word);
    public Task<ArticleQtyDto?> GetArticlesByTagFromDb(int qtyArticles, DateTime time, string word);
    public Task<PostQtyDto?> GetPostsByTagFromDb(int qtyPosts, DateTime time,  string word);
    public Task<GroupsQtySearchDto?> GetGroupsByWordFromDb(int qtyGroups, DateTime time, string word);
}