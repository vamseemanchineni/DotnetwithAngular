namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> Onlineusers = new Dictionary<string, List<string>>();
        public Task<bool> UserConnected(string username, string connectionId)
        {
            bool isOnline = false;
            lock(Onlineusers)
            {
                if(Onlineusers.ContainsKey(username))
                {
                    Onlineusers[username].Add(connectionId);
                }
                else 
                {
                    Onlineusers.Add(username, new List<string>{connectionId});
                    isOnline = true;
                }
            }
            return Task.FromResult(isOnline);
        }
        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock(Onlineusers)
            {
                if(!Onlineusers.ContainsKey(username)) return Task.FromResult(isOffline);
                Onlineusers[username].Remove(connectionId);
                if(Onlineusers[username].Count == 0)
                {
                    Onlineusers.Remove(username);
                    isOffline = true;
                }
            }
            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock(Onlineusers)
            {
                onlineUsers = Onlineusers.OrderBy(k=>k.Key).Select(k=>k.Key).ToArray();
            }
            return Task.FromResult(onlineUsers);
        }
        public static Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;
            lock(Onlineusers)
            {
                connectionIds = Onlineusers.GetValueOrDefault(username);
            }
            return Task.FromResult(connectionIds);
        }
    }
}