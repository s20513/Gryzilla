using Gryzilla_App.Controllers;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.LikesArticle;

public class LikesArticleControllerTests
{
    private readonly LikesArticleController _articleController;
    private readonly Mock<ILikesArticleDbRepository> _likesArticleDbRepositoryMock = new();

    public LikesArticleControllerTests()
    {
        _articleController = new LikesArticleController(_likesArticleDbRepositoryMock.Object);
    }

    [Fact]
    public async void AddNewLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        object objectResult = "Added like";
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.AddLikeToArticle(idUser, idArticle))
            .ReturnsAsync(objectResult);

        //Act
        var actionResult = await _articleController.AddNewLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(objectResult, resultValue);
    }
    
    [Fact]
    public async void AddNewLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        object objectResult = "Article or user doesn't exist";
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.AddLikeToArticle(idUser, idArticle))
            .ReturnsAsync(objectResult);

        //Act
        var actionResult = await _articleController.AddNewLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(objectResult, resultValue);
    }
    
    [Fact]
    public async void DeleteLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        object objectResult = "Deleted like";
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.DeleteLikeFromArticle(idUser, idArticle))
            .ReturnsAsync(objectResult);

        //Act
        var actionResult = await _articleController.DeleteLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(objectResult, resultValue);
    }
    
    [Fact]
    public async void DeleteLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        object objectResult = "Article or user doesn't exist";
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.DeleteLikeFromArticle(idUser, idArticle))
            .ReturnsAsync(objectResult);

        //Act
        var actionResult = await _articleController.DeleteLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(objectResult, resultValue);
    }
    
    [Fact]
    public async void ExistLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var boolResult = true;
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.ExistLikeArticle(idUser, idArticle))
            .ReturnsAsync(boolResult);

        //Act
        var actionResult = await _articleController.ExistLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as bool?;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.True(resultValue);
    }
    
    [Fact]
    public async void ExistLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        bool? boolResult = null;
        
        _likesArticleDbRepositoryMock
            .Setup(x => x.ExistLikeArticle(idUser, idArticle))
            .ReturnsAsync(boolResult);

        //Act
        var actionResult = await _articleController.ExistLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Article or user doesn't exist", resultValue);
    }
}