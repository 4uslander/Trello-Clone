using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Trello.Application.DTOs.User;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Application.Utilities.Helper.JWT;
using Trello.Application.Utilities.Helper.PasswordEncryption;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IJwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDetail> CreateUserAsync(CreateUserDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Check if the email already exists
            var existingEmail = await GetUserByEmailAsync(requestBody.Email);
            if (existingEmail != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.EMAIL_FIELD, ErrorMessage.EMAIL_ALREADY_EXIST);
            }

            // Hash the password with a salt
            var hashedPasswordWithSalt = PasswordHelper.HashPasswordWithSalt(requestBody.Password);

            // Map requestBody to a User entity
            var user = _mapper.Map<User>(requestBody);
            user.Id = Guid.NewGuid();
            user.IsActive = true;
            user.Password = hashedPasswordWithSalt;
            user.CreatedDate = DateTime.UtcNow;
            user.CreatedUser = user.Id;

            // Insert the new user into the repository
            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Map the created user to a UserDetail DTO and return it
            var newUser = _mapper.Map<UserDetail>(user);
            return newUser;
        }
        public async Task<string> LoginAsync(UserLoginDTO requestBody)
        {
            // Check if the Email is match
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == requestBody.Email);
            if (user == null)
            {
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.LOGIN_FIELD, ErrorMessage.INVALID_EMAIL);
            }

            // Check if the Password is match
            if (!PasswordHelper.VerifyPassword(requestBody.Password, user.Password))
            {
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.LOGIN_FIELD, ErrorMessage.INVALID_PASSWORD);
            }

            // Check if the user is active
            if (user.IsActive != true)
                throw new ExceptionResponse(HttpStatusCode.Forbidden, ErrorField.LOGIN_FIELD, ErrorMessage.INACTIVE_USER);

            // claim data
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            //generate JWT token
            var token = _jwtHelper.generateJwtToken(claims);
            return token;
        }

        public async Task<List<UserDetail>> GetAllUserAsync()
        {
            // Get all active user
            IQueryable<User> usersQuery = _unitOfWork.UserRepository.GetAll();
            usersQuery = usersQuery.Where(u => u.IsActive);

            // Map the User to UserDetail and return them
            List<UserDetail> users = await usersQuery
                .Select(u => _mapper.Map<UserDetail>(u))
                .ToListAsync();
            return users;
        }

        public async Task<List<UserDetail>> GetUserByFilterAsync(string? email, string? name, bool? isActive)
        {
            // Get all user
            IQueryable<User> usersQuery = _unitOfWork.UserRepository.GetAll();


            // Filter user by email if provided
            if (!string.IsNullOrEmpty(email))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(email));
            }

            // Filter user by name if provided
            if (!string.IsNullOrEmpty(name))
            {
                usersQuery = usersQuery.Where(u => u.Name.Contains(name));
            }

            // Filter user by status if provided
            if (isActive.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == isActive.Value);
            }

            // Map the User to UserDetail and return them
            List<UserDetail> users = await usersQuery
                .Select(u => _mapper.Map<UserDetail>(u))
                .ToListAsync();
            return users;
        }
        public async Task<object> GetUserProfileAsync(string jwtToken)
        {
            //Get user id from JWT token
            var userId = _jwtHelper.GetUserIdFromToken(jwtToken);

            // Check if the user is exist
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            // Map the User to UserDetail and return them
            var userDetail = _mapper.Map<UserDetail>(user);
            return userDetail;
        }

        public async Task<object> GetUserAsync(Guid userId)
        {
            // Check if the user is exist
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            // Map the User to UserDetail and return them
            object userDetail = null!;
            userDetail = _mapper.Map<UserDetail>(user);
            return userDetail;
        }

        public async Task<UserDetail> UpdateUserAsync(Guid id, UpdateUserDTO requestBody)
        {
            // Check if the user is exist
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (currentUserId != id)
            {
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);
            }

            // Update the user properties
            user.UpdatedUser = currentUserId;
            user.UpdatedDate = DateTime.UtcNow;
            user = _mapper.Map(requestBody, user);

            // Update the user in the repository and save changes
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // Map the User to UserDetail and return them
            var userDetail = _mapper.Map<UserDetail>(user);
            return userDetail;
        }

        public async Task<UserDetail> ChangeStatusAsync(Guid userId, bool isActive)
        {
            // Check if the user is exist
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (currentUserId != userId)
            {
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);
            }

            // Update the user properties
            user.UpdatedUser = currentUserId;
            user.UpdatedDate = DateTime.UtcNow;
            user.IsActive = isActive;

            // Update the user in the repository and save changes
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // Map the User to UserDetail and return them
            var mappedUser = _mapper.Map<UserDetail>(user);
            return mappedUser;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email.Equals(email));
        }
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(userId);
        }

        public async Task<List<UserDetail>> GetUsersByToDoIdAsync(Guid toDoId)
        {

            var usersQuery = from cardMember in _unitOfWork.CardMemberRepository.GetAll()
                             join card in _unitOfWork.CardRepository.GetAll() on cardMember.CardId equals card.Id
                             join toDo in _unitOfWork.ToDoRepository.GetAll() on card.Id equals toDo.CardId
                             where toDo.Id == toDoId && cardMember.IsActive 
                             select cardMember.User;

            List<UserDetail> users = await usersQuery
                .Select(u => _mapper.Map<UserDetail>(u))
                .ToListAsync();

            if (!users.Any())
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_FOUND);
            }

            return users;
        }

        public async Task<Guid> GetUserIdByBoardMemberIdAsync(Guid boardMemberId) //
        {
            var userQuery = from boardMember in _unitOfWork.BoardMemberRepository.GetAll()
                            where boardMember.Id == boardMemberId && boardMember.IsActive
                            select boardMember.UserId;

            Guid userId = await userQuery.FirstOrDefaultAsync();

            return userId;
        }

        public async Task<Guid> GetUserIdByCardMemberIdAsync(Guid cardMemberId) //
        {
            var userQuery = from cardMember in _unitOfWork.CardMemberRepository.GetAll()
                            where cardMember.Id == cardMemberId && cardMember.IsActive
                            select cardMember.UserId;

            Guid userId = await userQuery.FirstOrDefaultAsync();

            return userId;
        }
    }
}
