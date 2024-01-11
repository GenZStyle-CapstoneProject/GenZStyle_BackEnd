using AutoMapper;
using GenZStyleAPP.BAL.DTOs.FashionItems;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.FashionItems
{
    public class FashionItemProfile : Profile
    {
        public FashionItemProfile() 
        {
            CreateMap<FashionItem, GetFashionItemResponse>().ReverseMap();
        }
    }
}
