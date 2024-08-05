using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.Utilities.GlobalVariables
{
    public class GlobalVariable
    {
        public static class ErrorField
        {
            public const string EMAIL_FIELD = "Email";
            public const string REQUEST_BODY = "Request body";
            public const string STATUS_FIELD = "Status";
            public const string USER_FIELD = "User";
            public const string BOARD_FIELD = "Board";
            public const string LOGIN_FIELD = "Login";
            public const string USER_ID_FIELD = "User Id";
            public const string BOARD_ID_FIELD = "Board Id";
            public const string AUTHENTICATION_FIELD = "Authentication";
            public const string LIST_FIELD = "List";
            public const string CARD_FIELD = "Card";
            public const string DATE_FIELD = "Date";
            public const string TITLE_FIELD = "Title";
            public const string ROLE_FIELD = "Role";
            public const string BOARD_MEMBER_FIELD = "Board Member";
            public const string CARD_MEMBER_FIELD = "Card Member";
            public const string COMMENT_FIELD = "Comment";
            public const string TODO_FIELD = "ToDo";
            public const string TASK_FIELD = "Task";
            public const string USER_FCM_TOKEN_FIELD = "User FCM Token";
            public const string NOTIFICATION_FIELD = "Notification";
            public const string LABEL_FIELD = "Label";
            public const string CARD_LABEL_FIELD = "Card Label";
            public const string CARD_ACTIVITY_FIELD = "Card Activity";
        }
        public static class ErrorMessage
        {
            public const string EMAIL_ALREADY_EXIST = "This email already exists!";
            public const string BOARD_ALREADY_EXIST = "This board name already exists!";
            public const string NULL_REQUEST_BODY = "Request body is null!";
            public const string INVALID_CREDENTIALS = "Invalid Credentials";
            public const string USER_NOT_EXIST = "This user does not exist!";
            public const string BOARD_NOT_EXIST = "This board does not exist!";
            public const string INVALID_EMAIL = "Invalid Email";
            public const string INVALID_PASSWORD = "Invalid Password";
            public const string INACTIVE_USER = "This user is inactive";
            public const string UNAUTHORIZED = "Unauthorized";
            public const string LIST_ALREADY_EXIST = "This list name already exists!";
            public const string LIST_NOT_EXIST = "This list does not exist!";
            public const string LISTS_IN_DIFFERENT_BOARD = "Lists belong to different boards";
            public const string INVALID_LIST_POSITION = "List position must be greater than or equal to 1.";
            public const string LIST_POSITION_ALREADY_EXIST = "List position already exists within the same board.";
            public const string LIST_POSITION_SPECIFIED = "Invalid position specified.";
            public const string CARD_ALREADY_EXIST = "This card title already exists!";
            public const string CARD_NOT_EXIST = "This card does not exist!";
            public const string INVALID_END_DATE = "EndDate must be later than StartDate.";
            public const string INVALID_REMINDER_DATE = "ReminderDate must be equal or sooner than EndDate.";
            public const string INVALID_START_DATE = "StartDate must be equal or sooner than ReminderDate.";
            public const string INVALID_DUDE_DATE = "Due date date must be between start date and end date.";
            public const string TITLE_ALREADY_EXIST = "This card title already exists!";
            public const string ROLE_NOT_EXIST = "This role does not exist!";
            public const string ROLE_ALREADY_EXIST = "This role name already exists!";
            public const string BOARD_MEMBER_NOT_EXIST = "This board member does not exist!";
            public const string CARD_MEMBER_NOT_EXIST = "This card member does not exist!";
            public const string COMMENT_NOT_EXIST = "This comment does not exist!";
            public const string TODO_NOT_EXIST = "This ToDo list does not exist!";
            public const string TASK_NOT_EXIST = "This task does not exist!";
            public const string USER_NOT_FOUND = "No users found for the provided ToDo ID.";
            public const string USER_FCM_TOKEN_NOT_EXIST = "This user FCM token does not exist!";
            public const string USER_FCM_TOKEN_NOT_FOUND = "This user FCM token does not found!";
            public const string NOTIFICATION_NOT_FOUND = "This notification does not found!";

            public const string LABEL_NOT_EXIST = "This label does not exist!";
            public const string CARD_LABEL_NOT_EXIST = "This card label does not exist!";
            public const string CARD_ACTIVITY_NOT_EXIST = "This card activity does not exist!";
        }

        public static class NotificationTitleField
        {
            public const string INVITE_TO_BOARD = "Invited to a new board";
            public const string MEMBER_ROLE_UPDATED = "Role Updated";
            public const string MEMBER_REMOVED = "Removed from Board";
            public const string ADDED_TO_CARD = "Added to a new card";
            public const string CARD_MEMBER_REMOVED = "Removed from card";
            public const string ASSIGNED_TO_TASK = "Assigned to a new task";
            public const string TASK_UPDATED = "Task Updated";
            public const string TASK_CHECKED = "Task Checked";
            public const string TASK_REMOVED = "Task Removed";

        }

        public static class NotificationBodyField
        {
            public const string INVITE_TO_BOARD = "You have been invited to the board";
            public const string MEMBER_ROLE_UPDATED = "Your role has been updated by Admin.";
            public const string MEMBER_REMOVED = "You have been removed from the board by Admin.";
            public const string ADDED_TO_CARD = "You have been added to the card";
            public const string CARD_MEMBER_REMOVED = "You have been removed from the card.";
            public const string ASSIGNED_TO_TASK = "You have assigned to the task";
            public const string TASK_UPDATED = " has been updated!";
            public const string TASK_CHECKED = " has been checked!";
            public const string TASK_REMOVED = " has been removed!";

           
        }
    }
}
