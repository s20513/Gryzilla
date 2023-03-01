using Gryzilla_App.DTOs.Requests.ProfileComment;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class ProfileCommentDbRepository : IProfileCommentDbRepository
{
    private readonly GryzillaContext _context;
    
    public ProfileCommentDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ProfileCommentDto>?> GetProfileCommentFromDb(int idUser)
    {
        var user = await _context
            .UserData
            .Where(x => x.IdUser == idUser)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        var profileComments = await _context
            .ProfileComments.Where(x => x.IdUser == idUser)
            .Select(x => new ProfileCommentDto
            {
                IdUser        = x.IdUser,
                IdUserAccount = x.IdProfileComment,
                Nick          = _context.UserData.SingleOrDefault(y=>y.IdUser == x.IdProfileComment).Nick,
                Description   = x.Description
            }).ToArrayAsync();
        
        return profileComments;
    }

    public async Task <ProfileCommentDto?> AddProfileCommentToDb(int idUser, NewProfileComment newProfileComment)
    {
        var user = await _context
            .UserData
            .Where(x => x.IdUser == idUser)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        var userComment = await _context
            .UserData
            .Where(x => x.IdUser == newProfileComment.IdUserComment)
            .SingleOrDefaultAsync();

        if (userComment is null)
        {
            return null;
        }

        var profileComment = new ProfileComment
        {
            IdUser        = idUser,
            IdUserComment = newProfileComment.IdUserComment,
            Description   = newProfileComment.Description
        };

        await _context.ProfileComments.AddAsync(profileComment);
        await _context.SaveChangesAsync();
        return new ProfileCommentDto
        {
            IdUser        = idUser,
            IdUserAccount = newProfileComment.IdUserComment,
            Nick          = userComment.Nick,
            Description   = newProfileComment.Description
        };
    }

    public async Task<ProfileCommentDto?> DeleteProfileCommentFromDb(int idProfileComment)
    {
        var profileComment =
            await _context
                .ProfileComments
                .SingleOrDefaultAsync(x => x.IdProfileComment == idProfileComment);

        if (profileComment is null)
        {
            return null;
        }

        var deleteProfileComment = new ProfileCommentDto
        {
            IdUser = profileComment.IdUser,
            IdUserAccount = profileComment.IdUserComment,
            Nick = _context.UserData.SingleOrDefault(x => x.IdUser == profileComment.IdUserComment).Nick,
            Description = profileComment.Description
        };
        
        _context.ProfileComments.Remove(profileComment);
        await _context.SaveChangesAsync();
        return deleteProfileComment;
    }

    public async Task<ProfileCommentDto?> ModifyProfileCommentFromDb(int idProfileComment, ModifyProfileComment modifyProfileComment)
    {
        var profileComment =
            await _context
                .ProfileComments
                .SingleOrDefaultAsync(x => x.IdProfileComment == idProfileComment);

        if (profileComment is null)
        {
            return null;
        }

        profileComment.Description = modifyProfileComment.Description;
        await _context.SaveChangesAsync();
        
       return new ProfileCommentDto
        {
            IdUser = profileComment.IdUser,
            IdUserAccount = profileComment.IdUserComment,
            Nick = _context.UserData.SingleOrDefault(x => x.IdUser == profileComment.IdUserComment).Nick,
            Description = profileComment.Description
        };
        
    }
}