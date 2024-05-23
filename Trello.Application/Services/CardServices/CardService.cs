﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.List;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.CardServices
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CardService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CardDetail> CreateCardAsync(CardDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            await IsExistCardTitle(requestBody.Title);
            await IsExistListId(requestBody.ListId);

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);

            var card = _mapper.Map<Card>(requestBody);
            card.Id = Guid.NewGuid();
            card.IsActive = true;
            card.CreatedDate = DateTime.UtcNow;
            card.CreatedUser = Guid.Parse(currentUserId);

            await _unitOfWork.CardRepository.InsertAsync(card);
            await _unitOfWork.SaveChangesAsync();

            var createdCardDto = _mapper.Map<CardDetail>(card);
            return createdCardDto;
        }

        public List<CardDetail> GetAllList(string? title)
        {
            IQueryable<Card> cardsQuery = _unitOfWork.CardRepository.GetAll();


            if (!string.IsNullOrEmpty(title))
            {
                cardsQuery = cardsQuery.Where(u => u.Title.Contains(title));
            }

            List<CardDetail> cards = cardsQuery
                .Select(u => _mapper.Map<CardDetail>(u))
                .ToList();

            return cards;
        }

        public async System.Threading.Tasks.Task IsExistCardTitle(string? title)
        {
            var isExist = await _unitOfWork.CardRepository.AnyAsync(x => x.Title.Equals(title));
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TITLE_FIELD, ErrorMessage.TITLE_ALREADY_EXIST);
        }

        public async System.Threading.Tasks.Task IsExistListId(Guid? id)
        {
            var listExists = await _unitOfWork.ListRepository.AnyAsync(x => x.Id.Equals(id));
            if (!listExists)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);
        }
    }
}
