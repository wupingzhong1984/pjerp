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
    public partial class Analyse : PageBase
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

                GasHelper.DropDownListGroupDataBind(ddlFGroup);//班组

                //GasHelper.DropDownListProducerDataBind(ddlFProducer);

                //GasHelper.DropDownListWorkshopDataBind(ddlWorkShop);

                ////产品
                //tbxFName.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFName.ClientID)
                //        + Window2.GetShowReference("../../Common/WinProducReference.aspx");
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
        }

        protected void tbxFName_OnTextChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(tbxFName.Text.Trim()))
            //{
            //    var bottle = GasHelper.BottleByCode(txtFCode.Text, CurrentUser.AccountComId);

            //    if (bottle != null)
            //        tbxFBottle.SelectedValue = bottle.FBottleCode;
            //}
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=分析计件{0}.xls", SequenceGuid.GetGuidReplace()));
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

            parms.Add("@companyid", Convert.ToInt32(ddlCompany.SelectedValue));
            parms.Add("@beginDate", Convert.ToDateTime(dpkFDateBegin.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@endDate", Convert.ToDateTime(dpkFDateEnd.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@group", ddlFGroup.SelectedValue);
            //parms.Add("@WorkShop", ddlWorkShop.SelectedValue);
            //parms.Add("@FItemCode",txtFCode.Text.Trim());

            var list = SqlService.ExecuteProcedureCommand("proc_Analyse", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = list;
                Grid1.DataBind();

                //合计
                var summary = new JObject
                    {
                        {"KeyId", "合计"},
                        {"FQty", list.Compute("sum(FQty)","true").ToString()},
                        {"FAmount", list.Compute("sum(FAmount)","true").ToString()},
                    };

                Grid1.SummaryData = summary;
            }
            else
            {
                Grid1.DataSource = null;
                //合计
                var summary = new JObject
                    {
                        {"KeyId", "合计"},
                        {"FQty", "0.00"},
                        {"FAmount", "0.00"},
                    };

                Grid1.SummaryData = summary;
            }
        }

        #endregion
    }
}