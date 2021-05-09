using Enterprise.Framework.Log;
using Enterprise.Service.Base;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace Enterprise.IIS.Common
{
    public class HttpRequest
    {
        public void httpRequest(tmsModel t, string uri)
        {
            string result = "";

            string details = "";
            string message = "";

            try
            {
                if (t.stockOutDetailsList != null)
                {
                    foreach (LHStockOutDetails item in t.stockOutDetailsList)
                    {
                        details += item.FId + "---" + item.FItemCode + ",";
                    }
                }
                if (t.stockOutList != null && t.stockOutList.Count > 0)
                {
                    message += "，发货单：" + t.stockOutList[0].KeyId;
                }
                if (t.stockOutDetailsList != null)
                {
                    message += "，发货单详细单号：" + details;
                }
                if (t.passCardList != null && t.passCardList.Count > 0)
                {
                    message += "，销售单：" + t.passCardList[0].KeyId;
                }
                if (t.passCardDetailsList != null)
                {
                    string sdetails = "";
                    foreach (LHPassCardDetails item in t.passCardDetailsList)
                    {
                        sdetails += item.FId + "---" + item.FItemCode + ",";
                    }
                    message += "，销售单详细单号：" + sdetails;
                }
                if (t.DispatchDetailsList != null && t.DispatchDetailsList.Count > 0)
                {
                    message += "，调度单：" + t.dispatchCenterList[0].KeyId;
                }
                LogUtil.InfoLog(message);
                string url = System.Configuration.ConfigurationManager.AppSettings["ineterface"];
                WebRequest req = HttpWebRequest.Create(url + uri + "?appkey=e50f9e48d3234807a20ffd3653fafa7b") as WebRequest;
                req.Method = "post";
                string json = "params=" + JsonConvert.SerializeObject(t);
                byte[] bt = Encoding.UTF8.GetBytes(json);
                req.ContentLength = bt.Length;
                req.ContentType = "application/x-www-form-urlencoded";
                Stream st = req.GetRequestStream();
                st.Write(bt, 0, bt.Length);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader red = new StreamReader(stream);
                result = red.ReadToEnd();
                TmsReturnModel model = JsonConvert.DeserializeObject<TmsReturnModel>(result);


                if (model.code != "200")
                {
                    message += "，同步失败,状态码：" + model.code + ",信息：" + model.msg;
                    LogUtil.InfoLog(message);
                }
            }
            catch (System.Exception ex)
            {
                details = "";
                message += "错误：";
                if (t.stockOutDetailsList != null)
                {
                    foreach (LHStockOutDetails item in t.stockOutDetailsList)
                    {
                        details += item.FId + "---" + item.FItemCode + ",";
                    }
                }
                if (t.stockOutList != null && t.stockOutList.Count > 0)
                {
                    message += "，发货单：" + t.stockOutList[0].KeyId;
                }
                if (t.stockOutDetailsList != null)
                {
                    message += "，发货单详细单号：" + details;
                }
                if (t.passCardList != null && t.passCardList.Count > 0)
                {
                    message += "，销售单：" + t.passCardList[0].KeyId;
                }
                if (t.passCardDetailsList != null)
                {
                    string sdetails = "";
                    foreach (LHPassCardDetails item in t.passCardDetailsList)
                    {
                        sdetails += item.FId + "---" + item.FItemCode + ",";
                    }
                    message += "，销售单详细单号：" + sdetails;
                }
                if (t.DispatchDetailsList != null && t.DispatchDetailsList.Count > 0)
                {
                    message += "，调度单：" + t.dispatchCenterList[0].KeyId;
                }
                message += ",错误信息：" + ex.Message;
                message += ",错误堆栈：" + ex.StackTrace;
                LogUtil.InfoLog(message);
            }

        }
    }
}