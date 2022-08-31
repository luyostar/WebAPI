using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubanData.Model;

namespace DoubanAPI.Model
{
    public class UsersProfile:Profile
    {
        public UsersProfile()
        {
            CreateMap<Users, UsersDto>()
                //.ForMember(dest=>dest.uId,option=>option.MapFrom(src=>src.uId))
                ;
        }
    }
}
