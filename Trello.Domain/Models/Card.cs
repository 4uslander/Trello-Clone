using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Card
    {
        public Card()
        {
            CardActivities = new HashSet<CardActivity>();
            CardLabels = new HashSet<CardLabel>();
            Comments = new HashSet<Comment>();
            ToDos = new HashSet<ToDo>();
        }

        public Guid Id { get; set; }
        public Guid ListId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsActive { get; set; }

        public virtual List List { get; set; } = null!;
        public virtual ICollection<CardActivity> CardActivities { get; set; }
        public virtual ICollection<CardLabel> CardLabels { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
