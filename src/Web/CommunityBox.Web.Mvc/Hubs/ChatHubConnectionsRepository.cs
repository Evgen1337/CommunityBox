using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityBox.Web.Mvc.Hubs
{
    public class ChatHubConnectionsRepository
    {
        private static readonly ConcurrentDictionary<string, UserConnection>
            UserConnections = new();

        public static IReadOnlyCollection<UserConnection> GetByUserId(string userId) =>
            UserConnections.Where(f => f.Value.UserId == userId)
                .Select(s => s.Value)
                .ToArray();

        public static UserConnection GetByCollectionId(string connectionId)
        {
            UserConnections.TryGetValue(connectionId, out var userConnection);
            return userConnection;
        }
        
        public static bool Add(string connectionId, UserConnection connection) =>
            UserConnections.TryAdd(connectionId, connection);

        public static bool Remove(string connectionId) =>
            UserConnections.TryRemove(connectionId, out var connection);
    }
}