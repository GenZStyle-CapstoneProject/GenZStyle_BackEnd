using AutoMapper;
using BMOS.DAL.Enums;
using GenZStyleApp.DAL.Enums;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.Package;
using GenZStyleAPP.BAL.Exceptions;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class PackageRepository : IPackageRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PackageRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetPackageResponse> PurcharePackage(int PackageId, HttpContext httpContext)
        {
            try
            {   var role = await _unitOfWork.RoleDAO.GetRoleAsync((int)RoleEnum.Role.Blogger);
                var Package = await _unitOfWork.packageDAO.GetPackageByIdAsync(PackageId);
                if (Package == null)
                {
                    throw new NotFoundException("Package does not exist");
                }
                // Example: Check if the package is available for registration
                
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                var user = await _unitOfWork.UserDAO.GetUserByEmailAsync(emailFromClaim);
                var walletCustomer = await _unitOfWork.WalletDAO.GetWalletByAccountIdAsync(account.AccountId);

                var registrationFee = Package.Cost; 

                if (walletCustomer.Balance < registrationFee)
                {
                    throw new BadRequestException("Your account balance is not enough for package registration.");
                }
                // Tạo giao dịch ví cho đăng ký gói dịch vụ
                var registrationTransaction = new Invoice
                {
                    RechargeID = DateTime.Now.Ticks.ToString(),
                    Date = DateTime.Now,
                    Total = registrationFee,
                    //Content = $"Package registration for {Package.PackageName}",
                    PaymentType = TransactionEnum.TransactionType.SEND.ToString(),
                    Status = (int)TransactionEnum.RechangeStatus.SUCCESSED,
                    Wallet = walletCustomer
                };

                // Thực hiện ghi log và cập nhật số dư ví
                await _unitOfWork.InvoiceDAO.CreateInvoiceAsync(registrationTransaction);

                // Trừ số dư ví của khách hàng
                walletCustomer.Balance -= registrationFee;
                _unitOfWork.WalletDAO.UpdateWallet(walletCustomer);

                // Update role 
                user.Role = role; // Change to the new role
                _unitOfWork.UserDAO.UpdateUser(user);

                

                // Lưu thông tin đăng ký gói dịch vụ vào cơ sở dữ liệu
                var packageRegistration = new PackageRegistration
                {
                    AccountId = account.AccountId,
                    PackageId = PackageId,
                    RegistrationDate = DateTime.Now
                    // Các thông tin khác của đăng ký gói dịch vụ nếu cần
                };
                Package.PackageRegistrations.Add(packageRegistration);

                //await _unitOfWork.PackageRegistrationDAO.CreatePackageRegistrationAsync(packageRegistration);
                
                // Save and update changes in the database
                await _unitOfWork.CommitAsync();
                // Trả về thông tin gói dịch vụ sau khi đăng ký thành công
                return _mapper.Map<GetPackageResponse>(Package);


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
            catch (PaymentFailedException ex)
            {
                // Handle payment failure
                throw new BadRequestException("Payment failed. Please try again or choose a different payment method.");
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
            

        }

    }
}
