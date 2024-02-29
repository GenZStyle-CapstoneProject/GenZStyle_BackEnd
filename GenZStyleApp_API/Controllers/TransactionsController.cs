using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Transactions;
using GenZStyleAPP.BAL.DTOs.Transactions.MoMo;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    public class TransactionsController : ODataController
    {
        private readonly IOptions<MomoConfigModel> _optionsMomo;
        private IValidator<PostTransactionRequest> _postTransactionValidator;
        private readonly ITransactionRepository _transactionRepository;


        public TransactionsController (IOptions<MomoConfigModel> optionsMomo,
                                        IValidator<PostTransactionRequest> postTransactionValidator,
                                        ITransactionRepository transactionRepository)
        {
            _optionsMomo = optionsMomo;
            _postTransactionValidator = postTransactionValidator;
            _transactionRepository = transactionRepository;
        }
        #region Creat wallet transaction(Momo)
        [HttpPost("odata/WalletTransactions/CreateMomoTransaction")]
        [EnableQuery]
        //[PermissionAuthorize("Customer")]
        public async Task<IActionResult> PostMomo([FromBody] PostTransactionRequest request)
        {
            var validationResult = await _postTransactionValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }

            var result = await this._transactionRepository.CreateWalletTransactionAsync(request, _optionsMomo.Value);
            return Ok(result);
        }
        #endregion
    }
}
