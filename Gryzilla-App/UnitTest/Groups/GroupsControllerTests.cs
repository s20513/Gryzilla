using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Groups;

public class GroupsControllerTests
{
    private readonly GroupsController _groupsController;
    private readonly Mock<IGroupDbRepository> _groupRepositoryMock = new();

    public GroupsControllerTests()
    {
        _groupsController = new GroupsController(_groupRepositoryMock.Object);
    }
    
    [Fact]
    public async void GetGroup_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var group = new GroupDto();
        
        _groupRepositoryMock.Setup(x => x.GetGroup(id)).ReturnsAsync(group);

        //Act
        var actionResult = await _groupsController.GetGroup(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GroupDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(group, resultValue);
    }
    
    [Fact]
    public async void GetGroup_Not_Found()
    {
        //Arrange
        var id = 1;
        GroupDto? nullValue = null;
        
        _groupRepositoryMock.Setup(x => x.GetGroup(id)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.GetGroup(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no group with given id", resultValue);
    }

    [Fact]
    public async void GetGroups_Returns_Ok()
    {
        //Arrange
        var groups = new GroupDto[5];
        
        _groupRepositoryMock.Setup(x => x.GetGroups()).ReturnsAsync(groups);

        //Act
        var actionResult = await _groupsController.GetGroups();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GroupDto[];
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(groups, resultValue);
    }
    
    [Fact]
    public async void GetGroups_Returns_Not_Found()
    {
        //Arrange
        var groups = Array.Empty<GroupDto>();
        
        _groupRepositoryMock.Setup(x => x.GetGroups()).ReturnsAsync(groups);

        //Act
        var actionResult = await _groupsController.GetGroups();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no groups", resultValue);
    }
    
    [Fact]
    public async void ModifyGroup_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var groupRequestDto = new GroupRequestDto
        {
            IdGroup = id
        };
        var group = new GroupDto();
        
        _groupRepositoryMock.Setup(x => x.ModifyGroup(id, groupRequestDto)).ReturnsAsync(group);

        //Act
        var actionResult = await _groupsController.ModifyGroup(id, groupRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GroupDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(group, resultValue);
    }
    
    [Fact]
    public async void ModifyGroup_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        var groupRequestDto = new GroupRequestDto
        {
            IdGroup = id
        };
        GroupDto? nullValue = null;
        
        _groupRepositoryMock.Setup(x => x.ModifyGroup(id, groupRequestDto)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.ModifyGroup(id, groupRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no group with given id", resultValue);
    }
    
    [Fact]
    public async void ModifyGroup_With_Different_Ids_Returns_Bad_Request()
    {
        //Arrange
        var id = 1;
        var groupRequestDto = new GroupRequestDto();
        var group = new GroupDto();
        
        _groupRepositoryMock.Setup(x => x.ModifyGroup(id, groupRequestDto)).ReturnsAsync(group);

        //Act
        var actionResult = await _groupsController.ModifyGroup(id, groupRequestDto);
        
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
    public async void ModifyGroup_Throws_SameNameException_Returns_Bad_Request()
    {
        //Arrange
        var id = 1;
        var groupRequestDto = new GroupRequestDto
        {
            IdGroup = id
        };

        _groupRepositoryMock
            .Setup(x => x.ModifyGroup(id, groupRequestDto))
            .Throws(new SameNameException("This name of group is already taken"));

        //Act
        var actionResult = await _groupsController.ModifyGroup(id, groupRequestDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("This name of group is already taken", resultValue);
    }
    
    
    //dokończę potem - Mati
    
    
    
    

}