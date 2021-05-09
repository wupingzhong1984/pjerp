using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.Reports
{
    public partial class SalaryMonthDetails : PageBase
    {

        /// <summary>
        ///     FCode
        /// </summary>
        protected string FCode
        {
            get { return Request["FCode"]; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string FMonth
        {
            get { return Request["fmonth"]; }
        }
        /// <summary>
        ///     FCode
        /// </summary>
        protected string Fcompanyid
        {
            get { return Request["fcompanyid"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@companyid", Fcompanyid);
            parms.Add("@FCustomerCode", FCode);
            parms.Add("@FirstDay", MonthFirstDay(Convert.ToDateTime(FMonth)));
            parms.Add("@EndDay", MonthEnd(Convert.ToDateTime(FMonth)));
            parms.Add("@FDate", Convert.ToDateTime(FMonth).ToString("yyyy-MM-dd"));


            var list = SqlService.ExecuteProcedureCommand("rpt_SalaryMonthDetail", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = list;
                Grid1.DataBind();

                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        {"FInit", list.Compute("sum(FInit)","true").ToString()},
                        {"FSales", list.Compute("sum(FSales)","true").ToString()},
                        {"FReturned", list.Compute("sum(FReturned)","true").ToString()},
                        {"FReturn", list.Compute("sum(FReturn)","true").ToString()},
                        {"FEnd", list.Compute("sum(FEnd)","true").ToString()},
                        {"FDiscountAmount", list.Compute("sum(FDiscountAmount)","true").ToString()},

                        
                    };

                Grid1.SummaryData = summary;
            }
            else
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        {"FInit", "0.00"},
                        {"FSales", "0.00"},
                        {"FReturned", "0.00"},
                        {"FReturn",  "0.00"},
                        {"FEnd",  "0.00"},
                        {"FDiscountAmount",  "0.00"},
                    };

                Grid1.SummaryData = summary;
            }
        }
    }
}