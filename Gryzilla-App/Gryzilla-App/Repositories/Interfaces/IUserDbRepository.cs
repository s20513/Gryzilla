using Gryzilla_App.DTO.Requests.User;
using Gryzilla_App.DTO.Responses;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IUserDbRepository
{
     public Task<UserDto?> GetUserFromDb(int idUser);
     public Task<IEnumerable<UserDto?>> GetUsersFromDb();
     public Task<UserDto?> ModifyUserFromDb( int idUser, PutUserDto putUserDto);
     
     
     
}