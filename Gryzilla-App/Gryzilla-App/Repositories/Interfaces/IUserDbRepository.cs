using Gryzilla_App.DTO.Responses;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IUserDbRepository
{
     public Task<UserDto> GetUserFromDb(int idUser);
}