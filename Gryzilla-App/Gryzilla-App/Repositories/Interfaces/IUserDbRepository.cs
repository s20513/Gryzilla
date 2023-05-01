using System.Drawing;
using System.Net.Mime;
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
     public Task<TokenResponseDto?> Login(LoginRequestDto loginRequestDto);
     public Task<TokenResponseDto?> RefreshToken(RefreshTokenDto refreshToken);
     public Task<UserDto?> ChangeUserRank(UserRank userRank);
     public Task<UserDto?> SetUserPhoto(IFormFile photo, int idUser);
     public Task<UserPhotoResponseDto?> GetUserPhoto(int idUser);
     public Task<bool?> ChangePassword(ChangePasswordDto changePasswordDto, int idUser);
     public Task<bool> ChangePassword(ChangePasswordShortDto changePasswordShortDto, int idUser);
}