using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<GroupDto?> ModifyGroup(int idGroup, GroupRequestDto groupRequestDtoDto)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(e => e.IdGroup == idGroup);
        if (group is null)
            return null;

        group.GroupName = groupRequestDtoDto.GroupName;
        group.Description = groupRequestDtoDto.Description;

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
            .SelectMany(e => e.IdUsers)
            .ToListAsync();
        
        _context.RemoveRange(groupUsers);
        foreach (var groupUser in groupUsers)
        {
            group.IdUsers.Remove(groupUser);
        }
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

    public async Task<GroupDto?> RemoveUserFromGroup(int idGroup, UserToGroupDto userToGroupDtoDto)
    {
        if (idGroup != userToGroupDtoDto.IdGroup)
            return null;
        
        var group = await _context.Groups.SingleOrDefaultAsync(e => e.IdGroup == userToGroupDtoDto.IdGroup);
        if (group is null || group.IdUserCreator == userToGroupDtoDto.IdUser)
            return null;

        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == userToGroupDtoDto.IdUser);
        if (user is null)
            return null;

        var groupUser = await _context.Groups
            .Where(e => e.IdGroup == idGroup)
            .SelectMany(e => e.IdUsers)
            .Where(e => e.IdUser == userToGroupDtoDto.IdUser)
            .SingleOrDefaultAsync();

        if (groupUser is null)
            return null;

        group.IdUsers.Remove(groupUser);
        await _context.SaveChangesAsync();
        
        return new GroupDto
        {
            IdGroup = group.IdGroup,
            IdUserCreator = group.IdGroup,
            GroupName = group.GroupName,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            Users = await _context.Groups.Where(g => g.IdGroup == userToGroupDtoDto.IdGroup)
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
    
    public async Task<GroupDto?> AddNewGroup(int idUser, GroupRequestDto groupRequestDto)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(e => e.GroupName == groupRequestDto.GroupName);
        if (group is not null)
            return null;

        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == idUser);
        if (user is null)
            return null;

        var newGroup = new Group
        {
            IdUserCreator = idUser,
            GroupName = groupRequestDto.GroupName,
            Description = groupRequestDto.Description,
            CreatedAt = DateTime.Today.Date
        };
        await _context.Groups.AddAsync(newGroup);
        
        await _context.SaveChangesAsync();
        
        group = await _context.Groups.SingleOrDefaultAsync(e => e.GroupName == groupRequestDto.GroupName);
        Console.WriteLine("aieubgabdgbadgb9adbg9a9dg9a");
        Console.WriteLine(group);
        Console.WriteLine("aieubgabdgbadgb9adbg9a9dg9a");
        
        await _context.SaveChangesAsync();
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

    public async Task<GroupDto?> AddUserToGroup(int idGroup, UserToGroupDto userToGroupDtoDto)
    {
        if (idGroup != userToGroupDtoDto.IdGroup)
            return null;

        var group = await _context.Groups.SingleOrDefaultAsync(e => e.IdGroup == userToGroupDtoDto.IdGroup);
        if (group is null)
            return null;

        var user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == userToGroupDtoDto.IdUser);
        if (user is null)
            return null;

        var groupUser = await _context.Groups
            .Where(e => e.IdGroup == userToGroupDtoDto.IdGroup)
            .SelectMany(e => e.IdUsers)
            .Where(e => e.IdUser == userToGroupDtoDto.IdUser)
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