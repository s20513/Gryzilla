using System.Security.Claims;
using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Helpers;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class GroupDbRepository: IGroupDbRepository
{
    private readonly GryzillaContext _context;

    public GroupDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    private async Task<Group?> GetGroupById(int idGroup)
    {
        var group = await _context
            .Groups
            .Where(e => e.IdGroup == idGroup)
            .Include(e => e.GroupUsers)
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
                Nick          = _context.UserData
                    .Where(x => x.IdUser == e.IdUserCreator)
                    .Select(x=>x.Nick)
                    .SingleOrDefault(),
                GroupName     = e.GroupName,
                Content   = e.Description,
                CreatedAt     = e.CreatedAt,
                Type          = e.PhotoType,
                base64PhotoData = Convert.ToBase64String(e.Photo ?? Array.Empty<byte>()),
                Users         = _context.GroupUsers
                    .Where(g => g.IdGroup == idGroup)
                    .Include(g => g.IdUserNavigation)
                    .Include(g => g.IdUserNavigation.IdRankNavigation)
                    //.SelectMany(g => g.IdUsers)
                    //.Include(g => g.IdRankNavigation)
                    .Select(g => new UserDto
                    {
                        IdUser      = g.IdUser,
                        IdRank      = g.IdUserNavigation.IdRank,
                        Nick        = g.IdUserNavigation.Nick,
                        Email       = g.IdUserNavigation.Email,
                        PhoneNumber = g.IdUserNavigation.PhoneNumber,
                        CreatedAt   = g.IdUserNavigation.CreatedAt,
                        RankName    = g.IdUserNavigation.IdRankNavigation.Name
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
                Nick          = _context.UserData
                    .Where(x => x.IdUser == e.IdUserCreator)
                    .Select(x=>x.Nick)
                    .SingleOrDefault(),
                GroupName     = e.GroupName,
                Content   = e.Description,
                CreatedAt     = e.CreatedAt,
                Type          = e.PhotoType,
                base64PhotoData = Convert.ToBase64String(e.Photo ?? Array.Empty<byte>()),
                Users         = _context.Groups
                    .Where(g => g.IdGroup == e.IdGroup)
                    .SelectMany(g => g.GroupUsers)
                    .Include(g => g.IdUserNavigation)
                    .Include(g => g.IdUserNavigation.IdRankNavigation)
                    .Select(g => new UserDto
                    {
                        IdUser      = g.IdUser,
                        IdRank      = g.IdUserNavigation.IdRank,
                        Nick        = g.IdUserNavigation.Nick,
                        Email       = g.IdUserNavigation.Email,
                        PhoneNumber = g.IdUserNavigation.PhoneNumber,
                        CreatedAt   = g.IdUserNavigation.CreatedAt,
                        RankName    = g.IdUserNavigation.IdRankNavigation.Name
                    }).ToList()
            }).ToArrayAsync();
        
        return groups;
    }

    public async Task<GroupDto?> ModifyGroup(int idGroup, GroupRequestDto groupRequestDto, ClaimsPrincipal userClaims)
    {
        var group = await GetGroupById(idGroup);

        if (group is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, group.IdUserCreator))
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
        group.Description = groupRequestDto.Content;
        
        await _context.SaveChangesAsync();

        return await GetGroup(idGroup);
    }

    public async Task<GroupDto?> DeleteGroup(int idGroup, ClaimsPrincipal userClaims)
    {
        var group = await _context
            .Groups
            .Where(e => e.IdGroup == idGroup)
            .SingleOrDefaultAsync();

        if (group is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, group.IdUserCreator))
        {
            return null;
        }

        var groupUsers = await _context.GroupUsers
            .Where(e => e.IdGroup == idGroup)
            .ToListAsync();
        
        _context.GroupUsers.RemoveRange(groupUsers);

        var groupDto = await GetGroup(idGroup);
        
        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return groupDto;
    }

    public async Task<GroupDto?> RemoveUserFromGroup(int idGroup, UserToGroupDto userToGroupDto, ClaimsPrincipal userClaims)
    {
        var group = await GetGroupById(idGroup);
        var user = await GetUserById(userToGroupDto.IdUser);
        
        if (group is null || 
            user is null || 
            !ActionAuthorizer.IsTheUserToRemoveFromGroupOrAuthorOrHasRightRole(userClaims, group.IdUserCreator, userToGroupDto.IdUser))
        {
            return null;
        }

        if (group.IdUserCreator == userToGroupDto.IdUser)
        {
            throw new UserCreatorException("The creator of the group cannot be removed");
        }
        
        var groupUser = await _context.GroupUsers
            .Where(e => e.IdGroup == idGroup
                            && e.IdUser == userToGroupDto.IdUser)
            .SingleOrDefaultAsync();

        if (groupUser is null)
        {
            return null;
        }

        _context.GroupUsers.Remove(groupUser);
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
        
        var newCreatedGroup = await GetGroupByName(groupRequestDto.GroupName);

        await _context.GroupUsers.AddAsync(new GroupUser
        {
            IdUser = user.IdUser,
            IdGroup = newCreatedGroup.IdGroup
        });

        await _context.SaveChangesAsync();
        
        return await GetGroup(newCreatedGroup.IdGroup);
    }

    public async Task<GroupDto?> AddUserToGroup(int idGroup, UserToGroupDto userToGroupDto)
    {
        var group = await GetGroupById(idGroup);
        var user = await GetUserById(userToGroupDto.IdUser);
        
        if (user is null || group is null)
        {
            return null; 
        }

        var groupUser = await _context.GroupUsers
            .Where(e => e.IdGroup == userToGroupDto.IdGroup
                                && e.IdUser == userToGroupDto.IdUser)
            .SingleOrDefaultAsync();

        if (groupUser is not null)
        {
            return null; 
        }

        await _context.AddAsync(new GroupUser
        {
            IdUser = userToGroupDto.IdUser,
            IdGroup = userToGroupDto.IdGroup
        });
        
        await _context.SaveChangesAsync();

        return await GetGroup(group.IdGroup);
    }

    public async Task<ExistUserGroupDto> UserIsInGroup(int idGroup, int idUser)
    {
        var groupUser = await _context.GroupUsers
            .Where(e => e.IdGroup == idGroup 
                                && e.IdUser == idUser)
            .SingleOrDefaultAsync();

        return new ExistUserGroupDto
        {
            Member = groupUser is not null
        };
    }

    public async Task<GroupDto[]?> GetUserGroups(int idUser)
    {
        var user = await GetUserById(idUser);

        if (user is null)
        {
            return null;
        }

        var groups = await _context
                     .GroupUsers
                     .Where(x => x.IdUser == idUser)
                     .Include(e => e.IdGroupNavigation)
                     .Select(x => new GroupDto
                     {
                            IdGroup       = x.IdGroup,
                            IdUserCreator = x.IdGroupNavigation.IdUserCreator,
                            Nick          = _context.UserData
                                .Where(e => e.IdUser == x.IdGroupNavigation.IdUserCreator)
                                .Select(e=>e.Nick)
                                .SingleOrDefault(),
                            GroupName     = x.IdGroupNavigation.GroupName,
                            Content       = x.IdGroupNavigation.Description,
                            CreatedAt     = x.IdGroupNavigation.CreatedAt,
                            Type          = x.IdGroupNavigation.PhotoType,
                            base64PhotoData = Convert.ToBase64String(x.IdGroupNavigation.Photo ?? Array.Empty<byte>()),
                            Users         = _context.Groups
                                .Where(g => g.IdGroup == x.IdGroup)
                                .SelectMany(g => g.GroupUsers)
                                .Include(g => g.IdUserNavigation)
                                .Include(g => g.IdUserNavigation.IdRankNavigation)
                                .Select(g => new UserDto
                                {
                                    IdUser      = g.IdUser,
                                    IdRank      = g.IdUserNavigation.IdRank,
                                    Nick        = g.IdUserNavigation.Nick,
                                    Email       = g.IdUserNavigation.Email,
                                    PhoneNumber = g.IdUserNavigation.PhoneNumber,
                                    CreatedAt   = g.IdUserNavigation.CreatedAt,
                                    RankName    = g.IdUserNavigation.IdRankNavigation.Name
                                }).ToList()
                     }).ToArrayAsync();

        return groups;
    }
    public async Task<GroupDto?> SetGroupPhoto(IFormFile photo, int idGroup, ClaimsPrincipal userClaims)
    {
        var group = await GetGroupById(idGroup);
        
        if (group is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, group.IdUserCreator))
        {
            return null;
        }
        
        byte[]? photoBytes = null;
        if (photo.Length > 0)
        {
            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);
            photoBytes = ms.ToArray();
            
            ms.Close();
            await ms.DisposeAsync();
        }

        if (photoBytes is null)
        {
            return null;
        }
        group.Photo = photoBytes;
        group.PhotoType = Path.GetExtension(photo.FileName).Remove(0,1);
        await _context.SaveChangesAsync();

        return await GetGroup(idGroup);
    }

    public async Task<GroupPhotoResponseDto?> GetGroupPhoto(int idGroup)
    {
        var group = await _context.Groups
            .Where(x => x.IdGroup == idGroup)
            .Select(e => new
            {
                e.Photo,
                e.PhotoType
            })
            .SingleOrDefaultAsync();

        if (group?.PhotoType is null || group.Photo is null)
        {
            return null;
        }
        
        return new GroupPhotoResponseDto
        {
            Type = group.PhotoType,
            base64PhotoData = Convert.ToBase64String(group.Photo)
        };
    }
    
    
}