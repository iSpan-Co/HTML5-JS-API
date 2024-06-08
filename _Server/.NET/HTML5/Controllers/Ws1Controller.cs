using Microsoft.AspNetCore.Mvc;
using System.Text;
using System;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text.Json;
using HTML5.Models;

namespace HTML5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Ws1Controller : ControllerBase
    {

        //紀錄所有連線的人
        static ConcurrentDictionary<int, WebSocket> WebSockets = new ConcurrentDictionary<int, WebSocket>();

        public Ws1Controller()
        {

        }

        [HttpGet("chatWs")]
        public async Task chatWs()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                WebSockets.TryAdd(webSocket.GetHashCode(), webSocket);  //將連接進來的使用者加到WebSockets集合(ConcurrentDictionary)
                await ProcessWebSocket(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        //定義ProcessWebSocket函式
        private async Task ProcessWebSocket(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4]; //建立一個4k大小的RAM空間，用來存放要傳送的資料

            //將接收到資料塞進buffer中，不做取消的處理
            var res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!res.CloseStatus.HasValue)
            {
                string json = Encoding.UTF8.GetString(buffer, 0, res.Count);
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                MsgObj? receivedMsg = JsonSerializer.Deserialize<MsgObj>(json, options);
                if (receivedMsg != null)
                {
                    Broadcast(receivedMsg); //接收到的資料傳給Broadcase自訂函式，在此函式中廣播給所有連線的使用者
                }
                res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            //websocket關閉
            await webSocket.CloseAsync(res.CloseStatus.Value, res.CloseStatusDescription, CancellationToken.None);
            //從WebSockets集合(ConcurrentDictionary)移除離線使用者
            WebSockets.TryRemove(webSocket.GetHashCode(), out var removed);
        }

        //定義Broadcase函式
        private void Broadcast(MsgObj msgObj)
        {
            //平行運算
            Parallel.ForEach(WebSockets.Values, async (webSocket) =>
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    string msg = msgObj.User + " 說: " + msgObj.Message;
                    await webSocket.SendAsync(Encoding.UTF8.GetBytes(msg), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            });
        }

    }
}
