using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.ToDo;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            CreateMap<ToDo, ToDoDetail>().ReverseMap();
            CreateMap<ToDoDTO, ToDo>().ReverseMap();
            CreateMap<CreateToDoDTO, ToDo>().ReverseMap();
        }
    }
}
