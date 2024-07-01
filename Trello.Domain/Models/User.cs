using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class User
    {
        public User()
        {
            BoardMembers = new HashSet<BoardMember>();
            CardActivities = new HashSet<CardActivity>();
            CardMembers = new HashSet<CardMember>();
            Comments = new HashSet<Comment>();
            Tasks = new HashSet<Task>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<BoardMember> BoardMembers { get; set; }
        public virtual ICollection<CardActivity> CardActivities { get; set; }
        public virtual ICollection<CardMember> CardMembers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
