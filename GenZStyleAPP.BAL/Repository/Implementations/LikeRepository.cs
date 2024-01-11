using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.PostLike;
using GenZStyleAPP.BAL.Repository.Interfaces;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
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

        public async Task<GetPostLikeResponse> GetLikeByPostIdAsync(int postId)
        {
            try
            {
                var post = await _unitOfWork.PostDAO.GetPostByIdAsync(postId);
                if (post == null)
                {
                    throw new NotFoundException("PostId does not exist in system.");
                }
                Like like = new Like
                {
                    PostId = postId,
                    Post = post,                    
                };
                
                await _unitOfWork.LikeDAO.AddLikeAsync(like);
                      _unitOfWork.LikeDAO.ChangeLike(post);
                
                await _unitOfWork.CommitAsync();
                return _mapper.Map<GetPostLikeResponse>(post);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
