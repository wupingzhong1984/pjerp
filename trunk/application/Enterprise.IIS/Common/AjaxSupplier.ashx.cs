using System.Linq;
using System.Web;
using Enterprise.Service.Base.ERP;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.Common
{
    /// <summary>
    /// AjaxSupplier 的摘要说明
    /// </summary>
    public class AjaxSupplier : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string code = context.Request.QueryString["term"];
            if (!string.IsNullOrEmpty(code))
            {
                var service = new SupplierService();

                var ja = new JArray();

                foreach (
                    var custmoer in
                        service.Where(
                            p => p.FFlag == 1 &&
                                ((p.FCode.Contains(code) || p.FName.Contains(code)) ||
                                 p.FSpell.Contains(code))).OrderBy(p => p.FName).Take(10))
                {
                    ja.Add(string.Format("{0}", custmoer.FName));
                }

                context.Response.ContentType = "text/plain";
                context.Response.Write(ja.ToString());
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}