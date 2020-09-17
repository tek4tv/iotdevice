using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Tek4TV.Devices.Apis
{
    [RoutePrefix("api/iot")]
    public class IOTController : ApiController
    {
        private static string _appID = "bc6da08b-3ad4-4452-8f29-d56bc69e31995";
        private static string _apiKey = "5G2Zix5YcWLdatLFrr+81d7ldMV7Yt5CGftGF5VTqhM=8";
        private static string _accountID = "1fb0495c-9cbf-45a3-b904-c990e9c859fd";
        private static string _playbackUrl = "https://mam.tek4tv.vn/";
        [Route("category")]
        public async Task<HttpResponseMessage> GetLiveAsync()
        {
            try
            {
                string policyKey = "";
                string _liveID = ConfigurationManager.AppSettings["LiveID"];
                var data = new { AppID = _appID, ApiKey = _apiKey, AccountId = _accountID };
                using (var httpClient = new HttpClient())
                {
                    String domain = HttpContext.Current.Request.Url.Host;
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string urlToken = _playbackUrl + "api/token";
                    var response = httpClient.PostAsJsonAsync(urlToken, data).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        policyKey = response.Content.ReadAsAsync<string>().Result;
                    }
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", policyKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string url = _playbackUrl + "iot/v1/accounts/" + _accountID + "/category";
                    var responsePost = await httpClient.GetAsync(url);
                    if (responsePost.IsSuccessStatusCode)
                    {
                        string responseBody = await responsePost.Content.ReadAsStringAsync();
                        dynamic output = JsonConvert.DeserializeObject(responseBody);
                        return Request.CreateResponse(HttpStatusCode.OK, (Object)output, Configuration.Formatters.JsonFormatter);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
       
        [Route("video")]
        public async Task<HttpResponseMessage> PostVideoAsync(RequestBodyVideo requestBodyVideo)
        {
            try
            {
                string policyKey = "";              
                string _liveID = ConfigurationManager.AppSettings["LiveID"];
                var data = new { AppID = _appID, ApiKey = _apiKey, AccountId = _accountID };              
                using (var httpClient = new HttpClient())
                {
                    String domain = HttpContext.Current.Request.Url.Host;
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string urlToken = _playbackUrl + "api/token";
                    var response = httpClient.PostAsJsonAsync(urlToken, data).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        policyKey = response.Content.ReadAsAsync<string>().Result;
                    }
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", policyKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string url = _playbackUrl + "iot/v1/video";
                    var responsePost = await httpClient.PostAsJsonAsync(url, requestBodyVideo);
                    if (responsePost.IsSuccessStatusCode)
                    {
                        string responseBody = await responsePost.Content.ReadAsStringAsync();
                        dynamic output = JsonConvert.DeserializeObject(responseBody);
                        return Request.CreateResponse(HttpStatusCode.OK, (Object)output, Configuration.Formatters.JsonFormatter);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
