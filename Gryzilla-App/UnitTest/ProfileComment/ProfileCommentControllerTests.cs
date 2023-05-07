using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.ProfileComment;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.ProfileComment;

public class ProfileCommentControllerTests
{
    private readonly ProfileCommentController _profileCommentController;
    private readonly Mock<IProfileCommentDbRepository> _profileRepositoryMock = new();

    public ProfileCommentControllerTests()
    {
        _profileCommentController= new ProfileCommentController(_profileRepositoryMock.Object);
    }
    [Fact]
    public async void GetProfileComments_Returns_Ok()
    {
        //Arrange
        var profileComment = new List<ProfileCommentDto>();

        _profileRepositoryMock.Setup(e => e.GetProfileCommentFromDb(1)).ReturnsAsync(profileComment);
        
        //Act
        var actionResult = await _profileCommentController.GetProfileComments(1);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as List<ProfileCommentDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(profileComment, resultValue);
    }
    
    [Fact]
    public async void GetProfileComment_Returns_NotFound()
    {
        //Arrange
        List<ProfileCommentDto>? profileComment = null;

        _profileRepositoryMock.Setup(e => e.GetProfileCommentFromDb(1)).ReturnsAsync(profileComment);
        
        //Act
        var actionResult = await _profileCommentController.GetProfileComments(1);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There are no comments for the profile", resultValue.Message);
    }
    
    [Fact]
    public async void CreateProfileComment_Returns_Ok()
    {
        //Arrange
        var newProfileCommentDto = new NewProfileComment();
        var returnedProfileComment = new ProfileCommentDto();
        
        _profileRepositoryMock
            .Setup(x => x.AddProfileCommentToDb(newProfileCommentDto))
            .ReturnsAsync(returnedProfileComment);

        //Act
        var actionResult = await _profileCommentController.PostProfileComment(newProfileCommentDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ProfileCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedProfileComment, resultValue);
    }

    [Fact]
    public async void CreateProfileComment_Returns_Not_Found()
    {
        //Arrange
        var newProfileCommentDto = new NewProfileComment();
        ProfileCommentDto? nullValue = null;
        
        _profileRepositoryMock
            .Setup(x => x.AddProfileCommentToDb(newProfileCommentDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _profileCommentController.PostProfileComment(newProfileCommentDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue.Message);
    }
    
    [Fact]
    public async void ModifyProfileComment_Returns_Ok()
    {
        //Arrange
        var putProfileComment = new ModifyProfileComment()
        {
            Content = "string1" 
        };
        
        var returnedComment = new ProfileCommentDto();
        
        _profileRepositoryMock
            .Setup(x => x.ModifyProfileCommentFromDb(1, putProfileComment))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _profileCommentController.ModifyProfileComment( 1, putProfileComment );
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ProfileCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void ModifyProfileComment_Returns_Not_Found()
    {
        //Arrange
        var putProfileComment = new ModifyProfileComment()
        {
            Content = "string1" 
        };

        ProfileCommentDto? nullValue = null;
        
        _profileRepositoryMock
            .Setup(x => x.ModifyProfileCommentFromDb(1, putProfileComment))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _profileCommentController.ModifyProfileComment( 1, putProfileComment );
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no comment for the profile", resultValue.Message);
    }
    
    [Fact]
    public async void DeleteProfileComment_Returns_Ok()
    {
        //Arrange
        var returnedComment = new ProfileCommentDto();
        
        _profileRepositoryMock
            .Setup(x => x.DeleteProfileCommentFromDb(1))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _profileCommentController.DeleteProfileComment( 1);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ProfileCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void DeleteProfileComment_Returns_Not_Found()
    {
        //Arrange
        ProfileCommentDto? nullValue = null;
        
        _profileRepositoryMock
            .Setup(x => x.DeleteProfileCommentFromDb(1))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _profileCommentController.DeleteProfileComment( 1);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Profile comment doesn't exist", resultValue.Message);
    }
}

