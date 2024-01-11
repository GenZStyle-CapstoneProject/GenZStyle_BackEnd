using GenZStyleAPP.BAL.DTOs.Posts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IPostRepository
    {
        public Task<List<GetPostResponse>> GetPostByUserFollowId(HttpContext httpContext);
    }
}
