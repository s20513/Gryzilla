using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.ReportUser;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.ReportUser;

public class ReportUserControllerTests
{
    private readonly ReportUserController _reportController;
    private readonly Mock<IReportUserDbRepository> _reportRepositoryMock = new();

    public ReportUserControllerTests()
    {
        _reportController = new ReportUserController(_reportRepositoryMock.Object);
    }
    
    [Fact]
    public async void GetReports_Returns_Ok()
    {
        //Arrange
        var reports = new ReportUserDto[5];

        _reportRepositoryMock.Setup(e => e.GetUsersReportsFromDb()).ReturnsAsync(reports);
        
        //Act
        var actionResult = await _reportController.GetReports();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportUserDto[];
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(reports, resultValue);
    }

    [Fact]
    public async void GetReport_Returns_Ok()
    {
        //Arrange
        var reportUser = 1;
        var report = new ReportUserDto();

        _reportRepositoryMock.Setup(e => e.GetUserReportFromDb(reportUser)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.GetReport(reportUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportUserDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void GetReport_Returns_NotFound()
    {
        //Arrange
        var reportUser = 1;
        ReportUserDto? report = null;

        _reportRepositoryMock.Setup(e => e.GetUserReportFromDb(reportUser)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.GetReport(reportUser);
        
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
        var newReportUser = new NewReportUserDto();
        var report = new ReportUserDto();

        _reportRepositoryMock.Setup(e => e.AddReportUserToDb(newReportUser)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.AddReport(newReportUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportUserDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void AddReport_Returns_NotFound()
    {
        //Arrange
        var newReportUser = new NewReportUserDto();
        ReportUserDto? report = null;

        _reportRepositoryMock.Setup(e => e.AddReportUserToDb(newReportUser)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.AddReport(newReportUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal("Users or reason does not exist", resultValue);
    }
    
    [Fact]
    public async void AddReport_Returns_BadRequest()
    {
        //Arrange
        var newReportUser = new NewReportUserDto();
        var text = "text";
        var exception = new UserCreatorException(text);

        _reportRepositoryMock.Setup(e => e.AddReportUserToDb(newReportUser)).ThrowsAsync(exception);
        
        //Act
        var actionResult = await _reportController.AddReport(newReportUser);
        
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
        var updateReportUser = new ModifyReportUser();
        var report = new ReportUserDto();

        _reportRepositoryMock.Setup(e => e.UpdateReportUserFromDb(updateReportUser)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.UpdateReport(updateReportUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportUserDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void UpdateReport_Returns_NotFound()
    {
        //Arrange
        var updateReportUser = new ModifyReportUser();
        ReportUserDto? report = null;

        _reportRepositoryMock.Setup(e => e.UpdateReportUserFromDb(updateReportUser)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.UpdateReport(updateReportUser);
        
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
        var reportUser = 1;
        var report = new ReportUserDto();

        _reportRepositoryMock.Setup(e => e.DeleteReportUserFromDb(reportUser)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.DeleteReport(reportUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as ReportUserDto;
        Assert.NotNull(resultValue);

        if (resultValue is null) return;
        Assert.Equal(report, resultValue);
    }
    
    [Fact]
    public async void DeleteReport_Returns_NotFound()
    {
        //Arrange
        var reportUser = 1;
        ReportUserDto? report = null;

        _reportRepositoryMock.Setup(e => e.DeleteReportUserFromDb(reportUser)).ReturnsAsync(report);
        
        //Act
        var actionResult = await _reportController.DeleteReport(reportUser);
        
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