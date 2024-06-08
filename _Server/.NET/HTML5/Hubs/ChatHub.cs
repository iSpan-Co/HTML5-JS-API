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

            //if (ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault() == null)
            //{
            //    ConnIDList.Add(Context.ConnectionId);
            //}
            //// 更新連線 ID 列表
            //string jsonString = JsonConvert.SerializeObject(ConnIDList);
            //await Clients.All.SendAsync("UpdList", jsonString);

            //// 更新個人 ID
            //await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId);

            //// 更新聊天內容
            //await Clients.All.SendAsync("UpdContent", "新連線 ID: " + Context.ConnectionId);

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
            //string id = ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault();
            //if (id != null)
            //{
            //    ConnIDList.Remove(id);
            //}
            //// 更新連線 ID 列表
            //string jsonString = JsonConvert.SerializeObject(ConnIDList);
            //await Clients.All.SendAsync("UpdList", jsonString);

            //// 更新聊天內容
            //await Clients.All.SendAsync("UpdContent", "已離線 ID: " + Context.ConnectionId);

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
            //if (string.IsNullOrEmpty(user))
            //{
            //}
            //else
            //{
            //    // 接收人
            //    await Clients.Client(sendToID).SendAsync("UpdContent", selfID + " 私訊向你說: " + message);

            //    // 發送人
            //    await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "你向 " + sendToID + " 私訊說: " + message);
            //}
        }

    }
}
