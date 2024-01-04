using AutoMapper;
using BMOS.BAL.DTOs.Authentications;
using BMOS.BAL.DTOs.JWT;
using BMOS.BAL.Helpers;
using BMOS.DAL.Enums;
using BMOS.DAL.Models;
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

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Issuer = jwtAuth.Issuer,
                    Audience = jwtAuth.Audience,
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddHours(1),
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
                    ExpiredDate = DateTime.UtcNow.AddDays(5),
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
