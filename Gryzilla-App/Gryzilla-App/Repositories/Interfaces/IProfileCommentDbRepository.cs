using Gryzilla_App.DTOs.Requests.ProfileComment;
using Gryzilla_App.DTOs.Responses;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IProfileCommentDbRepository
{
    public Task<ProfileCommentDto?> DeleteProfileCommentFromDb(int idProfileComment);
    public Task<ProfileCommentDto?> AddProfileCommentFromDb(int idUser, NewProfileComment newProfileComment);
    public Task<IEnumerable<ProfileCommentDto>?> GetProfileCommentFromDb(int idUser);

    public Task<ProfileCommentDto?> ModifyProfileCommentFromDb(int idProfileComment, ModifyProfileComment modifyProfileComment);
}