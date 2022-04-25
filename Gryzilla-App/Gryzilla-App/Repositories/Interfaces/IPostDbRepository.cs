using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IPostDbRepository
{
    public Task<IEnumerable<PostDto>?> GetPostsFromDb();
    public Task<string?> AddNewPostFromDb(AddPostDto addPostDto);
    public Task<string?> DeletePostFromDb(int idPost);
}