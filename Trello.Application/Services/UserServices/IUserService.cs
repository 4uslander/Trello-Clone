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
        public Task<GetUserDetail> CreateUserAsync(CreateUserDTO requestBody);
        Task<string> LoginAsync(LoginDTO loginRequest);
        public Task IsExistEmail(string? Email);
    }
}
