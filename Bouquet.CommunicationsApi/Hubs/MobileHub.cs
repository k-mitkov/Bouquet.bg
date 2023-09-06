using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Bouquet.CommunicationsApi.Hubs
{
    public class MobileHub : Hub
    {
        public static Dictionary<string, string> ConnectedClients = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            // Extract some client identifier (e.g., user ID) from the Context if needed
            string clientId = Context.GetHttpContext().Request.Query["clientId"];

            ConnectedClients[clientId] = connectionId;

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Remove client from the dictionary when disconnected
            string clientId = ConnectedClients.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(clientId))
            {
                ConnectedClients.Remove(clientId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private string GetClientIdFromContext(HubCallerContext context)
        {
            // Example: Retrieve the user's ID from claims
            var userIdClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }

        // Other hub methods...
    }
}
