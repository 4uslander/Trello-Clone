using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.List;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardDetail>()
                .ForMember(dest => dest.IsActiveString, opt => opt.MapFrom(src => src.IsActive? "Active" : "Inactive"));

            CreateMap<CreateCardDTO, Card>().ReverseMap();
            //CreateMap<UpdateListDTO, List>().ReverseMap();
        }
    }
}
