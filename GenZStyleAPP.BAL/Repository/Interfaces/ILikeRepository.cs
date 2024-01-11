using GenZStyleAPP.BAL.DTOs.PostLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface ILikeRepository
    {
        public  Task<GetPostLikeResponse> GetLikeByPostIdAsync(int postId);
    }
}
