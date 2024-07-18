using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardLabel;
using Trello.Domain.Models;

namespace Trello.Application.Services.CardLabelServices
{
    public interface ICardLabelService
    {
        public Task<CardLabelDetail> CreateCardLabelAsync(CardLabelDTO requestBody);
        public Task<List<CardLabelDetail>> GetAllCardLabelAsync(Guid cardId);
        public Task<List<CardLabelDetail>> GetCardLabelByFilterAsync(Guid cardId, string? labelName, bool? isActive);
        public Task<CardLabelDetail> ChangeStatusCardLabelAsync(Guid Id, bool? isActive);
        public Task<CardLabel> GetCardLabelByLabelIdAsync(Guid Id, Guid labelId);
    }
}
