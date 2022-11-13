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

    [Fact]
    public async void DeleteGroup_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var group = new GroupDto();
        
        _groupRepositoryMock.Setup(x => x.DeleteGroup(id)).ReturnsAsync(group);

        //Act
        var actionResult = await _groupsController.DeleteGroup(id);
        
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
    public async void DeleteGroup_Not_Found()
    {
        //Arrange
        var id = 1;
        GroupDto? nullValue = null;
        
        _groupRepositoryMock.Setup(x => x.DeleteGroup(id)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.DeleteGroup(id);
        
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
    public async void CreateNewGroup_Returns_Ok()
    {
        //Arrange
        var newGroupRequestDto = new NewGroupRequestDto();
        var group = new GroupDto();
        
        _groupRepositoryMock
            .Setup(x => x.AddNewGroup(newGroupRequestDto.IdUser, newGroupRequestDto))
            .ReturnsAsync(group);

        //Act
        var actionResult = await _groupsController.CreateNewGroup(newGroupRequestDto);
        
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
    public async void CreateNewGroup_Returns_Not_Found()
    {
        //Arrange
        var newGroupRequestDto = new NewGroupRequestDto();
        GroupDto? nullValue = null;
        
        _groupRepositoryMock
            .Setup(x => x.AddNewGroup(newGroupRequestDto.IdUser, newGroupRequestDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.CreateNewGroup(newGroupRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot add group or wrong userId", resultValue);
    }
    
    [Fact]
    public async void CreateNewGroup_Returns_Bad_Request()
    {
        //Arrange
        var newGroupRequestDto = new NewGroupRequestDto();

        _groupRepositoryMock
            .Setup(x => x.AddNewGroup(newGroupRequestDto.IdUser, newGroupRequestDto))
            .Throws(new SameNameException("The name of the group is already taken"));

        //Act
        var actionResult = await _groupsController.CreateNewGroup(newGroupRequestDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("The name of the group is already taken", resultValue);
    }

    [Fact]
    public async void RemoveUserFromGroup_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var userToGroupDto = new UserToGroupDto
        {
            IdGroup = id
        };
        var group = new GroupDto();
        
        _groupRepositoryMock
            .Setup(x => x.RemoveUserFromGroup(id, userToGroupDto))
            .ReturnsAsync(group);

        //Act
        var actionResult = await _groupsController.RemoveUserFromGroup(id, userToGroupDto);
        
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
    public async void RemoveUserFromGroup_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        var userToGroupDto = new UserToGroupDto
        {
            IdGroup = id
        };
        GroupDto? nullValue = null;
        
        _groupRepositoryMock
            .Setup(x => x.RemoveUserFromGroup(id, userToGroupDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.RemoveUserFromGroup(id, userToGroupDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Group or user was not found", resultValue);
    }
    
    [Fact]
    public async void RemoveUserFromGroup_With_Different_Ids_Returns_Bad_Request()
    {
        //Arrange
        var id = 1;
        var userToGroupDto = new UserToGroupDto();
        GroupDto? nullValue = null;
        
        _groupRepositoryMock
            .Setup(x => x.RemoveUserFromGroup(id, userToGroupDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.RemoveUserFromGroup(id, userToGroupDto);
        
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
    public async void RemoveUserFromGroup_Throws_UserCreatorException_Returns_Bad_Request()
    {
        //Arrange
        var id = 1;
        var userToGroupDto = new UserToGroupDto
        {
            IdGroup = id
        };

        _groupRepositoryMock
            .Setup(x => x.RemoveUserFromGroup(id, userToGroupDto))
            .Throws(new UserCreatorException("The creator of the group cannot be removed"));

        //Act
        var actionResult = await _groupsController.RemoveUserFromGroup(id, userToGroupDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("The creator of the group cannot be removed", resultValue);
    }
    
    [Fact]
    public async void AddUserToGroup_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var userToGroupDto = new UserToGroupDto
        {
            IdGroup = id
        };
        var group = new GroupDto();
        
        _groupRepositoryMock
            .Setup(x => x.AddUserToGroup(id, userToGroupDto))
            .ReturnsAsync(group);

        //Act
        var actionResult = await _groupsController.AddUserToGroup(id, userToGroupDto);
        
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
    public async void AddUserToGroup_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        var userToGroupDto = new UserToGroupDto
        {
            IdGroup = id
        };
        GroupDto? nullValue = null;
        
        _groupRepositoryMock
            .Setup(x => x.AddUserToGroup(id, userToGroupDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.AddUserToGroup(id, userToGroupDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User or Group not found", resultValue);
    }
    
    [Fact]
    public async void AddUserToGroup_Returns_Bad_Request()
    {
        //Arrange
        var id = 1;
        var userToGroupDto = new UserToGroupDto();
        var group = new GroupDto();
        
        _groupRepositoryMock
            .Setup(x => x.AddUserToGroup(id, userToGroupDto))
            .ReturnsAsync(group);

        //Act
        var actionResult = await _groupsController.AddUserToGroup(id, userToGroupDto);
        
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
    public async void ExistUserInTheGroup_Returns_Ok()
    {
        //Arrange
        var idGroup = 1;
        var idUser = 1;
        var exist = true;
        
        _groupRepositoryMock
            .Setup(x => x.ExistUserInTheGroup(idGroup, idUser))
            .ReturnsAsync(exist);

        //Act
        var actionResult = await _groupsController.ExistUserInTheGroup(idGroup, idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as bool?;
        
        if (resultValue is null) return;
        Assert.True(resultValue);
    }
    
    [Fact]
    public async void GetUserGroups_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var groups = new UserGroupDto[5];
        
        _groupRepositoryMock
            .Setup(x => x.GetUserGroups(id))
            .ReturnsAsync(groups);

        //Act
        var actionResult = await _groupsController.GetUserGroups(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserGroupDto[];
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(groups, resultValue);
    }
    
    [Fact]
    public async void GetUserGroups_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        var groups = Array.Empty<UserGroupDto>();
        
        _groupRepositoryMock
            .Setup(x => x.GetUserGroups(id))
            .ReturnsAsync(groups);

        //Act
        var actionResult = await _groupsController.GetUserGroups(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no groups", resultValue);
    }
}