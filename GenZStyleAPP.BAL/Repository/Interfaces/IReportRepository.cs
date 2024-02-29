using GenZStyleAPP.BAL.DTOs.Reports;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IReportRepository
    {
        public Task<List<GetReportResponse>> GetAllReports();
        public Task<GetReportResponse> GetActiveReportName(string reportname);
        public Task<List<GetReportResponse>> BanReportAsync(int reportId);
        //public Task DeleteReportAsync(int postId, HttpContext httpContext);
        public Task<GetReportResponse> CreateNewReportByPostIdAsync(AddReportRequest addReportRequest, HttpContext httpContext);
        public Task<GetReportResponse> CreateNewReportByReporterIdAsync(AddReporterRequest addReportRequest, HttpContext httpContext);
    }
}
