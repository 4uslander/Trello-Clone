using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class UserFcmToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FcmToken { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
