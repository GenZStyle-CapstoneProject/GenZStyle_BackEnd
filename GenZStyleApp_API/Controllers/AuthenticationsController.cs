using BMOS.BAL.DTOs.JWT;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.DTOs.Authentications;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Repositories.Interfaces;
using ProjectParticipantManagement.WebAPI.Helpers;

namespace ProjectParticipantManagement.WebAPI.Controllers
{
    public class AuthenticationsController : ODataController
    {
        private IAuthenticationRepository _authenticationRepository;
        private IValidator<GetLoginRequest> _authenticationValidator;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public AuthenticationsController(IAuthenticationRepository authenticationRepository, 
                                        IValidator<GetLoginRequest> authenticationValidator,
                                        IOptions<JwtAuth> jwtAuthOptions)
        {
            this._authenticationRepository = authenticationRepository;
            this._authenticationValidator = authenticationValidator;
            this._jwtAuthOptions = jwtAuthOptions;
        }

        [EnableQuery]
        [HttpPost("odata/authentications/login")]
        public async Task<IActionResult> Login([FromBody] GetLoginRequest account)
        {
            ValidationResult validationResult = await this._authenticationValidator.ValidateAsync(account);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }
            var result = await this._authenticationRepository.LoginAsync(account,_jwtAuthOptions.Value );
            return Ok(result);
        }
    }
}
