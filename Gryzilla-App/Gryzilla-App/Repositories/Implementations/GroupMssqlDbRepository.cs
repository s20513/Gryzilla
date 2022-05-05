using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class GroupMssqlDbRepository: IGroupDbRepository
{
    private readonly GryzillaContext _context;

    public GroupMssqlDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    

    public async Task<GroupDto?> GetGroup(int idGroup)
    {
        var group = await _context.Groups
            .Where(e => e.IdGroup == idGroup)
            .Select(e => new GroupDto
            {
                IdGroup = e.IdGroup,
                IdUserCreator = e.IdUserCreator,
                GroupName = e.GroupName,
                Description = e.Description,
                CreatedAt = e.CreatedAt,
                Users = _context.Groups.Where(g => g.IdGroup == idGroup)
                    .SelectMany(g => g.IdUsers)
                    .Select(g => new UserDto
                    {
                        IdUser = g.IdUser,
                        IdRank = g.IdRank,
                        Nick = g.Nick,
                        Email = g.Email,
                        PhoneNumber = g.PhoneNumber,
                        CreatedAt = g.CreatedAt,
                        RankName = _context.UserData
                            .Where(t => t.IdUser == g.IdUser)
                            .Include(t => t.IdRankNavigation)
                            .Select(t => t.IdRankNavigation.Name).ToString()
                    }).ToList()
            }).SingleOrDefaultAsync();

        return group ?? null;
    }

    public async Task<GroupDto?> ModifyGroup(int idGroup, GroupRequest groupRequestDto)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(e => e.IdGroup == idGroup);
        if (group is null)
            return null;

        group.GroupName = groupRequestDto.GroupName;
        group.Description = groupRequestDto.Description;

        await _context.SaveChangesAsync();

        return new GroupDto
        {
            IdGroup = group.IdGroup,
            IdUserCreator = group.IdGroup,
            GroupName = group.GroupName,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            Users = await _context.Groups.Where(g => g.IdGroup == idGroup)
                .SelectMany(g => g.IdUsers)
                .Select(g => new UserDto
                {
                    IdUser = g.IdUser,
                    IdRank = g.IdRank,
                    Nick = g.Nick,
                    Email = g.Email,
                    PhoneNumber = g.PhoneNumber,
                    CreatedAt = g.CreatedAt,
                    RankName = _context.UserData
                        .Where(t => t.IdUser == g.IdUser)
                        .Include(t => t.IdRankNavigation)
                        .Select(t => t.IdRankNavigation.Name).ToString()
                }).ToListAsync()
        };

    }

    public async Task<GroupDto?> DeleteGroup(int idGroup)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(e => e.IdGroup == idGroup);
        if (group is null)
            return null;

        var groupUsers = await _context.Groups
            .Where(e => e.IdGroup == idGroup)
            .Select(e => e.IdUsers)
            .ToListAsync();
        
        _context.RemoveRange(groupUsers);
        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return new GroupDto
        {
            IdGroup = group.IdGroup,
            IdUserCreator = group.IdGroup,
            GroupName = group.GroupName,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            Users = await _context.Groups.Where(g => g.IdGroup == idGroup)
                .SelectMany(g => g.IdUsers)
                .Select(g => new UserDto
                {
                    IdUser = g.IdUser,
                    IdRank = g.IdRank,
                    Nick = g.Nick,
                    Email = g.Email,
                    PhoneNumber = g.PhoneNumber,
                    CreatedAt = g.CreatedAt,
                    RankName = _context.UserData
                        .Where(t => t.IdUser == g.IdUser)
                        .Include(t => t.IdRankNavigation)
                        .Select(t => t.IdRankNavigation.Name).ToString()
                }).ToListAsync()
        };
    }

    public async Task<GroupDto?> RemoveUserFromGroup(UserToGroup userToGroupDto)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(e => e.IdGroup == userToGroupDto.IdGroup);
        if (group is null || group.IdUserCreator == userToGroupDto.IdUser)
            return null;

        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == userToGroupDto.IdUser);
        if (user is null)
            return null;
        
        var groupUser = await _context.UserData
            .Where(e => e.IdUser == userToGroupDto.IdUser)
            .SelectMany(e => e.Groups)
            .Where(e => e.IdGroup == userToGroupDto.IdGroup)
            .SingleOrDefaultAsync();

        if (groupUser is null)
            return null;
        
        _context.Remove(groupUser);
        
        return new GroupDto
        {
            IdGroup = group.IdGroup,
            IdUserCreator = group.IdGroup,
            GroupName = group.GroupName,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            Users = await _context.Groups.Where(g => g.IdGroup == userToGroupDto.IdGroup)
                .SelectMany(g => g.IdUsers)
                .Select(g => new UserDto
                {
                    IdUser = g.IdUser,
                    IdRank = g.IdRank,
                    Nick = g.Nick,
                    Email = g.Email,
                    PhoneNumber = g.PhoneNumber,
                    CreatedAt = g.CreatedAt,
                    RankName = _context.UserData
                        .Where(t => t.IdUser == g.IdUser)
                        .Include(t => t.IdRankNavigation)
                        .Select(t => t.IdRankNavigation.Name).ToString()
                }).ToListAsync()
        };
    }
    
    public async Task<GroupDto?> AddNewGroup(int idUser, GroupRequest groupRequestDto)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(e => e.GroupName == groupRequestDto.GroupName);
        if (group is not null)
            return null;

        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == idUser);
        if (user is null)
            return null;

        await _context.SaveChangesAsync();
        
        group = await _context.Groups.SingleOrDefaultAsync(e => e.GroupName == groupRequestDto.GroupName);
        group.IdUsers.Add(user);

        await _context.SaveChangesAsync();
        return new GroupDto
        {
            IdGroup = group.IdGroup,
            IdUserCreator = group.IdGroup,
            GroupName = group.GroupName,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            Users = await _context.Groups.Where(g => g.IdGroup == group.IdGroup)
                .SelectMany(g => g.IdUsers)
                .Select(g => new UserDto
                {
                    IdUser = g.IdUser,
                    IdRank = g.IdRank,
                    Nick = g.Nick,
                    Email = g.Email,
                    PhoneNumber = g.PhoneNumber,
                    CreatedAt = g.CreatedAt,
                    RankName = _context.UserData
                        .Where(t => t.IdUser == g.IdUser)
                        .Include(t => t.IdRankNavigation)
                        .Select(t => t.IdRankNavigation.Name).ToString()
                }).ToListAsync()
        };
    }

    public async Task<GroupDto?> AddUserToGroup(UserToGroup userToGroupDto)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(e => e.IdGroup == userToGroupDto.IdGroup);
        if (group is null)
            return null;

        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == userToGroupDto.IdUser);
        if (user is null)
            return null;

        var groupUser = await _context.Groups
            .Where(e => e.IdGroup == userToGroupDto.IdGroup)
            .SelectMany(e => e.IdUsers)
            .Where(e => e.IdUser == userToGroupDto.IdUser)
            .SingleOrDefaultAsync();
        if (groupUser is not null)
            return null;
        
        group.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        return new GroupDto
        {
            IdGroup = group.IdGroup,
            IdUserCreator = group.IdGroup,
            GroupName = group.GroupName,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            Users = await _context.Groups.Where(g => g.IdGroup == group.IdGroup)
                .SelectMany(g => g.IdUsers)
                .Select(g => new UserDto
                {
                    IdUser = g.IdUser,
                    IdRank = g.IdRank,
                    Nick = g.Nick,
                    Email = g.Email,
                    PhoneNumber = g.PhoneNumber,
                    CreatedAt = g.CreatedAt,
                    RankName = _context.UserData
                        .Where(t => t.IdUser == g.IdUser)
                        .Include(t => t.IdRankNavigation)
                        .Select(t => t.IdRankNavigation.Name).ToString()
                }).ToListAsync()
        };
    }
    
}