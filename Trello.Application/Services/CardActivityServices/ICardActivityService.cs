using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardActivity;

namespace Trello.Application.Services.CardActivityServices
{
    public interface ICardActivityService
    {
        public Task<List<CardActivityDetail>> GetCardActivityDetailAsync(Guid cardId);
        public Task<CardActivityDetail> CreateCardActivityAsync(CreateCardActivityDTO requestBody);
    }
}
