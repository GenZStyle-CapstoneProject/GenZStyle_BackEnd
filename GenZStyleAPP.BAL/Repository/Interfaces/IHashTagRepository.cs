using GenZStyleAPP.BAL.DTOs.HashTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IHashTagRepository
    {
        public Task<List<GetHashTagResponse>> GetHashTagsAsync();
    }
}
