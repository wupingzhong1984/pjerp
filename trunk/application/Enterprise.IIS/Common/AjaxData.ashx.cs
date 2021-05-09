using System.Linq;
using System.Web;
using Enterprise.Service.Base.ERP;
using Enterprise.Framework.FormsAuth;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.Common
{
    /// <summary>
    /// AjaxData 的摘要说明
    /// </summary>
    public class AjaxData : IHttpHandler
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

                var service= new ItemsService();

                var ja = new JArray();

                foreach (
                    var cate in service.Where(
                            p => p.FCompanyId==companydId&&
                                ((p.FCode.Contains(code) || p.FName.Contains(code)) ||
                                 p.FSpell.Contains(code))).OrderBy(p => p.FName).Take(10))
                {
                    ja.Add(string.Format("{0}", cate.FName));
                }

                context.Response.ContentType = "text/plain";
                context.Response.Write(ja.ToString());
            }
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