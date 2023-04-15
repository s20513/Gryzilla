using Gryzilla_App.DTOs.Requests.GroupUserMessage;
using Gryzilla_App.DTOs.Responses.GroupUserMessageDto;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IGroupUserMessageDbRepository
{
    public  Task<GroupUserMessageDto?> DeleteMessage(int idGroupMessage);
    public  Task<GroupUserMessageDto?> ModifyMessage(int idMessage, UpdateGroupUserMessageDto updateGroupUserMessage);

    public  Task<GroupUserMessageDto?> AddMessage(AddGroupUserMessageDto addGroupUserMessage);
    public  Task<GroupUserMessageDto[]?> GetMessages(int idGroup);
}