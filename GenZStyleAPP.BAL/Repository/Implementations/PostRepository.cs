using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class PostRepository : IPostRepository               
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PostRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }


        public async Task<List<GetPostResponse>> GetPostByUserFollowId(HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                List<Post> getPostResponses = new List<Post>();
                var listFollowingIds = new List<int>();
                var listRelation = await _unitOfWork.userRelationDAO.GetFollowing(account.AccountId);
                foreach ( var userRelation in listRelation) 
                {
                    // Kiểm tra xem thuộc tính FollowingId có giá trị không null
                    if (userRelation.FollowingId != null)
                    {
                        // Thêm giá trị của FollowingId vào mảng số nguyên
                        listFollowingIds.Add(userRelation.FollowingId);
                        foreach (int followingId in listFollowingIds)
                        {
                            var post = await _unitOfWork.PostDAO.GetPostById(followingId);
                            getPostResponses.Add(post);
                        }

                    }
                    
                }
                    return this._mapper.Map<List<GetPostResponse>>(getPostResponses);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
