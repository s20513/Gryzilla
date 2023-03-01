using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.ReportCommentPost;
using Gryzilla_App.DTOs.Responses.ReportCommentPost;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.ReportCommentPost;

public class ReportCommentPostControllerTests
{
    private readonly ReportCommentPostController _reportController;
    private readonly Mock<IReportCommentPostDbRepository> _reportRepositoryMock = new();

    public ReportCommentPostControllerTests()
    {
        _reportController = new ReportCommentPostController(_reportRepositoryMock.Object);
    }
    
     [Fact]
    public async void GetReports_Returns_Ok()
    {
        
        //Arrange
        var reports = new ReportCommentPostDto[5];

        _reportRepositoryMock.Setup(e => e.GetReportCommentPostsFromDb()).ReturnsAsync(reports);
        
        //Act
        var actionResult = await _reportController.GetReports();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentPostDto[];
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(reports, resultValue);
    }
    
    [Fact]
    public async void GetReport_Returns_Ok()
    {
        //Arrange
        var report = new ReportCommentPostDto
        {
             IdUser= 1,
             IdReason = 1,
             IdComment = 1
        };

        _reportRepositoryMock.Setup(e => e.GetOneReportCommentPostFromDb(report.IdReason, report.IdUser, report.IdComment)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.GetReport(report.IdReason, report.IdUser, report.IdComment);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentPostDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void GetReport_Returns_NotFound()
    {
        //Arrange
        int idUser = 1;
        int idReason = 1;
        int idCommentArticle = 1;
        ReportCommentPostDto? reports = null;

        _reportRepositoryMock.Setup(e => e.GetOneReportCommentPostFromDb(idReason, idUser, idCommentArticle)).ReturnsAsync(reports);

        //Act
        var actionResult = await _reportController.GetReport(idReason, idUser, idCommentArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No report with given id found", resultValue);
    }
    [Fact]
    public async void GetReports_Returns_NotFound()
    {
        //Arrange
        List<ReportCommentPostDto>? reports = new List<ReportCommentPostDto>();

        _reportRepositoryMock.Setup(e => e.GetReportCommentPostsFromDb()).ReturnsAsync(reports);

        //Act
        var actionResult = await _reportController.GetReports();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No reports", resultValue);
    }
    [Fact]
    public async void AddReport_Returns_BadRequest()
    {
        //Arrange
        var newReason = new NewReportCommentPostDto();
        var message = "The creator of the comment cannot report";

        _reportRepositoryMock
            .Setup(e => e.AddReportCommentPostToDb(newReason))
            .ThrowsAsync(new UserCreatorException("The creator of the comment cannot report"));
        
        //Act
        var actionResult = await _reportController.AddReportPostComment(newReason);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(message, resultValue);
    }
    [Fact]
    public async void CreateReportPostComment_Returns_Ok()
    {
        //Arrange
        var newReportPostCommentDto = new NewReportCommentPostDto();
        var returnedReportComment = new ReportCommentPostDto();
        
        _reportRepositoryMock
            .Setup(x => x.AddReportCommentPostToDb(newReportPostCommentDto))
            .ReturnsAsync(returnedReportComment);

        //Act
        var actionResult = await _reportController.AddReportPostComment(newReportPostCommentDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentPostDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedReportComment, resultValue);
    }
    
    [Fact]
    public async void CreateReportPostComment_Returns_Not_Found()
    {
        //Arrange
        var newReportPostCommentDto = new NewReportCommentPostDto();
        ReportCommentPostDto? nullValue = null;
        
         _reportRepositoryMock
            .Setup(x => x.AddReportCommentPostToDb(newReportPostCommentDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _reportController.AddReportPostComment(newReportPostCommentDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User, comment or reason is wrong", resultValue);
    }
    
    
    [Fact]
    public async void ModifyReportPostComment_Returns_Ok()
    {
        //Arrange
        var putReportPostCommentDto = new UpdateReportCommentPostDto()
        {
            Description = "string1" 
        };
        
        var returnedComment = new ReportCommentPostDto();
        
        _reportRepositoryMock
            .Setup(x => x.UpdateReportCommentPostFromDb(putReportPostCommentDto))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _reportController.UpdateReportPostComment(putReportPostCommentDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentPostDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void ModifyPostComment_Returns_Not_Found()
    {
        //Arrange
        var putReportPostCommentDto = new UpdateReportCommentPostDto()
        {
            Description = "string1" 
        };

        ReportCommentPostDto? nullValue = null;
        
        _reportRepositoryMock
            .Setup(x => x.UpdateReportCommentPostFromDb(putReportPostCommentDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _reportController.UpdateReportPostComment(putReportPostCommentDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No report with given id found", resultValue);
    }
    
    [Fact]
    public async void DeletePostComment_Returns_Ok()
    {
        //Arrange
        var returnedComment = new ReportCommentPostDto();
        
        var deleteReportPostCommentDto = new DefaultReportCommentPostDto()
        {
            IdUser = 1,
            IdReason = 1,
            IdComment = 1
        };
        _reportRepositoryMock
            .Setup(x => x.DeleteReportCommentPostFromDb(deleteReportPostCommentDto))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _reportController.DeleteReportPostComment(deleteReportPostCommentDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentPostDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void DeletePostComment_Returns_Not_Found()
    {
        //Arrange
        var deleteReportPostCommentDto = new DefaultReportCommentPostDto()
        {
            IdUser = 1,
            IdReason = 1,
            IdComment = 1
        };
        ReportCommentPostDto? nullValue = null;
        
        _reportRepositoryMock
            .Setup(x => x.DeleteReportCommentPostFromDb(deleteReportPostCommentDto))
            .ReturnsAsync( nullValue);

        //Act
        var actionResult = await _reportController.DeleteReportPostComment(deleteReportPostCommentDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No report with given id found", resultValue);
    }
}