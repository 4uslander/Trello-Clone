using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardLabel;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class CardLabelProfile : Profile
    {
        public CardLabelProfile() 
        {
            CreateMap<CardLabel, CardLabelDetail>().ReverseMap();
            CreateMap<CardLabelDTO, CardLabel>().ReverseMap();
          
        }
    }
}
