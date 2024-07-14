using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Label;
using Trello.Domain.Models;

namespace Trello.Application.Utilities.Helper.Mapping.MappingProfile
{
    public class LabelProfile: Profile
    {
        public LabelProfile() {

            CreateMap<Label ,LabelProfile>().ReverseMap();
            CreateMap<LabelDTO, Label>().ReverseMap();
            CreateMap<UpdateLabelDTO, Label>().ReverseMap();

        }
    }
}
