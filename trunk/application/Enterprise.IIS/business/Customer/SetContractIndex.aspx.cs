using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.Customer
{
    public partial class SetContractIndex : PageBase
    {
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "FID";
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

       

        private ContractServie _contractService;

        protected ContractServie ContractService
        {
            get { return _contractService; }
            set { _contractService = value; }
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
                //dpkFDateBegin.SelectedDate = DateTime.Now;
                //this.dpkFDateEnd.SelectedDate = DateTime.Now;
                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./SetContractEdit.aspx?action=1"), "设置客户对应产品销售价");
                //btnView.OnClientClick = Window2.GetShowReference(string.Format("./SetContractIndex.aspx?action=1"), "查看客户合同");
                btnBatchDelete.ConfirmText = "你确定要执行删除操作吗？";

                BindDataGrid();
            }
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            //Grid1.PageIndex = e.NewPageIndex;
            //BindDataGrid();
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
        ///     删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                Log(string.Format(@"删除帐号记录ID:{0}成功。", selectIds));
                ContractService.Delete(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.FContractCode));
                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndex][0].ToString();
                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("./SetContractEdit.aspx?FID={0}&action=2",
                            sid), "设置合同"));
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
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
         

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            
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
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(txtFName.Text.Trim()))
            {
                strWhere += " and c.FName like '%" + txtFName.Text.Trim() + "%'";
            }
            if (!string.IsNullOrWhiteSpace(this.txtFItemName.Text.Trim()))
            {
                strWhere += " and d.FName like '%" + txtFItemName.Text.Trim() + "%'";
            }
            if (!string.IsNullOrWhiteSpace(this.txtminPrice.Text.Trim()))
            {
                strWhere += " and b.FPrice >=" + this.txtminPrice.Text.Trim() ;
            }
            if (!string.IsNullOrWhiteSpace(this.txtMaxPrice.Text.Trim()))
            {
                strWhere += " and b.FPrice <=" + this.txtMaxPrice.Text.Trim();
            }
            //strWhere += " and a.FCreatedon>='"++"'";
            //parms.Add("@Customer", txtFName.Text.Trim());            
            ////parms.Add("@beginDate", Convert.ToDateTime(dpkFDateBegin.SelectedDate).ToString("yyyy-MM-dd 00:00:00"));
            ////parms.Add("@endDate", Convert.ToDateTime(dpkFDateEnd.SelectedDate).ToString("yyyy-MM-dd 23:59:59"));
            //parms.Add("@ProductName", this.txtFItemName.Text.Trim());
            //parms.Add("@minPrice", this.txtminPrice.Text.Trim());
            parms.Add("@strSQL", strWhere);
            var list = SqlService.ExecuteProcedureCommand("proc_GetContractList", parms).Tables[0];
            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();

        }

      

        #endregion

        
        private void BindPrice()
        {
            if (Grid1.SelectedRowIndex < 0)
            {
                return;
            }
            string strWhere = " and FCompanyId="+CurrentUser.AccountComId;
            string strCode = Grid1.DataKeys[Grid1.SelectedRowIndex][1].ToString();
            if (!string.IsNullOrEmpty(strCode))
            {
                strWhere += " and FOrderCode='" + strCode + "'";
            }
            var date = SqlService.Where(string.Format("select * from vm_contractList where 1=1 " + strWhere));
            Grid2.DataSource = date;
            Grid2.DataBind();
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            BindPrice();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {

        }

        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndex < 0)
            {
                return;
            }
            string strWhere = string.Empty;
            string strCode = Grid1.DataKeys[Grid1.SelectedRowIndex][1].ToString();
            var parms = new Dictionary<string, object>();
            parms.Clear();
            parms.Add("@orderCode", strCode);
            parms.Add("@companyid", CurrentUser.AccountComId);
            if(SqlService.ExecuteProcedureNonQuery("proc_ContractSubmit", parms)>0)
            {
                Alert.Show("提交成功");
                BindDataGrid();
            }
        }   
    }
}