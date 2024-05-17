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

        }
        public static class ErrorMessage
        {
            public const string EMAIL_ALREADY_EXIST = "This email already exists!";
            public const string BOARD_ALREADY_EXIST = "This board name already exists!";
            public const string NULL_REQUEST_BODY = "Request body is null!";
            public const string INVALID_CREDENTIALS = "Invalid Credentials";
            public const string USER_NOT_EXIST = "This user does not exist!";
            public const string INVALID_EMAIL_PASSWORD = "Invalid Email or Password";
            public const string INACTIVE_USER = "This user is inactive";
        }
    }
}
