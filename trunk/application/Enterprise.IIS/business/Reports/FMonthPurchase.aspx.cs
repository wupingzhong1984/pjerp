using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.Reports
{
    public partial class FMonthPurchase : PageBase
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

                dpFDateBegion.SelectedDate = DateTime.Now;
                dpFDateEnd.SelectedDate = DateTime.Now;

                //GasHelper.DropDownListShipperDataBind(ddlOperator);
            }
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {

            //if (Grid1.SelectedRowIndexArray.Length == 0)
            //{
            //    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
            //}

            //string keyid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

            //var parms = new Dictionary<string, object>();
            //parms.Clear();

            //parms.Add("@keyid",keyid);
            //parms.Add("@companyid", CurrentUser.AccountComId);
            //var data = SqlService.ExecuteProcedureCommand("rpt_StockOutDetails", parms) .Tables[0];

            //if (data != null && data.Rows.Count > 0)
            //{
            //    Grid2.DataSource = data;
            //    Grid2.DataBind();
            //}
            //else
            //{
            //    Grid2.DataSource = null;
            //    Grid2.DataBind();
            //}
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     禁用/启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnabled_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     引入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(string.Format("openAddFineUI('./business/Init/InitCustomer.aspx');"));
        }

        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Grid1.PageIndex = 0;
            BindDataGrid();
        }
        protected void btnRest_Click(object sender, EventArgs e)
        {
            //ddlBill.SelectedValue = "-1";
            txtFCode.Text = string.Empty;
            //txtFName.Text = string.Empty;
            //txtItemCode.Text = string.Empty;
            //txtItemName.Text = string.Empty;
            txtKeyId.Text = string.Empty;
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=调度汇总表{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var selectIds = new string[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Grid1.DataKeys[selections[i]][0].ToString();
            }
            return selectIds;
        }

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", CurrentUser.AccountComId);
            parms.Add("@FBegin", Convert.ToDateTime(dpFDateBegion.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@FEnd", Convert.ToDateTime(dpFDateEnd.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@FCode", txtFCode.Text.Trim());
            parms.Add("@FItemCode", txtItemCode.Text.Trim());
            parms.Add("@KeyId", txtKeyId.Text.Trim());
            //

            var list = SqlService.ExecuteProcedureCommand("rpt_FMonthPurchase", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                Grid1.DataSource = list;
                Grid1.DataBind();
                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        //{"FQty", list.Compute("sum(FQty)","true").ToString()},
                        {"FAmount", list.Compute("sum(FAmount)","true").ToString()},
                        {"FBottleQty", list.Compute("sum(FBottleQty)","true").ToString()},
                        {"FRecycleQty", list.Compute("sum(FRecycleQty)","true").ToString()},
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
                        {"FAmount", "0.00"},
                        {"FBottleQty", "0.00"},
                        {"FRecycleQty", "0.00"},
                    };

                Grid1.SummaryData = summary;
            }
        }

        #endregion
    }
}