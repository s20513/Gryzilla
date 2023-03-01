using Gryzilla_App.DTOs.Requests.ReportCommentArticle;
using Gryzilla_App.DTOs.Requests.ReportCommentPost;
using Gryzilla_App.DTOs.Responses.ReportCommentArticle;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IReportCommentArticleDbRepository
{
    public Task<ReportCommentArticleDto?> AddReportCommentArticleToDb(NewReportCommentArticleDto newReportCommentDto);
    public Task<ReportCommentArticleDto?> DeleteReportCommentArticleFromDb(DeleteReportCommentArticleDto deleteReportCommentDto);
    public Task<ReportCommentArticleDto?> UpdateReportCommentArticleFromDb(UpdateReportCommentArticleDto updateReportCommentDto);
    public Task<ReportCommentArticleDto?> GetOneReportCommentArticleFromDb(int idUser, int idComment, int idReason);
    public Task<IEnumerable<ReportCommentArticleDto>> GetReportCommentArticlesFromDb();
}