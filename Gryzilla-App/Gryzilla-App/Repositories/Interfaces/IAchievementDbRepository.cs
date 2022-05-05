using Gryzilla_App.DTO.Requests;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTO.Responses.Rank;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IAchievementDbRepository
{
    public Task<IEnumerable<AchievementDto>?> GetAchievementsFromDb();
    public Task<AchievementDto?> ModifyAchievement(int idAchievement, PutAchievementDto putAchievementDto);
    public Task<AchievementDto?> AddNewAchievement(AddAchievementDto addAchievementDto);
    public Task<AchievementDto?> DeleteAchievement(int idAchievement);
    public Task<AchievementDto?> DeleteUserAchievement(int idAchievement, int idUser);
    public Task<AchievementDto?> AddNewUserAchievement(int idAchievement, int idUser);
}