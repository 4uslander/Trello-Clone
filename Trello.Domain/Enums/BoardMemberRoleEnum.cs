using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Domain.Enums
{
    public enum BoardMemberRoleEnum
    {
        [Description("Member")]
        Member,

        [Description("Admin")]
        Admin
    }
}
