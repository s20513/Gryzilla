using Gryzilla_App.DTOs.Requests.ProfileComment;
using Gryzilla_App.DTOs.Responses;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IProfileCommentDbRepository
{
    public Task<ProfileCommentDto?> DeleteProfileCommentFromDb(int idProfileComment);
    public Task<ProfileCommentDto?> AddProfileCommentToDb(NewProfileComment newProfileComment);
    public Task<IEnumerable<ProfileCommentDto>?> GetProfileCommentFromDb(int idUserComments);

    public Task<ProfileCommentDto?> ModifyProfileCommentFromDb(int idProfileComment, ModifyProfileComment modifyProfileComment);
}