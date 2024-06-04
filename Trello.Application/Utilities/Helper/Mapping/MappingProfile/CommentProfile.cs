using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.Comment;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDetail>().ReverseMap();
            CreateMap<CommentDTO, Comment>().ReverseMap();
        }
    }
}
