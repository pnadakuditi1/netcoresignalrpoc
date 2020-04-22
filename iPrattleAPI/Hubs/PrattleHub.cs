using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iPrattle.Services;
using iPrattle.Services.Entities;
using Microsoft.AspNetCore.SignalR;

namespace iPrattleAPI.Hubs
{
    public class PrattleHub : Hub
    {
        private static readonly Dictionary<string, Guid> liveConnections
        = new Dictionary<string, Guid>();

        private readonly ICommunicationSvc _commSvc;

        public PrattleHub(ICommunicationSvc commSvc)
        {
            this._commSvc = commSvc;
        }

        public void NewMessage(Message message)
        {
            message.CreatedOn = DateTime.UtcNow;

            var receiverClientConnectionIds = liveConnections.Where(c => c.Value == message.ReceiverId || c.Value == message.SenderId).Select(c => c.Key).ToList();
            foreach(string s in receiverClientConnectionIds)
            {
                Console.WriteLine($"qualified: {s}");
            }
            _commSvc.AddMessage(message);
            Clients.Clients(receiverClientConnectionIds).SendAsync("MessageReceived", message);
        }

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var httpContext = Context.GetHttpContext();
            if (Guid.TryParse(httpContext.Request.Query["clientId"].ToString(), out Guid userId))
            {
                if (!liveConnections.ContainsKey(connectionId))
                {
                    liveConnections.Add(connectionId, userId);
                }
            }
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var httpContext = Context.GetHttpContext();
            if (Guid.TryParse(httpContext.Request.Query["clientId"].ToString(), out Guid userId))
            {
                if (liveConnections.ContainsKey(connectionId))
                {
                    liveConnections.Remove(connectionId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
