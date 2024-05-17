using AutoMapper;
using Trello.Application.DTOs.Board;
using Trello.Domain.Models;
using Trello.Domain.Enums;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Board, GetBoardDetail>()
                .ForMember(dest => dest.IsActiveString, opt => opt.MapFrom(src => Enum.GetName(typeof(BoardStatus), src.IsActive)))
                .ForMember(dest => dest.IsPublicString, opt => opt.MapFrom(src => Enum.GetName(typeof(BoardPublicStatus), src.IsPublic)));

            CreateMap<CreateBoardDTO, Board>().ReverseMap();
        } 
    }
}
