using Gryzilla_App.DTOs.Requests.ReportCommentPost;
using Gryzilla_App.DTOs.Responses.ReportCommentPost;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IReportCommentPostDbRepository
{
    public Task<ReportCommentPostDto?> AddReportCommentPostToDb(NewReportCommentPostDto newReportCommentDto);
    public Task<ReportCommentPostDto?> DeleteReportCommentPostFromDb(DefaultReportCommentPostDto deleteReportCommentDto);
    public Task<ReportCommentPostDto?> UpdateReportCommentPostFromDb(UpdateReportCommentPostDto updateReportCommentDto);
    public Task<ReportCommentPostDto?> GetOneReportCommentPostFromDb(int idUser, int idComment, int idReason);
    public Task<IEnumerable<ReportCommentPostDto>> GetReportCommentPostsFromDb();
}