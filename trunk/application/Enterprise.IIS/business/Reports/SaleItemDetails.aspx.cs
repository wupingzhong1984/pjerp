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
    public partial class SaleItemDetails : PageBase
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
        protected string FBDate
        {
            get { return Request["fBDate"]; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string FEDate
        {
            get { return Request["fEDate"]; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string FSeller
        {
            get { return Request["fSeller"]; }
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
            parms.Add("@ItemCode", FCode);
            parms.Add("@begindate", FBDate);
            parms.Add("@enddate", FEDate);
            if (!string.IsNullOrEmpty(FSeller))
            {
                parms.Add("@Seller", FSeller);
            }
            var list = SqlService.ExecuteProcedureCommand("rpt_SaleItemDetail", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = list;
                Grid1.DataBind();

                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        {"fQty", list.Compute("sum(fQty)","true").ToString()},
                        {"fAmount", list.Compute("sum(fAmount)","true").ToString()},                        
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
                        {"fQty", "0"},
                        {"fAmount", "0.00"},   
                    };

                Grid1.SummaryData = summary;
            }
        }
    }
}