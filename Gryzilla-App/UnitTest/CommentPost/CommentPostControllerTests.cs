using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.PostComment;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.CommentPost;

public class CommentPostControllerTests
{
    private readonly CommentPostController _commentPostController;
    private readonly Mock<ICommentPostDbRepository> _commentPostRepositoryMock = new();

    public CommentPostControllerTests()
    {
        _commentPostController = new CommentPostController(_commentPostRepositoryMock.Object);
    }
    
    [Fact]
    public async void CreatePostComment_Returns_Ok()
    {
        //Arrange
        var newPostCommentDto = new NewPostCommentDto();
        var returnedComment = new PostCommentDto();
        
        _commentPostRepositoryMock
            .Setup(x => x.AddCommentToPost(newPostCommentDto))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _commentPostController.CreatePostComment(newPostCommentDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void CreatePostComment_Returns_Not_Found()
    {
        //Arrange
        var newPostCommentDto = new NewPostCommentDto();
        PostCommentDto? nullValue = null;
        
        _commentPostRepositoryMock
            .Setup(x => x.AddCommentToPost(newPostCommentDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentPostController.CreatePostComment(newPostCommentDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User or post not found", resultValue.Message);
    }

    [Fact]
    public async void ModifyPostComment_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var putPostCommentDto = new PutPostCommentDto
        {
            IdComment = id
        };
        var returnedComment = new PostCommentDto();
        
        _commentPostRepositoryMock
            .Setup(x => x.ModifyPostCommentFromDb(putPostCommentDto, id))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _commentPostController.ModifyPostComment(putPostCommentDto, id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void ModifyPostComment_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        var putPostCommentDto = new PutPostCommentDto
        {
            IdComment = id
        };
        PostCommentDto? nullValue = null;
        
        _commentPostRepositoryMock
            .Setup(x => x.ModifyPostCommentFromDb(putPostCommentDto, id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentPostController.ModifyPostComment(putPostCommentDto, id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Comment not found", resultValue.Message);
    }

    [Fact]
    public async void ModifyPostComment_Returns_Bad_Request()
    {
        //Arrange
        var id = 1;
        var putPostCommentDto = new PutPostCommentDto();
        PostCommentDto? nullValue = null;
        
        _commentPostRepositoryMock
            .Setup(x => x.ModifyPostCommentFromDb(putPostCommentDto, id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentPostController.ModifyPostComment(putPostCommentDto, id);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Id from route and Id in body have to be same", resultValue.Message);
    }
    
    [Fact]
    public async void DeletePostComment_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var returnedComment = new PostCommentDto();
        
        _commentPostRepositoryMock
            .Setup(x => x.DeleteCommentFromDb(id))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _commentPostController.DeletePostComment(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void DeletePostComment_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        PostCommentDto? nullValue = null;
        
        _commentPostRepositoryMock
            .Setup(x => x.DeleteCommentFromDb(id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentPostController.DeletePostComment(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Comment not found", resultValue.Message);
    }
    
    [Fact]
    public async void GetPostComment_Returns_Ok()
    {
        //Arrange
        var returnedComment = new GetPostCommentDto();
        
        _commentPostRepositoryMock
            .Setup(x => x.GetPostCommentsFromDb(1))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _commentPostController.GetPostComments(1);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GetPostCommentDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void GetPostComment_Returns_Not_Found()
    {
        //Arrange
        GetPostCommentDto? nullValue = null;
        
        _commentPostRepositoryMock
            .Setup(x => x.GetPostCommentsFromDb(1))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _commentPostController.GetPostComments(1);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Comments not found", resultValue.Message);
    }
}