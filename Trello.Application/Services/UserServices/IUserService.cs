using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.User;

namespace Trello.Application.Services.UserServices
{
    public interface IUserService
    {
        public Task<UserDetail> CreateUserAsync(CreateUserDTO requestBody);
        Task<string> LoginAsync(UserLoginDTO loginRequest);
        Task<List<UserDetail>> GetAllUserAsync(string? email, string? name, string? gender);
        public Task<object> GetUserLoginAsync(Guid userId);
        public Task<UserDetail> UpdateUserAsync(Guid id, UpdateUserDTO requestBody);
        public Task<UserDetail> ChangeStatusAsync(Guid Id);
        public Task IsExistEmail(string? Email);
    }
}
    