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

        // GetAll
        [HttpGet("odata/Users/Active/User")]
        [EnableQuery]
        public async Task<IActionResult> ActiveUsers()
        {
            List<User> users = await this._userRepository.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("odata/Users/Active/User/{userId}")]
        [EnableQuery(MaxExpansionDepth = 3)]
        public async Task<IActionResult> ActiveUser(int userId)
        {
            User user = await this._userRepository.GetUserDetailByIdAsync(userId);
            return Ok(user);
        }

        #region Update Product
        [HttpPut("UpdateUser")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Put([FromRoute] int key, [FromForm] UpdateUserRequest updateUserRequest)
        {
            //var resultValid = _updateProductValidator.Validate(updateUserRequest);
            //if (!resultValid.IsValid)
            //{
            //    string error = ErrorHelper.GetErrorsString(resultValid);
            //    throw new BadRequestException(error);
            //}
            User user = await this._userRepository.UpdateUserAsync(key, updateUserRequest, HttpContext);
            return Updated(user);
        }
        #endregion

    }
}
