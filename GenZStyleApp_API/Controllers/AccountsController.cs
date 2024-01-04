using FluentValidation;
using FluentValidation.Results;
using GenZStyleAPP.BAL.DTOs.Account;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    public class AccountsController : ODataController
    {
        private IAccountRepository _accountRepository;
        private IValidator<ChangePasswordRequest> _changePasswordValidator;
        

        public AccountsController(IAccountRepository accountRepository, 
            IValidator<ChangePasswordRequest> changePasswordValidator)
        {
            _accountRepository = accountRepository;
            _changePasswordValidator = changePasswordValidator;
        }

        [EnableQuery]
        [HttpPut("odata/Accounts/{key}/Update")]
        public async Task<IActionResult> Put([FromRoute] int key, [FromBody] ChangePasswordRequest changePasswordRequest)
        {
            ValidationResult validationResult = await _changePasswordValidator.ValidateAsync(changePasswordRequest);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }
            GetAccountResponse result = await this._accountRepository.ChangPassword(key, changePasswordRequest);
            return Updated(result);
        }

    }
}
