using System.Security.Claims;
using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.DTOs.Responses.Link;
using Gryzilla_App.Helpers;
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

    public async Task<string?> DeleteLinkSteam(int idUser, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == idUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, idUser))
        {
            return null;
        }
        user.SteamLink = null;
        await _context.SaveChangesAsync();

        return "Link removed";
    }
    
    public async Task<string?> DeleteLinkDiscord(int idUser, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == idUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, idUser))
        {
            return null;
        }
        
        user.DiscordLink = null;
        await _context.SaveChangesAsync();

        return "Link removed";
    }

    public async Task<string?> DeleteLinkXbox(int idUser, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == idUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, idUser))
        {
            return null;
        }
        
        user.XboxLink = null;
        await _context.SaveChangesAsync();

        return "Link removed";
    }
    
    public async Task<string?> DeleteLinkPs(int idUser, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == idUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, idUser))
        {
            return null;
        }
        
        user.PsLink = null;
        await _context.SaveChangesAsync();

        return "Link removed";
    }
    public async Task<string?> DeleteLinkEpic(int idUser, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == idUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, idUser))
        {
            return null;
        }
        
        user.EpicLink = null;
        await _context.SaveChangesAsync();

        return "Link removed";
    }
    public async Task<string?> PutLinkDiscord(LinkDto linkDto, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == linkDto.IdUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, linkDto.IdUser))
        {
            return null;
        }
        if (linkDto.Link.Contains("https://discord.gg/"))
        {
            user.DiscordLink = linkDto.Link;
            await _context.SaveChangesAsync();
            return "Link changed";
        }

        return null;
    }
    
    public async Task<string?> PutLinkSteam(LinkDto linkDto, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser==linkDto.IdUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, linkDto.IdUser))
        {
            return null;
        }

        if (linkDto.Link.Contains("https://steamcommunity.com/profiles/"))
        {
            user.SteamLink = linkDto.Link;
            await _context.SaveChangesAsync();
            return "Link changed";
        }
        
        return null;
    }

    public async Task<string?> PutLinkXbox(LinkDto linkDto, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser == linkDto.IdUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, linkDto.IdUser))
        {
            return null;
        }
        if (linkDto.Link.Contains("https://account.xbox.com/pl-pl/Profile?"))
        {
            user.XboxLink = linkDto.Link;
            await _context.SaveChangesAsync();
            return "Link changed";
        }
        
        return null;
    }
    
    public async Task<string?> PutLinkPs(LinkDto linkDto, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser==linkDto.IdUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, linkDto.IdUser))
        {
            return null;
        }
        
        user.PsLink = linkDto.Link;
        await _context.SaveChangesAsync();

        return "Link changed";
    }
    public async Task<string?> PutLinkEpic(LinkDto linkDto, ClaimsPrincipal userClaims)
    {
        var user = await _context.UserData.Where(x=>x.IdUser==linkDto.IdUser).SingleOrDefaultAsync();

        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, linkDto.IdUser))
        {
            return null;
        }
        
        if (linkDto.Link.Contains("https://launcher.store.epicgames.com/"))
        {
            user.EpicLink = linkDto.Link;
            await _context.SaveChangesAsync();
            return "Link changed";
        }
        
        return null;
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
            DiscordLink = user.DiscordLink,
            EpicLink    = user.EpicLink,
            XboxLink    = user.XboxLink,
            PsLink      = user.PsLink
        };
    }
}