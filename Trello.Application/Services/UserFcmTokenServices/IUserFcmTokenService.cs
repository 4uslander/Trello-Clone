using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Task;
using Trello.Application.DTOs.UserFcmToken;

namespace Trello.Application.Services.UserFcmTokenServices
{
    public interface IUserFcmTokenService
    {
        public Task<UserFcmTokenDetail> CreateUserFcmTokenAsync(CreateUserFcmTokenDTO requestBody);
        public Task<List<UserFcmTokenDetail>> GetAllUserFcmTokenAsync(Guid userId);
        public Task<UserFcmTokenDetail> UpdateUserFcmTokenAsync(Guid id, UserFcmTokenDTO requestBody);
        public Task<UserFcmTokenDetail> ChangeStatusAsync(string fcmToken, Guid userId, bool isActive);
    }
}
