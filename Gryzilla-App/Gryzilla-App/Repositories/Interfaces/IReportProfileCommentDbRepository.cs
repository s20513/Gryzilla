using Gryzilla_App.DTOs.Requests.ReportProfileComment;
using Gryzilla_App.DTOs.Responses.ReportProfileComment;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IReportProfileCommentDbRepository
{
    public Task<ReportProfileCommentResponseDto?> AddReportProfileCommentToDb(NewReportProfileCommentRequestDto newReportProfileCommentRequestDto);

    public Task<ReportProfileCommentResponseDto?> DeleteReportProfileCommentFromDb(ReportProfileCommentIdsRequestDto reportProfileCommentIdsRequestDto);

    public Task<ReportProfileCommentResponseDto?> UpdateReportProfileCommentFromDb(UpdateReportProfileCommentRequestDto updateReportProfileCommentRequestDto);

    public Task<ReportProfileCommentResponseDto?> GetReportProfileCommentFromDb(ReportProfileCommentIdsRequestDto reportProfileCommentIdsRequestDto);
    public Task<IEnumerable<ReportProfileCommentResponseDto?>> GetReportProfileCommentsFromDb();
    
}