using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.Task
{
    public class TaskDetail
    {
        public Guid Id { get; set; }
        public Guid TodoId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string PriorityLevel { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = null!;
        public bool IsChecked { get; set; }
        public bool IsActive { get; set; }
    }
}
