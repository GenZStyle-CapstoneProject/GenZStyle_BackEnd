using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Notifications;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.Repository.Interfaces;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.FashionItems;
using GenZStyleAPP.BAL.DTOs.HashTags;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class FashionItemRepository: IFashionItemRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public FashionItemRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetFashionItemResponse>> SearchByFashionName(string fashionName)
        {
            try
            {

                List<FashionItem> fashions = await _unitOfWork.FashionItemDAO.SearchByFashionName(fashionName);


                List<GetFashionItemResponse> fashionDTOs = _mapper.Map<List<GetFashionItemResponse>>(fashions);

                return fashionDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

