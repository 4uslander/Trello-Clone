using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.User;

namespace Trello.Application.Services.UserServices
{
    public interface IUserService
    {
        public Task<GetUserDetail> CreateUserAsync(CreateUserDTO requestBody);
        Task<string> LoginAsync(LoginDTO loginRequest);
        public List<GetUserDetail> GetAllUser(SearchUserDTO searchKey);
        public Task<object> GetUserLoginAsync(int userId);
        public Task<GetUserDetail> UpdateUserAsync(int id, UpdateUserDTO requestBody);
        public Task<GetUserDetail> ChangeStatusAsync(int Id);
        public Task IsExistEmail(string? Email);
        public Task<string> HandleGoogleLoginAsync(ClaimsPrincipal principal);
        Task SignOutAsync();
    }
}
