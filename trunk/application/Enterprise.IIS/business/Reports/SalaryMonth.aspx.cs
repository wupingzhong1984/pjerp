using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.Reports
{
    public partial class SalaryMonth : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }


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

                GasHelper.DropDownListCompanyDataBind(ddlCompany);
                GasHelper.DropDownListAuditorsDataBind(ddlSeller);
                tbxFCustomer.OnClientTriggerClick = Window1.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window1.GetShowReference("../../Common/WinCustomer.aspx");

            }
        }
        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                var custmoer = CustomerService.Where(p => p.FName == tbxFCustomer.Text.Trim() //
                    && p.FFlag == 1
                    && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFCode.Text = custmoer.FCode;

                    BindDataGrid();

                }
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();

                    if (e.CommandName == "actView")
                    {
                        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}','{1}','{2}');", sid,CurrentUser.AccountComId,dpkFDateBegin.SelectedDate.Value.ToString("yyyy-MM-dd")));
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("操作失败！", MessageBoxIcon.Warning);
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=销售月报{0}.xls", SequenceGuid.GetGuidReplace()));
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
            parms.Add("@FCode", string.IsNullOrWhiteSpace(txtFCode.Text.Trim()) ? "-1" : txtFCode.Text.Trim());
            parms.Add("@FirstDay", MonthFirstDay(Convert.ToDateTime(dpkFDateBegin.SelectedDate)));
            parms.Add("@EndDay", MonthEnd(Convert.ToDateTime(dpkFDateBegin.SelectedDate)));
            parms.Add("@FDate", Convert.ToDateTime(dpkFDateBegin.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@Seller", ddlSeller.SelectedText);

            var list = SqlService.ExecuteProcedureCommand("rpt_SalaryMonth", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = list;
                Grid1.DataBind();

                //合计
                var summary = new JObject
                    {
                        {"FCode", "合计"},
                        {"FInit", list.Compute("sum(FInit)","true").ToString()},
                        {"FSales", list.Compute("sum(FSales)","true").ToString()},
                        {"FReturned", list.Compute("sum(FReturned)","true").ToString()},
                        {"FReturn", list.Compute("sum(FReturn)","true").ToString()},
                        {"FEnd", list.Compute("sum(FEnd)","true").ToString()},
                        {"FDiscountAmount", list.Compute("sum(FDiscountAmount)","true").ToString()},

                        
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
                        {"FInit", "0.00"},
                        {"FSales", "0.00"},
                        {"FReturned", "0.00"},
                        {"FReturn",  "0.00"},
                        {"FEnd",  "0.00"},
                        {"FDiscountAmount",  "0.00"},
                    };

                Grid1.SummaryData = summary;
            }
        }

        #endregion

        protected void btnResase_Click(object sender, EventArgs e)
        {
            txtFCode.Text = string.Empty;
            tbxFCustomer.Text = string.Empty;
        }
    }
}