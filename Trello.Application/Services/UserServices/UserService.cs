using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Trello.Application.DTOs.User;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.JWT;
using Trello.Application.Utilities.Helper.PasswordEncryption;
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

            await IsExistEmail(requestBody.Email);

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
        public async Task<string> LoginAsync(LoginDTO loginRequest)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

            if (user == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized,ErrorField.LOGIN_FIELD, ErrorMessage.INVALID_EMAIL_PASSWORD);

            if (user == null || !PasswordHelper.VerifyPassword(loginRequest.Password, user.Password))
            {
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.LOGIN_FIELD, ErrorMessage.INVALID_EMAIL_PASSWORD);
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
        public List<UserDetail> GetAllUser(string? email, string? name, string? gender)
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
            if (!string.IsNullOrEmpty(gender))
            {
                usersQuery = usersQuery.Where(u => u.Gender.Contains(gender));
            }

            List<UserDetail> users = usersQuery
                .Select(u => _mapper.Map<UserDetail>(u))
                .ToList();

            return users;
        }
        public async Task<object> GetUserLoginAsync(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.Get(u => u.Id == userId).SingleOrDefaultAsync()
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            object userDetail = null!;

            userDetail = _mapper.Map<UserDetail>(user);

            return userDetail;
        }
        public async Task<UserDetail> UpdateUserAsync(Guid id, UpdateUserDTO requestBody)
        {
            if (id != requestBody.UserId)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_ID_FIELD, ErrorMessage.USER_NOT_EXIST);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);

            user.UpdatedUser = Guid.Parse(currentUserId);
            user.UpdatedDate = DateTime.UtcNow;
            user = _mapper.Map(requestBody, user);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var userDetail = _mapper.Map<UserDetail>(user);
            return userDetail;
        }
        public async Task<UserDetail> ChangeStatusAsync(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            if (user.IsActive == true)
            {
                user.IsActive = false;
            }
            else
            {
                user.IsActive = true;
            }

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var mappedUser = _mapper.Map<UserDetail>(user);
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
