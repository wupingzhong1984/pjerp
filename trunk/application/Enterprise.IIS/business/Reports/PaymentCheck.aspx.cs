using System;
using System.Collections.Generic;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;


namespace Enterprise.IIS.business.Reports
{
    public partial class PaymentCheck : PageBase
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
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
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

                dpkFDateBegin.SelectedDate = DateTime.Now;

                dpkFDateEnd.SelectedDate = DateTime.Now;

                GasHelper.DropDownListCompanyDataBind(ddlCompany);
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
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    if (e.CommandName == "actView")
                    {
                        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}','{1}','{2}');" //
                            , Convert.ToDateTime(dpkFDateBegin.SelectedDate).ToString("yyyy-MM-dd")
                            , Convert.ToDateTime(dpkFDateEnd.SelectedDate).ToString("yyyy-MM-dd")
                            , sid));
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=财务账户核销汇总表{0}.xls", SequenceGuid.GetGuidReplace()));
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

            parms.Add("@companyid", ddlCompany.SelectedValue);
            parms.Add("@beginDate", Convert.ToDateTime(dpkFDateBegin.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@endDate", Convert.ToDateTime(dpkFDateEnd.SelectedDate).ToString("yyyy-MM-dd"));

            var list = SqlService.ExecuteProcedureCommand("rpt_PaymentCheck", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = list;
                Grid1.DataBind();

                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        {"FAmount", list.Compute("sum(FAmount)","true").ToString()}
                    };

                Grid1.SummaryData = summary;
            }
            else
            {
                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        {"FAmount", "0.00"}
                    };

                Grid1.SummaryData = summary;
            }
        }

        #endregion
    }
}