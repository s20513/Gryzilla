using System.Security.Claims;
using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IGroupDbRepository
{
    public Task<GroupDto?> GetGroup(int idGroup);
    public Task<GroupDto[]> GetGroups();
    public Task<GroupDto?> ModifyGroup(int idGroup, GroupRequestDto groupRequestDtoDto, ClaimsPrincipal userClaims);
    public Task<GroupDto?> DeleteGroup(int idGroup, ClaimsPrincipal userClaims);
    public Task<GroupDto?> AddNewGroup(int idGroup, NewGroupRequestDto groupRequestDtoDto);
    public Task<GroupDto?> RemoveUserFromGroup(int idGroup, UserToGroupDto userToGroupDto, ClaimsPrincipal userClaims);
    public Task<GroupDto?> AddUserToGroup(int idGroup, UserToGroupDto userToGroupDtoDto);
    public Task<ExistUserGroupDto> UserIsInGroup(int idGroup, int idUser);
    public Task<GroupDto[]?> GetUserGroups(int idUser);
    public Task<GroupDto?> SetGroupPhoto(IFormFile photo, int idGroup, ClaimsPrincipal userClaims);
    public Task<GroupPhotoResponseDto?> GetGroupPhoto(int idGroup);
}