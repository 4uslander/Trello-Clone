using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.User;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.ConvertDate;
using Trello.Application.Utilities.Helper.JWT;
using Trello.Application.Utilities.Helper.Searching;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IJwtHelper jwtHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtHelper = jwtHelper;

        }

        public async Task<GetUserDetail> CreateUserAsync(CreateUserDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            await IsExistEmail(requestBody.Email);

            var user = _mapper.Map<User>(requestBody);
            user.IsActive = (int)UserStatus.Active;

            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var newUser = _mapper.Map<GetUserDetail>(user);
            return newUser;
        }
        public async Task<string> LoginAsync(LoginDTO loginRequest)
        {
            if (loginRequest == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == loginRequest.Email && x.Password == loginRequest.Password);

            if (user == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.REQUEST_BODY, "Invalid credentials");

            if (user.IsActive != (int)UserStatus.Active)
            {
                throw new ExceptionResponse(HttpStatusCode.Forbidden, ErrorField.REQUEST_BODY, "User is inactive");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = _jwtHelper.generateJwtToken(claims);
            return token;
        }
        public List<GetUserDetail> GetAllUser(SearchUserDTO searchKey)
        {
            
            IQueryable<User> usersQuery = _unitOfWork.UserRepository.GetAll();

            
            if (!string.IsNullOrEmpty(searchKey?.Name))
            {
                usersQuery = usersQuery.Where(u => u.Name.Contains(searchKey.Name));
            }

            List<GetUserDetail> users = usersQuery
                .Select(u => _mapper.Map<GetUserDetail>(u))
                .ToList();

            return users;
        }
        public async Task<object> GetUserLoginAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.Get(u => u.Id == userId).SingleOrDefaultAsync()
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            object userDetail = null!;

            userDetail = _mapper.Map<GetUserDetail>(user);

            return userDetail;
        }
        public async Task<GetUserDetail> ChangeStatusAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            if (user.IsActive == (int)UserStatus.Active)
            {
                user.IsActive = (int)UserStatus.InActive;
            }
            else
            {
                user.IsActive = (int)UserStatus.Active;
            }

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var mappedUser = _mapper.Map<GetUserDetail>(user);
            return mappedUser;
        }

        public async System.Threading.Tasks.Task IsExistEmail(string? Email)
        {
            var isExist = await _unitOfWork.UserRepository.AnyAsync(x => x.Email.Equals(Email));
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.EMAIL_FIELD, ErrorMessage.EMAIL_ALREADY_EXIST);
        }
    }
}
