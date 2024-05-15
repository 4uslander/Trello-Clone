using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Card
    {
        public Card()
        {
            CardActivities = new HashSet<CardActivity>();
            Comments = new HashSet<Comment>();
            ToDos = new HashSet<ToDo>();
        }

        public int Id { get; set; }
        public int ListId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public int IsActive { get; set; }

        public virtual List List { get; set; } = null!;
        public virtual ICollection<CardActivity> CardActivities { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
