using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Task;
using Trello.Application.DTOs.ToDo;
using Trello.Domain.Models;
using Task = Trello.Domain.Models.Task;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Task, TaskDetail>().ReverseMap();
            CreateMap<TaskDTO, Task>().ReverseMap();
        }
    }
}
