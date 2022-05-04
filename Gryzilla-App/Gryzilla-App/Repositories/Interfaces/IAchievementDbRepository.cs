using Gryzilla_App.DTO.Requests;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTO.Responses.Rank;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IAchievementDbRepository
{
    public Task<IEnumerable<AchievementDto>?> GetAchievementsFromDb();
    public Task<string?> ModifyAchievement(int idAchievement, PutAchievementDto putAchievementDto);
    public Task<string?> AddNewAchievement(AddAchievementDto addAchievementDto);
    public Task<string?> DeleteAchievement(int idAchievement);
    public Task<string?> DeleteUserAchievement(int idAchievement, int idUser);
    public Task<string?> AddNewUserAchievement(int idAchievement, int idUser);
}