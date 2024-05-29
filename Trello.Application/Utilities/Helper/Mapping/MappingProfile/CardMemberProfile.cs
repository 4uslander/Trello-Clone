using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.CardMember;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class CardMemberProfile : Profile
    {
        public CardMemberProfile()
        {
            CreateMap<CardMember, CardMemberDetail>().ReverseMap();
            CreateMap<CardMemberDTO, CardMember>().ReverseMap();
        }
    }
}
