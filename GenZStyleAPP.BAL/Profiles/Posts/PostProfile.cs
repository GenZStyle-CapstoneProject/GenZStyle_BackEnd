using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.Posts
{
    public class PostProfile: Profile
    {
        public PostProfile() 
        {
            CreateMap<AddPostRequest, Post>().ReverseMap();
            //CreateMap<Post, GetPostResponse>().ForMember(dest => dest.FashionItems, opt => opt.MapFrom(src => src.FashionItems)).ReverseMap();
            CreateMap<Post, UpdatePostRequest>().ReverseMap();
            CreateMap<Post, GetPostResponse>().ReverseMap();
        }
        
    }
}
