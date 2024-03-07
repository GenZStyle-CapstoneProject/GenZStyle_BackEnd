using BMOS.BAL.Authorization;
using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Transactions;
using GenZStyleAPP.BAL.DTOs.Transactions.MoMo;
using GenZStyleAPP.BAL.DTOs.Transactions.ZaloPay;
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
        private readonly IOptions<ZaloConfigModel> _optionsZalopay;
        private IValidator<PostTransactionRequest> _postTransactionValidator;
        private readonly ITransactionRepository _transactionRepository;


        public TransactionsController (IOptions<MomoConfigModel> optionsMomo,
                                        IOptions<ZaloConfigModel> optionsZalopay,
                                        IValidator<PostTransactionRequest> postTransactionValidator,
                                        ITransactionRepository transactionRepository)
        {
            _optionsZalopay = optionsZalopay;
            _optionsMomo = optionsMomo;
            _postTransactionValidator = postTransactionValidator;
            _transactionRepository = transactionRepository;
        }
        #region Creat wallet transaction(Momo)
        [HttpPost("odata/WalletTransactions/CreateMomoTransaction")]
        [EnableQuery]
        //[PermissionAuthorize("Customer")]
        public async Task<IActionResult> PostMomo([FromBody] PostTransactionRequest request, int PackageId)
        {
            var validationResult = await _postTransactionValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }

            var result = await this._transactionRepository.CreateWalletTransactionAsync(request, PackageId, _optionsMomo.Value);
            return Ok(result);
        }
        #region Creat wallet transaction(Zalopay)
        [HttpPost("odata/WalletTransactions/CreateZalopayTransaction")]
        [EnableQuery]
        //[PermissionAuthorize("Customer")]
        public async Task<IActionResult> PostZalopay([FromBody] PostTransactionRequest request)
        {
            var validationResult = await _postTransactionValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }

            var result = await this._transactionRepository.CreateZaloTransactionAsync(request, _optionsZalopay.Value);
            return Ok(result);
        }
        #endregion
        #endregion
        #region IPN MOMO || Query transaction.then(Update transaction)
        [EnableQuery]
        [HttpPut("Invoice/{key}/UpdateInvoice")]
        /*[PermissionAuthorize("Customer")]*/
        public async Task<IActionResult> Put([FromRoute] string key)
        {
            var result = await _transactionRepository.PaymentNotificationAsync(key, _optionsMomo.Value);
            return Updated(result);
        }
        #endregion

        #region IPN Zalo || Query transaction.then(Update transaction)
        [EnableQuery]
        [HttpPut("Invoice/{key}/UpdateInvoiceZalo")]
        /*[PermissionAuthorize("Customer")]*/
        public async Task<IActionResult> UpdateZaloPay([FromRoute] string key)
        {
            var result = await _transactionRepository.PaymentNotificationAsync(key, _optionsZalopay.Value);
            return Updated(result);
        }
        #endregion
    }
}
