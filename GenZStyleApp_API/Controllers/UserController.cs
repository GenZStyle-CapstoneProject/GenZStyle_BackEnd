using FluentValidation;
using FluentValidation.Results;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    public class UserController : ODataController
    {
        private IUserRepository _userRepository;
        private IValidator<RegisterRequest> _registerValidator;
        private IOptions<FireBaseImage> _firebaseImageOptions;



        public UserController(IUserRepository userRepository,
            IValidator<RegisterRequest> registerValidator,
            IOptions<FireBaseImage> firebaseImageOptions
            )
        {
            this._userRepository = userRepository;
            this._registerValidator = registerValidator;
            this._firebaseImageOptions = firebaseImageOptions;
        }

        #region Register
        [HttpPost("odata/User/Register")]
        [EnableQuery]
        public async Task<IActionResult> Post([FromForm] RegisterRequest registerRequest)
       {
            ValidationResult validationResult = await _registerValidator.ValidateAsync(registerRequest);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }
            GetUserResponse customer = await this._userRepository
                .Register(_firebaseImageOptions.Value, registerRequest);
            return Ok();
        }
        #endregion
    }
}
