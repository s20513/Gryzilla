using Gryzilla_App.DTOs.Requests.ReportCommentArticle;
using Gryzilla_App.DTOs.Requests.ReportCommentPost;
using Gryzilla_App.DTOs.Responses.ReportCommentArticle;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IReportCommentArticleDbRepository
{
    public Task<ReportCommentArticleDto?> AddReportCommentArticleToDb(NewReportCommentArticleDto newReportCommentDto);
    public Task<ReportCommentArticleDto?> DeleteReportCommentArticleToDb(DefaultReportCommentArticleDto deleteReportCommentDto);
    public Task<ReportCommentArticleDto?> UpdateReportCommentArticleToDb(UpdateReportCommentArticleDto updateReportCommentDto);
    public Task<ReportCommentArticleDto?> GetOneReportCommentArticleToDb(int idUser, int idComment, int idReason);
    public Task<IEnumerable<ReportCommentArticleDto>> GetReportCommentArticlesToDb();
}