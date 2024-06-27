using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Card;
using Trello.Domain.Models;

namespace Trello.Application.Services.CardServices
{
    public interface ICardService
    {
        public Task<CardDetail> CreateCardAsync(CreateCardDTO requestBody);
        public Task<List<CardDetail>> GetAllCardAsync(Guid listId);
        public Task<List<CardDetail>> GetCardByFilterAsync(Guid listId, string? title, bool? isActive);
        public Task<CardDetail> UpdateCardAsync(Guid id, UpdateCardDTO requestBody);
        public Task<CardDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<Card> GetCardByIdAsync(Guid cardId);
    }
}
