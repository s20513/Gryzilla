using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IGroupDbRepository
{
    public Task<GroupDto?> GetGroup(int idGroup);
    public Task<GroupDto?> ModifyGroup(int idGroup, GroupRequestDto groupRequestDtoDto);
    public Task<GroupDto?> DeleteGroup(int idGroup);
    public Task<GroupDto?> RemoveUserFromGroup(int idGroup, UserToGroupDto userToGroupDto);
    public Task<GroupDto?> AddNewGroup(int idUser, GroupRequestDto groupRequestDtoDto);
    public Task<GroupDto?> AddUserToGroup(int idGroup, UserToGroupDto userToGroupDtoDto);
}