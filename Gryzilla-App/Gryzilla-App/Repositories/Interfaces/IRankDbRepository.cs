using Gryzilla_App.DTO.Requests.Rank;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IRankDbRepository
{
    public Task<string?> AddNewRank(AddRankDto addRankDto);
    
    public Task<string?> ModifyRank(PutRankDto putRankDto, int idRank);
    public Task<string?> DeleteRank(int idRank);
}