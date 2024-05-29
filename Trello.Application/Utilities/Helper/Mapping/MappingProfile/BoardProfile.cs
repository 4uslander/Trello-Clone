using AutoMapper;
using Trello.Application.DTOs.Board;
using Trello.Domain.Models;
using Trello.Domain.Enums;
using Trello.Application.DTOs.User;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Board, BoardDetail>().ReverseMap();
            CreateMap<BoardDTO, Board>().ReverseMap();
        }
    }
}
