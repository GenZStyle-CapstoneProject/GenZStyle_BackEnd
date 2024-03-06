using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.Users
{
    public class CustomerProfile : Profile
    {       
        public CustomerProfile() {

            CreateMap<User, GetUserResponse>().ReverseMap();
            CreateMap<User, UpdateUserRequest>().ReverseMap();
        }
        
    }
}
