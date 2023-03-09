using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.ArticleComment;
using Gryzilla_App.DTOs.Responses.ArticleComment;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.CommentArticle;

public class CommentArticleControllerTests
{
    private readonly CommentArticleController _commentArticleController;
    private readonly Mock<ICommentArticleDbRepository> _commentArticleRepositoryMock = new();

    public CommentArticleControllerTests()
    {
        _commentArticleController = new CommentArticleController(_commentArticleRepositoryMock.Object);
    }
    
    [Fact]
    public async void CreateNewArticleComment_Returns_Ok()
    {
        //Arrange
        var newArticleCommentDto = new NewArticleCommentDto();
        var returnedComment = new ArticleCommentDto();
        
        _commentArticleRepositoryMock
            .Setup(x => x.AddCommentToArticle(newArticleCommentDto))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _commentArticleController.CreateNewArticleComment(newArticleCommentDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ArticleCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void CreateNewArticleComment_Returns_Not_Found()
    {
        //Arrange
        var newArticleCommentDto = new NewArticleCommentDto();
        ArticleCommentDto? nullValue = null;
        
        _commentArticleRepositoryMock
            .Setup(x => x.AddCommentToArticle(newArticleCommentDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentArticleController.CreateNewArticleComment(newArticleCommentDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User or article not found", resultValue);
    }
    
    [Fact]
    public async void ModifyArticleComment_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var putArticleCommentDto = new PutArticleCommentDto
        {
            IdComment = id
        };
        var returnedComment = new ArticleCommentDto();
        
        _commentArticleRepositoryMock
            .Setup(x => x.ModifyArticleCommentFromDb(putArticleCommentDto, id))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _commentArticleController.ModifyArticleComment(putArticleCommentDto, id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ArticleCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void ModifyArticleComment_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        var putArticleCommentDto = new PutArticleCommentDto
        {
            IdComment = id
        };
        ArticleCommentDto? nullValue = null;
        
        _commentArticleRepositoryMock
            .Setup(x => x.ModifyArticleCommentFromDb(putArticleCommentDto, id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentArticleController.ModifyArticleComment(putArticleCommentDto, id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Comment not found", resultValue);
    }
    
    [Fact]
    public async void ModifyArticleComment_Returns_Bad_Request()
    {
        //Arrange
        var id = 1;
        var putArticleCommentDto = new PutArticleCommentDto();
        ArticleCommentDto? nullValue = null;
        
        _commentArticleRepositoryMock
            .Setup(x => x.ModifyArticleCommentFromDb(putArticleCommentDto, id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentArticleController.ModifyArticleComment(putArticleCommentDto, id);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Id from route and Id in body have to be same", resultValue);
    }
    
    [Fact]
    public async void DeleteArticleComment_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var returnedComment = new ArticleCommentDto();
        
        _commentArticleRepositoryMock
            .Setup(x => x.DeleteArticleCommentFromDb(id))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _commentArticleController.DeleteArticleComment(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ArticleCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void DeleteArticleComment_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        ArticleCommentDto? nullValue = null;
        
        _commentArticleRepositoryMock
            .Setup(x => x.DeleteArticleCommentFromDb(id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentArticleController.DeleteArticleComment(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Comment not found", resultValue);
    }
    
    [Fact]
    public async void GetArticleComments_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var returnedComment = new GetArticleCommentDto();
        
        _commentArticleRepositoryMock
            .Setup(x => x.GetArticleCommentsFromDb(id))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _commentArticleController.GetArticleComment(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GetArticleCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void GetArticleComments_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        GetArticleCommentDto? nullValue = null;
        
        _commentArticleRepositoryMock
            .Setup(x => x.GetArticleCommentsFromDb(id))!
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentArticleController.GetArticleComment(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Comments not found", resultValue);
    }
}