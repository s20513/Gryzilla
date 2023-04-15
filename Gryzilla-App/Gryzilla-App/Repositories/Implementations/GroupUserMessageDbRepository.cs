using Gryzilla_App.DTOs.Requests.GroupUserMessage;
using Gryzilla_App.DTOs.Responses.GroupUserMessageDto;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class GroupUserMessageDbRepository : IGroupUserMessageDbRepository
{
    private readonly GryzillaContext _context;

    public GroupUserMessageDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    private async Task<GroupUserMessageDto> GetMessageDto(GroupUserMessage message)
    {
        var user = await _context.UserData
            .SingleOrDefaultAsync(x => x.IdUser == message.IdUser);
        
        return new GroupUserMessageDto
        {
            IdMessage = message.IdMessage,
            IdGroup   = message.IdGroup,
            IdUser    = message.IdUser,
            Nick      = user.Nick,
            Content   = message.Message,
            CreatedAt = message.CreatedAt
        };
    }

    public async Task<GroupUserMessageDto?> DeleteMessage(int idGroupMessage)
    {
        var message = await _context.GroupUserMessages
            .SingleOrDefaultAsync(x => x.IdMessage == idGroupMessage);
        
        if (message is null)
        {
            return null;
        }
        
        _context.GroupUserMessages.Remove(message);
        await _context.SaveChangesAsync();

        return await GetMessageDto(message);
    }
    
    public async Task<GroupUserMessageDto?> ModifyMessage(int idMessage, UpdateGroupUserMessageDto updateGroupUserMessage)
    {
        var message = await _context.GroupUserMessages
            .SingleOrDefaultAsync(x => x.IdMessage == updateGroupUserMessage.IdMessage);
        
        if (message is null)
        {
            return null;
        }
        message.Message = updateGroupUserMessage.Content;
        await _context.SaveChangesAsync();

        return await GetMessageDto(message);
    }
    
    public async Task<GroupUserMessageDto?> AddMessage(AddGroupUserMessageDto addGroupUserMessage)
    {
        var user = await _context.UserData
            .SingleOrDefaultAsync(x => x.IdUser == addGroupUserMessage.IdUser);
        
        var group  = await _context.Groups
            .SingleOrDefaultAsync(x => x.IdGroup == addGroupUserMessage.IdGroup);

        var userGroup = await _context.GroupUsers
            .Where(x => x.IdGroup == addGroupUserMessage.IdGroup)
            .Where(x => x.IdUser == addGroupUserMessage.IdUser)
            .SingleOrDefaultAsync();
        
        if (group is null || user is null || userGroup is null)
        {
            return null;
        }

        var newMessage = new GroupUserMessage
        {
            IdUser = user.IdUser,
            IdGroup = group.IdGroup,
            Message = addGroupUserMessage.Content,
            CreatedAt = DateTime.Now
        };

        _context.GroupUserMessages.Add(newMessage);
        await _context.SaveChangesAsync();
        
        int groupMessageId = _context.GroupUserMessages.Max(e => e.IdMessage);
         return new GroupUserMessageDto
        {
            IdMessage = groupMessageId,
            IdUser    = newMessage.IdUser,
            IdGroup   = newMessage.IdGroup,
            Nick      = user.Nick,
            Content   = newMessage.Message,
            CreatedAt = newMessage.CreatedAt
        };
    }

    public async Task<GroupUserMessageDto []?> GetMessages(int idGroup)
    {
        var groups = await _context.GroupUserMessages.Select(
            x => new GroupUserMessageDto
            {
                IdMessage = x.IdMessage,
                IdUser    = x.IdUser,
                Nick      = _context.UserData
                    .Where(e => e.IdUser == x.IdUser)
                    .Select(e => e.Nick).SingleOrDefault(),
                Content   = x.Message,
                CreatedAt = x.CreatedAt,
                IdGroup   = x.IdGroup
            }).ToArrayAsync();

        return groups;
    }
}