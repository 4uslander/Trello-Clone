using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.Utilities.Helper.GetUserAuthorization
{
    public static class UserAuthorizationHelper
    {
        public static Guid GetUserAuthorizationById(HttpContext httpContext)
        {
            var currentUserId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null || !Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
                throw new Exception("Unable to retrieve user authorization ID.");

            return currentUserIdGuid;
        }
    }
}
