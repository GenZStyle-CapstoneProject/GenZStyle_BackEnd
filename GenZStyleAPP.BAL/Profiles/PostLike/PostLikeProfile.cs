using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.PostLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.PostLike
{
    public class PostLikeProfile : Profile
    {
        

        public PostLikeProfile()
        {
            CreateMap<Post ,GetPostLikeResponse>().ReverseMap();
            CreateMap<Like, GetPostLikeResponse>().ReverseMap();
        }
    }
}
