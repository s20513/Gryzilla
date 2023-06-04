using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTOs.Responses.Rank;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IRankDbRepository
{
    public Task<RankDto?> AddNewRank(AddRankDto addRankDto);
    
    public Task<RankDto?> ModifyRank(PutRankDto putRankDto, int idRank);
    public Task<RankDto?> DeleteRank(int idRank);
    public Task<RankDto[]> GetRanks();
}