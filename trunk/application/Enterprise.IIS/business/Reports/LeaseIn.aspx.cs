using System;
using System.Collections.Generic;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using FineUI;


namespace Enterprise.IIS.business.Reports.Lease
{
    public partial class LeaseIn : PageBase
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

                //操作员
                GasHelper.DropDownListShipperDataBind(ddlOperator);
            }
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            string keyid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();
            string ftype = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@keyid", keyid);
            parms.Add("@bill", ftype);

            var data = SqlService.ExecuteProcedureCommand("proc_StockInDetails", parms);

            if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                Grid2.DataSource = data;
                Grid2.DataBind();
            }
            else
            {
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
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
        }

        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Grid1.PageIndex = 0;
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=气瓶租金单记录{0}.xls", SequenceGuid.GetGuidReplace()));
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

            parms.Add("@companyid", CurrentUser.AccountComId);
            parms.Add("@beginDate", Convert.ToDateTime(dpFDateBegion.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@endDate", Convert.ToDateTime(dpFDateEnd.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@FCode", txtFCode.Text.Trim());
            parms.Add("@Bill", ddlBill.SelectedValue);
            parms.Add("@FItemCode", txtItemCode.Text.Trim());
            parms.Add("@createBy", ddlOperator.SelectedValue);
            parms.Add("@keyId", txtKeyId.Text.Trim());

            var list = SqlService.ExecuteProcedureCommand("rpt_StockIn", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                Grid1.DataSource = list;
                Grid1.DataBind();
            }
            else
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
            }
        }

        

        #endregion
    }
}