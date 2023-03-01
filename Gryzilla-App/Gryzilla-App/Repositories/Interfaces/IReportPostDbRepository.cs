using Gryzilla_App.DTOs.Requests.ReportPost;
using Gryzilla_App.DTOs.Responses.ReportPost;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IReportPostDbRepository
{
    public Task<ReportPostResponseDto?> AddReportPostToDb(NewReportPostRequestDto newReportPostRequestDto);
    public Task<ReportPostResponseDto?> DeleteReportPostFromDb(ReportPostIdsRequestDto reportPostIdsRequestDto);
    public Task<ReportPostResponseDto?> UpdateReportPostFromDb(UpdateReportPostRequestDto updateReportPostRequestDto);
    public Task<ReportPostResponseDto?> GetReportPostFromDb(ReportPostIdsRequestDto updateReportPostRequestDto);
    public Task<IEnumerable<ReportPostResponseDto?>> GetReportPostsFromDb();
}