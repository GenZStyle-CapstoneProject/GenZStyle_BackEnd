using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class ReportDAO
    {
        private GenZStyleDbContext _dbContext;
        public ReportDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        //get reports
        public async Task<List<Report>> GetAllReposts()
        {
            try
            {

                List<Report> reports = await _dbContext.Reports.ToListAsync();
                return reports;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Add new Report
        public async Task AddNewReport(Report report)
        {
            try
            {
                await _dbContext.Reports.AddAsync(report);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Report> GetReportByName(string reportname)
        {

            try
            {
                return await this._dbContext.Reports
                                    
                                    .SingleOrDefaultAsync(a => a.ReportName.Equals(reportname));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Report>> GetReportByPostIdAsync(int postId)
        {
            try
            {

                return await _dbContext.Reports.Where(u => u.Post.PostId == postId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Report>> GetReportsByReporterId(int reporterId)
        {
            try
            {

                return await _dbContext.Reports.Where(u => u.ReporterId == reporterId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Report> GetReportByReportIdAsync(int reportId)
            {
                try
                {

                    return await _dbContext.Reports.SingleOrDefaultAsync(u => u.Id == reportId);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        public async Task<List<Report>> GetReportsToDeleteAsync()
        {
            try
            {
                return await _dbContext.Reports
                    .Where(report => report.IsStatusReport == 1) // Lọc ra các báo cáo có IsStatusReport là 1
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ một cách phù hợp
                throw new Exception(ex.Message);
            }
        }

        //ban report by updating status
        public void AcceptReport(Report report)
        {
            try
            {
                this._dbContext.Entry<Report>(report).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region DeleteReport
        public void DeleteReport(Report report)
        {
            try
            {
                this._dbContext.Reports.Remove(report);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
