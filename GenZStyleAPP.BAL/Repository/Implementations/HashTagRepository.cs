using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.Repository.Interfaces;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.HashTags;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class HashTagRepository: IHashTagRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HashTagRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetHashTagReponse>> SearchByHashTagName(string hashtag)
        {
            try
            {
                
                List<Hashtag> hashtags = await _unitOfWork.HashTagDAO.SearchByHashTagName(hashtag);

                
                List<GetHashTagReponse> hashtagDTOs = _mapper.Map<List<GetHashTagReponse>>(hashtags);

                return hashtagDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
