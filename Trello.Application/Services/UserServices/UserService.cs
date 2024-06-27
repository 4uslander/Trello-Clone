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

            //check exist email
            var existingEmail = await GetUserByEmailAsync(requestBody.Email);
            if (existingEmail != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.EMAIL_FIELD, ErrorMessage.EMAIL_ALREADY_EXIST);
            }

            // Hash the password with the salt
            var hashedPasswordWithSalt = PasswordHelper.HashPasswordWithSalt(requestBody.Password);

            var user = _mapper.Map<User>(requestBody);
            user.Id = Guid.NewGuid();
            user.IsActive = true;
            user.Password = hashedPasswordWithSalt;
            user.CreatedDate = DateTime.UtcNow;
            user.CreatedUser = user.Id;
            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var newUser = _mapper.Map<UserDetail>(user);
            return newUser;
        }
        public async Task<string> LoginAsync(UserLoginDTO requestBody)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == requestBody.Email);
            if (user == null)
            {
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.LOGIN_FIELD, ErrorMessage.INVALID_EMAIL);
            }

            if (!PasswordHelper.VerifyPassword(requestBody.Password, user.Password))
            {
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.LOGIN_FIELD, ErrorMessage.INVALID_PASSWORD);
            }

            if (user.IsActive != true)
                throw new ExceptionResponse(HttpStatusCode.Forbidden, ErrorField.LOGIN_FIELD, ErrorMessage.INACTIVE_USER);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = _jwtHelper.generateJwtToken(claims);
            return token;
        }

        public async Task<List<UserDetail>> GetAllUserAsync()
        {
            IQueryable<User> usersQuery = _unitOfWork.UserRepository.GetAll();

            List<UserDetail> users = await usersQuery
                .Select(u => _mapper.Map<UserDetail>(u))
                .ToListAsync();
            return users;
        }

        public async Task<List<UserDetail>> GetUserByFilterAsync(string? email, string? name, bool? isActive)
        {

            IQueryable<User> usersQuery = _unitOfWork.UserRepository.GetAll();

            if (!string.IsNullOrEmpty(email))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(name))
            {
                usersQuery = usersQuery.Where(u => u.Name.Contains(name));
            }

            if (isActive.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == isActive.Value);
            }

            List<UserDetail> users = await usersQuery
                .Select(u => _mapper.Map<UserDetail>(u))
                .ToListAsync();
            return users;
        }

        public async Task<object> GetUserProfileAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            object userDetail = null!;
            userDetail = _mapper.Map<UserDetail>(user);

            return userDetail;
        }
        public async Task<UserDetail> UpdateUserAsync(Guid id, UpdateUserDTO requestBody)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (currentUserId != id)
            {
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);
            }

            user.UpdatedUser = currentUserId;
            user.UpdatedDate = DateTime.UtcNow;
            user = _mapper.Map(requestBody, user);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var userDetail = _mapper.Map<UserDetail>(user);
            return userDetail;
        }

        public async Task<UserDetail> ChangeStatusAsync(Guid userId, bool isActive)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (currentUserId != userId)
            {
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);
            }

            user.UpdatedUser = currentUserId;
            user.UpdatedDate = DateTime.UtcNow;
            user.IsActive = isActive;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

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
    }
}
