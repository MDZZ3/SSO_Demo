using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSO.Server.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SSO.Server.Helper;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using SSO.Server.Models.CustomizeException.Token;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SSO.Server.Controllers
{
    public class LoginController : Controller
    {

       
        [HttpGet]
        public IActionResult Index(string returnUrl)
        {

            string token = null;
            if (HttpContext.User.Claims.Any())
            {
                token = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Authentication).Value;
                //验证放在Cookie的Token有无过期，如果过期了就跳转到登录页
                try
                {
                   var info=TokenHelper.VerificationToken(token);
                }
                catch (Exception)
                {
                    return View();
                }

                if (returnUrl != null)
                {
                    return Redirect($"{returnUrl}?Token={token}");
                }

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginInputModel model)
        {
            string token = null;

            if (model == null)
            {
                //返回错误信息
            }
            if (string.IsNullOrEmpty(model.Username) ||string.IsNullOrEmpty(model.Password))
            {
                //返回错误信息
                ModelState.AddModelError(model.Password, "错误的请求");
            }
            if (model.Username == "123" && model.Password == "123456")
            {
                //生成Token
                token = TokenHelper.GetToken(model.Username);



                var principal = CookieHelper.GetPrincipal(token);

                var pories = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(5)
                };

                await HttpContext.SignInAsync("sso", principal, pories);
            }
            if (model.ReturnUrl != null)
            {
                return Redirect($"{model.ReturnUrl}?Token={token}");
            }

            return View();
        }


        /// <summary>
        /// 检查客户端发的Token是否有效
        /// </summary>
        /// <param name="jObject">Token</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult checkToken([FromBody] JObject jObject)
        {
            if (jObject == null)
            {
                return Json(new { code = "400", Message = "参数为空" });
            }
            string Token = jObject["Token"].ToString();
            if (string.IsNullOrEmpty(Token))
            {
                return Json(new { code = "400", Message = "参数为空" });
            }
            TokenModel principal = null;
            try
            {
                //验证Token
                principal = TokenHelper.VerificationToken(Token);
                
            }
            catch(TokenSignatureException Signature)
            {
                return Json(new { code = "401",info="", Message = Signature.Message });
            }
            catch (TokenExpiredException expired)
            {
                return Json(new { code = "401",info="", Message = expired.Message });
            }
            return Json(new { code = "200",info=principal, Message = "Token有效" });
        }


       
        
    }
}