using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.Utilities.Helper.SignalRHub.UserConnection
{
    public interface IUserConnectionManager
    {
        void KeepUserConnection(string userId, string connectionId);
        void RemoveUserConnection(string userId, string connectionId);
        List<string> GetUserConnections(string userId);
    }

    public class UserConnectionManager : IUserConnectionManager
    {
        private static readonly ConcurrentDictionary<string, List<string>> _userConnections = new ConcurrentDictionary<string, List<string>>();

        public void KeepUserConnection(string userId, string connectionId)
        {
            _userConnections.AddOrUpdate(userId,
                new List<string> { connectionId },
                (key, oldValue) => {
                    oldValue.Add(connectionId);
                    return oldValue;
                });
        }

        public void RemoveUserConnection(string userId, string connectionId)
        {
            if (_userConnections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    _userConnections.TryRemove(userId, out _);
                }
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            _userConnections.TryGetValue(userId, out var connections);
            return connections ?? new List<string>();
        }
    }

}
