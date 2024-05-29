using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.CardMember;

namespace Trello.Application.Services.CardMemberServices
{
    public interface ICardMemberService
    {
        public Task<CardMemberDetail> CreateCardMemberAsync(CardMemberDTO requestBody);
        public Task IsExistCard(Guid cardId);
        public Task IsExistUser(Guid userId);
    }
}
