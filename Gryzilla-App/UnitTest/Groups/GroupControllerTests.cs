using System.Security.Claims;
using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Groups;

public class GroupsControllerTests
{
    private readonly GroupsController _groupsController;
    private readonly Mock<IGroupDbRepository> _groupRepositoryMock = new();
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public GroupsControllerTests()
    {
        _groupsController = new GroupsController(_groupRepositoryMock.Object);
        
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        _mockClaimsPrincipal.Setup(x => x.Claims).Returns(new List<Claim>());

        _groupsController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = _mockClaimsPrincipal.Object }
        };
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
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no group with given id", resultValue.Message);
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
    public async void ModifyGroup_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var groupRequestDto = new GroupRequestDto
        {
            IdGroup = id
        };
        var group = new GroupDto();
        
        _groupRepositoryMock.Setup(x => x.ModifyGroup(id, groupRequestDto, _mockClaimsPrincipal.Object)).ReturnsAsync(group);

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
        
        _groupRepositoryMock.Setup(x => x.ModifyGroup(id, groupRequestDto, _mockClaimsPrincipal.Object)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.ModifyGroup(id, groupRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no group with given id", resultValue.Message);
    }
    
    [Fact]
    public async void ModifyGroup_With_Different_Ids_Returns_Bad_Request()
    {
        //Arrange
        var id = 1;
        var groupRequestDto = new GroupRequestDto();
        var group = new GroupDto();
        
        _groupRepositoryMock.Setup(x => x.ModifyGroup(id, groupRequestDto, _mockClaimsPrincipal.Object)).ReturnsAsync(group);

        //Act
        var actionResult = await _groupsController.ModifyGroup(id, groupRequestDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Id from route and Id in body have to be same", resultValue.Message);
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
            .Setup(x => x.ModifyGroup(id, groupRequestDto, _mockClaimsPrincipal.Object))
            .Throws(new SameNameException("This name of group is already taken"));

        //Act
        var actionResult = await _groupsController.ModifyGroup(id, groupRequestDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("This name of group is already taken", resultValue.Message);
    }

    [Fact]
    public async void DeleteGroup_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var group = new GroupDto();
        
        _groupRepositoryMock.Setup(x => x.DeleteGroup(id, _mockClaimsPrincipal.Object)).ReturnsAsync(group);

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
        
        _groupRepositoryMock.Setup(x => x.DeleteGroup(id, _mockClaimsPrincipal.Object)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.DeleteGroup(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no group with given id", resultValue.Message);
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
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot add group or wrong userId", resultValue.Message);
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
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("The name of the group is already taken", resultValue.Message);
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
            .Setup(x => x.RemoveUserFromGroup(id, userToGroupDto, _mockClaimsPrincipal.Object))
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
            .Setup(x => x.RemoveUserFromGroup(id, userToGroupDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.RemoveUserFromGroup(id, userToGroupDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Group or user was not found", resultValue.Message);
    }
    
    [Fact]
    public async void RemoveUserFromGroup_With_Different_Ids_Returns_Bad_Request()
    {
        //Arrange
        var id = 1;
        var userToGroupDto = new UserToGroupDto();
        GroupDto? nullValue = null;
        
        _groupRepositoryMock
            .Setup(x => x.RemoveUserFromGroup(id, userToGroupDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.RemoveUserFromGroup(id, userToGroupDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Id from route and Id in body have to be same", resultValue.Message);
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
            .Setup(x => x.RemoveUserFromGroup(id, userToGroupDto, _mockClaimsPrincipal.Object))
            .Throws(new UserCreatorException("The creator of the group cannot be removed"));

        //Act
        var actionResult = await _groupsController.RemoveUserFromGroup(id, userToGroupDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("The creator of the group cannot be removed", resultValue.Message);
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
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User or Group not found", resultValue.Message);
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
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Id from route and Id in body have to be same", resultValue.Message);
    }
    
    [Fact]
    public async void ExistUserInTheGroup_Returns_Ok()
    {
        //Arrange
        var idGroup = 1;
        var idUser = 1;
        var exist = new ExistUserGroupDto
        {
            Member = true
        };
        
        _groupRepositoryMock
            .Setup(x => x.UserIsInGroup(idGroup, idUser))
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
        var groups = new GroupDto[5];
        
        _groupRepositoryMock
            .Setup(x => x.GetUserGroups(id))
            .ReturnsAsync(groups);

        //Act
        var actionResult = await _groupsController.GetUserGroups(id);
        
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
    public async void SetGroupPhoto_Returns_Ok()
    {
        //Arrange
        var file = new Mock<IFormFile>();
        var idGroup = 1;
        var groupDto = new GroupDto();

        _groupRepositoryMock.Setup(x => x.SetGroupPhoto(file.Object, idGroup, _mockClaimsPrincipal.Object)).ReturnsAsync(groupDto);

        //Act
        var actionResult = await _groupsController.SetGroupPhoto(file.Object, idGroup);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GroupDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(groupDto, resultValue);
    }
    
    [Fact]
    public async void GetGroupPhoto_Returns_Ok()
    {
        //Arrange
        var idGroup= 1;
        var groupPhotoResponseDto = new GroupPhotoResponseDto();

        _groupRepositoryMock.Setup(x => x.GetGroupPhoto(idGroup)).ReturnsAsync(groupPhotoResponseDto);

        //Act
        var actionResult = await _groupsController.GetGroupPhoto(idGroup);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GroupPhotoResponseDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(groupPhotoResponseDto, resultValue);
    }
}