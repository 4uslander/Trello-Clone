using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.UserFcmToken
{
    public class UserFcmTokenDetail
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FcmToken { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
