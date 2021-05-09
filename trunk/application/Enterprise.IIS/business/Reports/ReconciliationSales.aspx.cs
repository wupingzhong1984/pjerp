using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.Reports
{
    public partial class ReconciliationSales : PageBase
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
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ItemsService _itemsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ItemsService ItemsService
        {
            get { return _itemsService ?? (_itemsService = new ItemsService()); }
            set { _itemsService = value; }
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

                GasHelper.DropDownListCompanyDataBind(ddlCompany);

                dpkFDateBegin.SelectedDate = DateTime.Now;

                GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

                LoadTreeSource();
            }
        }

        /// <summary>
        ///     部门树加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trDept_NodeCommand(object sender, TreeCommandEventArgs e)
        {
            BindDataGrid();
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

        protected void ddlFSalesman_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFSalesman.SelectedValue != null && !ddlFSalesman.SelectedValue.Equals("-1"))
            {
                LoadTreeSource();
            }
        }

        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                
                string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                PageContext.RegisterStartupScript(string.Format("LodopPrinter('{0}');", sid));
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=客户对账单{0}.xls", SequenceGuid.GetGuidReplace()));
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
        ///     树绑定
        /// </summary>
        private void LoadTreeSource()
        {
            trDept.Nodes.Clear();

            var rootNode = new TreeNode
            {
                Text = "全部客户",
                NodeID = "-1",
                Icon = Icon.BulletBlue,
                EnableClickEvent = true,
                Expanded = true
            };

            trDept.Nodes.Add(rootNode);

            trDept.SelectedNodeID = "-1";

            //加载子部门信息
            LoadChildNodes(rootNode);
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;
        /// <summary>
        ///     数据服务
        /// </summary>
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }


        /// <summary>
        ///     绑定子节点
        /// </summary>
        private void LoadChildNodes(TreeNode treeNode)
        {
            IList<LHCustomer> list;
            if (!ddlFSalesman.SelectedValue.Equals("-1"))
            {
                list = CustomerService.Where(p => p.FFlag == 1//
                    && p.FCompanyId == CurrentUser.AccountComId//
                    && p.FSalesman == ddlFSalesman.SelectedValue)
                .ToList();
            }
            else
            {
                list = CustomerService.Where(p => p.FFlag == 1 && p.FCompanyId == CurrentUser.AccountComId)
                                .ToList();
            }

            foreach (var customer in list)
            {
                var node = new TreeNode
                {
                    Text = string.Format("{0} {1}",customer.FCode,customer.FName),
                    NodeID = customer.FCode,
                    Icon = Icon.BulletBlue,
                    EnableClickEvent = true,
                    Expanded = true
                };
                treeNode.Nodes.Add(node);
            }
        }

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
            if (trDept.SelectedNode != null)
            {
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyid", ddlCompany.SelectedValue);

                if (!string.IsNullOrEmpty(txtFName.Text.Trim()))
                {
                    parms.Add("@FCode", txtFName.Text.Trim());
                }
                else
                {
                    parms.Add("@FCode", trDept.SelectedNode.NodeID);
                }
                parms.Add("@FirstDay", MonthFirstDay(Convert.ToDateTime(dpkFDateBegin.SelectedDate)));
                parms.Add("@End", MonthEnd(Convert.ToDateTime(dpkFDateBegin.SelectedDate)));
                parms.Add("@FDate", Convert.ToDateTime(dpkFDateBegin.SelectedDate).ToString("yyyy-MM-dd"));

                var list = SqlService.ExecuteProcedureCommand("proc_ReconciliationAR", parms).Tables[0];

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
        }

        #endregion
    }
}