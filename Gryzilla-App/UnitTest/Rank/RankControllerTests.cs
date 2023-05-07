using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.Rank;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Rank;

public class RankControllerTests
{
    private readonly RankController _ranksController;
    private readonly Mock<IRankDbRepository> _rankRepositoryMock = new();
    
    
    public RankControllerTests()
    {
        _ranksController = new RankController(_rankRepositoryMock.Object);
    }

    [Fact]
    public async void CreateNewRank_Returns_Ok()
    {
        //Arrange
        var addRankDto = new AddRankDto();
        var rank = new RankDto();
        
        _rankRepositoryMock
            .Setup(x => x.AddNewRank(addRankDto))
            .ReturnsAsync(rank);
        
        //Act
        var actionResult = await _ranksController.PostNewRank(addRankDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as RankDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(rank, resultValue);
    }
    
    
    [Fact]
    public async void CreateNewRank_Returns_Not_Found()
    {
        //Arrange
        var addRankDto = new AddRankDto();
        RankDto? nullValue = null;
        
        _rankRepositoryMock
            .Setup(x => x.AddNewRank(addRankDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _ranksController.PostNewRank(addRankDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot add new rank", resultValue.Message);
    }
    
    [Fact]
    public async void CreateNewRank_Returns_Bad_Request()
    {
        //Arrange
        var addRankDto = new AddRankDto();
        
        _rankRepositoryMock
            .Setup(x => x.AddNewRank(addRankDto))
            .Throws(new SameNameException("Rank with given name already exists!"));

        //Act
        var actionResult = await _ranksController.PostNewRank(addRankDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Rank with given name already exists!", resultValue.Message);
    }
    
    
    [Fact]
    public async void DeleteRank_Returns_Ok()
    {
        //Arrange
        const int idRank = 1;
        var rank = new RankDto();
        
        _rankRepositoryMock
            .Setup(x => x.DeleteRank(idRank))
            .ReturnsAsync(rank);

        //Act
        var actionResult = await _ranksController.DeleteRank(idRank);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as RankDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(rank, resultValue);
    }
    
    
    [Fact]
    public async void DeleteRank_Returns_Not_Found()
    {
        //Arrange
        var idRank = 1;
        RankDto? nullValue = null;
        
        _rankRepositoryMock
            .Setup(x => x.DeleteRank(idRank))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _ranksController.DeleteRank(idRank);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot delete rank", resultValue.Message);
    }
    [Fact]
    public async void DeleteRank_Returns_Bad_Request()
    {
        //Arrange
        var idRank = 1;
        var exceptionMessage = "Cannot delete. Some user have this rank!";
        
        _rankRepositoryMock
            .Setup(x => x.DeleteRank(idRank))
            .ThrowsAsync(new ReferenceException(exceptionMessage));

        //Act
        var actionResult = await _ranksController.DeleteRank(idRank);
        
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
    public async void ModifyRank_Returns_Ok()
    {
        //Arrange
        const int idRank = 1;
        var punRankDto = new PutRankDto
        {
            IdRank = idRank
        };
        
        var rank = new RankDto();
        
        _rankRepositoryMock
            .Setup(x => x.ModifyRank(punRankDto, idRank))
            .ReturnsAsync(rank);

        //Act
        var actionResult = await _ranksController.ModifyRank(punRankDto, idRank);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as RankDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(rank, resultValue);
    }
    
    [Fact]
    public async void ModifyRank_Returns_Not_Found()
    {
        //Arrange
        const int idRank = 1;
        var punRankDto = new PutRankDto
        {
            IdRank = idRank
        };
        
        RankDto? nullValue = null;
        
        _rankRepositoryMock
            .Setup(x => x.ModifyRank(punRankDto, idRank))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _ranksController.ModifyRank(punRankDto, idRank);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot modify rank", resultValue.Message);
    }
    
    [Fact]
    public async void ModifyRank_Returns_SameNameException_Bad_Request()
    {
        //Arrange
        const int idRank = 1;
        var punRankDto = new PutRankDto
        {
            IdRank = idRank
        };

        var exceptionMessage = "Rank with given name already exists!";
        
        _rankRepositoryMock
            .Setup(x => x.ModifyRank(punRankDto, idRank))
            .ThrowsAsync(new SameNameException(exceptionMessage));

        //Act
        var actionResult = await _ranksController.ModifyRank(punRankDto, idRank);
        
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
    public async void ModifyRank_Returns_Bad_Request()
    {
        //Arrange
        const int idRank = 1;
        var punRankDto = new PutRankDto
        {
            IdRank = 2
        };

        //Act
        var actionResult = await _ranksController.ModifyRank(punRankDto, idRank);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Id from route and Id in body have to be same", resultValue.Message);
    }
}