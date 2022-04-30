using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IPostDbRepository
{
    public Task<IEnumerable<PostDto>?> GetPostsFromDb();
    public Task<IEnumerable<PostDto>?> GetPostsByLikesFromDb();
    public Task<IEnumerable<PostDto>?> GetPostsByLikesLeastFromDb();
    public Task<IEnumerable<PostDto>?> GetPostsByDateFromDb();
    public Task<IEnumerable<PostDto>?> GetPostsByDateOldestFromDb();
    public Task<string?> AddNewPostFromDb(AddPostDto addPostDto);
    public Task<string?> DeletePostFromDb(int idPost);
    public Task<string?> DeleteTagFromPost(int idPost, int idTag);
    public Task<string?> ModifyPostFromDb(PutPostDto putPostDto);
}