using AutoMapper;
using Trello.Application.DTOs.Role;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDetail>().ReverseMap();
            CreateMap<RoleDTO, Role>().ReverseMap();
        }
    }
}
