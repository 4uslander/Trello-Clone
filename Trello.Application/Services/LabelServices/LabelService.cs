using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Label;

namespace Trello.Application.Services.LabelServices
{
    public class LabelService : ILabelService
    {



        public Task<LabelDetail> ChangeStatusAsync(Guid Id, bool IsActive)
        {
            throw new NotImplementedException();
        }

        public Task<LabelDetail> CreateLabelAsync(CreateLabelDTO requestBody)
        {
            throw new NotImplementedException();
        }

        public Task<LabelDetail> GetAllLabelAsync(Guid BoardId)
        {
            throw new NotImplementedException();
        }

        public Task<LabelDetail> GetLabelByFilterAsync(Guid BoardId, string? Name, string? Color, bool? isActive)
        {
            throw new NotImplementedException();
        }

        public Task<LabelDetail> UpdateLabelAsync(Guid Id, UpdateLabelDTO requestBody)
        {
            throw new NotImplementedException();
        }
    }
}
