using System.Security.Claims;
using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.Link;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Link;

public class LinkControllerTests
{
    private readonly LinkController _linkController;
    private readonly Mock<ILinkDbRepository> _linkDbRepositoryMock = new();
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public LinkControllerTests()
    {
        _linkController = new LinkController(_linkDbRepositoryMock.Object);
        
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        _mockClaimsPrincipal.Setup(x => x.Claims).Returns(new List<Claim>());

        _linkController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = _mockClaimsPrincipal.Object }
        };
    }
    
    [Fact]
    public async void ModifySteamLink_Returns_Ok()
    {
        //Arrange
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "steamLink"
        };
        
        var info = "Link changed";
        
        _linkDbRepositoryMock
            .Setup(x => x.PutLinkSteam(linkDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.PutSteamLink(linkDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void ModifySteamLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        string ?stringResult = null;
        var info = "User doesn't exist";
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "steamLink"
        };
        
        _linkDbRepositoryMock
            .Setup(x => x.PutLinkSteam(linkDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.PutSteamLink(linkDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void ModifyDiscordLink_Returns_Ok()
    {
        //Arrange
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "discordLink"
        };
        
        var info = "Link changed";
        
        _linkDbRepositoryMock
            .Setup(x => x.PutLinkDiscord(linkDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.PutDiscordLink(linkDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void ModifyDiscordLink_Returns_Not_Found()
    {
        //Arrange
        string ?stringResult = null;
        var info = "User doesn't exist";
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "steamLink"
        };
        
        _linkDbRepositoryMock
            .Setup(x => x.PutLinkDiscord(linkDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.PutDiscordLink(linkDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void ModifyXboxLink_Returns_Ok()
    {
        //Arrange
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "xboxLink"
        };
        
        var info = "Link changed";
        
        _linkDbRepositoryMock
            .Setup(x => x.PutLinkXbox(linkDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.PutXboxLink(linkDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void ModifyXboxLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        string ?stringResult = null;
        var info = "User doesn't exist";
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "xboxLink"
        };
        
        _linkDbRepositoryMock
            .Setup(x => x.PutLinkXbox(linkDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.PutXboxLink(linkDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }

    [Fact]
    public async void ModifyEpicLink_Returns_Ok()
    {
        //Arrange
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "epicLink"
        };
        
        var info = "Link changed";
        
        _linkDbRepositoryMock
            .Setup(x => x.PutLinkEpic(linkDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.PutEpicLink(linkDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void ModifyEpicLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        string ?stringResult = null;
        var info = "User doesn't exist";
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "epicLink"
        };
        
        _linkDbRepositoryMock
            .Setup(x => x.PutLinkEpic(linkDto, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.PutEpicLink(linkDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    [Fact]
    public async void DeleteDiscordLink_Returns_Ok()
    {
        var idUser = 1;
        var info = "Link removed";

        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkDiscord(idUser, _mockClaimsPrincipal.Object))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.DeleteDiscordLink(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteDiscordLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        string ?stringResult = null;
        var info = "User doesn't exist";
        
        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkDiscord(idUser, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.DeleteDiscordLink(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    [Fact]
    public async void DeleteSteamLink_Returns_Ok()
    {
        var idUser = 1;
        var info = "Link removed";

        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkSteam(idUser, _mockClaimsPrincipal.Object))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.DeleteSteamLink(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteSteamLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        string ?stringResult = null;
        var info = "User doesn't exist";
        
        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkSteam(idUser, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.DeleteSteamLink(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    [Fact]
    public async void DeleteEpicLink_Returns_Ok()
    {
        var idUser = 1;
        var info = "Link removed";

        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkEpic(idUser, _mockClaimsPrincipal.Object))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.DeleteEpicLink(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteEpicLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        string ?stringResult = null;
        var info = "User doesn't exist";
        
        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkEpic(idUser, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.DeleteEpicLink(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteXboxLink_Returns_Ok()
    {
        var idUser = 1;
        var info = "Link removed";

        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkXbox(idUser, _mockClaimsPrincipal.Object))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.DeleteXboxLink(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteXboxLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        string ?stringResult = null;
        var info = "User doesn't exist";
        
        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkXbox(idUser, _mockClaimsPrincipal.Object))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.DeleteXboxLink(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void GetUserLinks_Returns_Ok()
    {
        var idUser = 1;

        var getLinks = new LinksDto
        {
            SteamLink = "steamLink",
            DiscordLink = "DiscordLink"
        };
        
        _linkDbRepositoryMock
            .Setup(x => x.GetUserLinks(idUser))
            .ReturnsAsync(getLinks);

        //Act
        var actionResult = await _linkController.GetUserLinks(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as LinksDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(getLinks.DiscordLink, resultValue.DiscordLink);
    }
    
    [Fact]
    public async void GetUserLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        LinksDto? nullValue = null;
        
        _linkDbRepositoryMock
            .Setup(x => x.GetUserLinks(idUser))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _linkController.GetUserLinks(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        Assert.Equal("User doesn't exist", resultValue.Message);
    }
}