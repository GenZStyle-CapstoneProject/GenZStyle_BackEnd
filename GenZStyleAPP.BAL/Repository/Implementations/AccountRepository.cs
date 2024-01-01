using AutoMapper;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class AccountRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public AccountRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }



    }
}
