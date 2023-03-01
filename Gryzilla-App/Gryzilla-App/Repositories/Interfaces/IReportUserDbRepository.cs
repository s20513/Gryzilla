using Gryzilla_App.DTOs.Requests.ReportPost;
using Gryzilla_App.DTOs.Requests.ReportUser;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IReportUserDbRepository
{
    public Task<ReportUserDto?> AddReportUserToDb(NewReportUserDto newReportUserDto);
    public Task<ReportUserDto?> UpdateReportUserFromDb(ModifyReportUser modifyReportUser);
    public Task<IEnumerable<ReportUserDto?>> GetUsersReportsFromDb();
    public Task<ReportUserDto?> GetUserReportFromDb(int idReport);
    public Task<ReportUserDto?> DeleteReportUserFromDb(int idReport);
}