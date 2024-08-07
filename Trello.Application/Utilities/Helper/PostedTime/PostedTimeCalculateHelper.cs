﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.Utilities.Helper.PostedTime
{
    public static class PostedTimeCalculateHelper
    {
        public static string GetElapsedTimeSinceCreation(DateTime? createdAt)
        {
            if (createdAt.HasValue)
            {
                DateTime currentTime = DateTime.UtcNow;
                TimeSpan timeDifference = currentTime - createdAt.Value;

                if (timeDifference.TotalSeconds < 60)
                {
                    return $"{(int)timeDifference.TotalSeconds} seconds ago";
                }
                else if (timeDifference.TotalMinutes < 60)
                {
                    return $"{(int)timeDifference.TotalMinutes} minutes ago";
                }
                else if (timeDifference.TotalHours < 24)
                {
                    return $"{(int)timeDifference.TotalHours} hours ago";
                }
                else if (timeDifference.TotalDays < 7)
                {
                    return $"{(int)timeDifference.TotalDays} days ago";
                }
                else
                {
                    return createdAt.Value.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                return "Creation date not available.";
            }
        }
    }
}
