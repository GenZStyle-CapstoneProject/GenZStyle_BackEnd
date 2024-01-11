using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.HashTag;

namespace GenZStyleAPP.BAL.Profiles.HashTagProfile
{
    
        public class HashtagProfile : Profile
        {

            public HashtagProfile()
            {
                CreateMap<Hashtag, GetHashTagResponse>().ReverseMap();
            }

        }
    
}
