using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CommentRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }


        public async Task <List<GetCommentResponse>> GetCommentByPostId(int id)
        {
            try
            {
                var post = await _unitOfWork.CommentDAO.GetCommentByPostIdAsync(id);
                return _mapper.Map<List<GetCommentResponse>>(post);
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
  

        public async Task<GetCommentResponse> UpdateCommentByPostId(GetCommentRequest commentRequest ,int PostId,HttpContext httpContext )
        {
            try
            {
                var post = await _unitOfWork.PostDAO.GetPostByIdAsync(PostId);
                if (post == null)
                {
                    throw new NotFoundException("PostId does not exist in system.");
                }

                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                post.TotalComment += 1;
                Comment comment = new Comment
                {
                    Content = commentRequest.Content,
                    CreateAt = commentRequest.CreateAt,
                    PostId = PostId,
                    CommentBy = post.PostId,
                    Account = post.Account,
                    Post = post,
                };
                Notification notification = new Notification
                {
                    CreateAt = DateTime.Now,
                    AccountId = post.AccountId,
                    Message = account.Lastname + " " + "đã bình luận bài viết của bạn",
                    Account = account,
                };

                /*post.Comments.Add(comment);*/
                await _unitOfWork.CommentDAO.AddCommentAsync(comment);
                     this._unitOfWork.PostDAO.UpdatePostComment(post);
                await _unitOfWork.NotificationDAO.AddNotiAsync(notification);

                await _unitOfWork.CommitAsync();
                return _mapper.Map<GetCommentResponse>(post);



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


    }
}

