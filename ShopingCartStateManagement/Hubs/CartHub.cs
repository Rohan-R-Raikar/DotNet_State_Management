using Microsoft.AspNetCore.SignalR;
// for live count
using System.Collections.Concurrent;
namespace ShopingCartStateManagement.Hubs
{
    public class CartHub : Hub
    {
        // Track all active connections
        private static ConcurrentDictionary<string, string> ConnectedUsers = new();
        // Track only logged-in users (UserIdentifier != null)
        private static ConcurrentDictionary<string, string> LoggedInUsers = new();

        //public override async Task OnConnectedAsync()
        //{
        //    var userId = Context.UserIdentifier;
        //    Console.WriteLine($"User connected to CartHub: {userId}");
        //    await base.OnConnectedAsync();
        //}

        public override async Task OnConnectedAsync()
        {
            ConnectedUsers.TryAdd(Context.ConnectionId, Context.ConnectionId);

            if (Context.User?.Identity?.IsAuthenticated == true && Context.UserIdentifier != null)
            {
                LoggedInUsers.TryAdd(Context.UserIdentifier, Context.ConnectionId);
            }

            await BroadcastCounts();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedUsers.TryRemove(Context.ConnectionId, out _);

            if (Context.User?.Identity?.IsAuthenticated == true && Context.UserIdentifier != null)
            {
                LoggedInUsers.TryRemove(Context.UserIdentifier, out _);
            }

            await BroadcastCounts();

            await base.OnDisconnectedAsync(exception);
        }

        private Task BroadcastCounts()
        {
            var total = ConnectedUsers.Count;
            var loggedIn = LoggedInUsers.Count;
            return Clients.All.SendAsync("UpdateUserCounts", new { total, loggedIn });
        }

        //public async Task NotifyStockChanged()
        //{
        //    await Clients.All.SendAsync("StockUpdated");
        //}
    }
}
