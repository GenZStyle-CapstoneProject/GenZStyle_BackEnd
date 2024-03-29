﻿using AutoMapper;
using GenZStyleApp.BAL.Helpers;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.DTOs.Reports;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public ReportRepository(IUnitOfWork unitOfWork, IMapper mapper, IServiceProvider serviceProvider)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<GetReportResponse>> GetPostReports()
        {
            try
            {
                List<Report> reports = await this._unitOfWork.ReportDAO.GetAllReposts();

                // Lọc danh sách báo cáo theo bài đăng sử dụng LINQ
                var postReports = reports.Where(r => r.PostId != null).ToList();

                // Sử dụng AutoMapper để ánh xạ danh sách báo cáo đã lọc sang các GetReportResponse tương ứng
                var mappedPostReports = this._mapper.Map<List<GetReportResponse>>(postReports);

                return mappedPostReports;
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        public async Task<List<GetReportResponse>> GetUserReports()
        {
            try
            {
                List<Report> reports = await this._unitOfWork.ReportDAO.GetAllReposts();

                // Lọc danh sách báo cáo theo người bị báo cáo sử dụng LINQ
                var userReports = reports.Where(r => r.ReporterId != null).ToList();

                // Sử dụng AutoMapper để ánh xạ danh sách báo cáo đã lọc sang các GetReportResponse tương ứng
                var mappedUserReports = this._mapper.Map<List<GetReportResponse>>(userReports);

                return mappedUserReports;
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        public async Task<List<GetReportResponse>> GetPostReportsByReportId(int reportId)
        {
            try
            {
                // Lấy báo cáo theo reportId
                Report report = await this._unitOfWork.ReportDAO.GetReportByReportIdAsync(reportId);

                if (report == null || report.PostId == null)
                {
                    throw new NotFoundException("No post reports found for the specified report.");
                }

                // Lấy tất cả các báo cáo liên quan đến cùng một bài đăng
                List<Report> reports = await this._unitOfWork.ReportDAO.GetReportByPostIdAsync(report.PostId.Value);

                // Sử dụng AutoMapper để ánh xạ danh sách báo cáo sang các GetReportResponse tương ứng
                var mappedReports = this._mapper.Map<List<GetReportResponse>>(reports);

                return mappedReports;
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        public async Task<List<GetReportResponse>> GetUserReportsByReportId(int reportId)
        {
            try
            {
                // Lấy báo cáo theo reportId
                Report report = await this._unitOfWork.ReportDAO.GetReportByReportIdAsync(reportId);

                if (report == null || report.ReporterId == null)
                {
                    throw new NotFoundException("No user reports found for the specified report.");
                }

                // Lấy tất cả các báo cáo liên quan đến cùng một người bị báo cáo
                List<Report> reports = await this._unitOfWork.ReportDAO.GetReportsByReporterId(report.ReporterId.Value);

                // Sử dụng AutoMapper để ánh xạ danh sách báo cáo sang các GetReportResponse tương ứng
                var mappedReports = this._mapper.Map<List<GetReportResponse>>(reports);

                return mappedReports;
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }



        public async Task<GetReportResponse> GetActiveReportName(string reportname)
        {
            try
            {
                Report report = await this._unitOfWork.ReportDAO.GetReportByName(reportname);
                if (report == null)
                {
                    throw new NotFoundException("User does not exist in the system.");
                }
                return this._mapper.Map<GetReportResponse>(report);
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("User Id", ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        #region CreateNewReportAsync
        public async Task<GetReportResponse> CreateNewReportByPostIdAsync(AddReportRequest addReportRequest, HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                Report existingReport = await _unitOfWork.ReportDAO.GetReportByName(addReportRequest.ReportName);

                Report report;

                if (existingReport != null)
                {
                    report = existingReport;
                }
                else
                {
                    // Create a new report
                    report = new Report
                    {
                        AccountId = accountStaff.AccountId,
                        PostId = addReportRequest.PostId,
                        ReporterId = null,
                        ReportName = addReportRequest.ReportName,
                        //IsReport = false,
                        Account = accountStaff
                    };

                    await _unitOfWork.ReportDAO.AddNewReport(report);
                }

                await _unitOfWork.CommitAsync();
                return this._mapper.Map<GetReportResponse>(report);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion

        #region CreateNewReportAsync
        public async Task<GetReportResponse> CreateNewReportByReporterIdAsync(AddReporterRequest addReportRequest, HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                Report existingReport = await _unitOfWork.ReportDAO.GetReportByName(addReportRequest.ReportName);

                Report report;

                if (existingReport != null)
                {
                    report = existingReport;
                }
                else
                {
                    // Create a new report
                    report = new Report
                    {
                        AccountId = accountStaff.AccountId,
                        PostId = null,
                        ReporterId = addReportRequest.ReporterId,
                        ReportName = addReportRequest.ReportName,
                        //IsReport = false,
                        Account = accountStaff
                    };

                    await _unitOfWork.ReportDAO.AddNewReport(report);
                }

                await _unitOfWork.CommitAsync();
                return this._mapper.Map<GetReportResponse>(report);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion






        public async Task<List<GetReportResponse>> BanReportAsync(int reportId, string status)
        {
            try
            {
                // Get the report by its Id
                Report report = await _unitOfWork.ReportDAO.GetReportByReportIdAsync(reportId);

                if (report == null)
                {
                    throw new NotFoundException("ReportId does not exist in the system.");
                }

                // Check if the report has IsStatusReport set to 0 (not accepted)
                if (report.IsStatusReport == 0)
                {
                    // Set IsStatusReport to 1 for the report (accepted)
                    report.IsStatusReport = 1;

                    // Update the status
                    if (status.ToLower() == "active")
                    {
                        report.Status = "Active";
                    }
                    else if (status.ToLower() == "reject")
                    {
                        report.Status = "Reject";
                        report.IsStatusReport = 2;
                    }

                    _unitOfWork.ReportDAO.AcceptReport(report);

                    await this._unitOfWork.CommitAsync();

                    // Check and delete post immediately if needed
                    await CheckAndDeletePost(report.PostId.Value);

                    // Assuming you want to return a response for the report
                    return _mapper.Map<List<GetReportResponse>>(new List<Report> { report });
                }
                else
                {
                    throw new InvalidOperationException("The report is already banned.");
                }
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }


        private async Task CheckAndDeletePost(int? postId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GenZStyleDbContext>();

                // Get the post by its Id with related reports
                var postToDelete = dbContext.Posts
                    .Include(p => p.Reports)
                    .SingleOrDefault(p => p.PostId == postId);

                // Check if the post exists and has enough reports
                if (postToDelete != null && postToDelete.Reports.Count(r => r.IsStatusReport == 1) >= 2)
                {
                    // Remove the post
                    dbContext.Posts.Remove(postToDelete);

                    // Save changes to the database
                    dbContext.SaveChanges();
                }
            }
        }
    }


        //public async Task<List<GetReportResponse>> RestoreReportAsync(int reportId)
        //{
        //    try
        //    {
        //        // Lấy báo cáo từ cơ sở dữ liệu
        //        var report = await _unitOfWork.ReportDAO.GetReportByReportIdAsync(reportId);

        //        if (report == null)
        //        {
        //            throw new NotFoundException("ReportId does not exist in the system.");
        //        }

                

        //        if (report.IsStatusReport == 0)
        //        {
        //            // Set IsStatusReport to 1 for the report (accepted)
        //            report.IsStatusReport = 2;
        //            _unitOfWork.ReportDAO.AcceptReport(report);

        //            await this._unitOfWork.CommitAsync();

                  

        //            // Assuming you want to return a response for the report
        //            return _mapper.Map<List<GetReportResponse>>(new List<Report> { report });
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("The report is already banned.");
        //        }

                
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ErrorHelper.GetErrorString(ex.Message);
        //        throw new Exception(error);
        //    }
        //}

        







    }

    //private async Task AutoDeletePostsAsync(int reportId)
    //{
    //    try
    //    {
    //        // Get the associated report
    //        Report report = await _unitOfWork.ReportDAO.GetReportByReportIdAsync(reportId);

    //        if (report != null && report.Post != null)
    //        {
    //            // Get the PostId from the associated report
    //            int postId = report.Post.PostId;

    //            // Check if there are more than 2 reports for the post
    //            if (report.Post.Reports.Count(r => r.IsReport) >= 2)
    //            {
    //                // Delete the post
    //                Post postToDelete = await _unitOfWork.PostDAO.GetPostByIdAsync(postId);
    //                if (postToDelete != null)
    //                {
    //                    _unitOfWork.PostDAO.DeletePost(postToDelete);
    //                    await _unitOfWork.CommitAsync();
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string error = ErrorHelper.GetErrorString(ex.Message);
    //        throw new Exception(error);
    //    }
    //}





    //#region DeleteReport
    //public async Task DeleteReportAsync(int postId, HttpContext httpContext)
    //{
    //    try
    //    {

    //        List<Report> reports = await _unitOfWork.ReportDAO.GetReportByPostIdAsync(postId);

    //        if (reports == null)
    //        {
    //            throw new NotFoundException("Post id does not exist in the system.");
    //        }

    //        // Đếm số lượng bài repost có IsReport là true
    //        int countTrueReports = reports.Count(report => report.IsReport);

    //        if (countTrueReports >= 2)
    //        {
    //            // Lấy tất cả bài repost có IsReport là true
    //            var repostsToDelete = reports.Where(report => report.IsReport).ToList();

    //            // Xóa tất cả các bài repost có IsReport là true
    //            foreach (var repost in repostsToDelete)
    //            {
    //                _unitOfWork.ReportDAO.DeleteReport(repost);
    //            }

    //            await _unitOfWork.CommitAsync();
    //        }
    //    }
    //    catch (NotFoundException ex)
    //    {
    //        string error = ErrorHelper.GetErrorString("Post Id", ex.Message);
    //        throw new NotFoundException(error);
    //    }
    //    catch (Exception ex)
    //    {
    //        string error = ErrorHelper.GetErrorString(ex.Message);
    //        throw new Exception(error);
    //    }
    //}
    //#endregion




