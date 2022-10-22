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
        var achievements = 
            await _context
                .Achievements
                .Select(x=>new AchievementDto
                {
                    IdAchievement = x.IdAchievement,
                    Description = x.Descripion,
                    AchievementName = x.AchievementName,
                    Points = x.Points
                })
                .ToArrayAsync();
        
        return achievements.Length == 0 ? null : achievements;
    }

    public async Task<AchievementDto?> ModifyAchievement(int idAchievement, PutAchievementDto putAchievementDto)
    {
        var achievement = 
            await _context
                .Achievements
                .Where(x => x.IdAchievement == idAchievement)
                .SingleOrDefaultAsync();

        if (achievement is null)
        {
            return null;
        }
        
        achievement.Descripion = putAchievementDto.description;
        achievement.Points = putAchievementDto.Points;
        achievement.AchievementName = putAchievementDto.achievementName;
        
        await _context.SaveChangesAsync();
        
        return new AchievementDto
        {
            IdAchievement = achievement.IdAchievement,
            AchievementName = achievement.AchievementName,
            Description = achievement.Descripion,
            Points = achievement.Points
        };
    }

    public async Task<AchievementDto?> AddNewAchievement(AddAchievementDto addAchievementDto)
    {
        int idAchievement;
        var sameNameAchievement = 
            await _context
                .Achievements
                .Where(x => x.AchievementName == addAchievementDto.AchievementName)
                .SingleOrDefaultAsync();
        
        if (sameNameAchievement is not null)
        {
            return null;
        }
        
        var newAchievement = new Achievement
        {
            Descripion = addAchievementDto.Description,
            AchievementName = addAchievementDto.AchievementName,
            Points = addAchievementDto.Points
        };
        
        await _context.Achievements.AddAsync(newAchievement);
        await _context.SaveChangesAsync();

        idAchievement = 
            await _context
                .Achievements
                .Where(x => x.AchievementName == addAchievementDto.AchievementName)
                .Select(x => x.IdAchievement)
                .FirstAsync();
        
        return  new AchievementDto
        {
            //IdAchievement = await _context.Achievements.Select(x=>x.IdAchievement).OrderByDescending(x=>x).FirstAsync(),
            IdAchievement = idAchievement,
            AchievementName = newAchievement.AchievementName,
            Description = newAchievement.Descripion,
            Points = newAchievement.Points
        };
    }

    public async Task<AchievementDto?> DeleteAchievement(int idAchievement)
    {
        Achievement?      achievement;
        AchievementUser[] achievementUsers;
        
        achievement = 
            await _context
                .Achievements
                .Where(x => x.IdAchievement == idAchievement)
                .SingleOrDefaultAsync();

        if (achievement is null)
        {
            return null;
        }
        
        achievementUsers = 
            await _context
                .UserData
                .SelectMany(x => x.AchievementUsers)
                .Where(x => x.IdAchievement == idAchievement)
                .ToArrayAsync();
        
        if (achievementUsers.Length > 0)
        {
            //nie usuwać jeżeli jakiś user ma ten achievement???
            return null;
        }
        
        _context.Achievements.Remove(achievement);
        await _context.SaveChangesAsync();
        
        //sens zwracania obiektu przy deletcie???
        return new AchievementDto
        {
            IdAchievement = achievement.IdAchievement,
            AchievementName = achievement.AchievementName,
            Description = achievement.Descripion,
            Points = achievement.Points
        };
    }

    public async Task<AchievementDto?> DeleteUserAchievement(int idAchievement, int idUser)
    {
        UserDatum?   user;
        Achievement? achievement;

        achievement = 
            await _context
                .Achievements
                .Where(x => x.IdAchievement == idAchievement)
                .SingleOrDefaultAsync();
        
        user =
            await _context
                .UserData
                .Where(x => x.IdUser == idUser)
                .SingleOrDefaultAsync();

        if (user is null || achievement is null)
            //czy to ma sens w usuwaniu? więzy spójności i tak w bazie nie pozwolą na trzymanie referencji do pustych obiektów.
            //Wystarczy sprawdzenie tylko czy istnieje userAchievement
        {
            return null;
        }

        var userAchievement = 
            await _context
                .AchievementUsers
                .Where(x => x.IdUser == idUser && x.IdAchievement == idAchievement)
                .SingleOrDefaultAsync();

        if (userAchievement is null)
        {
            return null;
        }

        _context.AchievementUsers.Remove(userAchievement);
        await _context.SaveChangesAsync();
        
        return  new AchievementDto
        {
            IdAchievement = userAchievement.IdAchievement,
            AchievementName = achievement.AchievementName,
            Description = achievement.Descripion,
            Points = achievement.Points
        };
    }

    public async Task<AchievementDto?> AddNewUserAchievement(int idAchievement, int idUser)
    {
        UserDatum?   user;
        Achievement? achievement;

        achievement = 
            await _context
                .Achievements
                .Where(x => x.IdAchievement == idAchievement)
                .SingleOrDefaultAsync();
        
        user =
            await _context
                .UserData
                .Where(x => x.IdUser == idUser)
                .SingleOrDefaultAsync();

        if (user is null || achievement is null)
        {
            return null;
        }
        
        var userAchievement = 
            await _context
                .AchievementUsers
                .Where(x => x.IdUser == idUser && x.IdAchievement == idAchievement)
                .SingleOrDefaultAsync();

        if (userAchievement != null)
        {
            return null;
        }
        
        await _context.AchievementUsers.AddAsync(new AchievementUser
        {
            IdUser = idUser,
            IdAchievement = idAchievement,
            ReceivedAt = DateTime.Now
        });
        
        await _context.SaveChangesAsync();
        
        //czy będzie możliwość edytowania pojedycznego achievka?
        //wtedy lepiej tu dodać id + w geta z jednym achievkiem
        return  new AchievementDto
        {
            IdAchievement = achievement.IdAchievement,
            AchievementName = achievement.AchievementName,
            Description = achievement.Descripion,
            Points = achievement.Points
        };
    }
}