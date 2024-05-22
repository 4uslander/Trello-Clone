using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.User;

namespace Trello.Application.Services.UserServices
{
    public interface IUserService
    {
        public Task<UserDetail> CreateUserAsync(CreateUserDTO requestBody);
        Task<string> LoginAsync(LoginDTO loginRequest);
        public List<UserDetail> GetAllUser(string? email, string? name, string? gender);
        public Task<object> GetUserLoginAsync(int userId);
        public Task<UserDetail> UpdateUserAsync(int id, UpdateUserDTO requestBody);
        public Task<UserDetail> ChangeStatusAsync(int Id);
        public Task IsExistEmail(string? Email);
    }
}
    