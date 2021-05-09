using System;
using System.Collections.Generic;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.Reports.Vehicle
{
    public partial class DispatchDay : PageBase
    {
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "KeyId";
                return sort;
            }
            set { ViewState["SortField"] = value; }
        }

        /// <summary>
        ///     排序方向
        /// </summary>
        protected string SortDirection
        {
            get
            {
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "DESC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }

        #region Protected Method

        /// <summary>
        ///     页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

                dpkFDate.SelectedDate = DateTime.Today;

                tbxFCustomer.OnClientTriggerClick = Window1.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window1.GetShowReference("../../Common/WinUnit.aspx");

                GasHelper.DropDownListCompanyDataBind(ddlCompany);
            }
        }
        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                Window1.Hidden = true;
                Window2.Hidden = true;
            }
        }

        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SortField = string.Format(@"{0}", e.SortField);
            SortDirection = e.SortDirection;
            BindDataGrid();
        }

        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDataGrid();
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=客户每月应收款统计{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }
        #endregion

        #region Private Method

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@begin", MonthFirstDay(Convert.ToDateTime(dpkFDate.SelectedDate)));
            parms.Add("@end", MonthEnd(Convert.ToDateTime(dpkFDate.SelectedDate)));
            parms.Add("@FCode", txtFCode.Text);
            parms.Add("@FBottle", "");

            parms.Add("@companyid", ddlCompany.SelectedValue);


            var list = SqlService.ExecuteProcedureCommand("proc_UnitAllBottle", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = list;
                Grid1.DataBind();

                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        //{"FInitAR", list.Compute("sum(FInitAR)","true").ToString()},
                        //{"FPeriodAR", list.Compute("sum(FPeriodAR)","true").ToString()},
                        //{"FPeriodReturn", list.Compute("sum(FPeriodReturn)","true").ToString()},
                        //{"FPeriodRecover", list.Compute("sum(FPeriodRecover)","true").ToString()},
                        {"FBottleQty", list.Compute("sum(FBottleQty)","true").ToString()},

                    };

                Grid1.SummaryData = summary;
            }
            else
            {
                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        {"FInitAR", "0.00"},
                        {"FPeriodAR", "0.00"},
                        {"FPeriodReturn", "0.00"},
                        {"FPeriodRecover", "0.00"},
                        {"FFinalAR", "0.00"},
                    };

                Grid1.SummaryData = summary;
            }
        }

        #endregion
    }
}