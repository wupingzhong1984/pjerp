using System;
using System.Linq;
using System.Web;
using IPQuery.Facade;
using Enterprise.Framework.FormsAuth;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Service.Logs;
using Enterprise.Service.Logs.SV;

namespace Enterprise.IIS.Common
{
    /// <summary>
    /// ProcessMessage1 的摘要说明
    /// </summary>
    public class Ajax : IHttpHandler
    {
        private string _operType = string.Empty;
        private string _response = string.Empty;

        /// <summary>
        /// 站内消息提醒
        /// </summary>
        private BulletinService _bulletinService;
        /// <summary>
        /// 站内消息提醒
        /// </summary>
        protected BulletinService BulletinService
        {
            get { return _bulletinService ?? (_bulletinService = new BulletinService()); }
            set { _bulletinService = value; }
        }

        public void ProcessRequest(HttpContext context)
        {
            _operType = context.Request["oper"] ?? "";
            switch (_operType)
            {
                case "ajaxGetList":
                    AjaxGetList();
                    break;

                case "ajaxGetServerTime":
                    AjaxGetServerTime();
                    break;

                case "ajaxLogin":
                    var uName = context.Request["name"];
                    var uPassword = context.Request["password"];
                    AjaxLogin(context, uName, uPassword);
                    break;
            }
            context.Response.Write(_response);
        }

        /// <summary>
        /// 用户认证
        /// </summary>
        private void AjaxLogin(HttpContext context, string xname, string xpassword)
        {
            //var msg = new StringBuilder();
            var ip = SingleQuery.Instance.Find(IpHelper.ClientIp);
            var ipAddress = string.Format("{0} {1}", new object[] {ip.Country, ip.Area});

            try
            {
                //msg.Append("000、======================>"); 
                var server = new AccountService();
                //msg.Append("001、======================>" + xpassword); ;
                //var password = EncryptUtil.Encrypt(xpassword);
                //msg.Append("002、======================>"+password);
                var accountNumber = xname.Trim();
                var account = server.Where
                    (p => (p.account_number == accountNumber || p.account_name==accountNumber)
                          && p.account_password == xpassword && p.deleteflag == 0)
                    .FirstOrDefault();
                //msg.Append("1、读用户信息");

                if (account != null)
                {
                   // msg.Append("2、用户信息验证通过");
                    if (!account.account_flag.Equals(1))
                    {
                        _response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + "请所提供的帐号已被管理员禁用，请联系管理员。" + "\"}";
                        return;
                    }

                    //执行用户登录操作
                    int urlPort = context.Request.Url.Port;
                    //执行用户登录操作
                    FormsPrincipal<base_account>.SignIn(accountNumber, //
                        new LoginUser(account.id, //
                            account.account_name, //
                            account.account_number, //
                            account.account_number,//
                            Convert.ToInt32(account.account_org_id),//
                            Convert.ToInt32(account.FCompanyId),//
                            account.account_role_id,//
                            urlPort),//
                            99999);

                    //最后一次登录时间
                    account.account_last_date = DateTime.Now;
                    account.account_ip = IpHelper.ClientIp;
                    account.account_ip_addres = ipAddress;
                    server.SaveChanges();

                  //  msg.Append("3、记录用户本次登录信息");

                    var name = string.Format("{0}->{1}({2})", account.base_orgnization.org_name, //
                        account.account_name, account.account_number);

                    //记录用户登录系统日志
                    var loginService = new LoginSevice();
                    var login = new t_logins
                    {
                        account_name = name,
                        account_ip = IpHelper.ClientIp,
                        account_address = ipAddress,
                        account_id = CurrentUser.Id,
                        action_on = DateTime.Now,
                        action_desc = "用户登录成功。"
                    };

                    loginService.Add(login);
                    //msg.Append("4、日志记录完毕");


                  //  msg.Append("5、调用友接口开始");
                    //接口
                    T6Account.Receiver = "U8";
                    T6Account.Dynamicdate = DateTime.Now.ToString("MM/dd/yyyy");
                    T6Account.Sender = "008";

                   // msg.Append("6、登录完成");
                    //context.Response.Redirect(FormsAuthentication.DefaultUrl);
                    _response = "{\"result\" :\"" + 1 + "\",\"returnval\" :\"" + "登录成功，正在转到主页..." + "\"}";
                }
                else
                _response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + "您所提供的用户名或者密码不正确，请联系管理员。" + "\"}";
            }
            catch(Exception ex)
            {
                _response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" +ex.Message +"\"}";//ex.Message
            }
        }

        /// <summary>
        /// 取服务器时间
        /// </summary>
        private void AjaxGetServerTime()
        {
            _response = "{\"result\" :\"" + 1 + "\",\"returnval\" :\"" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒") + "\"}";
        }

        /// <summary>
        /// 取提醒记录
        /// </summary>
        private void AjaxGetList()
        {
            //var bulletin = BulletinService.Where(p => p.receiver == CurrentUser.id && p.deleteflag == 0 && p.isrepeal == 0).OrderBy(p => p.precedence).FirstOrDefault();

            //if (bulletin != null)
            //{
            //    _response = "{\"result\" :\"" + 1 + "\",\"returnval\" :\"" + bulletin.title + "\"}";

            //    BulletinService.Delete(bulletin);
            //}
            //else
            //{
            //    _response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + "暂时没有消息。" + "\"}";
            //}
        }

        /// <summary>
        /// 当前用户对象
        /// </summary>
        protected LoginUser CurrentUser
        {
            get
            {
                var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
                if (formsPrincipal != null)
                {
                    return formsPrincipal.UserInfo;
                }
                return null;
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