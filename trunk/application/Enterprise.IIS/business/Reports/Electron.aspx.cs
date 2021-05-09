using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public partial class Electron : PageBase
    {
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "FName";
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

                dpkFDateBegin.SelectedDate = DateTime.Now;

                //dpkFDateEnd.SelectedDate = DateTime.Now;

                GasHelper.DropDownListBottleAllDataBind(ddlBottle);

                GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

                LoadTreeSource();

                //全部
                BindDataGrid();
            }
        }


        protected void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            LoadTreeSource();
        }

        /// <summary>
        ///     部门树加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trDept_NodeCommand(object sender, TreeCommandEventArgs e)
        {
            txtFCode.Text = e.NodeID;

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

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {

            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
            }

            string KeyId = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

            PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}','{1}');" //
                            , KeyId, Convert.ToDateTime(dpkFDateBegin.SelectedDate).ToString("yyyy-MM-dd")));
        }


        protected void ddlFSalesman_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlFSalesman.SelectedValue != null && !ddlFSalesman.SelectedValue.Equals("-1"))
            //{
            //    LoadTreeSource();
            //}
        }

        //protected void txtFName_TextChanged(object sender, EventArgs e)
        //{
        //    trDept.FindNode(txtFName.Text.Trim());
        //    //BindDataGrid();
        //}

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=客户回瓶/提瓶明细表{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }

        protected void txtFName_OnTextChanged(object sender, EventArgs e)
        {
            BindDataGrid();
        }

        protected void txtFBottleName_OnTextChanged(object sender, EventArgs e)
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
                Text = "所有客户",
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
                    Text = string.Format("{0} {1}", customer.FCode, customer.FName),
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
        protected void btnRest_Click(object sender, EventArgs e)
        {
            txtFCode.Text = string.Empty;
            tbxFCustomer.Text = string.Empty;
            ddlBottle.SelectedValue = "-1";
            ddlFSalesman.SelectedValue = "-1";
        }

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            //if (trDept.SelectedNode != null)
            {
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@FCompanyId", CurrentUser.AccountComId);
                parms.Add("@FDate", Convert.ToDateTime(dpkFDateBegin.SelectedDate).ToString("yyyy-MM-dd"));
                parms.Add("@FCode", txtFCode.Text);
                parms.Add("@FArea", "");
                parms.Add("@FBottle", ddlBottle.SelectedValue.Equals("-1") ? "" : ddlBottle.SelectedValue);
                parms.Add("@FSalesman", ddlFSalesman.SelectedValue.Equals("-1") ? "" : ddlFSalesman.SelectedValue);
                
                var list = SqlService.ExecuteProcedureCommand("proc_BottleQty2", parms).Tables[0];

                if (list != null && list.Rows.Count > 0)
                {
                    Grid1.DataSource = list;
                    Grid1.DataBind();

                    SumIQty.Text = list.Compute("sum(FIQty)", "true").ToString();
                    SumOQty.Text = list.Compute("sum(FOQty)", "true").ToString();

                    //合计
                    //var summary = new JObject
                    //{
                    //    {"FBottle", "合计"},
                    //    {"FInQty", list.Compute("sum(FInQty)","true").ToString()},
                    //    {"FOutQty", list.Compute("sum(FOutQty)","true").ToString()},
                    //};

                    //Grid1.SummaryData = summary;
                }
                else
                {
                    Grid1.DataSource = null;
                    Grid1.DataBind();

                    SumIQty.Text = "0";
                    SumOQty.Text = "0";


                    //var summary = new JObject
                    //{
                    //    {"FBottle", "合计"},
                    //    {"FInQty", "0"},
                    //    {"FOutQty", "0"},
                    //};

                    //Grid1.SummaryData = summary;
                }
            }
        }

        #endregion

        private string tempBottle = string.Empty;
        protected void Grid1_OnRowDataBound(object sender, GridRowEventArgs e)
        {
           
            //var row = e.DataItem as DataRowView;
            //if (row != null)
            //{
            //    string KeyId = e.Values[9].ToString();//Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();

            //    if (tempBottle.Equals(KeyId))
            //    {
            //        e.Values[8] = String.Format("<span class=\"{0}\">{1}</span>", "colorgreen", e.Values[8]);
            //    }
            //    else
            //    {
            //        e.Values[8] = String.Format("<span class=\"{0}\">{1}</span>", "colorred", e.Values[8]);

            //    }

            //    tempBottle = KeyId;

            //}

        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFSalesman_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTreeSource();
        }
    }
}