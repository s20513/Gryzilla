using Gryzilla_App.Controllers;
using Gryzilla_App.Repositories.Interfaces;
using Moq;

namespace UnitTest.Tag;

public class TagControllerTests
{
    private readonly TagController _tagsController;
    private readonly Mock<ITagDbRepository> _tagRepositoryMock = new();
    
    
    public TagControllerTests()
    {
        _tagsController = new TagController(_tagRepositoryMock.Object);
    }
    
   
}