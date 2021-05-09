using System.Web;
using Enterprise.DataAccess.SQLServer;

namespace Enterprise.IIS.Common
{
    /// <summary>
    /// AjaxReport 的摘要说明
    /// </summary>
    public class AjaxReport : IHttpHandler
    {

        /// <summary>
        ///     数据服务
        /// </summary>
        private SqlService _sqlService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SqlService SqlService
        {
            get { return _sqlService ?? (_sqlService = new SqlService()); }
            set { _sqlService = value; }
        }

        private string _operType = string.Empty;
        private string _response = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            _operType = context.Request["oper"] ?? "";
            switch (_operType)
            {
                case "AjaxSalaryMonth"://发货单
                    AjaxSalaryMonth(context);
                    break;

                
                    break;
            }
            context.Response.Write(_response);
        }

        private void AjaxSalaryMonth(HttpContext context)
        {
            //JObject jo = new JObject();

            //JArray jaFields = JArray.Parse("[\"FCode\",\"FName\",{\"FInit\":\"FSales\",\"FReturned\":\"FReturn\"};
            //jo.Add("fields", jaFields);

            //JArray ja = new JArray();
            //for (int i = 0; i < total; i++)
            //{
                

            //    JArray jaItem = new JArray();
            //    jaItem.Add(i + 1);
                

            //    ja.Add(jaItem);
            //}

            //jo.Add("data", ja);


            //return jo.ToString(Newtonsoft.Json.Formatting.None);
        }



        //private string GetLargeData(int total)
        //{
        //    JArray ja = new JArray();

        //    Random rd = new Random();
        //    for (int i = 0; i < total; i++)
        //    {
        //        int entranceYear = rd.Next(2000, 2016);
        //        bool atSchool = false;
        //        if (entranceYear >= 2008)
        //        {
        //            atSchool = true;
        //        }
        //        int rdUserIndex = rd.Next(0, USER_NAMES.Length);
        //        if (rdUserIndex % 2 == 1)
        //        {
        //            rdUserIndex--;
        //        }
        //        string userName = USER_NAMES[rdUserIndex + 1];
        //        int gender = USER_NAMES[rdUserIndex] == "男" ? 1 : 0;

        //        JObject jo = new JObject();
        //        jo.Add("Id", i + 1);
        //        jo.Add("Name", userName);
        //        jo.Add("Gender", gender);
        //        jo.Add("EntranceYear", entranceYear);
        //        jo.Add("AtSchool", atSchool ? 1 : 0);
        //        jo.Add("Major", MAJORS[rd.Next(0, MAJORS.Length)]);
        //        jo.Add("Group", rd.Next(1, 6));

        //        ja.Add(jo);
        //    }


        //    return ja.ToString(Newtonsoft.Json.Formatting.None);
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}