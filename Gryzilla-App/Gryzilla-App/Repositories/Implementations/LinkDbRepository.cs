using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.DTOs.Responses.Link;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class LinkDbRepository : ILinkDbRepository
{
    private readonly GryzillaContext _context;

    public LinkDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    public async Task<string?> DeleteLinkSteam(int idUser)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == idUser).SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        user.SteamLink = null;
        await _context.SaveChangesAsync();

        return "Link removed";
    }
    
    public async Task<string?> DeleteLinkDiscord(int idUser)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == idUser).SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        user.DiscordLink = null;
        await _context.SaveChangesAsync();

        return "Link removed";
    }

    public async Task<string?> PutLinkDiscord(LinkDto linkDto)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == linkDto.IdUser).SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        user.DiscordLink = linkDto.Link;
        await _context.SaveChangesAsync();

        return "Link changed";
    }
    
    public async Task<string?> PutLinkSteam(LinkDto linkDto)
    {
        var user = await _context.UserData.Where(x=>x.IdUser==linkDto.IdUser).SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        user.SteamLink = linkDto.Link;
        await _context.SaveChangesAsync();

        return "Link changed";
    }

    public async Task<LinksDto?> GetUserLinks(int idUser)
    {
        var user = await _context.UserData.SingleOrDefaultAsync(x => x.IdUser == idUser);

        if (user is null)
        {
            return null;
        }

        return new LinksDto
        {
            SteamLink   = user.SteamLink,
            DiscordLink = user.DiscordLink
        };
    }
}