using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace GDOCService.API.Infraestructure.Hubs
{
    public class DiagnosticsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Send", $"{Context.ConnectionId} joined");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.Others.SendAsync("Send", $"{Context.ConnectionId} left");
        }

        public Task SendRequestTrace(string message)
        {
            return Clients.All.SendAsync("SendRequestTrace", message);
        }

    }
}
