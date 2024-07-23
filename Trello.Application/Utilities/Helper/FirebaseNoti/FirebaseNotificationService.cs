using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Trello.Application.Services.UserServices;
using Trello.Application.Services.UserFcmTokenServices;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;
using System.Net;
using Trello.Application.Utilities.ErrorHandler;
using Newtonsoft.Json.Linq;


namespace Trello.Application.Utilities.Helper.FirebaseNoti
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly IUserService _userService;
        private readonly IUserFcmTokenService _userFcmTokenService;

        public FirebaseNotificationService(IUserService userService, IUserFcmTokenService userFcmTokenService)
        {
            _userService = userService;
            _userFcmTokenService = userFcmTokenService;
        }

        public async Task SendNotificationAsync(Guid userId, string title, string message)
        {
            var user = await _userService.GetUserByIdAsync(userId)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            var userFcmTokens = await _userFcmTokenService.GetAllUserFcmTokenAsync(userId);
            if (userFcmTokens == null || !userFcmTokens.Any())
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FCM_TOKEN_FIELD, ErrorMessage.USER_FCM_TOKEN_NOT_FOUND);
            }

            var sendTasks = userFcmTokens.Select(token => SendFirebaseNotification(token.FcmToken, title, message));
            await Task.WhenAll(sendTasks);
        }

        private async Task SendFirebaseNotification(string notificationToken, string title, string body)
        {
            var message = new Message()
            {
                Token = notificationToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                },
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine("Successfully sent message: " + response);
        }
    }
}
