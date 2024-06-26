using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.User;
using Trello.Domain.Models;

namespace Trello.Application.Services.UserServices
{
    public interface IUserService
    {
        public Task<UserDetail> CreateUserAsync(CreateUserDTO requestBody);
        public Task<string> LoginAsync(UserLoginDTO requestBody);
        public Task<List<UserDetail>> GetAllUserAsync();
        public Task<List<UserDetail>> GetUserByFilterAsync(string? email, string? name, string? gender, bool? isActive);
        public Task<object> GetUserProfileAsync(Guid userId);
        public Task<UserDetail> UpdateUserAsync(Guid id, UpdateUserDTO requestBody);
        public Task<UserDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> GetUserByIdAsync(Guid userId);
    }
}
