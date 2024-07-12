using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Label;

namespace Trello.Application.Services.LabelServices
{
    public interface ILabelService
    {
        public Task<LabelDetail> CreateLabelAsync(CreateLabelDTO requestBody);
        public Task<LabelDetail> GetAllLabelAsync(Guid BoardId);
        public Task<LabelDetail> GetLabelByFilterAsync(Guid BoardId, string? Name, string? Color, bool? isActive);
        public Task<LabelDetail> UpdateLabelAsync(Guid Id, UpdateLabelDTO requestBody);
        public Task<LabelDetail> ChangeStatusAsync(Guid Id, bool IsActive);

    }
}
