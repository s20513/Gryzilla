using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.DTOs.Responses.Link;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Link;

public class LinkControllerTests
{
    private readonly LinkController _linkController;
    private readonly Mock<ILinkDbRepository> _linkDbRepositoryMock = new();

    public LinkControllerTests()
    {
        _linkController = new LinkController(_linkDbRepositoryMock.Object);
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
            .Setup(x => x.PutLinkSteam(linkDto))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.PutSteamLink(linkDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue);
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
            .Setup(x => x.PutLinkSteam(linkDto))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.PutSteamLink(linkDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue);
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
            .Setup(x => x.PutLinkDiscord(linkDto))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.PutDiscordLink(linkDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue);
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
            .Setup(x => x.PutLinkDiscord(linkDto))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.PutDiscordLink(linkDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue);
    }
    
    [Fact]
    public async void DeleteDiscordLink_Returns_Ok()
    {
        var idUser = 1;
        var info = "Link removed";

        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkDiscord(idUser))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.DeleteDiscordLink(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue);
    }
    
    [Fact]
    public async void DeleteDiscordLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        string ?stringResult = null;
        var info = "User doesn't exist";
        
        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkDiscord(idUser))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.DeleteDiscordLink(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue);
    }
    [Fact]
    public async void DeleteSteamLink_Returns_Ok()
    {
        var idUser = 1;
        var info = "Link removed";

        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkSteam(idUser))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _linkController.DeleteSteamLink(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue);
    }
    
    [Fact]
    public async void DeleteSteamLink_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        string ?stringResult = null;
        var info = "User doesn't exist";
        
        _linkDbRepositoryMock
            .Setup(x => x.DeleteLinkSteam(idUser))
            .ReturnsAsync(stringResult);

        //Act
        var actionResult = await _linkController.DeleteSteamLink(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue);
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
        var actionResult = await _linkController.DeleteSteamLink(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue);
    }
}