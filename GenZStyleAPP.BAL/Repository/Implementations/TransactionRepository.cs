using AutoMapper;
using GenZStyleApp.DAL.Enums;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Invoices;
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

                Invoice invoice = new Invoice
                {
                    RechargeID = orderId,
                    Date = DateTime.Now,
                    Total = model.Amount,
                    PaymentType = TransactionEnum.TransactionType.DEPOSIT.ToString(),
                    Status = (int)TransactionEnum.RechangeStatus.PENDING,
                    Wallet = wallet!
                };

                await _unitOfWork.InvoiceDAO.CreateInvoiceAsync(invoice);
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
                    var walletResponse = _mapper.Map<GetTransactionResponse>(invoice);
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

        #region Listen notification from Momo
        public async Task<GetTransactionResponse> PaymentNotificationAsync(string id, MomoConfigModel _config)
        {
            try
            {
                #region Validation

                var invoice = await _unitOfWork.InvoiceDAO.GetInvoiceByRechargeId(id);
                if (invoice == null)
                {
                    throw new NotFoundException("Transaction does not exist.");
                }

                if (invoice.Status == (int)TransactionEnum.RechangeStatus.SUCCESSED ||
                    invoice.Status == (int)TransactionEnum.RechangeStatus.FAILED)
                {
                    throw new BadRequestException("This transaction has been processed.");
                }

                #region Query transaction
                var requestId = invoice.RechargeID + "id";
                var rawData = $"accessKey={_config.AccessKey}&orderId={invoice.RechargeID}&partnerCode={_config.PartnerCode}&requestId={requestId}";
                var signature = EncodeHelper.ComputeHmacSha256(rawData, _config.SecretKey!);

                var client = new RestClient(_config.PayGate! + "/query");
                var request = new RestRequest() { Method = Method.Post };
                request.AddHeader("Content-Type", "application/json; charset=UTF-8");

                QueryTransactionMomoRequest queryTransaction = new QueryTransactionMomoRequest
                {
                    partnerCode = _config.PartnerCode,
                    requestId = requestId,
                    orderId = invoice.RechargeID,
                    lang = _config.Lang,
                    signature = signature

                };

                request.AddParameter("application/json", JsonConvert.SerializeObject(queryTransaction), ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);

                var responseResult = JsonConvert.DeserializeObject<QueryTransactionMomoResponse>(response.Content!);
                #endregion

                // Check Amount and [Currency = fasle] 
                if (responseResult!.Amount != invoice.Total)
                {
                    throw new BadRequestException("Amount of transaction and notification does not matched!");
                }

                // Check legit of signature - coming soon
                #endregion

                #region Update wallettransaction and wallet (if success)
                // ResultCode = 0: giao dịch thành công
                // ResultCode = 9000: giao dịch được cấp quyền (authorization) thành công
                if (responseResult.ResultCode == 0 || responseResult.ResultCode == 9000)
                {
                    invoice.Status = (int)TransactionEnum.RechangeStatus.SUCCESSED;
                    _unitOfWork.InvoiceDAO.UpdateInvoice(invoice);

                    // If amount = null, amount = default value of type
                    invoice.Wallet.Balance += responseResult.Amount.GetValueOrDefault(0m);
                    _unitOfWork.WalletDAO.UpdateWallet(invoice.Wallet);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<GetTransactionResponse>(invoice);
                }
                else if (responseResult.ResultCode == 1000)
                {
                    throw new BadRequestException("Transaction is initiated, waiting for user confirmation!");
                }
                else
                {
                    invoice.Status = (int)TransactionEnum.RechangeStatus.FAILED;
                    _unitOfWork.InvoiceDAO.UpdateInvoice(invoice);
                    await _unitOfWork.CommitAsync();

                    throw new BadRequestException("Recharge failed!");
                }
                #endregion
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (BadRequestException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new BadRequestException(error);
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
