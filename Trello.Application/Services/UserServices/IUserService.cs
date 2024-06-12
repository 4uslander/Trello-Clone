using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.User;
using Trello.Domain.Models;

namespace Trello.Application.Services.UserServices
{
    public interface IUserService
    {
        public Task<UserDetail> CreateUserAsync(CreateUserDTO requestBody);
        public Task<string> LoginAsync(UserLoginDTO requestBody);
        public Task<List<UserDetail>> GetAllUserAsync(string? email, string? name, string? gender);
        public Task<object> GetUserProfileAsync(Guid userId);
        public Task<UserDetail> UpdateUserAsync(Guid id, UpdateUserDTO requestBody);
        public Task<UserDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> GetUserByIdAsync(Guid userId);
    }
}
    