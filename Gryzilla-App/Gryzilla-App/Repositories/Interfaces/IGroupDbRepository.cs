using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IGroupDbRepository
{
    public Task<GroupDto?> GetGroup(int idGroup);
    public Task<GroupDto?> ModifyGroup(int idGroup, GroupRequest groupRequestDto);
    public Task<GroupDto?> DeleteGroup(int idGroup);
    public Task<GroupDto?> RemoveUserFromGroup(UserToGroup userToGroup);
    public Task<GroupDto?> AddNewGroup(int idUser, GroupRequest groupRequestDto);
    public Task<GroupDto?> AddUserToGroup(UserToGroup userToGroupDto);
}