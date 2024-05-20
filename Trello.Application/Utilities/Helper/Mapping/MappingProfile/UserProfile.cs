using AutoMapper;
using Trello.Application.DTOs.User;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUserDetail>()
                .ForMember(dest => dest.IsActiveString, opt => opt.MapFrom(src => src.IsActive == 1 ? "Active" : "Inactive"));

            CreateMap<CreateUserDTO, User>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap();
        }
    }
}
