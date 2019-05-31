using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SSO.Server.Models;
using SSO.Server.Models.CustomizeException.Token;

namespace SSO.Server.Helper
{
    public class TokenHelper
    {
        private static string key = "this is my private_key";


        public static string GetToken(string uid)
        {
            //将用户信息化为Token的前部
            var userinfo = GetBase64UserInfo(uid);
            //制作Token签名
            var signature = GetMD5Signature(userinfo);

            return $"{userinfo}.{signature}";

        }

        public static TokenModel VerificationToken(string Token)
        {
            if (Token == null)
            {
                throw new ArgumentNullException("Token为null");
            }

            string[] verificationInfo = Token.Split(".");

            var TokenSignature = GetMD5Signature(verificationInfo[0]);

            //Token用户信息被人修改了
            if (TokenSignature != verificationInfo[1])
            {
                throw new TokenSignatureException("Token签名错误");
            }

            byte[] base64byte = Convert.FromBase64String(verificationInfo[0]);
            string userinfo = Encoding.UTF8.GetString(base64byte);

            var usermodel = JsonConvert.DeserializeObject<TokenModel>(userinfo);

            //判断Token是否过期
            if (CheckTokenExpired(usermodel.Expire))
            {
                throw new TokenExpiredException("已过期");
            }
       
            return usermodel;
        }

        /// <summary>
        /// 查看Token是否在规定时间（Token里面的指定时间+5分钟）
        /// </summary>
        /// <param name="Ticket">Token的过期Ticket</param>
        /// <returns></returns>
        protected static bool CheckTokenExpired(long Ticket)
        {
            long Now = DateTime.Now.Ticks;

            if (Ticket - Now <= 0)
            {
                var expiredTicket = Now - Ticket;
                //查看Token过期的多久
                TimeSpan delay = new TimeSpan(expiredTicket);
                double expiredMinutes = Math.Ceiling(delay.TotalMinutes);
                //如果过期了5分钟，就抛出异常
                if (expiredMinutes >= 5)
                {
                    throw new TokenExpiredException("Token已过期");
                }
            }
            return false;
        }

        protected static string GetBase64UserInfo(string uid)
        {
            var expirt = DateTime.Now.AddHours(3).Ticks;
            var userinfo = JsonConvert.SerializeObject(new TokenModel { UserName = uid, Expire = expirt });

            var user = ToByte(userinfo);

            return Convert.ToBase64String(user);
        }

        protected static Byte[] ToByte(string value)
        {

            return Encoding.UTF8.GetBytes(value);
        }


        protected static string GetMD5Signature(string base64info)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base64info);
            builder.Append(key);
            using (MD5 md5 = MD5.Create())
            {
                byte[] info = md5.ComputeHash(ToByte(builder.ToString()));
                builder.Clear();
                for (int i = 0; i < info.Length; i++)
                {
                    //将加密后的byte转换成32字符的16进制
                    builder.Append(info[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }
    }
}
