using Enterprise.Framework.Log;
using System.Web;
using Tencent;

namespace Enterprise.IIS.business.WeChat
{
    /// <summary>
    /// WeChatRecive 的摘要说明
    /// </summary>
    public class WeChatRecive : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string msg_signature = context.Request["msg_signature"];
            string timestamp = context.Request["timestamp"];
            string nonce = context.Request["nonce"];
            string echostr = context.Request["echostr"];
            LogUtil.InfoLog(msg_signature);
            LogUtil.InfoLog(timestamp);
            LogUtil.InfoLog(nonce);
            LogUtil.InfoLog(echostr);
            string sToken = "f6shKIwCJQVTCEI";
            string sCorpID = "wwee3e828e64b4f53e";
            string sEncodingAESKey = "iiVP4HOUpkWyYO2Vic5GI839aWBn5vLY61EeGISb0zo";

            WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sCorpID);
            int ret = 0;
            string sEchoStr = "";
            ret = wxcpt.VerifyURL(msg_signature, timestamp, nonce, echostr, ref sEchoStr);
            context.Response.Write(sEchoStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}