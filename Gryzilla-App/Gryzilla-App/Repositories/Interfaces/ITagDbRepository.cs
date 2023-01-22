using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Tag;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ITagDbRepository
{
    public Task<FullTagDto?> GetTagFromDb(int idTag);
    public Task<IEnumerable<FullTagDto>?> GetTagsFromDb();
    public Task<FullTagDto> AddTagToDb(NewTagDto newTagDto);
    public Task<IEnumerable<FullTagDto>> GetTagsStartingWithParamFromDb(string startOfTagName);
}