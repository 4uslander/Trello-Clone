﻿using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class ToDo
    {
        public ToDo()
        {
            Tasks = new HashSet<Task>();
        }

        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public bool IsActive { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
