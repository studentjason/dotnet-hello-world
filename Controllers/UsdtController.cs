using System;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UsdtController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UsdtController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    [HttpGet()]
    public IActionResult Index()
    {
        var fetchRybitRateTask = this.fetchRybitRateAsync();
        var fetchMaicoinRateTask = this.fetchMaicoinRateAsync();

        ViewData["rybitCoinRatePrice"] = fetchRybitRateTask.Result.price;
        ViewData["rybitCoinRateTime"] = fetchRybitRateTask.Result.time;
        ViewData["rybitCoinRateUtctime"] = fetchRybitRateTask.Result.UtcTime.ToString("yyyy/MM/dd HH:mm:ss");
        ViewData["MaicoinRatePrice"] = fetchMaicoinRateTask.Result.close;
        ViewData["MaicoinRateTime"] = fetchMaicoinRateTask.Result.time;
        ViewData["MaicoinRateUtctime"] = fetchMaicoinRateTask.Result.UtcTime.ToString("yyyy/MM/dd HH:mm:ss");
        var rateDiff = (((double)ViewData["MaicoinRatePrice"] - (double)ViewData["rybitCoinRatePrice"]) / (double)ViewData["rybitCoinRatePrice"]) * 100;
        ViewData["RateDiff"] = Math.Round(rateDiff, 3);

        return View();
    }

    protected async Task<RybitCoinRateModel> fetchRybitRateAsync()
    {
        // 取得 HttpClient 實例
        var client = _httpClientFactory.CreateClient();

        // Add custom headers to the request
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

        // 設定要取得的網址
        var url = "https://www.rybit.com/wallet-api/v1/kgi/exchange-rates/?symbol=USDT_TWD&client_id=rybit_web_v2023.05.08.40&device_id=ca008fec-9dfb-4d9b-849f-ef730bc15eb3&fp_did=unknown&app_ver=v2023.05.08.40&tz_name=Asia%2FTaipei&tz_offset=28800&sys_lang=zh-TW&app_lang=zh-TW";

        // 發送 HTTP GET 請求並取得回應
        var response = await client.GetAsync(url);
        var record = new RybitCoinRateModel();

        // 如果回應成功，讀取回應的內容
        if (response.IsSuccessStatusCode)
        {
            // {"code":0,"message":"success","data":{"buy_rate":"30.865","sell_rate":"30.772","symbol":"USDT_TWD","update_time":1683787620013}}
            var content = await response.Content.ReadAsStringAsync();
            var jsonObj = JObject.Parse(content);

            record.price = (double) jsonObj["data"]["buy_rate"];
            record.time = (long) jsonObj["data"]["update_time"];
        }

        return record;
    }

    protected async Task<MaiCoinRateModel> fetchMaicoinRateAsync()
    {
        // 取得 HttpClient 實例
        var client = _httpClientFactory.CreateClient();

        // Add custom headers to the request
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

        // 設定要取得的網址
        var url = "https://max-api.maicoin.com/api/v2/k?market=usdttwd&limit=1";

        // 發送 HTTP GET 請求並取得回應
        var response = await client.GetAsync(url);
        var record = new MaiCoinRateModel();

        // 如果回應成功，讀取回應的內容
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            JArray jsonArray = JArray.Parse(content);

            foreach (JArray item in jsonArray)
            {
                record.time = (long)item[0];
                record.open = (double)item[1];
                record.high = (double)item[2];
                record.low = (double)item[3];
                record.close = (double)item[4];
            }
        }

        return record;
    }
}