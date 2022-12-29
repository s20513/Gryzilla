using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Post;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Post;

public class PostControllerTests
{
    private readonly PostController _postsController;
    private readonly Mock<IPostDbRepository> _postRepositoryMock = new();
    
    
    public PostControllerTests()
    {
        _postsController = new PostController(_postRepositoryMock.Object);
    }

    private readonly IEnumerable<OnePostDto> _fakePost = new List<OnePostDto>
    {
        new()
        { 
            idPost = 1,
            Nick ="Ola",
            Title = "Test",
            Content = "XYZ",
            CreatedAt = DateTime.Now,
            Likes = 1
        },
    };
    
    private readonly IEnumerable<PostDto> _fakePosts = new List<PostDto>
    {
        new()
        { 
            idPost = 1,
            Nick ="Ola",
            Title = "Test",
            Comments = 1,
            Content = "XYZ",
            CreatedAt = DateTime.Now,
            Likes = 1
        },
        new()
        {
            idPost = 2,
            Nick ="Mateusz",
            Title = "Test1",
            Comments = 1,
            Content = "XYZ",
            CreatedAt = DateTime.Now,
            Likes = 1
        }
    };
    [Fact]
    public async void GetPost_Returns_Ok()
    {
        //Arrange
        _postRepositoryMock.Setup(x => x.GetPostsFromDb()).ReturnsAsync(_fakePosts);

        //Act
        var actionResult = await _postsController.GetPosts();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<PostDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakePosts, resultValue);
    }
    
    [Fact]
    public async void GetPost_Not_Found()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        
        _postRepositoryMock.Setup(x => x.GetPostsFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPosts();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No posts found", resultValue);
    }
    [Fact]
    public async void GetPostByMostLikesFromDb_Returns_Ok()
    {
        //Arrange
        _postRepositoryMock
            .Setup(x => x.GetPostsByLikesFromDb())
            .ReturnsAsync(_fakePosts.OrderByDescending(x => x.Likes));

        //Act
        var actionResult = await _postsController.GetPostsByLikesMost();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<PostDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakePosts.OrderByDescending(x => x.Likes), resultValue);
    }
    [Fact]
    public async void GetPostByMostLikesFromDb_Not_Found()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        
        _postRepositoryMock.Setup(x => x.GetPostsByLikesFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByLikesMost();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No posts found", resultValue);
    }
    [Fact]
    public async void GetPostByLeastLikesFromDb_Returns_Ok()
    {
        //Arrange
        _postRepositoryMock
            .Setup(x => x.GetPostsByLikesLeastFromDb())
            .ReturnsAsync(_fakePosts.OrderBy(x => x.Likes));

        //Act
        var actionResult = await _postsController.GetPostsByLikes();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<PostDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakePosts.OrderBy(x => x.Likes), resultValue);
    }
    [Fact]
    public async void GetPostByLeastLikesFromDb_Not_Found()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        
        _postRepositoryMock.Setup(x => x.GetPostsByLikesLeastFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByLikes();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No posts found", resultValue);
    }
    [Fact]
    public async void GetPostByLatestDateFromDb_Returns_Ok()
    {
        //Arrange
        _postRepositoryMock
            .Setup(x => x.GetPostsByDateFromDb())
            .ReturnsAsync(_fakePosts.OrderByDescending(x => x.CreatedAt));

        //Act
        var actionResult = await _postsController.GetPostsByDates();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<PostDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakePosts.OrderByDescending(x => x.CreatedAt), resultValue);
    }
    [Fact]
    public async void GetPostByLatestDateFromDb_Not_Found()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        
        _postRepositoryMock.Setup(x => x.GetPostsByDateFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByDates();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No posts found", resultValue);
    }
    [Fact]
    public async void GetPostByOldestDateFromDb_Returns_Ok()
    {
        //Arrange
        _postRepositoryMock
            .Setup(x => x.GetPostsByDateOldestFromDb())
            .ReturnsAsync(_fakePosts.OrderBy(x => x.CreatedAt));

        //Act
        var actionResult = await _postsController.GetPostsByDatesOldest();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<PostDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakePosts.OrderBy(x => x.CreatedAt), resultValue);
    }
    [Fact]
    public async void GetPostByOldestDateFromDb_Not_Found()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        
        _postRepositoryMock.Setup(x => x.GetPostsByDateOldestFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByDatesOldest();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No posts found", resultValue);
    }
    
    [Fact]
    public async void GetPostFromDb_Returns_Ok()
    {
        //Arrange
        var idPost = 1;
        
        _postRepositoryMock
            .Setup(x => x.GetOnePostFromDb(idPost))
            .ReturnsAsync(_fakePost.Single(x=>x.idPost == idPost));

        //Act
        var actionResult = await _postsController.GetOnePost(idPost);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as OnePostDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(idPost, resultValue.idPost);
    }
    
    [Fact]
    public async void GetPostFromDb_Returns_Not_Found()
    {
        //Arrange
        var idPost = 10;
        OnePostDto? nullValue = null;

        _postRepositoryMock
            .Setup(x => x.GetOnePostFromDb(idPost))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetOnePost(idPost);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Post with this Id doesn't exist", resultValue);
    }
    [Fact]
    public async void AddNewPostToDb_Returns_Ok()
    {
        //Arrange
        var idPost = 4;
        var newPost = new AddPostDto();
        var returnedPost = new NewPostDto
        {
            IdPost = idPost
        };
        
        _postRepositoryMock
            .Setup(x => x.AddNewPostToDb(newPost))
            .ReturnsAsync(returnedPost);

        //Act
        var actionResult = await _postsController.AddPost(newPost);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as NewPostDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(idPost, returnedPost.IdPost);
    }
    
    [Fact]
    public async void AddNewPostToDb_Returns_Not_Found()
    {
        //Arrange
        var newPost = new AddPostDto();
        NewPostDto? nullValue = null;
        
        _postRepositoryMock
            .Setup(x => x.AddNewPostToDb(newPost))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.AddPost(newPost);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot add new post", resultValue);
    }
      [Fact]
    public async void ModifyPostFromDb_Returns_Ok()
    {
        //Arrange
        var idPost = 10;
        var putPostRequestDto = new PutPostDto()
        {
            IdPost = idPost
        };
        
        var returnedPost = new ModifyPostDto()
        {
            IdPost = idPost
        };
        
        _postRepositoryMock
            .Setup(x => x.ModifyPostFromDb(putPostRequestDto, idPost))
            .ReturnsAsync(returnedPost);

        //Act
        var actionResult = await _postsController.ModifyPost(putPostRequestDto, idPost);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ModifyPostDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(idPost, resultValue.IdPost);
    }
    
    [Fact]
    public async void ModifyPostFromDb_Returns_Not_Found()
    {
        //Arrange
        var idPost = 10;
        var putPostRequestDto = new PutPostDto()
        {
            IdPost = idPost
        };
        ModifyPostDto? nullValue = null;

        _postRepositoryMock
            .Setup(x => x.ModifyPostFromDb(putPostRequestDto, idPost))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.ModifyPost(putPostRequestDto, idPost);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot modify post", resultValue);
    }
    
    [Fact]
    public async void ModifyPostFromDb_Returns_Bad_Request()
    {
        //Arrange
        var idPost = 10;
        var putPostRequestDto = new PutPostDto()
        {
            IdPost = 11
        };

        //Act
        var actionResult = await _postsController.ModifyPost(putPostRequestDto, idPost);
        
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
    public async void DeletePostFromDb_Returns_Ok()
    {
        //Arrange
        var idPost = 10;
        var returnedPost = new DeletePostDto()
        {
            IdPost = idPost
        };
        
        _postRepositoryMock
            .Setup(x => x.DeletePostFromDb(idPost))
            .ReturnsAsync(returnedPost);

        //Act
        var actionResult = await _postsController.DeletePost(idPost);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as DeletePostDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(idPost, resultValue.IdPost);
    }
    
    [Fact]
    public async void DeletePostFromDb_Returns_Not_Found()
    {
        //Arrange
        var idPost = 10;
        DeletePostDto? nullValue = null;

        _postRepositoryMock
            .Setup(x => x.DeletePostFromDb(idPost))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.DeletePost(idPost);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot delete new post", resultValue);
    }
    [Fact]
    public async void DeleteTagFromPost_Returns_Ok()
    {
        //Arrange
        var idPost = 10;
        var idTag = 1;
        
        var returnedPost = new DeleteTagDto()
        {
            IdTag = idTag
        };
        
        _postRepositoryMock
            .Setup(x => x.DeleteTagFromPost(idPost, idTag))
            .ReturnsAsync(returnedPost);

        //Act
        var actionResult = await _postsController.DeleteTagFromPost(idPost, idTag);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as DeleteTagDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(idTag, resultValue.IdTag);
    }
    
    [Fact]
    public async void DeleteTagFromPost_Returns_Not_Found()
    {
        //Arrange
        var idPost = 10;
        var idTag = 1;
        DeleteTagDto? nullValue = null;

        _postRepositoryMock
            .Setup(x => x.DeleteTagFromPost(idPost, idTag))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.DeleteTagFromPost(idPost, idTag);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot delete tag", resultValue);
    }
}