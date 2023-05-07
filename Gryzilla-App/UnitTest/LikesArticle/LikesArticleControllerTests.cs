using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.LikesArticle;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.LikesArticle;

public class LikesArticleControllerTests
{
    private readonly LikesArticleController _likesArticleController;
    private readonly Mock<ILikesArticleDbRepository> _likesArticleDbRepositoryMock = new();

    public LikesArticleControllerTests()
    {
        _likesArticleController = new LikesArticleController(_likesArticleDbRepositoryMock.Object);
    }

    [Fact]
    public async void AddNewLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var objectResult = "Added like";
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.AddLikeToArticle(idUser, idArticle))
            .ReturnsAsync(objectResult);

        //Act
        var actionResult = await _likesArticleController.AddNewLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(objectResult, resultValue.Message);
    }
    
    [Fact]
    public async void AddNewLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var objectResult = "Article or user doesn't exist";
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.AddLikeToArticle(idUser, idArticle))
            .ReturnsAsync(objectResult);

        //Act
        var actionResult = await _likesArticleController.AddNewLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(objectResult, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var objectResult = "Deleted like";
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.DeleteLikeFromArticle(idUser, idArticle))
            .ReturnsAsync(objectResult);

        //Act
        var actionResult = await _likesArticleController.DeleteLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(objectResult, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var objectResult = "Article or user doesn't exist";
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.DeleteLikeFromArticle(idUser, idArticle))
            .ReturnsAsync(objectResult);

        //Act
        var actionResult = await _likesArticleController.DeleteLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(objectResult, resultValue.Message);
    }
    
    [Fact]
    public async void ExistLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var boolResult = new LikesArticleDto
        {
            liked = true
        };
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.ExistLikeArticle(idUser, idArticle))
            .ReturnsAsync(boolResult);

        //Act
        var actionResult = await _likesArticleController.ExistLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as LikesArticleDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.True(resultValue.liked);
    }
    
    [Fact]
    public async void ExistLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        LikesArticleDto boolResult = null;
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.ExistLikeArticle(idUser, idArticle))
            .ReturnsAsync(boolResult);

        //Act
        var actionResult = await _likesArticleController.ExistLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Article or user doesn't exist", resultValue.Message);
    }
}