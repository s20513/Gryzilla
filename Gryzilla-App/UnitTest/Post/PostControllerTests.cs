using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Post;
using Gryzilla_App.DTOs.Responses.PostComment;
using Gryzilla_App.DTOs.Responses.Posts;
using Gryzilla_App.Exceptions;
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
            Content = "XYZ",
            CreatedAt = DateTime.Now,
            Likes = 1
        },
    };
    
    private readonly PostQtyDto _fakeQtyPost = new PostQtyDto
    {
        Posts = new PostDto []
        {
            new PostDto
            {
                idPost = 1,
                Nick ="Ola",
                CommentsDtos = new List<PostCommentDto>()
                {
                    new PostCommentDto
                    {
                        IdPost = 1,
                        Content = "Description",
                        IdUser = 1,
                    }
                },
                Content = "XYZ",
                CreatedAt = DateTime.Now,
                Likes = 1 
            }
        },
        IsNext = false
    };
    private readonly PostQtyDto _fakeQtyPosts = new PostQtyDto
    {
        Posts = new PostDto []
        {
            new PostDto
            {
                idPost = 1,
                Nick ="Nick2",
                CommentsDtos = new List<PostCommentDto>()
                {
                    new PostCommentDto
                    {
                        IdPost = 1,
                        Content = "Description",
                        IdUser = 1,
                    }
                },
                Content = "XYZ",
                CreatedAt = DateTime.Now,
                Likes = 1 
            },
            new PostDto
            {
                idPost = 2,
                Nick ="Nick2",
                CommentsDtos = new List<PostCommentDto>()
                {
                    new PostCommentDto
                    {
                        IdPost = 1,
                        Content = "Description",
                        IdUser = 1,
                    }
                },
                Content = "XYZ",
                CreatedAt = DateTime.Now,
                Likes = 1 
            },
            new PostDto
            {
                idPost = 3,
                Nick ="Nick2",
                CommentsDtos = new List<PostCommentDto>()
                {
                    new PostCommentDto
                    {
                        IdPost = 1,
                        Content = "Description",
                        IdUser = 1,
                    }
                },
                Content = "XYZ",
                CreatedAt = DateTime.Now,
                Likes = 1,
                Tags = new[] { "samochody" }
            },
            new PostDto
            {
                idPost = 4,
                Nick ="Nick2",
                CommentsDtos = new List<PostCommentDto>()
                {
                    new PostCommentDto
                    {
                        IdPost = 1,
                        Content = "Description",
                        IdUser = 1,
                    }
                },
                Content = "XYZ",
                CreatedAt = DateTime.Now,
                Likes = 1 
            },
            new PostDto
            {
                idPost = 5,
                Nick ="Nick2",
                CommentsDtos = new List<PostCommentDto>()
                {
                    new PostCommentDto
                    {
                        IdPost = 1,
                        Content = "Description",
                        IdUser = 1,
                    }
                },
                Content = "XYZ",
                CreatedAt = DateTime.Now,
                Likes = 1 
            },
            new PostDto
            {
                idPost = 6,
                Nick ="Nick2",
                CommentsDtos = new List<PostCommentDto>()
                {
                    new PostCommentDto
                    {
                        IdPost = 1,
                        Content = "Description",
                        IdUser = 1,
                    }
                },
                Content = "XYZ",
                CreatedAt = DateTime.Now,
                Likes = 1 
            }
        },
        IsNext = false
    };
    
    private readonly IEnumerable<PostDto> _fakePosts = new List<PostDto>
    {
        new()
        { 
            idPost = 1,
            Nick ="Ola",
            CommentsDtos = new List<PostCommentDto>()
            {
                new PostCommentDto
                {
                    IdPost = 1,
                    Content = "Description",
                    IdUser = 1,
                }
            },
            Content = "XYZ",
            CreatedAt = DateTime.Now,
            Likes = 1
        },
        new()
        {
            idPost = 2,
            Nick ="Mateusz",
            CommentsDtos = new List<PostCommentDto>()
            {
                new PostCommentDto
                {
                    IdPost = 1,
                    Content = "Description",
                    IdUser = 1,
                }
            },
            Content = "XYZ",
            CreatedAt = DateTime.Now,
            Likes = 1,
            Tags = new []{"samochody"}
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
    public async void GetQtyPostByMostLikesFromDb_Returns_Ok()
    {
        //Arrange
        DateTime time = DateTime.Now;
        _postRepositoryMock
            .Setup(x => x.GetQtyPostsByLikesFromDb(5, time))
            .ReturnsAsync(_fakeQtyPost);

        //Act
        var actionResult = await _postsController.GetPostsByLikesMost(5, time);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostQtyDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyPost, resultValue);
    }
    [Fact]
    public async void GetQtyPostByMostLikesFromDb_Returns_WrongNumberException_Bad_Request()
    {
        //Arrange
        
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";
        DateTime time = DateTime.Now;
        _postRepositoryMock
            .Setup(x => x.GetQtyPostsByLikesFromDb(5, time))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));

        //Act
        var actionResult = await _postsController.GetPostsByLikesMost(5, time);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue);
    }
    
    [Fact]
    public async void GetQtyPostByMostLikesFromDb_Not_Found()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        DateTime time = DateTime.Now;
        _postRepositoryMock.Setup(x => x.GetPostsByLikesFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByLikesMost(5, time);
        
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
    public async void GetQtyPostsByCommentsFromDb_Returns_Ok()
    {
        //Arrange
        DateTime time = DateTime.Now;
        _postRepositoryMock
            .Setup(x => x.GetQtyPostsByCommentsFromDb(5, time))
            .ReturnsAsync(_fakeQtyPosts);

        //Act
        var actionResult = await _postsController.GetPostsByComments(5, time);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostQtyDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyPosts, resultValue);
    }
    [Fact]
    public async void GetQtyPostByCommentsFromDb_Returns_WrongNumberException_Bad_Request()
    {
        //Arrange
        
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";
        DateTime time = DateTime.Now;
        _postRepositoryMock
            .Setup(x => x.GetQtyPostsByCommentsFromDb(5, time))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));

        //Act
        var actionResult = await _postsController.GetPostsByComments(5, time);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue);
    }

    [Fact]
    public async void GetQtyPostsByCommentsFromDb_Not_Found()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        DateTime time = DateTime.Now;
        _postRepositoryMock.Setup(x => x.GetPostsByCommentsFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByComments(5, time);
        
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
    public async void GetPostsByCommentsFromDb_Returns_Ok()
    {
        //Arrange
        _postRepositoryMock
            .Setup(x => x.GetPostsByCommentsFromDb())
            .ReturnsAsync(_fakePosts.OrderBy(x => x.CommentsNumber));

        //Act
        var actionResult = await _postsController.GetPostsByComments();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<PostDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakePosts.OrderByDescending(x => x.CommentsNumber), resultValue);
    }
    [Fact]
    public async void GetPostsByCommentsFromDb_Not_Found()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        
        _postRepositoryMock.Setup(x => x.GetPostsByCommentsFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByComments();
        
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
    public async void GetQtyPostByLatestDateFromDb_Returns_Ok()
    {
        //Arrange
        DateTime time = DateTime.Now;
        _postRepositoryMock
            .Setup(x => x.GetQtyPostsByDateFromDb(5, time))
            .ReturnsAsync(_fakeQtyPost);

        //Act
        var actionResult = await _postsController.GetPostsByDates(5, time);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostQtyDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyPost, resultValue);
    }
    
    [Fact]
    public async void GetQtyPostByLatestDateFromDb_Returns_WrongNumberException_Bad_Request()
    {
        //Arrange
        DateTime time = DateTime.Now;
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";

        _postRepositoryMock
            .Setup(x => x.GetQtyPostsByDateFromDb(5,time))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));

        //Act
        var actionResult = await _postsController.GetPostsByDates(5,time);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue);
    }
    [Fact]
    public async void GetQtyPostByLatestDateFromDb_Not_Found()
    {
        //Arrange
        PostQtyDto? nullValue = null;
        DateTime time = DateTime.Now;
        _postRepositoryMock.Setup(x => x.GetQtyPostsByDateFromDb(5,time)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByDates(5,time);
        
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
    public async void GetQtyPostByOldestDateFromDb_Returns_Ok()
    {
        //Arrange
        DateTime time = DateTime.Now;
        _postRepositoryMock
            .Setup(x => x.GetQtyPostsByDateOldestFromDb(5, time))
            .ReturnsAsync(_fakeQtyPosts);

        //Act
        var actionResult = await _postsController.GetPostsByDatesOldest(5, time);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostQtyDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyPosts, resultValue);
    }
    
    [Fact]
    public async void GetQtyPostByOldestDateFromDb_Returns_WrongNumberException_Bad_Request()
    {
        //Arrange
        DateTime time = DateTime.Now;
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";

        _postRepositoryMock
            .Setup(x => x.GetQtyPostsByDateOldestFromDb(5, time))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));

        //Act
        var actionResult = await _postsController.GetPostsByDatesOldest(5, time);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue);
    }
    [Fact]
    public async void GetQtyPostByOldestDateFromDb_Not_Found()
    {
        //Arrange
        PostQtyDto nullValue = null;
        DateTime time = DateTime.Now;
        _postRepositoryMock.Setup(x => x.GetQtyPostsByDateOldestFromDb(5, time)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByDatesOldest(5,time);
        
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
    public async void GetQtyPostsFromDb_Returns_WrongNumberException_Bad_Request()
    {
        //Arrange
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";

        _postRepositoryMock
            .Setup(x => x.GetQtyPostsFromDb(5))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));

        //Act
        var actionResult = await _postsController.GetQtyPosts(5);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue);
    }
    [Fact]
    public async void GetQtyPostsFromDb_Returns_Null()
    {
        //Arrange
        PostQtyDto nullValue = null;
        
        _postRepositoryMock.Setup(x => x.GetQtyPostsFromDb(5)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetQtyPosts(5);
        
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
    public async void GetQtyPostsFromDb_Returns_ListOfPosts()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        
        //Arrange
        _postRepositoryMock
            .Setup(x => x.GetQtyPostsFromDb(5))
            .ReturnsAsync(_fakeQtyPost);

        //Act
        var actionResult = await _postsController.GetQtyPosts(5);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostQtyDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyPost, resultValue);
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
    
    [Fact]
    public async void GetTopPost_Not_Found()
    {
        //Arrange
        IEnumerable<PostDto>? nullValue = null;
        
        _postRepositoryMock.Setup(x => x.GetTopPosts()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetTopPosts();
        
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
    public async void GetTopPost_Returns_Ok()
    {
        //Arrange
        _postRepositoryMock
            .Setup(x => x.GetTopPosts())
            .ReturnsAsync(_fakePosts.OrderByDescending(x => x.Likes));

        //Act
        var actionResult = await _postsController.GetTopPosts();
        
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
    public async void GetUserPost_Returns_Posts()
    {
        //Arrange
        IEnumerable<PostDto> postDtos = new List<PostDto>();
        var userId = 1;
        
        _postRepositoryMock.Setup(x => x.GetUserPostsFromDb(userId)).ReturnsAsync(postDtos);

        //Act
        var actionResult = await _postsController.GetUserPost(userId);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<PostDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(postDtos, resultValue);
    }
    
     [Fact] 
    public async void GetQtyPostsByTagFromDb_Returns_WrongNumberException_Bad_Request()
    {
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";
        
        DateTime time = DateTime.Now;
        
        _postRepositoryMock
            .Setup(x => x.GetPostsByTagFromDb(4, time, "Samochody"))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));
        //Act
        var actionResult = await _postsController.GetPostsByTag(4, time, "Samochody");
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue);
    }
    [Fact]
    public async void GetQtyPostsByTagFromDb_Returns_Null()
    {
        //Arrange
        PostQtyDto nullValue = null;
        
        _postRepositoryMock.Setup(x => x.GetPostsByTagFromDb(5, DateTime.Now, "Samochody")).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _postsController.GetPostsByTag(5,DateTime.Now, "Samochody1");
        
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
    public async void GetQtyPostsByTagFromDb_Returns_ListOfPosts()
    {
        DateTime time = DateTime.Now;
        //Arrange
        _postRepositoryMock
            .Setup(x => x.GetPostsByTagFromDb(5, time, "samochody"))
            .ReturnsAsync(_fakeQtyPosts);

        //Act
        var actionResult = await _postsController.GetPostsByTag(5, time, "samochody");
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostQtyDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyPosts, resultValue);
    }
}