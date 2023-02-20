using Gryzilla_App.Controllers;
using Gryzilla_App.Repositories.Interfaces;
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

}