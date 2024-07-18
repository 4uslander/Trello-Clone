using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardLabel;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.LabelServices;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;

namespace Trello.Application.Services.CardLabelServices
{
    public class CardLabelService : ICardLabelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardService _cardService;
        private readonly ILabelService _labelService;
        private readonly IBoardService _boardService;

        public CardLabelService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, 
            ICardService cardService, ILabelService labelService, IBoardService boardService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _labelService = labelService;
            _boardService = boardService;
        }

        public Task<CardLabelDetail> ChangeStatusCardLabelAsync(Guid Id, bool? isActive)
        {
            throw new NotImplementedException();
        }

        public Task<CardLabelDetail> CreateCardLabelAsync(CardLabelDTO requestBody)
        {
            throw new NotImplementedException();
        }

        public Task<List<CardLabelDetail>> GetAllCardLabelAsync(Guid cardId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CardLabelDetail>> GetCardLabelByFilterAsync(Guid cardId, string? labelName, bool? isActive)
        {
            throw new NotImplementedException();
        }

        public Task<CardLabel> GetCardLabelByLabelIdAsync(Guid Id, Guid labelId)
        {
            throw new NotImplementedException();
        }
    }
}
