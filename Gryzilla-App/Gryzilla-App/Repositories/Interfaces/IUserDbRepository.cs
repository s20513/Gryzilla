using Gryzilla_App.DTOs.Requests.User;
using Gryzilla_App.DTOs.Responses.User;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IUserDbRepository
{
     public Task<UserDto?> GetUserFromDb(int idUser);
     public Task<IEnumerable<UserDto?>> GetUsersFromDb();
     public Task<UserDto?> ModifyUserFromDb( int idUser, PutUserDto putUserDto);
     public Task<UserDto?> AddUserToDb(AddUserDto addUserDto);
     public Task<UserDto?> DeleteUserFromDb(int idUser);
     
}