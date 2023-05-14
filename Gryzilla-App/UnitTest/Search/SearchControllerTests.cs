using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.DTOs.Responses.PostComment;
using Gryzilla_App.DTOs.Responses.Posts;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Search;

public class SearchControllerTests
{
    private readonly SearchController _searchController;
    private readonly Mock<ISearchDbRepository> _searchRepositoryMock = new();
   
    public SearchControllerTests()
    {
        _searchController = new SearchController(_searchRepositoryMock.Object);
    }
    private readonly PostQtySearchDto _fakeQtyPost = new PostQtySearchDto
    {
        Posts = new PostSearchDto []
        {
            new PostSearchDto
            {
                idPost = 1,
                Nick ="Ola",
                Content = "XYZ to jest",
                CreatedAt = DateTime.Now,
                Likes = 1,
                Tags = new []{"XYZ"}
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
    
    private readonly UsersQtyDto _fakeQtyUsers = new UsersQtyDto
    {
        Users = new UserDto []
        {
            new UserDto
            {
                IdUser = 1,
                Nick = "Name name",
                Email = "email@gmail.com",
                IdRank = 1,
                CreatedAt = DateTime.Now
            }
        },
        IsNext = false
    };
    private readonly ArticleQtySearchDto _fakeQtyArticles = new ArticleQtySearchDto
    {
        Articles = new ArticleSearchDto []
        {
            new()
            {
                IdArticle = 1,
                Author = new ReducedUserResponseDto
                {
                    IdUser = 1,
                    Nick = "Adam"
                },
                Title = "Title1",
                Content = "Content1",
                CreatedAt = DateTime.Now,
                LikesNum = 1
            },
            new()
            {
                IdArticle = 2,
                Author = new ReducedUserResponseDto
                {
                    IdUser = 2,
                    Nick = "Ola"
                },
                Title = "Title2",
                Content = "Content2",
                CreatedAt = DateTime.Now,
                LikesNum = 2
            },
            new()
            {
                IdArticle = 3,
                Author = new ReducedUserResponseDto
                {
                    IdUser = 3,
                    Nick = "Karol"
                },
                Title = "Title3",
                Content = "Content3",
                CreatedAt = DateTime.Now,
                LikesNum = 3,
                Tags = new[] { "samochody" }
            }
        },
        IsNext = false
    };
    
    private readonly ArticleQtyDto _fakeQtyArticlesTags = new ArticleQtyDto
    {
        Articles = new ArticleDto []
        {
            new()
            {
                IdArticle = 1,
                Author = new ReducedUserResponseDto
                {
                    IdUser = 1,
                    Nick = "Adam"
                },
                Title = "Title1",
                Content = "Content1",
                CreatedAt = DateTime.Now,
                LikesNum = 1
            },
            new()
            {
                IdArticle = 2,
                Author = new ReducedUserResponseDto
                {
                    IdUser = 2,
                    Nick = "Ola"
                },
                Title = "Title2",
                Content = "Content2",
                CreatedAt = DateTime.Now,
                LikesNum = 2
            },
            new()
            {
                IdArticle = 3,
                Author = new ReducedUserResponseDto
                {
                    IdUser = 3,
                    Nick = "Karol"
                },
                Title = "Title3",
                Content = "Content3",
                CreatedAt = DateTime.Now,
                LikesNum = 3,
                Tags = new[] { "samochody" }
            }
        },
        IsNext = false
    };
    
    [Fact]
    public async void GetPostsByWordFromDb_Returns_ListOfPosts()
    {
        DateTime time = DateTime.Now;
        //Arrange
        _searchRepositoryMock
            .Setup(x => x.GetPostByWordFromDb(5, time, "XYZ"))
            .ReturnsAsync(_fakeQtyPost);

        //Act
        var actionResult = await _searchController.GetPosts(5, time, "XYZ");
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as PostQtySearchDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyPost, resultValue);
    }
    [Fact]
    public async void GetQtyPostByWordFromDb_Returns_WrongNumberException_Bad_Request()
    {
        //Arrange
        DateTime time = DateTime.Now;
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";
        
        //Arrange
        _searchRepositoryMock
            .Setup(x => x.GetPostByWordFromDb(4, time, "XYZ"))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));

        //Act
        var actionResult = await _searchController.GetPosts(4, time, "XYZ");
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue.Message);
    }
    
    
    [Fact]
    public async void GetArticlesByWordFromDb_Returns_ListOfArticles()
    {
        DateTime time = DateTime.Now;
        //Arrange
        _searchRepositoryMock
            .Setup(x => x.GetArticleByWordFromDb(5, time, "Content3"))
            .ReturnsAsync(_fakeQtyArticles);

        //Act
        var actionResult = await _searchController.GetArticles(5, time, "Content3");
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ArticleQtySearchDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyArticles, resultValue);
    }
    
    [Fact]
    public async void GetQtyArticleByWordFromDb_Returns_WrongNumberException_Bad_Request()
    {
        //Arrange
        DateTime time = DateTime.Now;
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";
        
        //Arrange
        _searchRepositoryMock
            .Setup(x => x.GetArticleByWordFromDb(4, time, "Content3"))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));

        //Act
        var actionResult = await _searchController.GetArticles(4, time, "Content3");
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue.Message);
    }
    
    
    [Fact]
    public async void GetUsers_Returns_Ok()
    {
        DateTime time = DateTime.Now;
        //Arrange
        _searchRepositoryMock
            .Setup(x => x.GetUsersByNameFromDb(5, time, "Na"))
            .ReturnsAsync(_fakeQtyUsers);

        //Act
        var actionResult = await _searchController.GetUsers(5, time, "Na");
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UsersQtyDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyUsers, resultValue);
    }
    
    
    [Fact]
    public async void GetQtyUsersByNameFromDb_Returns_WrongNumberException_Bad_Request()
    {
        //Arrange
        DateTime time = DateTime.Now;
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";
        
        //Arrange
        _searchRepositoryMock.Setup(x => x.GetUsersByNameFromDb(4, time, "Na"))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));

        //Act
        var actionResult = await _searchController.GetUsers(4, time, "Na");
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue.Message);
    }
    
    [Fact]
    public async void GetQtyArticleByTagsFromDb_Returns_Ok()
    {
        DateTime time = DateTime.Now;
        //Arrange
        _searchRepositoryMock
            .Setup(x => x.GetArticlesByTagFromDb(5, time, "samochody"))
            .ReturnsAsync(_fakeQtyArticlesTags);

        //Act
        var actionResult = await _searchController.GetArticlesByTag(5, time, "samochody");
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ArticleQtyDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeQtyArticlesTags, resultValue);
    }
    [Fact]
    public async void GetQtyArticleByTagsFromDb_Returns_WrongNumberException_Bad_Request()
    {
        //Arrange
        DateTime time = DateTime.Now;
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";

        _searchRepositoryMock
            .Setup(x => x.GetArticlesByTagFromDb(4, time, "samochody"))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));

        //Act
        var actionResult = await _searchController.GetArticlesByTag(4, time, "samochody");
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue.Message);
    }
    
    [Fact] 
    public async void GetQtyPostsByTagFromDb_Returns_WrongNumberException_Bad_Request()
    {
        var exceptionMessage = "Wrong Number! Please insert number greater than 4!";
        
        DateTime time = DateTime.Now;
        
        _searchRepositoryMock
            .Setup(x => x.GetPostsByTagFromDb(4, time, "Samochody"))
            .ThrowsAsync(new WrongNumberException(exceptionMessage));
        //Act
        var actionResult = await _searchController.GetPostsByTag(4, time, "Samochody");
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue.Message);
    }
    [Fact]
    public async void GetQtyPostsByTagFromDb_Returns_ListOfPosts()
    {
        DateTime time = DateTime.Now;
        //Arrange
        _searchRepositoryMock
            .Setup(x => x.GetPostsByTagFromDb(5, time, "samochody"))
            .ReturnsAsync(_fakeQtyPosts);

        //Act
        var actionResult = await _searchController.GetPostsByTag(5, time, "samochody");
        
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