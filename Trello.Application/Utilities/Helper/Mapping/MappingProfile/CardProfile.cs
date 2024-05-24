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
            CreateMap<Card, CardDetail>().ReverseMap();
            CreateMap<CardDTO, Card>().ReverseMap();
            CreateMap<UpdateCardDTO, Card>().ReverseMap();
        }
    }
}
