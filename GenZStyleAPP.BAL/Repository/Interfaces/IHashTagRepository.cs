using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.HashTag;
using GenZStyleAPP.BAL.DTOs.HashTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IHashTagRepository
    {
        public Task<List<GetHashTagReponse>> SearchByHashTagName(string hashtag);
        public Task<List<GetHashTagResponse>> GetHashTagsAsync();

        public  Task<GetHashTagResponse> AddNewHashTag(FireBaseImage fireBaseImage, GetHashTagRequest hashTagRequest);
    }
}
