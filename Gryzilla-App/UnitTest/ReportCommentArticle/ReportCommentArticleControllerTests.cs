using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.ReportCommentArticle;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.ReportCommentArticle;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.ReportCommentArticle;

public class ReportCommentArticleControllerTests
{
    private readonly ReportCommentArticleController _reportController;
    private readonly Mock<IReportCommentArticleDbRepository> _reportRepositoryMock = new();

    public ReportCommentArticleControllerTests()
    {
        _reportController = new ReportCommentArticleController(_reportRepositoryMock.Object);
    }
    
   [Fact]
    public async void GetReports_Returns_Ok()
    {
        
        //Arrange
        var reports = new ReportCommentArticleDto[5];

        _reportRepositoryMock.Setup(e => e.GetReportCommentArticlesFromDb()).ReturnsAsync(reports);
        
        //Act
        var actionResult = await _reportController.GetReports();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentArticleDto[];
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(reports, resultValue);
    }
    
    [Fact]
    public async void GetReport_Returns_Ok()
    {
        //Arrange
        var report = new ReportCommentArticleDto
        {
             IdUser= 1,
             IdReason = 1,
             IdComment = 1
        };

        _reportRepositoryMock.Setup(e => e.GetOneReportCommentArticleFromDb(report.IdReason, report.IdUser, report.IdComment)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.GetReport(report.IdReason, report.IdUser, report.IdComment);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentArticleDto;
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
        ReportCommentArticleDto? reports = null;

        _reportRepositoryMock.Setup(e => e.GetOneReportCommentArticleFromDb(idReason, idUser, idCommentArticle)).ReturnsAsync(reports);

        //Act
        var actionResult = await _reportController.GetReport(idReason, idUser, idCommentArticle);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No report with given id found", resultValue.Message);
    }
    
    [Fact]
    public async void CreateReportArticleComment_Returns_Ok()
    {
        //Arrange
        var newReportPostCommentDto = new NewReportCommentArticleDto();
        var returnedReportComment = new ReportCommentArticleDto();
        
        _reportRepositoryMock
            .Setup(x => x.AddReportCommentArticleToDb(newReportPostCommentDto))
            .ReturnsAsync(returnedReportComment);

        //Act
        var actionResult = await _reportController.AddReportArticleComment(newReportPostCommentDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentArticleDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedReportComment, resultValue);
    }
    [Fact]
    public async void AddReport_Returns_BadRequest()
    {
        //Arrange
        var newReason = new NewReportCommentArticleDto();
        var message = "The creator of the comment cannot report";

        _reportRepositoryMock
            .Setup(e => e.AddReportCommentArticleToDb(newReason))
            .ThrowsAsync(new UserCreatorException("The creator of the comment cannot report"));
        
        //Act
        var actionResult = await _reportController.AddReportArticleComment(newReason);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(message, resultValue.Message);
    }

    [Fact]
    public async void CreateReportArticleComment_Returns_Not_Found()
    {
        //Arrange
        var newReportPostCommentDto = new NewReportCommentArticleDto();
        ReportCommentArticleDto? nullValue = null;
        
         _reportRepositoryMock
            .Setup(x => x.AddReportCommentArticleToDb(newReportPostCommentDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _reportController.AddReportArticleComment(newReportPostCommentDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User, comment or reason is wrong", resultValue.Message);
    }
    
    
    [Fact]
    public async void ModifyReportArticleComment_Returns_Ok()
    {
        //Arrange
        var putReportArticleCommentDto = new UpdateReportCommentArticleDto()
        {
            Content = "string1" 
        };
        
        var returnedComment = new ReportCommentArticleDto();
        
        _reportRepositoryMock
            .Setup(x => x.UpdateReportCommentArticleFromDb(putReportArticleCommentDto))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _reportController.UpdateReportPostComment(putReportArticleCommentDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentArticleDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void ModifyArticleComment_Returns_Not_Found()
    {
        //Arrange
        var putReportArticleCommentDto = new UpdateReportCommentArticleDto()
        {
            Content = "string1" 
        };

        ReportCommentArticleDto? nullValue = null;
        
        _reportRepositoryMock
            .Setup(x => x.UpdateReportCommentArticleFromDb(putReportArticleCommentDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _reportController.UpdateReportPostComment(putReportArticleCommentDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No report with given id found", resultValue.Message);
    }
    
    [Fact]
    public async void DeleteReportArticleComment_Returns_Ok()
    {
        //Arrange
        var returnedComment = new ReportCommentArticleDto();
        
        var deleteReportArticleCommentDto = new DeleteReportCommentArticleDto()
        {
            IdUser = 1,
            IdReason = 1,
            IdComment = 1
        };
        _reportRepositoryMock
            .Setup(x => x.DeleteReportCommentArticleFromDb(deleteReportArticleCommentDto))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _reportController.DeleteReportArticleComment(deleteReportArticleCommentDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportCommentArticleDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void DeleteReportArticleComment_Returns_Not_Found()
    {
        //Arrange
        var deleteReportArticleCommentDto = new DeleteReportCommentArticleDto()
        {
            IdUser = 1,
            IdReason = 1,
            IdComment = 1
        };
        ReportCommentArticleDto? nullValue = null;
        
        _reportRepositoryMock
            .Setup(x => x.DeleteReportCommentArticleFromDb(deleteReportArticleCommentDto))
            .ReturnsAsync( nullValue);

        //Act
        var actionResult = await _reportController.DeleteReportArticleComment(deleteReportArticleCommentDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No report with given id found", resultValue.Message);
    }

}