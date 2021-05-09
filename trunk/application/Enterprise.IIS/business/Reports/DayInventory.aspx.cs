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
    public partial class DayInventory : PageBase
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

                GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区
            }
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindDataGrid();
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Grid1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            //BindDataGrid();
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
                //if (Grid1.SelectedRowIndexArray.Length == 0)
                //{
                //    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                //}
                //else if (Grid1.SelectedRowIndexArray.Length > 1)
                //{
                //    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                //}
                //else
                //{
                //    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                //    if (e.CommandName == "ViewFGInQty")
                //    {
                //        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}','{1}','{2}');" //
                //            , Convert.ToDateTime(dpFDate.SelectedDate).ToString("yyyy-MM-dd")
                //            , "ViewFGInQty"
                //            , sid));
                //    }
                //    else if (e.CommandName == "ViewFBInQty")
                //    {
                //        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}','{1}','{2}');" //
                //            , Convert.ToDateTime(dpFDate.SelectedDate).ToString("yyyy-MM-dd")
                //            , "ViewFBInQty"
                //            , sid));
                //    }
                //    else if (e.CommandName == "ViewFGOutQty")
                //    {
                //        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}','{1}','{2}');" //
                //            , Convert.ToDateTime(dpFDate.SelectedDate).ToString("yyyy-MM-dd")
                //            , "ViewFGOutQty"
                //            , sid));
                //    }
                //    else
                //    {
                //        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}','{1}','{2}');" //
                //            , Convert.ToDateTime(dpFDate.SelectedDate).ToString("yyyy-MM-dd")
                //            , "ViewFBOutQty"
                //            , sid));
                //    }
                //}
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }

        }

        /// <summary>
        ///     作废
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     复制单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     审核单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAudit_Click(object sender, EventArgs e)
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
        ///     详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, EventArgs e)
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=当日气体气瓶库存表{0}.xls", SequenceGuid.GetGuidReplace()));
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

            //验证输入内容是否合法



            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FDate", Convert.ToDateTime(dpFDate.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@FPoint", ddlFDistributionPoint.SelectedValue);
            parms.Add("@FBottle", txtFName.Text.Trim());

            var data = SqlService.ExecuteProcedureCommand("rpt_Invent", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = data;
                Grid1.DataBind();

                var summary = new JObject
                    {
                        {"FName", "合计"},
                        //{"FInitTotal", data.Compute("sum(FInitTotal)","true").ToString()},
                        {"FDayInitKQty", data.Compute("sum(FDayInitKQty)","true").ToString()},
                        {"FDayInitSQty", data.Compute("sum(FDayInitSQty)","true").ToString()},

                        //{"FInTotal", data.Compute("sum(FInTotal)","true").ToString()},
                        {"DayInSQty", data.Compute("sum(DayInSQty)","true").ToString()},
                        {"DayInKQty", data.Compute("sum(DayInKQty)","true").ToString()},

                        //{"FOutTotal", data.Compute("sum(FOutTotal)","true").ToString()},
                        {"DayOutSQty", data.Compute("sum(DayOutSQty)","true").ToString()},
                        {"DayOutKQty", data.Compute("sum(DayOutKQty)","true").ToString()},

                        //{"FEndTotal", data.Compute("sum(FEndTotal)","true").ToString()},
                        {"EndSQty", data.Compute("sum(EndSQty)","true").ToString()},
                        {"EndKQty", data.Compute("sum(EndKQty)","true").ToString()},

                    };

                Grid1.SummaryData = summary;

            }
        }

        #endregion
    }
}