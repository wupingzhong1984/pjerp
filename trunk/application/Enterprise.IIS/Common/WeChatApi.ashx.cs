using Enterprise.Data;
using Enterprise.DataAccess.SQLServer;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enterprise.IIS.Common
{
    /// <summary>
    /// WeChatApi 的摘要说明
    /// </summary>
    public class WeChatApi : PageBase, IHttpHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string apitype = context.Request["oper"];
            WeReturnModel model = new WeReturnModel();
            switch (apitype)
            {
                case "ajaxLogin":
                    Login(context, model);
                    break;
                case "mainList":
                    mainList(context, model);
                    break;
                case "TaskList":
                    TaskList(context, model);
                    break;
                case "TaskDetail":
                    TaskDetail(context, model);
                    break;
                case "BottleDetail":
                    BottleDetail(context, model);
                    break;
                case "ZeroList":
                    break;
                default:
                    break;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            context.Response.Write(JsonConvert.SerializeObject(model,settings));
        }

        public void Login(HttpContext context, WeReturnModel model)
        {
            string name = context.Request["name"];
            string password = context.Request["password"];
            try
            {
                AccountService server = new AccountService();
                base_account _Account = server.Where(p => p.account_number == name && p.account_password == password).FirstOrDefault();
                if (_Account!=null)
                {
                    model.status = 0;
                    model.data = _Account;
                    model.msg = "登录成功";
                }
                else
                {
                    model.status = 0;
                    model.data = null;
                    model.msg = "用户名或密码错误"+ name+"----"+password;
                }
                
            }
            catch (System.Exception ex)
            {
                model.status = 1;
                model.errmsg = ex.Message;
                model.msg = "登录失败";
            }
        }
        /// <summary>
        /// 运送列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        public void mainList(HttpContext context, WeReturnModel model)
        {
            string fdate = context.Request["fdate"];
            string person = context.Request["person"];
            try
            {
                var starttime = DateTime.Parse(fdate + " 00:00:00");
                IQueryable<LHDispatchCenter> dispatchCenters = new DispatchCenterService().Where(p => p.FDate== starttime && (p.FDriver.Contains( person) || p.FSupercargo.Contains(person)));
                model.status = 0;
                model.data = dispatchCenters.ToList();
                model.msg = "登录成功";
            }
            catch (System.Exception ex)
            {
                model.status = 1;
                model.errmsg = ex.Message;
                model.msg = "登录失败";
            }
            

        }


        /// <summary>
        /// 运送列表详细 销售客户资料
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        public void TaskList(HttpContext context, WeReturnModel model)
        {
            string id = context.Request["id"];
            string AccountComId = context.Request["AccountComId"];
            try
            {
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyid", id);
                parms.Add("@FCompanyId", AccountComId);

                var data = SqlService.ExecuteProcedureCommand("proc_DispatchDetails", parms).Tables[0];
                model.status = 0;
                model.data = data;
                model.msg = "成功";
            }
            catch (System.Exception ex)
            {
                model.status = 1;
                model.errmsg = ex.Message;
                model.msg = "失败";
            }


        }

        /// <summary>
        /// 运送列表详细 气体及数量规格
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        public void TaskDetail(HttpContext context, WeReturnModel model)
        {
            string id = context.Request["id"];
            string AccountComId = context.Request["AccountComId"];
            try
            {
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyid", id);
                parms.Add("@companyId", AccountComId);

                var data = SqlService.ExecuteProcedureCommand("proc_DispatchCenterDetails", parms).Tables[0];
                model.status = 0;
                model.data = data;
                model.msg = "成功";
            }
            catch (System.Exception ex)
            {
                model.status = 1;
                model.errmsg = ex.Message;
                model.msg = "失败";
            }


        }

        /// <summary>
        /// 运送列表详细 气体及数量规格
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        public void BottleDetail(HttpContext context, WeReturnModel model)
        {
            string id = context.Request["id"];
            string AccountComId = context.Request["AccountComId"];
            try
            {
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyid", id);
                parms.Add("@companyId", AccountComId);

                var data = SqlService.ExecuteProcedureCommand("rpt_SalesBottles", parms).Tables[0];
                model.status = 0;
                model.data = data;
                model.msg = "成功";
            }
            catch (System.Exception ex)
            {
                model.status = 1;
                model.errmsg = ex.Message;
                model.msg = "失败";
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

    public class WeReturnModel
    {
        public string errmsg { get; set; }
        public string msg { get; set; }

        public object data { get; set; }

        public int status { get; set; }

    }
}