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
        public Task IsExistCardTitle(string? title);
        public Task IsExistListId(int? id);
    }
}
