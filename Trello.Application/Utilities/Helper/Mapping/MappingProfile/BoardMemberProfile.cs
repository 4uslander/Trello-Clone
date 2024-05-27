using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.BoardMember;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class BoardMemberProfile : Profile
    {
        public BoardMemberProfile()
        {
            CreateMap<BoardMember, BoardMemberDetail>().ReverseMap();
            CreateMap<BoardMemberDTO, BoardMember>().ReverseMap();
            //CreateMap<CreateBoardMemberDTO, BoardMember>().ReverseMap();
        }
    }
}
