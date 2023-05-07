using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.Articles;
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
    public async void GetQtyPostByWordFromDb_Not_Found()
    {
        DateTime time = DateTime.Now;
        //Arrange
        Func<PostQtySearchDto?> nullValue = null;
        
        //Arrange
        _searchRepositoryMock
            .Setup(x => x.GetPostByWordFromDb(5, time, "XYZ"))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _searchController.GetPosts(5, time, "XYZ");
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No posts found", resultValue.Message);
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
    public async void GetQtyArticleByTagFromDb_Not_Found()
    {
        DateTime time = DateTime.Now;
        //Arrange
        Func<ArticleQtySearchDto?> nullValue = null;
        
        //Arrange
        _searchRepositoryMock
            .Setup(x => x.GetArticleByWordFromDb(5, time, "Content4"))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _searchController.GetArticles(5, time, "Content4");
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No articles found", resultValue.Message);
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
    public async void GetUsers_Returns_Not_Found()
    {
        //Arrange
        Func<UsersQtyDto?> nullValue = null;
        
        _searchRepositoryMock.Setup(x => x.GetUsersByNameFromDb(5, DateTime.Now, "Name"))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _searchController.GetUsers(5, DateTime.Now, "Name");
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No users found", resultValue.Message);
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

}