using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.SalesPlanAudit
{
    public partial class Index : PageBase
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

        /// <summary>
        ///     
        /// </summary>
        private PassCardService _passCardService;

        /// <summary>
        ///     
        /// </summary>
        protected PassCardService PassCardService
        {
            get { return _passCardService ?? (_passCardService = new PassCardService()); }
            set { _passCardService = value; }
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

                dpFDateBegion.SelectedDate = DateTime.Now.AddDays(-1);
                dpFDateEnd.SelectedDate = DateTime.Now.AddDays(1);

                GasHelper.DropDownListShipperDataBind(ddlOperator);

                BindDataGrid();
            }
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {

            //if (Grid1.SelectedRowIndexArray.Length == 0)
            //{
            //    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
            //}

            if (!Grid1.SelectedRowIndexArray.Any())
            {
                return;
            }

            string keyId = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@KeyId", keyId);
            parms.Add("@FCompanyId", CurrentUser.AccountComId);
            var data = SqlService.ExecuteProcedureCommand("proc_SalesAuditDetails", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
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

        protected void Grid1_OnRowDeselect(object sender, GridRowSelectEventArgs e)
        {
            //if (Grid1.SelectedRowIndexArray.Length == 0)
            //{
            //    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
            //}
            if (!Grid1.SelectedRowIndexArray.Any())
            {
                return;
            }

            string keyId = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@KeyId", keyId);
            parms.Add("@FCompanyId", CurrentUser.AccountComId);
            var data = SqlService.ExecuteProcedureCommand("proc_SalesAuditDetails", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
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
        ///     引入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            //PageContext.RegisterStartupScript(string.Format("openAddFineUI('./business/Init/InitCustomer.aspx');"));
        }

        /// <summary>
        ///     审核单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAuditYes_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                //foreach (var ids in selectIds)
                //{
                    //审核
                    //string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PassCardService.Update(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.KeyId), p => new LHPassCard
                    {
                        FAuditFlag = 1, //
                        FAuditor = CurrentUser.AccountName
                    });

                    Alert.Show("审核通过！", MessageBoxIcon.Information);

                    BindDataGrid();
                //}
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     审核单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAuditNo_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                PassCardService.Update(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.KeyId), p => new LHPassCard
                {
                    FAuditFlag = 2, //
                    FAuditor = CurrentUser.AccountName
                });

                Alert.Show("审核不通过！", MessageBoxIcon.Information);

                BindDataGrid();
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
            //Grid1.PageIndex = 0;
            BindDataGrid();
        }
        protected void btnRest_Click(object sender, EventArgs e)
        {
            ddlFFlag.SelectedValue = "-1";
            txtFCode.Text = string.Empty;
            txtFName.Text = string.Empty;
            txtItemCode.Text = string.Empty;
            txtItemName.Text = string.Empty;
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=销售订单查询{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            var item = e.DataItem;

            if (item != null)
            {
                if (((DataRowView)item).Row.ItemArray[10].ToString() == "2")
                {
                    for (int i = 0; i < e.Values.Length; i++)
                    {
                        if (!e.Values[i].ToString().Contains("#@TPL@"))
                            e.Values[i] = String.Format("<span class=\"{0}\">{1}</span>", "colorred", e.Values[i].ToString().Contains("#@TPL@") ? "审核不通过" : e.Values[i]);
                    }
                }
            }
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
            parms.Add("@FName", txtFName.Text.Trim());
            parms.Add("@FItemCode", txtItemCode.Text.Trim());
            parms.Add("@FItemName", txtItemName.Text.Trim());
            parms.Add("@FDriver", ddlFDriver.SelectedValue);//司机
            parms.Add("@KeyId", txtKeyId.Text.Trim());
            parms.Add("@FShipper", ddlFShipper.SelectedValue);//发货人
            parms.Add("@FQty", txtFQty.Text.Trim());
            parms.Add("@FAuditFlag", ddlFFlag.SelectedValue);


            var list = SqlService.ExecuteProcedureCommand("proc_SalesAudit", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                Grid1.DataSource = list;
                Grid1.DataBind();

                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        {"FAmount", list.Compute("sum(FAmount)","true").ToString()},
                        {"FFreight", list.Compute("sum(FFreight)","true").ToString()},
                    };

                Grid1.SummaryData = summary;
            }
            else
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
                var summary = new JObject
                    {
                       {"FCode", "合计"},
                        {"FAmount", "0"},
                        {"FFreight", "0"},
                    };

                Grid1.SummaryData = summary;
            }
        }

        #endregion
    }
}