using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.PostLike;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class LikeRepository : ILikeRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public LikeRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetPostLikeResponse> GetLikeByPostIdAsync(int postId, HttpContext httpContext)
        {
            try
            {
                var post = await _unitOfWork.PostDAO.GetPostById(postId);
                if (post == null)
                {
                    throw new NotFoundException("PostId does not exist in system.");
                }
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                
                
                post.TotalLike += 1;
                Like like = new Like
                {
                    LikeBy = account.AccountId,
                    PostId = postId,
                    Post = post,
                    Account = account,

                };
                Notification notification = new Notification
                {
                    CreateAt = DateTime.Now,
                    AccountId = post.AccountId,
                    Message = account.Lastname + " " + "đã like bài viết của bạn",
                    Account = account,
                };

                await _unitOfWork.LikeDAO.AddLikeAsync(like);
                await _unitOfWork.NotificationDAO.AddNotiAsync(notification);        
                      _unitOfWork.LikeDAO.ChangeLike(post);

                await _unitOfWork.CommitAsync();
                return _mapper.Map<GetPostLikeResponse>(post);

            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("PostId", ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
    }
}
