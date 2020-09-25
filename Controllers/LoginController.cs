using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tek4TV.Devices.RequestBody;

namespace Tek4TV.Devices.Controllers
{
    public class LoginController : Controller
    {
        private static string _appID = "bc6da08b-3ad4-4452-8f29-d56bc69e31995";
        private static string _apiKey = "5G2Zix5YcWLdatLFrr+81d7ldMV7Yt5CGftGF5VTqhM=8";
        private static string _accountID = "1fb0495c-9cbf-45a3-b904-c990e9c859fd";
        private static string _playbackUrl = "https://mam.tek4tv.vn/";
        // GET: Login
        public ActionResult Index()
        {

            return View();
        }       
        public async Task<ActionResult> LoginAsync(RequestBodyLogin login)
        {
            try
            {
                string policyKey = "";
                var data = new { AppID = _appID, ApiKey = _apiKey, AccountId = _accountID };
                var dataLogin = new { UserName = login.UserName, PassWord = login.PassWord };
                using (var httpClient = new HttpClient())
                {
                    //String domain = HttpContext.CurrentNotification.Request.Url.Host;
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
                    string url = _playbackUrl + "iot/v1/login";
                    var responsePost = await httpClient.PostAsJsonAsync(url, login);
                    if (responsePost.IsSuccessStatusCode)
                    {
                        string responseBody = await responsePost.Content.ReadAsStringAsync();
                        dynamic output = JsonConvert.DeserializeObject(responseBody);
                        if (output == null)
                        {
                            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                        }
                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(output);
                        var tokenS = handler.ReadToken(output) as JwtSecurityToken;
                        var role = tokenS.Claims.First(claim => claim.Type == "role").Value;
                        Session["User"] = login.UserName;
                        Session["role"] = role;

                        return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index","Login");
        }

    }
}