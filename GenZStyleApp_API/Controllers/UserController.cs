using FluentValidation;
using FluentValidation.Results;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.Repository.Interfaces;
using GenZStyleAPP.BAL.Validators.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    public class UserController : ODataController
    {
        private IUserRepository _userRepository;
        private IValidator<RegisterRequest> _registerValidator;
        private IValidator<UpdateUserRequest> _updateUserValidator;

        public UserController(IUserRepository userRepository,
            IValidator<RegisterRequest> registerValidator,
            IValidator<UpdateUserRequest> updateUserValidator
            )
        {

            this._userRepository = userRepository;
            this._registerValidator = registerValidator;
            this._updateUserValidator = updateUserValidator;
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
            User user = await this._userRepository.GetActiveUser(userId);
            return Ok(user);
        }

        #region Update Product
        [HttpPut("User/Update/{userId}")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Put([FromRoute] int userId, [FromForm] UpdateUserRequest updateUserRequest)
        {
            var resultValid = _updateUserValidator.Validate(updateUserRequest);
            if (!resultValid.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(resultValid);
                throw new BadRequestException(error);
            }
            User user = await this._userRepository.UpdateUserAsync(userId, updateUserRequest, HttpContext);
            return Updated(user);
        }
        #endregion

        #region Delete User 
        
        [HttpDelete("User/{userId}")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Delete([FromRoute] int userId)
        {
            await this._userRepository.DeleteUserAsync(userId, this.HttpContext);
            return NoContent();
        }
        #endregion

    }
}
