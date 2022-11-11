﻿using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Article;

public class ArticleControllerTests
{
    private readonly ArticleController _articleController;
    private readonly Mock<IArticleDbRepository> _articleRepositoryMock = new();

    private readonly IEnumerable<ArticleDto> _fakeArticles = new List<ArticleDto>
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
            LikesNum = 3
        }
    };
    
    public ArticleControllerTests()
    {
        _articleController = new ArticleController(_articleRepositoryMock.Object);
    }
    
    [Fact]
    public async void GetAllArticlesFromDb_Returns_Ok()
    {
        //Arrange
        _articleRepositoryMock.Setup(x => x.GetArticlesFromDb()).ReturnsAsync(_fakeArticles);

        //Act
        var actionResult = await _articleController.GetAllArticlesFromDb();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<ArticleDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeArticles, resultValue);
    }
    
    [Fact]
    public async void GetAllArticlesFromDb_Returns_Not_Found()
    {
        //Arrange
        IEnumerable<ArticleDto>? nullValue = null;
        
        _articleRepositoryMock.Setup(x => x.GetArticlesFromDb()).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _articleController.GetAllArticlesFromDb();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No articles found", resultValue);
    }

    [Fact]
    public async void GetAllArticlesByMostLikesFromDb_Returns_Ok()
    {
        //Arrange
        _articleRepositoryMock
            .Setup(x => x.GetArticlesByMostLikesFromDb())
            .ReturnsAsync(_fakeArticles.OrderByDescending(x => x.LikesNum));

        //Act
        var actionResult = await _articleController.GetAllArticlesByMostLikesFromDb();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<ArticleDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeArticles.OrderByDescending(x => x.LikesNum), resultValue);
    }
    
    [Fact]
    public async void GetAllArticlesByMostLikesFromDb_Returns_Not_Found()
    {
        //Arrange
        IEnumerable<ArticleDto>? nullValue = null;
        
        _articleRepositoryMock
            .Setup(x => x.GetArticlesByMostLikesFromDb())
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _articleController.GetAllArticlesByMostLikesFromDb();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No articles found", resultValue);
    }
    
    [Fact]
    public async void GetAllArticlesByLeastLikesFromDb_Returns_Ok()
    {
        //Arrange
        _articleRepositoryMock
            .Setup(x => x.GetArticlesByLeastLikesFromDb())
            .ReturnsAsync(_fakeArticles.OrderBy(x => x.LikesNum));

        //Act
        var actionResult = await _articleController.GetAllArticlesByLeastLikesFromDb();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<ArticleDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeArticles.OrderBy(x => x.LikesNum), resultValue);
    }
    
    [Fact]
    public async void GetAllArticlesByLeastLikesFromDb_Returns_Not_Found()
    {
        //Arrange
        IEnumerable<ArticleDto>? nullValue = null;
        
        _articleRepositoryMock
            .Setup(x => x.GetArticlesByLeastLikesFromDb())
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _articleController.GetAllArticlesByLeastLikesFromDb();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No articles found", resultValue);
    }
    
    [Fact]
    public async void GetAllArticlesByEarliestDateFromDb_Returns_Ok()
    {
        //Arrange
        _articleRepositoryMock
            .Setup(x => x.GetArticlesByEarliestDateFromDb())
            .ReturnsAsync(_fakeArticles.OrderByDescending(x => x.CreatedAt));

        //Act
        var actionResult = await _articleController.GetAllArticlesByEarliestDateFromDb();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<ArticleDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeArticles.OrderByDescending(x => x.CreatedAt), resultValue);
    }
    
    [Fact]
    public async void GetAllArticlesByEarliestDateFromDb_Returns_Not_Found()
    {
        IEnumerable<ArticleDto>? nullValue = null;
        
        //Arrange
        _articleRepositoryMock
            .Setup(x => x.GetArticlesByEarliestDateFromDb())
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _articleController.GetAllArticlesByEarliestDateFromDb();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No articles found", resultValue);
    }
    
    [Fact]
    public async void GetAllArticlesByOldestDateFromDb_Returns_Ok()
    {
        //Arrange
        _articleRepositoryMock
            .Setup(x => x.GetArticlesByOldestDateFromDb())
            .ReturnsAsync(_fakeArticles.OrderBy(x => x.CreatedAt));

        //Act
        var actionResult = await _articleController.GetAllArticlesByOldestDateFromDb();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<ArticleDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeArticles.OrderBy(x => x.CreatedAt), resultValue);
    }
    
    [Fact]
    public async void GetAllArticlesByOldestDateFromDb_Returns_Not_Found()
    {
        //Arrange
        IEnumerable<ArticleDto>? nullValue = null;
        
        _articleRepositoryMock
            .Setup(x => x.GetArticlesByOldestDateFromDb())
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _articleController.GetAllArticlesByOldestDateFromDb();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No articles found", resultValue);
    }
    
    [Fact]
    public async void GetArticleFromDb_Returns_Ok()
    {
        //Arrange
        var id = 1;
        
        _articleRepositoryMock
            .Setup(x => x.GetArticleFromDb(id))
            .ReturnsAsync(_fakeArticles.Single(x => x.IdArticle == id));

        //Act
        var actionResult = await _articleController.GetArticleFromDb(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ArticleDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(id, resultValue.IdArticle);
    }
    
    [Fact]
    public async void GetArticleFromDb_Returns_Not_Found()
    {
        //Arrange
        var id = 10;
        ArticleDto? nullValue = null;

        _articleRepositoryMock
            .Setup(x => x.GetArticleFromDb(id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _articleController.GetArticleFromDb(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Article not found", resultValue);
    }
    
    [Fact]
    public async void AddNewArticleToDb_Returns_Ok()
    {
        //Arrange
        var idArticle = 4;
        var newArticle = new NewArticleRequestDto();
        var returnedArticle = new ArticleDto
        {
            IdArticle = idArticle
        };
        
        _articleRepositoryMock
            .Setup(x => x.AddNewArticleToDb(newArticle))
            .ReturnsAsync(returnedArticle);

        //Act
        var actionResult = await _articleController.AddNewArticleToDb(newArticle);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ArticleDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(idArticle, returnedArticle.IdArticle);
    }
    
    [Fact]
    public async void AddNewArticleToDb_Returns_Not_Found()
    {
        //Arrange
        var newArticle = new NewArticleRequestDto();
        ArticleDto? nullValue = null;
        
        _articleRepositoryMock
            .Setup(x => x.AddNewArticleToDb(newArticle))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _articleController.AddNewArticleToDb(newArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User not found", resultValue);
    }
    
    [Fact]
    public async void ModifyArticleFromDb_Returns_Ok()
    {
        //Arrange
        var id = 10;
        var putArticleRequestDto = new PutArticleRequestDto
        {
            IdArticle = id
        };
        
        var returnedArticle = new ArticleDto
        {
            IdArticle = id
        };
        
        _articleRepositoryMock
            .Setup(x => x.ModifyArticleFromDb(putArticleRequestDto, id))
            .ReturnsAsync(returnedArticle);

        //Act
        var actionResult = await _articleController.ModifyArticleFromDb(putArticleRequestDto, id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ArticleDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(id, resultValue.IdArticle);
    }
    
    [Fact]
    public async void ModifyArticleFromDb_Returns_Not_Found()
    {
        //Arrange
        var id = 10;
        var putArticleRequestDto = new PutArticleRequestDto
        {
            IdArticle = id
        };
        ArticleDto? nullValue = null;

        _articleRepositoryMock
            .Setup(x => x.ModifyArticleFromDb(putArticleRequestDto, id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _articleController.ModifyArticleFromDb(putArticleRequestDto, id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Article not found", resultValue);
    }
    
    [Fact]
    public async void ModifyArticleFromDb_Returns_Bad_Request()
    {
        //Arrange
        var id = 10;
        var putArticleRequestDto = new PutArticleRequestDto
        {
            IdArticle = 11
        };

        //Act
        var actionResult = await _articleController.ModifyArticleFromDb(putArticleRequestDto, id);
        
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
    public async void DeleteArticleFromDb_Returns_Ok()
    {
        //Arrange
        var id = 10;
        var returnedArticle = new ArticleDto
        {
            IdArticle = id
        };
        
        _articleRepositoryMock
            .Setup(x => x.DeleteArticleFromDb(id))
            .ReturnsAsync(returnedArticle);

        //Act
        var actionResult = await _articleController.DeleteArticleFromDb(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ArticleDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(id, resultValue.IdArticle);
    }
    
    [Fact]
    public async void DeleteArticleFromDb_Returns_Not_Found()
    {
        //Arrange
        var id = 10;
        ArticleDto? nullValue = null;

        _articleRepositoryMock
            .Setup(x => x.DeleteArticleFromDb(id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _articleController.DeleteArticleFromDb(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Article not found", resultValue);
    }
    
    
    
    
    
    
    
    
    
    
    
    
}