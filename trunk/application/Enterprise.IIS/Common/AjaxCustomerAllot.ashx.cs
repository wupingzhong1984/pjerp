using System.Linq;
using System.Web;
using Enterprise.Service.Base.ERP;
using Newtonsoft.Json.Linq;
using Enterprise.Framework.FormsAuth;

namespace Enterprise.IIS.Common
{
    /// <summary>
    /// AjaxCustomerAllot 的摘要说明
    /// </summary>
    public class AjaxCustomerAllot : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string code = context.Request.QueryString["term"];
            if (!string.IsNullOrEmpty(code))
            {
                var companydId = -1;
                var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
                if (formsPrincipal != null)
                {
                    companydId = formsPrincipal.UserInfo.AccountComId;
                }

                var service = new CustomerService();

                var ja = new JArray();

                foreach (
                    var custmoer in
                        service.Where(
                            p => p.FIsAllot == 1 && p.FCompanyId==companydId&&
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