using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Reason;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IReasonDbRepository
{
    public Task<List<FullTagDto>> GetReasonsFromDb();
    public Task<FullTagDto?> GetReasonFromDb(int idReason);
    public Task<FullTagDto> AddReasonToDb(NewReasonDto newReasonDto);
    public Task<FullTagDto?> DeleteReasonFromDb(int id);
}