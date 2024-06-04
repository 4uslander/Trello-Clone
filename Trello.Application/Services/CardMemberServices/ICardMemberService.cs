using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.CardMember;
using Trello.Domain.Models;

namespace Trello.Application.Services.CardMemberServices
{
    public interface ICardMemberService
    {
        public Task<CardMemberDetail> CreateCardMemberAsync(CardMemberDTO requestBody);
        public Task<List<CardMemberDetail>> GetAllCardMemberAsync(Guid cardId, string? userName);
        public Task<CardMemberDetail> ChangeStatusAsync(Guid Id, bool isActive);
    }
}
