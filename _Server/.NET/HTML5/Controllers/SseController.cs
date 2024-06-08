using Microsoft.AspNetCore.Mvc;
using System.Text;
using System;
using System.Net.Http;

namespace HTML5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SseController : ControllerBase
    {

        public SseController()
        {

        }

        [HttpGet("getServerTime")]
        public void getServerTime()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            string msg = "";
            //msg += $"event:message\n";    //未註明event，預設都是message
            msg += $"id:{Guid.NewGuid()}\n";
            msg += "retry:1000\n";
            msg += $"data:{DateTime.Now.ToString("HH:mm:ss")}\n\n";
            Response.WriteAsync(msg);
        }

        [HttpGet("getUtcServerTime")]
        public void getUtcServerTime()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            string msg = "";
            msg += "retry:1000\n";
            msg += $"event:serverTime\n";
            msg += $"id:{Guid.NewGuid()}\n";
            msg += $"data:{DateTime.Now.ToString("HH:mm:ss")}\n\n";

            msg += $"event:utcTime\n";
            msg += $"id:{Guid.NewGuid()}\n";
            msg += $"data:{DateTime.UtcNow.ToString("HH:mm:ss")}\n\n";
            Response.WriteAsync(msg);
        }

        [HttpGet("getTaipeiYoubikeInfo")]
        public async Task getTaipeiYoubikeInfo()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            string msg = "";
            msg += $"event:serverTime\n";
            msg += $"id:{Guid.NewGuid()}\n";
            msg += $"data:{DateTime.Now.ToString("HH:mm:ss")}\n\n";

            msg += $"event:youbikeInfo\n";
            msg += $"id:{Guid.NewGuid()}\n";
            msg += "retry:30000\n";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://tcgbusfs.blob.core.windows.net/dotapp/youbike/v2/youbike_immediate.json");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    msg += $"data:{json}\n\n";
                    Response.WriteAsync(msg);
                }
                else
                {
                    msg += $"data:null\n\n";
                    Response.WriteAsync(msg);
                }
            }
        }

        [HttpGet("getKaohsiungYoubikeInfo")]
        public async Task getKaohsiungYoubikeInfo()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            string msg = "";
            msg += $"event:serverTime\n";
            msg += $"id:{Guid.NewGuid()}\n";
            msg += $"data:{DateTime.Now.ToString("HH:mm:ss")}\n\n";

            msg += $"event:youbikeInfo\n";
            msg += $"id:{Guid.NewGuid()}\n";
            msg += "retry:30000\n";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://api.kcg.gov.tw/api/service/Get/b4dd9c40-9027-4125-8666-06bef1756092");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    msg += $"data:{json}\n\n";
                    Response.WriteAsync(msg);
                }
                else
                {
                    msg += $"data:null\n\n";
                    Response.WriteAsync(msg);
                }
            }
        }

        [HttpGet("getTwStock/{id}")]
        public async Task getTwStock(string id)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            string msg = "";
            msg += $"event:twStock\n";
            msg += $"id:{Guid.NewGuid()}\n";
            msg += "retry:3000\n";
            using (var client = new HttpClient())
            {
                //tse開頭為上市股票
                //otc開頭為上櫃股票
                var response = await client.GetAsync($"https://mis.twse.com.tw/stock/api/getStockInfo.jsp?json=1&delay=0&ex_ch=tse_t00.tw|tse_{id}.tw");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    msg += $"data:{json.Replace("\n", "")}\n\n";
                    Response.WriteAsync(msg);
                }
                else
                {
                    msg += $"data:null\n\n";
                    Response.WriteAsync(msg);
                }
            }
        }

    }
}
