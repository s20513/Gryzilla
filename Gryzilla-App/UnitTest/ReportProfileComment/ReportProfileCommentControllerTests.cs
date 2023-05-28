using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.ReportProfileComment;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.ReportProfileComment;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.ReportProfileComment;

public class ReportProfileCommentControllerTests
{
    private readonly ReportProfileCommentController _reportController;
    private readonly Mock<IReportProfileCommentDbRepository> _reportRepositoryMock = new();

    public ReportProfileCommentControllerTests()
    {
        _reportController = new ReportProfileCommentController(_reportRepositoryMock.Object);
    }

    [Fact]
    public async void GetReports_Returns_Ok()
    {
        //Arrange
        var reports = new ReportProfileCommentResponseDto[5];

        _reportRepositoryMock.Setup(e => e.GetReportProfileCommentsFromDb()).ReturnsAsync(reports);
        
        //Act
        var actionResult = await _reportController.GetReports();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportProfileCommentResponseDto[];
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(reports, resultValue);
    }

    [Fact]
    public async void GetReport_Returns_Ok()
    {
        //Arrange
        var reportProfileCommentIdsRequestDto = new ReportProfileCommentIdsRequestDto();
        var report = new ReportProfileCommentResponseDto();

        _reportRepositoryMock.Setup(e => e.GetReportProfileCommentFromDb(reportProfileCommentIdsRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.GetReport(reportProfileCommentIdsRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportProfileCommentResponseDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void GetReport_Returns_NotFound()
    {
        //Arrange
        var reportProfileCommentIdsRequestDto = new ReportProfileCommentIdsRequestDto();
        ReportProfileCommentResponseDto? report = null;

        _reportRepositoryMock.Setup(e => e.GetReportProfileCommentFromDb(reportProfileCommentIdsRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.GetReport(reportProfileCommentIdsRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal("No report with given data found", resultValue.Message);
    }
    
    [Fact]
    public async void AddReport_Returns_Ok()
    {
        //Arrange
        var newReportProfileCommentRequestDto = new NewReportProfileCommentRequestDto();
        var report = new ReportProfileCommentResponseDto();

        _reportRepositoryMock.Setup(e => e.AddReportProfileCommentToDb(newReportProfileCommentRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.AddReport(newReportProfileCommentRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportProfileCommentResponseDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void AddReport_Returns_NotFound()
    {
        //Arrange
        var newReportProfileCommentRequestDto = new NewReportProfileCommentRequestDto();
        ReportProfileCommentResponseDto? report = null;

        _reportRepositoryMock.Setup(e => e.AddReportProfileCommentToDb(newReportProfileCommentRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.AddReport(newReportProfileCommentRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal("User, profile comment or reason does not exist", resultValue.Message);
    }
    
    [Fact]
    public async void AddReport_Returns_BadRequest()
    {
        //Arrange
        var newReportProfileCommentRequestDto = new NewReportProfileCommentRequestDto();
        var text = "text";
        var exception = new UserCreatorException(text);

        _reportRepositoryMock.Setup(e => e.AddReportProfileCommentToDb(newReportProfileCommentRequestDto)).ThrowsAsync(exception);
        
        //Act
        var actionResult = await _reportController.AddReport(newReportProfileCommentRequestDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(text, resultValue.Message);
    }
    
    [Fact]
    public async void UpdateReport_Returns_Ok()
    {
        //Arrange
        var updateReportProfileCommentRequestDto = new UpdateReportProfileCommentRequestDto();
        var report = new ReportProfileCommentResponseDto();

        _reportRepositoryMock.Setup(e => e.UpdateReportProfileCommentFromDb(updateReportProfileCommentRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.UpdateReport(updateReportProfileCommentRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportProfileCommentResponseDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void UpdateReport_Returns_NotFound()
    {
        //Arrange
        var updateReportProfileCommentRequestDto = new UpdateReportProfileCommentRequestDto();
        ReportProfileCommentResponseDto? report = null;

        _reportRepositoryMock.Setup(e => e.UpdateReportProfileCommentFromDb(updateReportProfileCommentRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.UpdateReport(updateReportProfileCommentRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal("No report with given data found", resultValue.Message);
    }
    
    [Fact]
    public async void DeleteReport_Returns_Ok()
    {
        //Arrange
        var reportProfileCommentIdsRequestDto = new ReportProfileCommentIdsRequestDto();
        var report = new ReportProfileCommentResponseDto();

        _reportRepositoryMock.Setup(e => e.DeleteReportProfileCommentFromDb(reportProfileCommentIdsRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.DeleteReport(reportProfileCommentIdsRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportProfileCommentResponseDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void DeleteReport_Returns_NotFound()
    {
        //Arrange
        var reportProfileCommentIdsRequestDto = new ReportProfileCommentIdsRequestDto();
        ReportProfileCommentResponseDto? report = null;

        _reportRepositoryMock.Setup(e => e.DeleteReportProfileCommentFromDb(reportProfileCommentIdsRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.DeleteReport(reportProfileCommentIdsRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal("No report with given data found", resultValue.Message);
    }
}