using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.Repository.Interfaces;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections;
using System.Collections.Generic;
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


        public async Task<List<GetCommentResponse>> GetCommentByPostId(int id)
        {
            try
            {
                var comment = await _unitOfWork.CommentDAO.GetCommentByPostIdAsync(id);
                return _mapper.Map<List<GetCommentResponse>>(comment);
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

        public async Task<GetCommentResponse> UpdateCommentByPostId(GetCommentRequest commentRequest ,int PostId )
        {
            try
            {
                var post = await _unitOfWork.PostDAO.GetPostById(PostId);

                
                Comment comment = new Comment
                {
                    Content = commentRequest.Content,
                    CreateAt = commentRequest.CreateAt,
                    PostId = PostId,
                                         
                };
                
                post.Comments.Add(comment);
                await _unitOfWork.CommentDAO.AddCommentAsync(comment);
                return _mapper.Map<GetCommentResponse>(comment);



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

