using FluentValidation;
using FluentValidation.Results;
using GenZStyleAPP.BAL.DTOs.Accounts;
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

        #region SearchByUserName
        [HttpGet("odata/Accounts/{username}/SearchByUserName")]
        [EnableQuery]
        public async Task<IActionResult> SearchByUserName([FromRoute] string username)
        {
            try
            {
                List<GetAccountResponse> accountDTOs = await _accountRepository.SearchByUserName(username);

                // Nếu muốn thực hiện bất kỳ xử lý hoặc kiểm tra nào đó trước khi trả kết quả, bạn có thể thêm vào đây

                if (accountDTOs.Count > 0)
                {
                    // Thành công, trả về thông báo thành công và danh sách tài khoản
                    return Ok(new { Message = "Tìm kiếm thành công.", Accounts = accountDTOs });
                }
                else
                {
                    // Không tìm thấy tài khoản, trả về thông báo không có kết quả
                    return Ok(new { Message = "Không tìm thấy tài khoản nào." });
                }
            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
            #endregion
        }
    }
}
