﻿using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.ArticleComment;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.CommentArticle;

public class CommentArticleMssqlDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly CommentArticleMssqlDbRepository _repository;

    public CommentArticleMssqlDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new CommentArticleMssqlDbRepository(_context);
    }

    private async Task CreateTestData()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Rank
        {
            Name = "Rank1",
            RankLevel = 1
        });
        await _context.SaveChangesAsync();

        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick1",
            Password = "Pass1",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();

        await _context.Articles.AddAsync(new Gryzilla_App.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        
        await _context.CommentArticles.AddAsync(new Gryzilla_App.CommentArticle
        {
            IdUser = 1,
            IdArticle = 1,
            DescriptionArticle = "DescPost1"
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async void AddCommentToArticle_Returns_ArticleCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();
        
        var newArticleCommentDto = new NewArticleCommentDto
        {
            IdUser = 1,
            IdArticle = 1,
            Description = "DescArticle1"
        };
        
        //Act
        var res = await _repository.AddCommentToArticle(newArticleCommentDto);
        
        //Assert
        Assert.NotNull(res);
        
        var articleComments = _context.CommentArticles.ToList();
        Assert.True(articleComments.Count == 2);
        
        var articleComment = articleComments.SingleOrDefault(e => 
            e.IdCommentArticle       == res.IdComment &&
            e.IdArticle          == res.IdArticle &&
            e.IdUser          == res.IdUser &&
            e.DescriptionArticle == res.Description);
        Assert.NotNull(articleComment);
    }
    
    [Fact]
    public async void  AddCommentToArticle_With_No_Existing_Article_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();
        
        var newArticleCommentDto = new NewArticleCommentDto
        {
            IdUser = 1,
            IdArticle = 2,
            Description = "DescArticle1"
        };
        
        //Act
        var res = await _repository.AddCommentToArticle(newArticleCommentDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void  AddCommentToArticle_With_No_Existing_User_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await CreateTestData();

        var newArticleCommentDto = new NewArticleCommentDto
        {
            IdUser = 2,
            IdArticle = 1,
            Description = "DescArticle1"
        };
        
        //Act
        var res = await _repository.AddCommentToArticle(newArticleCommentDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void ModifyArticleCommentFromDb_Returns_ArticleCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();

        var idComment = 1;
        
        var putArticleCommentDto = new PutArticleCommentDto
        {
            IdComment = idComment,
            IdUser = 1,
            IdArticle = 1,
            Description = "NewDescArticle1"
        };
        
        //Act
        var res = await _repository.ModifyArticleCommentFromDb(putArticleCommentDto, idComment);
        
        //Assert
        Assert.NotNull(res);
        
        var articleComments = _context.CommentArticles.ToList();
        Assert.Single(articleComments);
        
        var articleComment = articleComments.SingleOrDefault(e => 
            e.IdCommentArticle       == res.IdComment &&
            e.IdArticle          == res.IdArticle &&
            e.IdUser          == res.IdUser &&
            e.DescriptionArticle == res.Description);
        Assert.NotNull(articleComment);
    }
    
    [Fact]
    public async void ModifyArticleCommentFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();

        var idComment = 2;
        
        var putArticleCommentDto = new PutArticleCommentDto
        {
            IdComment = idComment,
            IdUser = 1,
            IdArticle = 1,
            Description = "NewDescArticle1"
        };
        
        //Act
        var res = await _repository.ModifyArticleCommentFromDb(putArticleCommentDto, idComment);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void DeleteArticleCommentFromDb_Returns_ArticleCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();

        var id = 1;
        
        //Act
        var res = await _repository.DeleteArticleCommentFromDb(id);
        
        //Assert
        Assert.NotNull(res);
        
        var postComments = _context.CommentPosts.ToList();
        Assert.Empty(postComments);
    }
    
    [Fact]
    public async void DeleteArticleCommentFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();

        var id = 2;
        
        //Act
        var res = await _repository.DeleteArticleCommentFromDb(id);
        
        //Assert
        Assert.Null(res);
    }
    
}