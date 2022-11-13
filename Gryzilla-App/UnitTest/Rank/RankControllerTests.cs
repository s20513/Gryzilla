using FakeItEasy;
using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Requests.Rank;
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot add new rank", resultValue);
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Rank with given name already exists!", resultValue);
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot delete rank", resultValue);
    }
    [Fact]
    public void DeleteRank_Returns_Bad_Request()
    {
        //Arrange
        var idRank = 5;
        var exceptionMessage = "Cannot delete. Some user have this rank!";

        var fakeRepository = A.Fake<IRankDbRepository>();

        A.CallTo(() => fakeRepository.DeleteRank(idRank))
            .Throws(new ReferenceException(exceptionMessage));

        var controller = new RankController(fakeRepository);
        
        //Act
        var actionResult = controller.DeleteRank(idRank);
        
        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal(exceptionMessage, resultValue);
    }
    
    [Fact]
    public void ModifyRank_Returns_Ok()
    {
        //Arrange
        var idRank = 5;
        
        var rank= A.Dummy<PutRankDto>();
        rank.IdRank = idRank;
        
        var modifiedRank= A.Dummy<RankDto>();
        modifiedRank.Name = "test";
        
        var fakeRepository = A.Fake<IRankDbRepository>();
        
        A.CallTo(() => fakeRepository.ModifyRank(rank, idRank))!
            .Returns(Task.FromResult(modifiedRank));
        
        var controller = new RankController(fakeRepository);
        
        //Act
        var actionResult = controller.ModifyRank(rank, idRank);
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
                
        if (result is null) return;
        var resultValue = result.Value as RankDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("test", resultValue.Name);
    }
    
    [Fact]
    public void ModifyRank_Returns_Not_Found()
    {
        //Arrange
        var idRank = 5;
        var rank = A.Dummy<PutRankDto>();
        rank.IdRank = idRank;
        
        var fakeRepository = A.Fake<IRankDbRepository>();

        A.CallTo(() => fakeRepository.ModifyRank(rank, idRank))
            .Returns(Task.FromResult<RankDto?>(null));
        
        var controller = new RankController(fakeRepository);
        
        //Act
        var actionResult = controller.ModifyRank(rank, idRank);
        
        //Assert
        var result = actionResult.Result as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal("Cannot modify rank", resultValue);
    }
    
    [Fact]
    public void ModifyRank_Returns_SameNameException_Bad_Request()
    {
        //Arrange
        var idRank = 5;
        var exceptionMessage = "Rank with given name already exists!!";
        var modifiedRank = A.Dummy<PutRankDto>();
        modifiedRank.IdRank = 5;

        var fakeRepository = A.Fake<IRankDbRepository>();

        A.CallTo(() => fakeRepository.ModifyRank(modifiedRank, idRank))
            .Throws(new SameNameException(exceptionMessage));
        
        var controller = new RankController(fakeRepository);
        
        //Act
        var actionResult = controller.ModifyRank(modifiedRank, idRank);
        
        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal(exceptionMessage, resultValue);
    }
    [Fact]
    public void ModifyRank_Returns_Bad_Request()
    {
        //Arrange
        var idRank= 5;
        var rank = A.Dummy<PutRankDto>();
        rank.IdRank = 6;
        
        var fakeRepository = A.Fake<IRankDbRepository>();
        var modifiedRank = A.Dummy<RankDto>();
        modifiedRank.Name = "test";
        
        A.CallTo(() => fakeRepository.ModifyRank(rank, idRank))!
            .Returns(Task.FromResult(modifiedRank));
        
        var controller = new RankController(fakeRepository);
        
        //Act
        var actionResult = controller.ModifyRank(rank, idRank);
        
        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal("Id from route and Id in body have to be same", resultValue);
    }
}