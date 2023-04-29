using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.DTOs.Responses.Link;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ILinkDbRepository
{
    public Task<string?> DeleteLinkSteam(int idUser);
    public Task<string?> DeleteLinkDiscord(int idUser);
    public Task<string?> DeleteLinkPs(int idUser);
    public Task<string?> DeleteLinkXbox(int idUser);
    public Task<string?> DeleteLinkEpic(int idUser);
    public Task<string?> PutLinkDiscord(LinkDto linkDto);
    public Task<string?> PutLinkSteam(LinkDto linkDto);
    public Task<string?> PutLinkXbox(LinkDto linkDto);
    public Task<string?> PutLinkPs(LinkDto linkDto);
    public Task<string?> PutLinkEpic(LinkDto linkDto);
    public Task<LinksDto?> GetUserLinks(int idUser);
}