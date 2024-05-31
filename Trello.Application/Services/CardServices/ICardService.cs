using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.List;
using Trello.Domain.Models;

namespace Trello.Application.Services.CardServices
{
    public interface ICardService
    {
        public Task<CardDetail> CreateCardAsync(CreateCardDTO requestBody);
        public Task<List<CardDetail>> GetAllCardAsync(Guid listId, string? title);
        public Task<CardDetail> UpdateCardAsync(Guid id, UpdateCardDTO requestBody);
        public Task<CardDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<List> GetListById(Guid id);
    }
}
