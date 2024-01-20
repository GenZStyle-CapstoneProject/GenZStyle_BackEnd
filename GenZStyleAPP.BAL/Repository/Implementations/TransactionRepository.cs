using AutoMapper;
using GenZStyleApp.DAL.Enums;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Transactions;
using GenZStyleAPP.BAL.DTOs.Transactions.MoMo;
using GenZStyleAPP.BAL.Heplers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Newtonsoft.Json;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class TransactionRepository : ITransactionRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public TransactionRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        #region Create wallet transaction
        public async Task<GetTransactionResponse> CreateWalletTransactionAsync(PostTransactionRequest model, MomoConfigModel _config)
        {
            try
            {
                #region Validation
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(model.Email);
                if (account == null)
                {
                    throw new NotFoundException("Account does not exist.");
                }
                #endregion

                #region Add transaction to Db (Status: Pending)
                // Must save to database to check Amount and [Currency = false] when there is a notification from Momo
                var orderId = DateTime.Now.Ticks.ToString();
                var wallet = await _unitOfWork.WalletDAO.GetWalletByAccountIdAsync(account.AccountId);

                Transaction transaction = new Transaction
                {
                    TransactionDate = DateTime.Now,
                    Amount = model.Amount,                  
                    TransStyle = TransactionEnum.TransactionType.DEPOSIT.ToString(),
                    status = (int)TransactionEnum.RechangeStatus.PENDING,
                    wallet = wallet!
                };

                await _unitOfWork.TransactionDAO.CreateWalletTransactionAsync(transaction);
                await _unitOfWork.CommitAsync();
                #endregion

                #region Send request to momo
                var requestId = orderId + "id";
                var rawData = $"accessKey={_config.AccessKey}&amount={model.Amount}&extraData={_config.ExtraData}&ipnUrl={_config.NotifyUrl}&orderId={orderId}&orderInfo={_config.OrderInfo}&partnerCode={_config.PartnerCode}&redirectUrl={model.RedirectUrl}&requestId={requestId}&requestType={_config.RequestType}";
                var signature = EncodeHelper.ComputeHmacSha256(rawData, _config.SecretKey!);

                var client = new RestClient(_config.PayGate! + "/create");
                var request = new RestRequest() { Method = Method.Post };
                request.AddHeader("Content-Type", "application/json; charset=UTF-8");

                // Body of request
                PostTransactionMomoRequest bodyContent = new PostTransactionMomoRequest
                {
                    partnerCode = _config.PartnerCode,
                    partnerName = _config.PartnerName,
                    storeId = _config.PartnerCode,
                    requestType = _config.RequestType,
                    ipnUrl = _config.NotifyUrl,
                    redirectUrl = model.RedirectUrl,
                    orderId = orderId,
                    amount = model.Amount,
                    lang = _config.Lang,
                    autoCapture = _config.AutoCapture,
                    orderInfo = _config.OrderInfo,
                    requestId = requestId,
                    extraData = _config.ExtraData,
                    orderExpireTime = _config.OrderExpireTime,
                    signature = signature
                };

                request.AddParameter("application/json", JsonConvert.SerializeObject(bodyContent), ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var responseContent = JsonConvert.DeserializeObject<PostTransactionMomoResponse>(response.Content!);
                    var walletResponse = _mapper.Map<GetTransactionResponse>(transaction);
                    walletResponse.PayUrl = responseContent!.PayUrl;
                    walletResponse.Deeplink = responseContent!.Deeplink;
                    walletResponse.QrCodeUrl = responseContent!.QrCodeUrl;
                    walletResponse.Applink = responseContent.Applink;

                    return walletResponse;
                }

                throw new Exception("No server is available to handle this request.");

                #endregion

            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion
    }
}
