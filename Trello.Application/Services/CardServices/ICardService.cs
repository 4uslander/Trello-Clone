using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.List;

namespace Trello.Application.Services.CardServices
{
    public interface ICardService
    {
        public Task<CardDetail> CreateCardAsync(CreateCardDTO requestBody);
        public List<CardDetail> GetAllList(string? title);
        public Task<CardDetail> UpdateCardAsync(Guid id, UpdateCardDTO requestBody);
        public Task<CardDetail> ChangeStatusAsync(Guid Id);
        public Task IsExistCardTitle(string? title);
        public Task IsExistListId(Guid? id);
    }
}
