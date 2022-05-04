using System.Globalization;
using Gryzilla_App.DTO.Requests;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTO.Responses.Rank;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class AchievementDbRepository : IAchievementDbRepository
{
    private readonly GryzillaContext _context;

    public AchievementDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AchievementDto>?> GetAchievementsFromDb()
    {
        var achievements = await _context.Achievements.Select(x=>new AchievementDto
        {
            idAchievement = x.IdAchievement,
            description = x.Descripion,
            achievementName = x.AchievementName,
            points = x.Points
        }).ToArrayAsync();
        if (achievements.Length == 0) return null;
        return achievements;
    }

    public async Task<string?> ModifyAchievement(int idAchievement, PutAchievementDto putAchievementDto)
    {
        var achievement =
            await _context.Achievements.Where(x => x.IdAchievement == idAchievement).SingleOrDefaultAsync();
        if (achievement is null) return null;
        achievement.Descripion = putAchievementDto.description;
        achievement.Points = putAchievementDto.points;
        achievement.AchievementName = putAchievementDto.achievementName;
        await _context.SaveChangesAsync();
        return "Modified achievement";
    }

    public async Task<string?> AddNewAchievement(AddAchievementDto addAchievementDto)
    {
        //sprawdzamy czy jest achievement o takiej samej nazwie
        var achievement = await _context.Achievements.Where(x => x.AchievementName == addAchievementDto.achievementName)
            .SingleOrDefaultAsync();
        if (achievement is not null) return null;

        await _context.Achievements.AddAsync(new Achievement
        {
            Descripion = addAchievementDto.description,
            AchievementName = addAchievementDto.achievementName,
            Points = addAchievementDto.points
        });
        await _context.SaveChangesAsync();
        return "Added new achievement";
    }

    public async Task<string?> DeleteAchievement(int idAchievement)
    {
        var achievement = await _context.Achievements.Where(x => x.IdAchievement == idAchievement).SingleOrDefaultAsync();
        if (achievement is null) return null;
        var userAchievement = await _context.UserData.SelectMany(x => x.AchievementUsers)
            .Where(x => x.IdAchievement == idAchievement).ToArrayAsync();
        if (userAchievement.Length > 0) return null;
        _context.Achievements.Remove(achievement);
        await _context.SaveChangesAsync();
        return "Deleted achievement";
    }

    public async Task<string?> DeleteUserAchievement(int idAchievement, int idUser)
    {
        var achievement = await _context.Achievements.Where(x => x.IdAchievement == idAchievement).SingleOrDefaultAsync();
        var user = await _context.UserData.Where(x => x.IdUser == idUser).SingleOrDefaultAsync();
        if (user is null || achievement is null) return null;

        var userAchievement = await _context.AchievementUsers.Where(x => x.IdUser == idUser)
            .Where(x => x.IdAchievement == idAchievement).SingleOrDefaultAsync();
        if (userAchievement is null) return null;

        _context.AchievementUsers.Remove(userAchievement);
        await _context.SaveChangesAsync();
        return "Deleted achievement from user";
    }

    public async Task<string?> AddNewUserAchievement(int idAchievement, int idUser)
    {
        var achievement = await _context.Achievements.Where(x => x.IdAchievement == idAchievement).SingleOrDefaultAsync();
        var user = await _context.UserData.Where(x => x.IdUser == idUser).SingleOrDefaultAsync();
        if (user is null || achievement is null) return null;
        var userAchievement = await _context.AchievementUsers.Where(x => x.IdUser == idUser)
            .Where(x => x.IdAchievement == idAchievement).SingleOrDefaultAsync();
        if (userAchievement != null) return null;
        await _context.AchievementUsers.AddAsync(new AchievementUser
        {
            IdUser = idUser,
            IdAchievement = idAchievement,
            ReceivedAt = DateTime.Now
        });
        await _context.SaveChangesAsync();
        return "Added achievement to user";
    }
}