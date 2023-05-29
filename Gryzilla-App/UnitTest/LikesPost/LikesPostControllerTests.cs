using System.Security.Claims;
using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.LikesPost;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.LikesPost;

public class LikesPostControllerTests
{
    private readonly LikesPostController _likesPostController;
    private readonly Mock<ILikesPostDbRepository> _likesPostDbRepositoryMock = new();
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public LikesPostControllerTests()
    {
        _likesPostController = new LikesPostController(_likesPostDbRepositoryMock.Object);
        
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        _mockClaimsPrincipal.Setup(x => x.Claims).Returns(new List<Claim>());

        _likesPostController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = _mockClaimsPrincipal.Object }
        };
    }
    
    [Fact]
    public async void AddNewLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idPost = 1;
        var stringResult = "Added like";
        
        _likesPostDbRepositoryMock
            .Setup(x => x.AddLikeToPost(idUser, idPost))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _likesPostController.AddNewLike(idUser, idPost);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(stringResult, resultValue.Message);
    }
    
    [Fact]
    public async void AddNewLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idPost = 1;
        var stringResult = "Post or user doesn't exist";
        
        _likesPostDbRepositoryMock
            .Setup(x => x.AddLikeToPost(idUser, idPost))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _likesPostController.AddNewLike(idUser, idPost);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(stringResult, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idPost = 1;
        var stringResult = "Deleted like";
        
        _likesPostDbRepositoryMock
            .Setup(x => x.DeleteLikeFromPost(idUser, idPost, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _likesPostController.DeleteLike(idUser, idPost);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(stringResult, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idPost = 1;
        var stringResult = "Post or user doesn't exist";
        
        _likesPostDbRepositoryMock
            .Setup(x => x.DeleteLikeFromPost(idUser, idPost, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _likesPostController.DeleteLike(idUser, idPost);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(stringResult, resultValue.Message);
    }
    
    [Fact]
    public async void GetLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idPost = 1;
        var boolResult = new LikesPostDto
        {
            liked = true
        };
        
        _likesPostDbRepositoryMock
            .Setup(x => x.ExistLike(idUser, idPost))
            .ReturnsAsync(boolResult);

        //Act
        var actionResult = await _likesPostController.GetLike(idUser, idPost);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as LikesPostDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.True(resultValue.liked);
    }
    
    [Fact]
    public async void GetLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idPost = 1;
        LikesPostDto boolResult = null;
        
        _likesPostDbRepositoryMock
            .Setup(x => x.ExistLike(idUser, idPost))
            .ReturnsAsync(boolResult);

        //Act
        var actionResult = await _likesPostController.GetLike(idUser, idPost);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Post or user doesn't exist", resultValue.Message);
    }
}