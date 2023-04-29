using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IGroupDbRepository
{
    public Task<GroupDto?> GetGroup(int idGroup);
    public Task<GroupDto[]> GetGroups();
    public Task<GroupDto?> ModifyGroup(int idGroup, GroupRequestDto groupRequestDtoDto);
    public Task<GroupDto?> DeleteGroup(int idGroup);
    public Task<GroupDto?> AddNewGroup(int idGroup, NewGroupRequestDto groupRequestDtoDto);
    public Task<GroupDto?> RemoveUserFromGroup(int idGroup, UserToGroupDto userToGroupDto);
    public Task<GroupDto?> AddUserToGroup(int idGroup, UserToGroupDto userToGroupDtoDto);
    public Task<bool?> UserIsInGroup(int idGroup, int idUser);
    public Task<UserGroupDto[]?> GetUserGroups(int idUser);
    public Task<GroupDto?> SetGroupPhoto(IFormFile photo, int idGroup);
    public Task<GroupPhotoResponseDto?> GetGroupPhoto(int idGroup);
}