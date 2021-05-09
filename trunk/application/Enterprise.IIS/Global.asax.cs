using System;
using System.Web;
using System.Text;
using Enterprise.IIS.Common;
using IPQuery.Facade;
using Enterprise.Service.Logs;
using Enterprise.Service.Logs.SV;
using Enterprise.Framework.Log;
using Enterprise.Framework.Web;
using System.Timers;
using Enterprise.Framework.Log;

namespace Enterprise.IIS
{

    /// <summary>
    /// 
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            var ipAddress = Server.MapPath(@"~\App_Data\ip.dat");
            SingleQuery.Instance.SetDataFilePath(ipAddress);

            //计划任务调度
            var task = new Tasks();
            var timer = new Timer();
            timer.Elapsed += task.TimeEvent;
            timer.Interval = 3000;
            timer.Enabled = true;
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
            Application.Lock();
            Application["count"] = Convert.ToInt32(Application["count"]) + 1;//用于统计网站在线人数
            Application.UnLock();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //在出现未处理的错误时运行的代码
            var context = HttpContext.Current;
            if (context != null)
            {
                Exception ex = HttpContext.Current.Server.GetLastError();
                LogUtil.ErrorLog("\r\n错误开始===========================================" //
                    + "\r\n客户机IP:" //
                    + Request.UserHostAddress //
                    + "\r\n错误地址:" //
                    + Request.Url //
                    + "\r\n异常信息:" //
                    + Server.GetLastError().Message //
                    + "\r\n错误结束===========================================", ex);

                //当在派生类中重写时，返回 System.Exception，它是一个或多个并发的异常的根源。
                LogUtil.ErrorLog(ex.GetBaseException());

                //记录用户登录系统日志
                var errorService = new ErrorsService();
                var errors = new t_errors
                {
                    account_name = context.User.Identity.Name,
                    account_ip = Request.UserHostAddress,
                    account_id = 0,
                    action_on = DateTime.Now,
                    action_desc = Server.GetLastError().Message
                };

                errorService.Add(errors);

                if (ex.Message.Equals("系统授权已到期，请贵公司及时续签维护合同，联系河南禄恒软件科技有限公司，联系人李彦江（18768868380），谢谢您的合作。"))
                {
                    context.Response.Redirect("~/login.html");
                    return;
                }

                //var sb = new StringBuilder();
                //sb.Append("<b>系统出现如下错误：</b><br/><br/>");
                //sb.Append("<b>发生时间：</b>&nbsp;&nbsp;" + DateTime.Now + "<br/><br/>");
                //sb.Append("<b>错误描述：</b>&nbsp;&nbsp;" + ex.Message.Replace("\r\n", "") + "<br/><br/>");
                //sb.Append("<b>错误对象：</b>&nbsp;&nbsp;" + ex.Source + "<br/><br/>");
                //sb.Append("<b>错误页面：</b>&nbsp;&nbsp;" + HttpContext.Current.Request.Url + "<br/><br/>");
                //sb.Append("<b>浏览器IE：</b>&nbsp;&nbsp;" + HttpContext.Current.Request.UserAgent + "<br/><br/>");
                //sb.Append("<b>服务器IP：</b>&nbsp;&nbsp;" + HttpContext.Current.Request.ServerVariables.Get("Local_Addr") + "<br/><br/>");
                //sb.Append("<b>方法名称：</b>&nbsp;&nbsp;" + ex.TargetSite + "<br/><br/>");
                //sb.Append("<b>C#类名称：</b>&nbsp;&nbsp;" + ex.TargetSite.DeclaringType + "<br/><br/>");
                //sb.Append("<b>成员变量：</b>&nbsp;&nbsp;" + ex.TargetSite.MemberType + "<br/><br/>");

                //Cookie.SetObj("Http_Errors", sb.ToString());
                //Server.ClearError();

                ////出错画面处理
                //context.Response.Redirect("~/SystemError.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
            // 或 SQLServer，则不会引发该事件。
            Application.Lock();
            Application["count"] = Convert.ToInt32(Application["count"]) - 1;//用于统计网站在线人数
            Application.UnLock();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}