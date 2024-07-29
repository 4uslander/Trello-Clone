using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardActivity;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class CardActivityProfile : Profile
    {
        public CardActivityProfile() {
            CreateMap<CardActivity, CardActivityDetail>().ReverseMap();
            CreateMap<CardActivityDTO, CardActivity>().ReverseMap();
            CreateMap<CreateCardActivityDTO, CardActivity>().ReverseMap();
        }
    }
}
