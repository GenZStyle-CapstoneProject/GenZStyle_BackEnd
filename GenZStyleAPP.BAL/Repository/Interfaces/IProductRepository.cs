using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IProductRepository
    {
        public  Task<GetProductResponse> CreateNewProductAsync(AddProductRequest addProductRequest, FireBaseImage fireBaseImage, HttpContext httpContext);
    }
}
