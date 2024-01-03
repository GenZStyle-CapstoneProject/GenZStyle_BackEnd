using FluentValidation;
using FluentValidation.Results;
using GenZStyleApp.DAL.Models;
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


        public UserController(IUserRepository userRepository,
            IValidator<RegisterRequest> registerValidator
            )
        {
            this._userRepository = userRepository;
            this._registerValidator = registerValidator;
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
                .Register(registerRequest);
            return Ok();
        }
        #endregion

        [HttpGet("odata/Users/Active/User")]
        [EnableQuery]
        public async Task<IActionResult> ActiveUsers()
        {
            List<User> users = await this._userRepository.GetUsersAsync();
            return Ok(users);
        }
    }
}
