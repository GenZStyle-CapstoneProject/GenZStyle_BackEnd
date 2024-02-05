using FluentValidation;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.DTOs.Reports;
using GenZStyleAPP.BAL.Repository.Interfaces;
using GenZStyleAPP.BAL.Validators.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ReportsController : ODataController
    {
        private IReportRepository _reportRepository;
        private IOptions<FireBaseImage> _firebaseImageOptions;
        private IValidator<AddReportRequest> _reportValidator;

        public ReportsController(IReportRepository reportRepository, IOptions<FireBaseImage> firebaseImageOptions, IValidator<AddReportRequest> reportValidator)
        {
            _reportRepository = reportRepository;
            _firebaseImageOptions = firebaseImageOptions;
            _reportValidator = reportValidator;
        }


        #region Get AllReports
        [HttpGet("odata/Reports/Active/GetAllReport")]
        [EnableQuery]
        public async Task<IActionResult> GetAllReports()
        {
            List<GetReportResponse> reports = await this._reportRepository.GetAllReports();
            return Ok(reports);
        }
        #endregion

        [HttpGet("odata/Reports/Active/Report/{reportname}")]
        //[EnableQuery(MaxExpansionDepth = 3)]
        public async Task<IActionResult> ActiveReportByReportName(string reportname)
        {
            try
            {
                GetReportResponse report = await this._reportRepository.GetActiveReportName(reportname);

                // Kiểm tra nếu user không tồn tại
                if (report == null)
                {
                    return BadRequest("User not found. Please provide a valid userId.");
                }

                return Ok(new
                {
                    Status = "Get User By Id Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        #region Create New Report
        [HttpPost("odata/Report/AddNewReport")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Post([FromForm] AddReportRequest addReportRequest)
        {
            try
            {
                var resultValid = await _reportValidator.ValidateAsync(addReportRequest);
                if (!resultValid.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(resultValid);
                    throw new BadRequestException(error);
                }
                GetReportResponse report = await this._reportRepository.CreateNewReportAsync(addReportRequest, HttpContext);
                return Ok(new
                {
                    Status = "Add Report Success",
                    Data = Created(report)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        //ban Report
        [EnableQuery]
        [HttpPut("odata/Reports/{key}/BanReportByPostId")]
        //[PermissionAuthorize("Store Owner")]
        public async Task<IActionResult> BanReportByPostId([FromRoute] int key)
        {
            try
            {
                List<GetReportResponse> report = await this._reportRepository.BanReportAsync(key);
                if (report != null)
                {
                    return Ok(new
                    {
                        Status = " Accept Successfully",
                        Data = report
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Status = -1,
                        Message = "Ban Report Fail"
                    });

                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    
        //#region Delete Report

        //[HttpDelete("Report/{postId}")]
        //[EnableQuery]
        ////[PermissionAuthorize("Staff")]
        //public async Task<IActionResult> Delete([FromRoute] int postId)
        //{
        //    await this._reportRepository.DeleteReportAsync(postId, this.HttpContext);
        //    return Ok(new
        //    {
        //        Status = "Delete Report Success",

        //    });
        //}
        //#endregion

    }
}
