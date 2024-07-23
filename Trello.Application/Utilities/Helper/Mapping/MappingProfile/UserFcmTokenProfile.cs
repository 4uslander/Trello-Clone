using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.UserFcmToken;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class UserFcmTokenProfile : Profile
    {
        public UserFcmTokenProfile()
        {
            CreateMap<UserFcmToken, UserFcmTokenDetail>().ReverseMap();
            CreateMap<UserFcmTokenDTO, UserFcmToken>().ReverseMap();
        }
    }
}
