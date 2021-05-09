using Enterprise.IIS.WeChatModel;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Enterprise.IIS.Common
{
    public class WeChatService
    {
        protected const string WeChatUrl = "https://qyapi.weixin.qq.com/cgi-bin";
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        }
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private WeChatAccesstoken token;
        //发起请求
        public string WeChatRequest(string Url, string body, string method)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            HttpWebRequest request = WebRequest.Create(WeChatUrl + Url) as HttpWebRequest;
            request.Method = method;
            request.ProtocolVersion = HttpVersion.Version10;
            request.UserAgent = DefaultUserAgent;
            request.Proxy = null;
            //写入body内容
            if (!string.IsNullOrEmpty(body))
            {

                using (Stream st = request.GetRequestStream()){

                    byte[] bts = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = bts.Length;
                    st.Write(bts, 0, bts.Length);
                }
            }
            return WeChatResponse(request);
        }

        //接收数据
        public string WeChatResponse(HttpWebRequest request)
        {
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream st = response.GetResponseStream();
            StreamReader red = new StreamReader(st);
            string value = red.ReadToEnd();
            red.Close();
            st.Close();
            return value;
        }


        public string Acces_token()
        {
            if (token != null && token.LogTime.AddSeconds(token.expires_in) < DateTime.Now)
            {
                return token.access_token;
            }
            else
            {
                string tokenid = WeChatRequest("/gettoken?corpid=wwee3e828e64b4f53e&corpsecret=zwx-ZCLbH54NW7vMGn95-VQdLgRxyF4FEilHWu231kE", "", "get");
                token = JsonConvert.DeserializeObject<WeChatAccesstoken>(tokenid);
                return token.access_token;
            }
        }

    }
}