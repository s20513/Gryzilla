
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Moq;

namespace UnitTest.Achievements;

public class AchievementDbRepositoryTests
{
    //zostawiam na potem bo moqowanie dbcontext to grubsza sprawa
    //https://medium.com/@briangoncalves/dbcontext-dbset-mock-for-unit-test-in-c-with-moq-db5c270e68f3
    
    
    
    
    
    
    
    private readonly AchievementDbRepository _repository;
    private readonly Mock<GryzillaContext> _dbContextMock = new Mock<GryzillaContext>();

    public AchievementDbRepositoryTests()
    {
        //_dbContextMock.Setup(x => x.Achievements).Returns(new )
        
        _repository = new AchievementDbRepository(_dbContextMock.Object);
    }
    
    /*[Fact]
    public async void GetAchievementsFromDb_Returns_Achievements()
    {
        //Arrange
        


        //Act
        var achievements = await _repository.GetAchievementsFromDb();



        //Assert


    }*/
}