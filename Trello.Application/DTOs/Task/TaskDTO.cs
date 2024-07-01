using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.ToDo;
using Trello.Domain.Enums;

namespace Trello.Application.DTOs.Task
{
    public class TaskDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Priority Level is required")]
        public TaskPriorityLevelEnum PriorityLevel { get; set; }
        public Guid? AssignedUserId { get; set; }
        public string? Description { get; set; }
        public TaskStatusEnum Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
    public class CreateTaskDTO : TaskDTO
    {
        [Required(ErrorMessage = "To Do Id is required")]
        public Guid TodoId { get; set; }

    }
}
