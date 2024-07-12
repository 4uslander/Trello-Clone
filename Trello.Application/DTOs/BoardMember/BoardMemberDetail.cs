using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.BoardMember
{
    public class BoardMemberDetail
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public Guid BoardId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public bool IsActive { get; set; }
    }
}
