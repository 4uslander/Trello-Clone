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
        public Task<List<CardMemberDetail>> GetAllCardMemberAsync(Guid cardId);
        public Task<List<CardMemberDetail>> GetCardMemberByFilterAsync(Guid cardId, string? userName, bool? isActive);
        public Task<CardMemberDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<CardMember> GetCardMemberByUserIdAsync(Guid cardId, Guid userId);
    }
}
