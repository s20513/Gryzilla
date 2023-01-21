using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Tag;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class TagDbRepository: ITagDbRepository
{
    //spiąć w program
    private readonly GryzillaContext _context;

    public TagDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    public async Task<FullTagDto?> GetTagFromDb(int idTag)
    {
        var tag = await _context
            .Tags
            .Where(x => x.IdTag == idTag)
            .Select(x => new FullTagDto
            {
                Id = x.IdTag,
                Name = x.NameTag
            })
            .SingleOrDefaultAsync();

        return tag;
    }

    public async Task<IEnumerable<FullTagDto>?> GetTagsFromDb()
    {
        var tags = await _context
            .Tags
            .Select(x => new FullTagDto
            {
                Id = x.IdTag,
                Name = x.NameTag
            })
            .ToArrayAsync();

        return tags.Length == 0 ? null : tags;
    }

    public async Task<FullTagDto> AddTagToDb(NewTagDto newTagDto)
    {
        int id;
        Tag newTag;
        var sameNameTag = await _context
            .Tags
            .Where(x => x.NameTag == newTagDto.Name)
            .SingleOrDefaultAsync();
        
        if (sameNameTag is not null)
        {
            throw new SameNameException("Tag with given name already exists!");
        }

        newTag = new Tag
        {
            NameTag = newTagDto.Name
        };

        await _context.Tags.AddAsync(newTag);
        await _context.SaveChangesAsync();

        id = _context.Tags.Max(x => x.IdTag);
        
        return new FullTagDto
        {
            Id = id,
            Name = newTag.NameTag
        };
    }

    public async Task<IEnumerable<FullTagDto>?> GetTagsStartingWithParamFromDb(string startOfTagName)
    {
        var tags = await _context
            .Tags
            .Where(x => x.NameTag.StartsWith(startOfTagName))
            .Select(x => new FullTagDto
            {
                Id = x.IdTag,
                Name = x.NameTag
            })
            .ToArrayAsync();

        return tags.Length == 0 ? null : tags;
    }
}