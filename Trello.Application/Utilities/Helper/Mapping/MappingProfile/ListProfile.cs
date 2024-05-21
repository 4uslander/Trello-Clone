using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.List;
using Trello.Application.DTOs.User;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class ListProfile : Profile
    {
        public ListProfile() 
        {
            CreateMap<List, GetListDetail>()
                .ForMember(dest => dest.IsActiveString, opt => opt.MapFrom(src => src.IsActive == 1 ? "Active" : "Inactive"));

            CreateMap<CreateListDTO, List>().ReverseMap();
            CreateMap<UpdateListDTO, List>().ReverseMap();
        }
    }
}
