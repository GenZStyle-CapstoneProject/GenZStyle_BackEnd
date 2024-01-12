using GenZStyleAPP.BAL.DTOs.Comments;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface ICommentRepository
    {
        public Task<GetCommentResponse> GetCommentByPostId(int id);

        public Task<GetCommentResponse> UpdateCommentByPostId(GetCommentRequest commentRequest, int PostId, HttpContext httpContext);

    }
}
