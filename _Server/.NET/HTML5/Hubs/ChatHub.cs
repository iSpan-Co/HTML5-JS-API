using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;

namespace HTML5.Hubs
{
    public class ChatHub : Hub
    {

        /// <summary>
        /// 連線事件
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UpdContent", "新連線 ID: " + Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("UpdContent", "已離線 ID: " + Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("UpdContent", user + " 說: " + message);
        }

    }
}
