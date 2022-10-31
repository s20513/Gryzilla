using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
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

    private async Task<Group?> GetGroupById(int idGroup)
    {
        var group = await _context
            .Groups
            .Where(e => e.IdGroup == idGroup)
            .Include(x=>x.IdUsers)
            .SingleOrDefaultAsync();
        
        return group;
    }
    private async Task<UserDatum?> GetUserById(int idUser)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(e => e.IdUser == idUser);
        
        return user;
    }
    private async Task<Group?> GetGroupByName(string groupName)
    {
        var group = await _context
            .Groups
            .SingleOrDefaultAsync(e => e.GroupName == groupName);
        
        return group;
    }
   
    public async Task<GroupDto?> GetGroup(int idGroup)
    {
        var group = await _context.Groups
            .Where(e => e.IdGroup == idGroup)
            .Select(e => new GroupDto
            {
                IdGroup       = e.IdGroup,
                IdUserCreator = e.IdUserCreator,
                GroupName     = e.GroupName,
                Description   = e.Description,
                CreatedAt     = e.CreatedAt,
                Users         = _context.Groups
                    .Where(g => g.IdGroup == idGroup)
                    .SelectMany(g => g.IdUsers)
                    .Include(g => g.IdRankNavigation)
                    .Select(g => new UserDto
                    {
                        IdUser      = g.IdUser,
                        IdRank      = g.IdRank,
                        Nick        = g.Nick,
                        Email       = g.Email,
                        PhoneNumber = g.PhoneNumber,
                        CreatedAt   = g.CreatedAt,
                        RankName    = g.IdRankNavigation.Name
                    }).ToList()
                
            }).SingleOrDefaultAsync();

        return group ?? null;
    }

    public async Task<GroupDto[]> GetGroups()
    {
        var groups = await _context.Groups
            .Select(e => new GroupDto
            {
                IdGroup       = e.IdGroup,
                IdUserCreator = e.IdUserCreator,
                GroupName     = e.GroupName,
                Description   = e.Description,
                CreatedAt     = e.CreatedAt,
                Users         = _context.Groups
                    .Where(g => g.IdGroup == e.IdGroup)
                    .SelectMany(g => g.IdUsers)
                    .Include(g => g.IdRankNavigation)
                    .Select(g => new UserDto
                    {
                        IdUser      = g.IdUser,
                        IdRank      = g.IdRank,
                        Nick        = g.Nick,
                        Email       = g.Email,
                        PhoneNumber = g.PhoneNumber,
                        CreatedAt   = g.CreatedAt,
                        RankName    = g.IdRankNavigation.Name
                    }).ToList()
            }).ToArrayAsync();
        
        return groups;
    }

    public async Task<GroupDto?> ModifyGroup(int idGroup, GroupRequestDto groupRequestDto)
    {
        var group = await GetGroupById(idGroup);

        if (group is null)
        {
            return null;
        }

        var groupName =  _context
            .Groups
            .Where(x=>x.IdGroup != idGroup)
            .Count(x=>x.GroupName == groupRequestDto.GroupName);
        
        if (groupName > 0)
        {
            throw new SameNameException("This name of group is already taken");
        }
        
        group.GroupName   = groupRequestDto.GroupName; 
        group.Description = groupRequestDto.Description;
        
        await _context.SaveChangesAsync();

        return await GetGroup(idGroup);
    }

    public async Task<GroupDto?> DeleteGroup(int idGroup)
    {
        var group = await _context
            .Groups
            .Where(e => e.IdGroup == idGroup)
            .Include(x=>x.IdUsers)
            .SingleOrDefaultAsync();

        if (group is null)
        {
            return null;
        }

        var groupUsers = await _context.Groups
            .Where(e => e.IdGroup == idGroup)
            .SelectMany(e => e.IdUsers)
            .ToListAsync();
        
        foreach (var groupUser in groupUsers)
        {
            group.IdUsers.Remove(groupUser);
        }

        var groupDto = await GetGroup(idGroup);;
        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return groupDto;
    }

    public async Task<GroupDto?> RemoveUserFromGroup(int idGroup, UserToGroupDto userToGroupDto)
    {
        var group = await GetGroupById(idGroup);
        var user = await GetUserById(userToGroupDto.IdUser);
        
        if (group is null || user is null)
        {
            return null;
        }

        if (group.IdUserCreator == userToGroupDto.IdUser)
        {
            throw new UserCreatorException("The creator of the group cannot be removed");
        }
        
        var groupUser = await _context.Groups
            .Where(e => e.IdGroup == idGroup)
            .SelectMany(e => e.IdUsers)
            .Where(e => e.IdUser == userToGroupDto.IdUser)
            .SingleOrDefaultAsync();

        if (groupUser is null)
        {
            return null;
        }
        
        group.IdUsers.Remove(user);
        await _context.SaveChangesAsync();

        return await GetGroup(idGroup);
    }
    
    public async Task<GroupDto?> AddNewGroup(int idUser, NewGroupRequestDto groupRequestDto)
    {
        var user = await GetUserById(idUser);

        if (user is null)
        {
            return null; 
        }

        var group = await GetGroupByName(groupRequestDto.GroupName);

        if (group is not null)
        {
            throw new SameNameException("The name of the group is already taken");
        }

        var newGroup = new Group
        {
            IdUserCreator = idUser,
            GroupName     = groupRequestDto.GroupName,
            Description   = groupRequestDto.Description,
            CreatedAt     = DateTime.Now
        };
        
        await _context.Groups.AddAsync(newGroup);
        await _context.SaveChangesAsync();
        
        var addUserToGroup = await GetGroupByName(groupRequestDto.GroupName);
        
        if (addUserToGroup is null)
        {
            return null;
        }
        
        addUserToGroup.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        
        return await GetGroup(addUserToGroup.IdGroup);
    }

    public async Task<GroupDto?> AddUserToGroup(int idGroup, UserToGroupDto userToGroupDto)
    {
        var group = await GetGroupById(idGroup);
        var user = await GetUserById(userToGroupDto.IdUser);
        
        if (user is null || group is null)
        {
            return null; 
        }

        var groupUser = await _context.Groups
            .Where(e => e.IdGroup == userToGroupDto.IdGroup)
            .SelectMany(e => e.IdUsers)
            .Where(e => e.IdUser == userToGroupDto.IdUser)
            .SingleOrDefaultAsync();

        if (groupUser is not null)
        {
            return null; 
        }

        group.IdUsers.Add(user);
        await _context.SaveChangesAsync();

        return await GetGroup(group.IdGroup);
    }

    public async Task<bool?> ExistUserInTheGroup(int idGroup, int idUser)
    {
        var groupUser = await _context.Groups
            .Where(e => e.IdGroup == idGroup)
            .SelectMany(e => e.IdUsers)
            .Where(e => e.IdUser == idUser)
            .SingleOrDefaultAsync();

        return groupUser is not null;
    }

    public async Task<UserGroupDto[]?> GetUserGroups(int idUser)
    {
        var user = await GetUserById(idUser);

        if (user is null)
        {
            return null;
        }

        var groups = await _context
                     .UserData
                     .Where(x => x.IdUser == idUser)
                     .SelectMany(x => x.IdGroups)
                     .Select(x => new UserGroupDto
                     { 
                         IdGroup       = x.IdGroup,
                         IdUserCreator = x.IdUserCreator,
                         GroupName     = x.GroupName,
                         Description   = x.Description,
                         CreatedAt     = x.CreatedAt
                     }).ToArrayAsync();

        return groups;
    }
}