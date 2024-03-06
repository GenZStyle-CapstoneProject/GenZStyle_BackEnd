using GenZStyleAPP.BAL.DTOs.Collections;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface ICollectionRepository
    {
        public Task<GetCollectionResponse> SavePostToCollection(int postId, HttpContext httpContext);
    }
}
