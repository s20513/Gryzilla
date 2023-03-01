using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.ReportPost;
using Gryzilla_App.DTOs.Responses.ReportPost;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Implementations;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.ReportPost;

public class ReportPostControllerTests
{
    private readonly ReportPostController _reportController;
    private readonly Mock<IReportPostDbRepository> _reportRepositoryMock = new();

    public ReportPostControllerTests()
    {
        _reportController = new ReportPostController(_reportRepositoryMock.Object);
    }

    [Fact]
    public async void GetReports_Returns_Ok()
    {
        //Arrange
        var reports = new ReportPostResponseDto[5];

        _reportRepositoryMock.Setup(e => e.GetReportPostsFromDb()).ReturnsAsync(reports);
        
        //Act
        var actionResult = await _reportController.GetReports();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportPostResponseDto[];
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(reports, resultValue);
    }
    
    [Fact]
    public async void GetReports_Returns_NotFound()
    {
        //Arrange
        var reports = new List<ReportPostResponseDto>();

        _reportRepositoryMock.Setup(e => e.GetReportPostsFromDb()).ReturnsAsync(reports);
        
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
    public async void GetReport_Returns_Ok()
    {
        //Arrange
        var reportPostIdsRequestDto = new ReportPostIdsRequestDto();
        var report = new ReportPostResponseDto();

        _reportRepositoryMock.Setup(e => e.GetReportPostFromDb(reportPostIdsRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.GetReport(reportPostIdsRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportPostResponseDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void GetReport_Returns_NotFound()
    {
        //Arrange
        var reportPostIdsRequestDto = new ReportPostIdsRequestDto();
        ReportPostResponseDto? report = null;

        _reportRepositoryMock.Setup(e => e.GetReportPostFromDb(reportPostIdsRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.GetReport(reportPostIdsRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal("No report with given data found", resultValue);
    }
    
    [Fact]
    public async void AddReport_Returns_Ok()
    {
        //Arrange
        var newReportPostRequestDto = new NewReportPostRequestDto();
        var report = new ReportPostResponseDto();

        _reportRepositoryMock.Setup(e => e.AddReportPostToDb(newReportPostRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.AddReport(newReportPostRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportPostResponseDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void AddReport_Returns_NotFound()
    {
        //Arrange
        var newReportPostRequestDto = new NewReportPostRequestDto();
        ReportPostResponseDto? report = null;

        _reportRepositoryMock.Setup(e => e.AddReportPostToDb(newReportPostRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.AddReport(newReportPostRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal("User, post or reason does not exist", resultValue);
    }
    
    [Fact]
    public async void AddReport_Returns_BadRequest()
    {
        //Arrange
        var newReportPostRequestDto = new NewReportPostRequestDto();
        var text = "text";
        var exception = new UserCreatorException(text);

        _reportRepositoryMock.Setup(e => e.AddReportPostToDb(newReportPostRequestDto)).ThrowsAsync(exception);
        
        //Act
        var actionResult = await _reportController.AddReport(newReportPostRequestDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(text, resultValue);
    }
    
    [Fact]
    public async void UpdateReport_Returns_Ok()
    {
        //Arrange
        var updateReportPostRequestDto = new UpdateReportPostRequestDto();
        var report = new ReportPostResponseDto();

        _reportRepositoryMock.Setup(e => e.UpdateReportPostFromDb(updateReportPostRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.UpdateReport(updateReportPostRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportPostResponseDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void UpdateReport_Returns_NotFound()
    {
        //Arrange
        var updateReportPostRequestDto = new UpdateReportPostRequestDto();
        ReportPostResponseDto? report = null;

        _reportRepositoryMock.Setup(e => e.UpdateReportPostFromDb(updateReportPostRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.UpdateReport(updateReportPostRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal("No report with given data found", resultValue);
    }
    
    [Fact]
    public async void DeleteReport_Returns_Ok()
    {
        //Arrange
        var reportPostIdsRequestDto = new ReportPostIdsRequestDto();
        var report = new ReportPostResponseDto();

        _reportRepositoryMock.Setup(e => e.DeleteReportPostFromDb(reportPostIdsRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.DeleteReport(reportPostIdsRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportPostResponseDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void DeleteReport_Returns_NotFound()
    {
        //Arrange
        var reportPostIdsRequestDto = new ReportPostIdsRequestDto();
        ReportPostResponseDto? report = null;

        _reportRepositoryMock.Setup(e => e.DeleteReportPostFromDb(reportPostIdsRequestDto)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.DeleteReport(reportPostIdsRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal("No report with given data found", resultValue);
    }
    
}