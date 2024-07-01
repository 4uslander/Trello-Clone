using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Domain.Enums
{
    public enum TaskPriorityLevelEnum
    {
        [Description("High")]
        High,

        [Description("Medium")]
        Medium,

        [Description("Low")]
        Low,
    }
}
