using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;
using Trello.Application.DTOs.Notification;
using Trello.Application.Services.NotificationServices;
using Trello.Application.Utilities.Helper.FirebaseNoti;
using Trello.Infrastructure.IRepositories;
using Microsoft.Extensions.Hosting;
using Trello.Application.Services.TaskServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.CardMemberServices;

namespace Trello.Application.BackgroundServices
{
    public class ReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReminderService> _logger;
        //private readonly TimeSpan _interval = TimeSpan.FromHours(1);
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

        public ReminderService(IServiceProvider serviceProvider, ILogger<ReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckRemindersAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while checking reminders.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task CheckRemindersAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
                var cardService = scope.ServiceProvider.GetRequiredService<ICardService>();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var firebaseNotificationService = scope.ServiceProvider.GetRequiredService<IFirebaseNotificationService>();
                var cardMemberService = scope.ServiceProvider.GetRequiredService<ICardMemberService>();

                // Check for task reminders
                var upcomingTasks = await taskService.GetTasksForReminderAsync(DateTime.UtcNow);
                foreach (var task in upcomingTasks)
                {
                    if (task.AssignedUserId.HasValue)
                    {
                        var notificationRequest = new NotificationDTO
                        {
                            UserId = task.AssignedUserId.Value,
                            Title = "Task Reminder",
                            Body = $"Reminder: The task '{task.Name}' is due tomorrow."
                        };

                        // Check if a similar notification already exists
                        var existingNotification = await notificationService.GetExistingNotificationAsync(notificationRequest);
                        if (existingNotification == null)
                        {
                            var notificationDetail = await notificationService.CreateNotificationAsync(notificationRequest);
                            await firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);
                        }
                    }
                }

                // Check for card reminders
                var upcomingCards = await cardService.GetCardsForReminderAsync(DateTime.UtcNow);
                foreach (var card in upcomingCards)
                {
                    // Get all members of the card
                    var cardMembers = await cardMemberService.GetAllCardMemberAsync(card.Id);
                    foreach (var member in cardMembers)
                    {
                        var notificationRequest = new NotificationDTO
                        {
                            UserId = member.UserId,
                            Title = "Card Reminder",
                            Body = $"Reminder: The card '{card.Title}' has a reminder set for '{card.ReminderDate}'."
                        };

                        // Check if a similar notification already exists
                        var existingNotification = await notificationService.GetExistingNotificationAsync(notificationRequest);
                        if (existingNotification == null)
                        {
                            var notificationDetail = await notificationService.CreateNotificationAsync(notificationRequest);
                            await firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);
                        }
                    }
                }
            }
        }
    }
}
