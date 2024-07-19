//using FirebaseAdmin.Messaging;
//using FirebaseAdmin;
//using Google.Apis.Auth.OAuth2;
//using Trello.Application.Services.UserServices;


//namespace Trello.Application.Utilities.Helper.FirebaseNoti
//{
//    public class FirebaseNotificationService : IFirebaseNotificationService
//    {
//        private readonly IUserService _userService;

//        public FirebaseNotificationService(IUserService userService)
//        {
//            _userService = userService;
//        }

//        public FirebaseNotificationService()
//        {
//            if (FirebaseApp.DefaultInstance == null)
//            {
//                var basePath = AppDomain.CurrentDomain.BaseDirectory;
//                var credentialPath = Path.Combine(basePath, "Utilities", "FirebaseServiceKey", "clonetrello-103ad-firebase-adminsdk-plg5l-627e51f254.json");

//                FirebaseApp.Create(new AppOptions()
//                {
//                    Credential = GoogleCredential.FromFile(credentialPath),
//                });
//            }
//        }

//        public async Task SendNotificationAsync(Guid userId, string title, string message)
//        {
//            var user = await _userService.GetUserByIdAsync(userId);
//            if (user == null)
//            {
//                throw new Exception("User not found or notification token is missing");
//            }

//            await SendFirebaseNotification(user.Id.ToString(), title, message);
//        }

//        private async Task SendFirebaseNotification(string notificationToken, string title, string body)
//        {
//            var message = new Message()
//            {
//                Token = notificationToken,
//                Notification = new Notification
//                {
//                    Title = title,
//                    Body = body,
//                },
//            };

//            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
//            Console.WriteLine("Successfully sent message: " + response);
//        }
//    }
//}
