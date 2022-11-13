using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Tag;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.DTOs.Responses.Rank;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Tag;

public class TagControllerTests
{
    private readonly TagController _tagsController;
    private readonly Mock<ITagDbRepository> _tagRepositoryMock = new();
    
    
    public TagControllerTests()
    {
        _tagsController = new TagController(_tagRepositoryMock.Object);
    }
    
    [Fact]
    public async void GetTag_Returns_Ok()
    {
        //Arrange
        var idTag = 1;
        var tag = new FullTagDto();
        
        _tagRepositoryMock.Setup(x => x.GetTagFromDb(idTag)).ReturnsAsync(tag);

        //Act
        var actionResult = await _tagsController.GetTag(idTag);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as FullTagDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(tag, resultValue);
    }
    
    [Fact]
    public async void GetTag_Not_Found()
    {
        //Arrange
        var idTag = 1;
        FullTagDto? nullValue = null;
        
        _tagRepositoryMock.Setup(x => x.GetTagFromDb(idTag)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _tagsController.GetTag(idTag);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no tag with given id", resultValue);
    }
    
    [Fact]
    public async void GetTags_Returns_Ok()
    {
        //Arrange
        var tags= new FullTagDto[5];
        
        _tagRepositoryMock.Setup(x => x.GetTagsFromDb()).ReturnsAsync(tags);

        //Act
        var actionResult = await _tagsController.GetTags();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as FullTagDto[];
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(tags, resultValue);
    }
    
    [Fact]
    public async void GetTags_Returns_Not_Found()
    {
        //Arrange
        IEnumerable<FullTagDto>? nullValue = null;
        
        _tagRepositoryMock.Setup(x => x.GetTagsFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _tagsController.GetTags();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No tags found", resultValue);
    }
  [Fact]
    public async void CreateNewTag_Returns_Ok()
    {
        //Arrange
        var addTagDto = new NewTagDto();
        var tag= new FullTagDto();
        
        _tagRepositoryMock
            .Setup(x => x.AddTagToDb(addTagDto))
            .ReturnsAsync(tag);
        
        //Act
        var actionResult = await _tagsController.AddTag(addTagDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as FullTagDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(tag, resultValue);
    }
    
    
    [Fact]
    public async void CreateNewTag_Returns_Not_Found()
    { 
        //Arrange
        var tag= new NewTagDto();
        FullTagDto? nullValue = null;

        _tagRepositoryMock
            .Setup(x => x.AddTagToDb(tag))!
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _tagsController.AddTag(tag);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot add new tag", resultValue);
    }
    
    [Fact]
    public async void CreateNewTag_Returns_Bad_Request()
    {
        //Arrange
        var addTagDto = new NewTagDto();
        
        _tagRepositoryMock
            .Setup(x => x.AddTagToDb(addTagDto))
            .Throws(new SameNameException("Tag with given name already exists!"));

        //Act
        var actionResult = await _tagsController.AddTag(addTagDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Tag with given name already exists!", resultValue);
    }
    
}