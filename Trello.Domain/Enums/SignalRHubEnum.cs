using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Domain.Enums
{
    public enum SignalRHubEnum
    {
        ReceiveComment,
        UpdateComment,
        ReceiveTotalNotification,
        ReceiveActivity,
        ReceiveNotification,
    }
}
