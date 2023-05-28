using System.Security.Claims;
using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.DTOs.Responses.Link;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ILinkDbRepository
{
    public Task<string?> DeleteLinkSteam(int idUser, ClaimsPrincipal userClaims);
    public Task<string?> DeleteLinkDiscord(int idUser, ClaimsPrincipal userClaims);
    public Task<string?> DeleteLinkPs(int idUser, ClaimsPrincipal userClaims);
    public Task<string?> DeleteLinkXbox(int idUser, ClaimsPrincipal userClaims);
    public Task<string?> DeleteLinkEpic(int idUser, ClaimsPrincipal userClaims);
    public Task<string?> PutLinkDiscord(LinkDto linkDto, ClaimsPrincipal userClaims);
    public Task<string?> PutLinkSteam(LinkDto linkDto, ClaimsPrincipal userClaims);
    public Task<string?> PutLinkXbox(LinkDto linkDto, ClaimsPrincipal userClaims);
    public Task<string?> PutLinkPs(LinkDto linkDto, ClaimsPrincipal userClaims);
    public Task<string?> PutLinkEpic(LinkDto linkDto, ClaimsPrincipal userClaims);
    public Task<LinksDto?> GetUserLinks(int idUser);
}