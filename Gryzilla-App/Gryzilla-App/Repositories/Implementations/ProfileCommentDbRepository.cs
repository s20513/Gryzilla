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
    
    public async Task<IEnumerable<ProfileCommentDto>?> GetProfileCommentFromDb(int idUserComment)
    {
        var user = await _context
            .UserData
            .Where(x => x.IdUser == idUserComment)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        var profileComments = await _context
            .ProfileComments
            .Include(x=>x.IdUserNavigation)
            .Where(x => x.IdUserComment == idUserComment)
            .OrderByDescending(e => e.CreatedAt)
            .Select(x => new ProfileCommentDto
            {
                idProfileComment = x.IdProfileComment,
                IdUser           = x.IdUser,
                IdUserComment    = x.IdUserComment,
                Nick             = x.IdUserNavigation.Nick,
                Type             = x.IdUserNavigation.PhotoType,
                base64PhotoData  = Convert.ToBase64String(x.IdUserNavigation.Photo ?? Array.Empty<byte>()), 
                Content          = x.Description,
                CreatedAt        = x.CreatedAt
            }).ToArrayAsync();
        
        return profileComments;
    }

    public async Task <ProfileCommentDto?> AddProfileCommentToDb(NewProfileComment newProfileComment)
    {
        var user = await _context
            .UserData
            .Where(x => x.IdUser == newProfileComment.IdUser)
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
            IdUser        = newProfileComment.IdUser,
            IdUserComment = newProfileComment.IdUserComment,
            Description   = newProfileComment.Content,
            CreatedAt     = DateTime.Now
        };

        await _context.ProfileComments.AddAsync(profileComment);
        await _context.SaveChangesAsync();
        
        int profileCommentId = _context.ProfileComments.Max(e => e.IdProfileComment);
        return new ProfileCommentDto
        {
            idProfileComment = profileCommentId,
            IdUser           = newProfileComment.IdUser,
            IdUserComment    = newProfileComment.IdUserComment,
            Nick             = user.Nick,
            Content          = newProfileComment.Content,
            CreatedAt        = profileComment.CreatedAt
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
            idProfileComment = profileComment.IdProfileComment,
            IdUser           = profileComment.IdUser,
            IdUserComment    = profileComment.IdUserComment,
            Nick             = _context.UserData
                .Where(e => e.IdUser == profileComment.IdUser)
                .Select(e => e.Nick).SingleOrDefault(),
            Content          = profileComment.Description
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

        profileComment.Description = modifyProfileComment.Content;
        await _context.SaveChangesAsync();
        
       return new ProfileCommentDto
        {
            idProfileComment = profileComment.IdProfileComment,
            IdUser           = profileComment.IdUser,
            IdUserComment    = profileComment.IdUserComment,
            Nick             = _context.UserData
                .Where(e => e.IdUser == profileComment.IdUser)
                .Select(e => e.Nick).SingleOrDefault(),
            Content         = profileComment.Description,
            CreatedAt       = profileComment.CreatedAt
        };
        
    }
}