using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Friends;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IFriendsDbRepository
{
    public Task<IEnumerable<FriendDto>?> GetFriendsFromDb(int idUser);
    public Task<string?> DeleteFriendFromDb(int idUser, int idUserFriend);
    public Task<string?> AddNewFriendFromDb(int idUser, int idUserFriend);
}