using System;
using System.Collections.Generic;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.Reports
{
    public partial class RealtimeExpensesBottle : PageBase
    {
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "FCode";
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

                dpFDate.SelectedDate = DateTime.Now;

                GasHelper.DropDownListCompanyDataBind(ddlCompany);

                GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=中转站结存表{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
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
                    PageContext.RegisterStartupScript(//
                        string.Format("openDetailsUI('{0}','{1}','{2}','{3}');",//
                        sid, //
                        CurrentUser.AccountComId, //
                        dpFDate.SelectedDate.Value.ToString("yyyy-MM-dd"),//
                        ddlFDistributionPoint.SelectedValue));
                }
            }
            catch (Exception)
            {
                Alert.Show("操作失败！", MessageBoxIcon.Warning);
            }
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
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

            parms.Add("@FDate", Convert.ToDateTime(dpFDate.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@FCompanyId", ddlCompany.SelectedValue);
            parms.Add("@FPoint", ddlFDistributionPoint.SelectedValue);
            var list = SqlService.ExecuteProcedureCommand("proc_RealtimeExpensesBottle", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = list;
                Grid1.DataBind();

                //合计
                var summary = new JObject
                    {
                        {"FName", "合计"},
                        {"FInit", list.Compute("sum(FInit)","true").ToString()},
                        {"FIQty", list.Compute("sum(FIQty)","true").ToString()},
                        {"FOQty", list.Compute("sum(FOQty)","true").ToString()},
                        {"FEndQty", list.Compute("sum(FEndQty)","true").ToString()},
                    };

                Grid1.SummaryData = summary;
            }
            else
            {
                //合计
                var summary = new JObject
                    {
                        {"FName", "合计"},
                        {"FInit", "0.00"},
                        {"FIQty", "0.00"},
                        {"FOQty", "0.00"},
                        {"FEndQty", "0.00"},
                    };

                Grid1.SummaryData = summary;
            }
        }
        #endregion
    }
}