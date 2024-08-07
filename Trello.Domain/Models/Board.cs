﻿using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Board
    {
        public Board()
        {
            BoardMembers = new HashSet<BoardMember>();
            Labels = new HashSet<Label>();
            Lists = new HashSet<List>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedUser { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<BoardMember> BoardMembers { get; set; }
        public virtual ICollection<Label> Labels { get; set; }
        public virtual ICollection<List> Lists { get; set; }
    }
}
