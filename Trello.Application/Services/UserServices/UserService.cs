using AutoMapper;
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

            // Hash the password with the salt
            var hashedPasswordWithSalt = PasswordHelper.HashPasswordWithSalt(requestBody.Password);


            var user = _mapper.Map<User>(requestBody);
            user.IsActive = (int)UserStatus.Active;
            user.Password = hashedPasswordWithSalt;

            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var newUser = _mapper.Map<GetUserDetail>(user);
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

            if (user.IsActive != (int)UserStatus.Active)
                throw new ExceptionResponse(HttpStatusCode.Forbidden, ErrorField.LOGIN_FIELD, ErrorMessage.INACTIVE_USER);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
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
        public async Task<GetUserDetail> UpdateUserAsync(int id, UpdateUserDTO requestBody)
        {
            if (id != requestBody.UserId)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_ID_FIELD, ErrorMessage.USER_NOT_EXIST);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);


            user = _mapper.Map(requestBody, user);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var userDetail = _mapper.Map<GetUserDetail>(user);
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
