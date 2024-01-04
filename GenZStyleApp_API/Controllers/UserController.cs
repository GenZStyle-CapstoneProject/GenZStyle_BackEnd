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
            try
            {
                List<User> users = await this._userRepository.GetUsersAsync();
                return Ok(new
                {
                    Status = "Get List Success",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("odata/Users/Active/User/{userId}")]
        [EnableQuery(MaxExpansionDepth = 3)]
        public async Task<IActionResult> ActiveUser(int userId)
        {
            try
            {
                User user = await this._userRepository.GetActiveUser(userId);
                return Ok(new
                {
                    Status = "Get User By Id Success",
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        #region Update User
        [HttpPut("User/{key}/UpdateUser")]
        [EnableQuery]
        //[PermissionAuthorize("Customer", "Store Owner")]
        public async Task<IActionResult> Put([FromRoute] int key, [FromForm] UpdateUserRequest updateUserRequest)
        {
            try
            {
                ValidationResult validationResult = await _updateUserValidator.ValidateAsync(updateUserRequest);
                if (!validationResult.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(validationResult);
                    throw new BadRequestException(error);
                }
                User user = await this._userRepository.UpdateUserProfileByAccountIdAsync(key,
                                                                                                                    //_firebaseImageOptions.Value,
                                                                                                                    updateUserRequest);

                return Ok(new
                {
                    Status = "Update User Success",
                    Data = Updated(user)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

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


        //ban user
        [EnableQuery]
        [HttpPut("odata/Users/{key}/Ban")]
        //[PermissionAuthorize("Store Owner")]
        public async Task<IActionResult> BanUser([FromRoute] int key)
        {
            try
            {
                User user = await this._userRepository.BanUserAsync(key);
                if(user != null)
                {
                    return Ok(new
                    {
                        Status = "Ban User Successfully",
                        Data = user
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Status = -1,
                        Message = "Ban User Fail"
                    });
                
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}

