using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.HashTag;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using System.ComponentModel.DataAnnotations;

namespace GenZStyleApp_API.Controllers
{
    public class HashTagsController : ODataController
    {
        private IHashTagRepository _hashTagRepository;
        private IValidator<GetHashTagRequest> _getHashTagValidator;
        private IOptions<FireBaseImage> _firebaseImageOptions;

        public HashTagsController(IHashTagRepository hashTagRepository,
                                  IValidator<GetHashTagRequest> getHashTagRequest,
                                    IOptions<FireBaseImage> firebaseImageOptions)
        {
            this._hashTagRepository = hashTagRepository;
            this._getHashTagValidator = getHashTagRequest;
            this._firebaseImageOptions = firebaseImageOptions;
        }
        #region GetHashTags
        [EnableQuery]
        [HttpGet("odata/Hashtags/GetHashTag")]
        public async Task<IActionResult> Get()
        {


            List<GetHashTagResponse> result = await _hashTagRepository.GetHashTagsAsync();
            return Ok(result);
        }
        #endregion


        [EnableQuery]
        [HttpPost("odata/Hashtags")]
        public async Task<IActionResult> Post([FromForm] GetHashTagRequest hashTagRequest)
        {
            var validationResult = await _getHashTagValidator.ValidateAsync(hashTagRequest);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }
            GetHashTagResponse hashTagResponse = await _hashTagRepository.AddNewHashTag(_firebaseImageOptions.Value, hashTagRequest);
            return Created(hashTagResponse);
        }


    }


}
