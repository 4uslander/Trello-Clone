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
        }
        public static class ErrorMessage
        {
            public const string EMAIL_ALREADY_EXIST = "This email already exists!";
            public const string NULL_REQUEST_BODY = "Request body is null!";

        }
    }
}
