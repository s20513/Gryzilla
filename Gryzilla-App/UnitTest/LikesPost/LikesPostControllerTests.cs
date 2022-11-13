using Gryzilla_App.Controllers;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.LikesPost;

public class LikesPostControllerTests
{
    private readonly LikesPostController _likesPostController;
    private readonly Mock<ILikesPostDbRepository> _likesPostDbRepositoryMock = new();

    public LikesPostControllerTests()
    {
        _likesPostController = new LikesPostController(_likesPostDbRepositoryMock.Object);
    }
    
    [Fact]
    public async void AddNewLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var stringResult = "Added like";
        
        _likesPostDbRepositoryMock
            .Setup(x => x.AddLikeToPost(idUser, idArticle))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _likesPostController.AddNewLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(stringResult, resultValue);
    }
    
    [Fact]
    public async void AddNewLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var stringResult = "Post or user doesn't exist";
        
        _likesPostDbRepositoryMock
            .Setup(x => x.AddLikeToPost(idUser, idArticle))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _likesPostController.AddNewLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(stringResult, resultValue);
    }
    
    [Fact]
    public async void DeleteLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var stringResult = "Deleted like";
        
        _likesPostDbRepositoryMock
            .Setup(x => x.DeleteLikeFromPost(idUser, idArticle))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _likesPostController.DeleteLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(stringResult, resultValue);
    }
    
    [Fact]
    public async void DeleteLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var stringResult = "Post or user doesn't exist";
        
        _likesPostDbRepositoryMock
            .Setup(x => x.DeleteLikeFromPost(idUser, idArticle))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _likesPostController.DeleteLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(stringResult, resultValue);
    }
    
    [Fact]
    public async void GetLike_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        var boolResult = true;
        
        _likesPostDbRepositoryMock
            .Setup(x => x.ExistLike(idUser, idArticle))
            .ReturnsAsync(boolResult);

        //Act
        var actionResult = await _likesPostController.GetLike(idUser, idArticle);
        
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
    public async void GetLike_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idArticle = 1;
        bool? boolResult = null;
        
        _likesPostDbRepositoryMock
            .Setup(x => x.ExistLike(idUser, idArticle))
            .ReturnsAsync(boolResult);

        //Act
        var actionResult = await _likesPostController.GetLike(idUser, idArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Post or user doesn't exist", resultValue);
    }
}