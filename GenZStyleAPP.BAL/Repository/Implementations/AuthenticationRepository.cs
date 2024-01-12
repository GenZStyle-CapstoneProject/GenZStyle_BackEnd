using AutoMapper;
using BMOS.BAL.DTOs.Authentications;
using BMOS.BAL.DTOs.JWT;
using BMOS.BAL.Helpers;
using BMOS.DAL.Enums;
using GenZStyleApp.DAL.Models;
using GenZStyleApp.DAL.Models;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ProjectParticipantManagement.BAL.DTOs.Authentications;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.BAL.Repositories.Interfaces;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public AuthenticationRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }

        public async Task<PostLoginResponse> LoginAsync(GetLoginRequest request, JwtAuth jwtAuth)
        {
            try
            {
                var account = await _unitOfWork.AccountDAO.GetAccountByEmailAndPasswordAsync(request.UserName, request.PasswordHash.Trim());
                if (account == null)
                {
                    throw new BadRequestException("Email or password is invalid.");
                }

                var loginResponse = new PostLoginResponse();
                loginResponse.AccountId = account.AccountId;
                loginResponse.Email = account.Email;
                loginResponse.Role = account.User.Role.RoleName; 
                loginResponse.FullName = account.Lastname + " " + account.Firstname;

                //123abc2323
                var resultLogin = await GenerateToken(loginResponse, jwtAuth, account);
                return resultLogin;
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
        private bool IsAdmin(GetLoginRequest account)
        {
            try
            {
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfiguration configuration = builder.Build();

                string adminEmail = configuration.GetSection("AdminAccount:Email").Value;
                string adminPassword = configuration.GetSection("AdminAccount:Password").Value;
                if (account.UserName.Equals(adminEmail) && account.PasswordHash.Equals(adminPassword))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region GenerateToken
        private async Task<PostLoginResponse> GenerateToken(PostLoginResponse response, JwtAuth jwtAuth, Account account)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuth.Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new ClaimsIdentity(new[] {
                 new Claim(JwtRegisteredClaimNames.Sub, response.Email),
                 new Claim(JwtRegisteredClaimNames.Email, response.Email),
                 new Claim(JwtRegisteredClaimNames.Name, response.FullName),
                 new Claim("Role", response.Role),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             });
                Token tokenn = await _unitOfWork.TokenDAO.GetLastToken();
                
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Issuer = jwtAuth.Issuer,
                    Audience = jwtAuth.Audience,
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddDays(5),
                    SigningCredentials = credentials,
                };

                var token = jwtTokenHandler.CreateToken(tokenDescription);
                string accessToken = jwtTokenHandler.WriteToken(token);

                string refreshToken = GenerateRefreshToken();
                Token refreshTokenModel = new Token
                {
                   
                    JwtID = token.Id,
                    RefreshToken = refreshToken,
                    CreatedDate = DateTime.UtcNow,
                    ExpiredDate = DateTime.UtcNow.AddMinutes(5),           /*AddDays(5),*/
                    IsUsed = false,
                    IsRevoked = false,
                    Account = account,
                };
        
                await _unitOfWork.TokenDAO.CreateTokenAsync(refreshTokenModel);
                await _unitOfWork.CommitAsync();

                response.AccessToken = accessToken;
                response.RefreshToken = refreshToken;

                return response;
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion
        public async Task<PostRecreateTokenResponse> ReCreateTokenAsync(PostRecreateTokenRequest request, JwtAuth jwtAuth)
        {
            #region Config
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(jwtAuth.Key);
            var tokenValidationParameters = new TokenValidationParameters
            {
                //Tự cấp token nên phần này bỏ qua
                ValidateIssuer = false,
                ValidateAudience = false,
                //Ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateLifetime = false, //khong kiem tra token het han
                ClockSkew = TimeSpan.Zero // thoi gian expired dung voi thoi gian chi dinh
            };
            #endregion

            try
            {
                #region Validation
                //Check 1: Access token is valid format
                var tokenVerification = jwtTokenHandler.ValidateToken(request.AccessToken, tokenValidationParameters, out var validatedToken);

                //Check 2: Check Alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        throw new BadRequestException("Invalid token.");
                    }
                }

                //Check 3: check accessToken expried?
                var utcExpiredDate = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiredDate = DateHelper.ConvertUnixTimeToDateTime(utcExpiredDate);
                if (expiredDate > DateTime.UtcNow)
                {
                    throw new BadRequestException("Access token has not yet expired.");
                }

                //Check 4: Check refresh token exist in Db
                Token existedRefreshToken = await this._unitOfWork.TokenDAO.GetTokenByRefreshTokenAsync(request.RefreshToken);
                if (existedRefreshToken == null)
                {
                    throw new NotFoundException("Refresh token does not exist.");
                }

                //Check 5: Refresh Token is used / revoked?
                if (existedRefreshToken.IsUsed)
                {
                    throw new BadRequestException("Refresh token is used.");
                }
                if (existedRefreshToken.IsRevoked)
                {
                    throw new BadRequestException("Refresh token is revoked.");
                }

                //Check 6: Id of refresh token == id of access token
                var jwtId = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (existedRefreshToken.JwtID.Equals(jwtId) == false)
                {
                    throw new Exception("Refresh token is not match with access token.");
                }

                //Check 7: refresh token is expired
                if (existedRefreshToken.ExpiredDate < DateTime.UtcNow)
                {
                    throw new Exception("Refresh token expired.");
                }
                #endregion

                #region Update old refresh token in Db
                existedRefreshToken.IsRevoked = true;
                existedRefreshToken.IsUsed = true;
                this._unitOfWork.TokenDAO.UpdateToken(existedRefreshToken);
                await this._unitOfWork.CommitAsync();
                #endregion

                #region Create new token
                var loginResponse = new PostLoginResponse();
                loginResponse.AccountId = existedRefreshToken.Account.AccountId;
                loginResponse.Email = existedRefreshToken.Account.Email;
                loginResponse.Role = existedRefreshToken.Account.User.Role.RoleName;

                if (existedRefreshToken.Account.User.Role.Id == (int)RoleEnum.Role.User)
                {
                    var customer = await _unitOfWork.UserDAO.GetUserByAccountIdAsync(existedRefreshToken.Account.AccountId);
                    loginResponse.FullName = customer.City;
                }
                /*else if (existedRefreshToken.Account.User.Role.Id == (int)RoleEnum.Role.Blogger)
                {
                    var staff = await _unitOfWork.StaffDAO.GetStaffDetailAsync(existedRefreshToken.Account.ID);
                    loginResponse.FullName = staff.FullName;
                }*/
                else
                {
                    loginResponse.FullName = "Owner Store";
                }

                var newRefreshToken = await GenerateToken(loginResponse, jwtAuth, existedRefreshToken.Account);
                #endregion

                var newToken = new PostRecreateTokenResponse
                {
                    AccessToken = newRefreshToken.AccessToken,
                    RefreshToken = newRefreshToken.RefreshToken,
                };

                return newToken;
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
        #region Generate refresh token
        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        #endregion
    }
}
