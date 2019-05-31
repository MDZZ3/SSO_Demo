using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSO.Client2.Models;
using Microsoft.AspNetCore.Authentication;
using SSO.Client2.Helper;
using System.Security.Claims;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using SSO.Client2.Models.Bean;

namespace SSO.Client.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index(string Token)
        {
            string name = null;

            if (Token != null)
            {
                var userinfo = await VerificationToken(Token);
                if (userinfo == null)
                {
                    return Redirect("http://www.c.cn:52529/Login/Index?returnUrl=http://www.b.net:53658/Home/Index");
                }

                var Properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(5)
                };

                Claim[] claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,userinfo.UserName)
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                //将用户信息存入Cookie
                await HttpContext.SignInAsync("Client2", principal, Properties);

                name = principal.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.Name).Value;

                //将用户信息存入Session，这里我存了Token进行，可以修改
                HttpContext.Session.SetString(name,Token);
              

                ViewData["name"] = name;
            }
            else
            {
                //查看Cookie在不在
                name = HttpContext.User.Identity.Name;
            }
            //如果name为空，证明没有登录，所以跳转到认证中心
            if (string.IsNullOrEmpty(name))
            {
                return Redirect("http://www.c.cn:52529/Login/Index?returnUrl=http://www.b.net:53658/Home/Index");
            }
            var token = HttpContext.Session.GetString(name);
            return View();
        }

        /// <summary>
        ///发送给SSO，判断Token是否有效
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<UserInfo> VerificationToken(string token)
        {
            using (HttpClient hc = new HttpClient())
            {
                //设置请求的Body
                var jsonToken = JsonConvert.SerializeObject(new { Token = token });
                StringContent content = new StringContent(jsonToken, Encoding.UTF8, "application/json");
                //发送请求
                HttpResponseMessage response = await hc.PostAsync("http://www.c.cn:52529/Login/checkToken", content);
                //获取响应的内容
                string contentString = await response.Content.ReadAsStringAsync();
                VerificationTokenBean verificationbean = JsonConvert.DeserializeObject<VerificationTokenBean>(contentString);

                //根据响应结果来决定Token是否有效
                if (verificationbean.code == "401" || verificationbean.code == "400")
                {
                    return null;
                }
                return verificationbean.info;
            }
        }



        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
