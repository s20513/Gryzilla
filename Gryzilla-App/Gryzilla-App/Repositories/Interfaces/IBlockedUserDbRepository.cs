using Gryzilla_App.DTOs.Requests.BlockedUser;
using Gryzilla_App.DTOs.Responses.BlockedUser;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IBlockedUserDbRepository
{
    public Task<IEnumerable<BlockedUserDto>> GetBlockedUsers();
    public Task<BlockedUserDto?> BlockUser(BlockedUserRequestDto blockedUserRequestDto);
    public Task<string?> UnlockUser(int idUser);
    public Task<UserBlockingHistoryDto?> GetUserBlockingHistory(int idUser);
}