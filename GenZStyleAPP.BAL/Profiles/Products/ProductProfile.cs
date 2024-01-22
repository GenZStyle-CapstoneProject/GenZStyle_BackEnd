using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.Products;

namespace GenZStyleAPP.BAL.Profiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<AddProductRequest, Product>().ReverseMap(); 
            CreateMap<Product, GetProductResponse>().ReverseMap();

        }

    }
}
