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
    public partial class MonthStockBottle : PageBase
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=气体当日结存表{0}.xls", SequenceGuid.GetGuidReplace()));
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
            var list = SqlService.ExecuteProcedureCommand("proc_RealtimeInventoryBottle", parms).Tables[0];

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
                        {"FPurchaseIQty", list.Compute("sum(FPurchaseIQty)","true").ToString()},
                        {"FPurchaseOQty", list.Compute("sum(FPurchaseOQty)","true").ToString()},
                        {"FIQty", list.Compute("sum(FIQty)","true").ToString()},
                        {"FOQty", list.Compute("sum(FOQty)","true").ToString()},
                        {"FSalesOQty", list.Compute("sum(FSalesOQty)","true").ToString()},
                        {"FSalesIQty", list.Compute("sum(FSalesIQty)","true").ToString()},
                        {"FAllotOQty", list.Compute("sum(FAllotOQty)","true").ToString()},
                        {"FAllotIQty", list.Compute("sum(FAllotIQty)","true").ToString()},
                        {"FProfitQty", list.Compute("sum(FProfitQty)","true").ToString()},
                        {"FLossessQty", list.Compute("sum(FLossessQty)","true").ToString()},
                        {"FSumIQty", list.Compute("sum(FSumIQty)","true").ToString()},
                        {"FSumOQty", list.Compute("sum(FSumOQty)","true").ToString()},
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
                        {"FPurchaseIQty","0.00"},
                        {"FPurchaseOQty", "0.00"},
                        {"FIQty", "0.00"},
                        {"FOQty", "0.00"},
                        {"FSalesOQty", "0.00"},
                        {"FSalesIQty", "0.00"},
                        {"FAllotOQty", "0.00"},
                        {"FAllotIQty", "0.00"},
                        {"FProfitQty", "0.00"},
                        {"FLossessQty", "0.00"},
                        {"FSumIQty", "0.00"},
                        {"FSumOQty", "0.00"},
                        {"FEndQty", "0.00"},
                    };

                Grid1.SummaryData = summary;
            }
        }
        #endregion
    }
}