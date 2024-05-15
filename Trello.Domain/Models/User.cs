using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class User
    {
        public User()
        {
            CardActivities = new HashSet<CardActivity>();
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Gender { get; set; }
        public int IsActive { get; set; }

        public virtual ICollection<CardActivity> CardActivities { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
