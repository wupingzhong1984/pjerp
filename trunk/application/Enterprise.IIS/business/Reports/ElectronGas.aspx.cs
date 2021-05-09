using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class ElectronGas : PageBase
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


        /// <summary>
        ///     数据服务
        /// </summary>
        private ItemsService _itemsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        private ProjectService _projectService;

        /// <summary>
        ///     数据服务
        /// </summary>
        private ProjectItemsService _projectItemsService;

        /// <summary>
        ///     分类
        /// </summary>
        private IList<LHProjectItems> _projectItemses;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ProjectItemsService ProjectItemsService
        {
            get { return _projectItemsService ?? (_projectItemsService = new ProjectItemsService()); }
            set { _projectItemsService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ProjectService ProjectService
        {
            get { return _projectService ?? (_projectService = new ProjectService()); }
            set { _projectService = value; }
        }

        /// <summary>
        ///     分类Key
        /// </summary>
        protected string ProjectKey
        {
            get { return "1000"; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ItemsService ItemsService
        {
            get { return _itemsService ?? (_itemsService = new ItemsService()); }
            set { _itemsService = value; }
        }

        /// <summary>
        ///     产品分类
        /// </summary>
        protected IList<LHProjectItems> ProjectItemses
        {
            get
            {
                return _projectItemses ?? (_projectItemses = ProjectItemsService
                    .Where(p => p.FCompanyId == CurrentUser.AccountComId //
                        && p.FFlag == 1 //
                        && p.FSParent == ProjectKey).ToList());
            }
            set { _projectItemses = value; }
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

                //LoadTreeSource();

                GasHelper.DropDownListCompanyDataBind(ddlCompany);

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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=电子气本{0}.xls", SequenceGuid.GetGuidReplace()));
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
        ///     树绑定
        /// </summary>
        private void LoadTreeSource()
        {
            //trDept.Nodes.Clear();

            var project = ProjectItemsService.Where(p => p.FId == "2001" //
                && p.FCompanyId == CurrentUser.AccountComId && p.FFlag == 1).FirstOrDefault();
            if (project != null)
            {
                var rootNode = new TreeNode
                {
                    Text = string.Format("{0}-{1}", project.FId, project.FName),
                    NodeID = project.FId.ToString(CultureInfo.InvariantCulture),
                    Icon = Icon.BulletBlue,
                    EnableClickEvent = true,
                    Expanded = true
                };

                //trDept.Nodes.Add(rootNode);

                //trDept.SelectedNodeID = "2001";

                //加载子部门信息
                LoadChildNodes(rootNode);

            }
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(TreeNode node)
        {
            var nodeId = node.NodeID;

            if (ProjectItemsService.Count(p => p.FParentId == nodeId && p.FFlag == 1 && p.FCompanyId == CurrentUser.AccountComId) == 0)
            {
                node.Leaf = true;
            }
            else
            {
                node.Expanded = true;
                node.Nodes.Clear();
                foreach (
                    var item in
                        ProjectItemsService.Where(p => p.FParentId == nodeId && p.FFlag == 1 && p.FCompanyId == CurrentUser.AccountComId))
                {
                    var cNode = new TreeNode
                    {
                        Text = string.Format(@"{0}-{1}", item.FId, item.FName),
                        NodeID = item.FId.ToString(CultureInfo.InvariantCulture),
                        EnableClickEvent = true,
                        Expanded = true
                    };

                    //加载子部门信息
                    node.Nodes.Add(cNode);

                    //
                    LoadChildNodes(cNode);
                }
            }
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
            parms.Add("@FCompanyId", ddlCompany.SelectedValue);
            parms.Add("@BottleFName", txtFBottleName.Text);
            parms.Add("@UnitType",ddlUnit.SelectedValue);
            parms.Add("@FName", txtFName.Text.Trim());
            parms.Add("@FSalesman", txtFSalesman.Text.Trim());

            var data = SqlService.ExecuteProcedureCommand("rpt_ElectronGas", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = data;
                Grid1.DataBind();

                var summary = new JObject
                {
                    {"FName", "合计"},
                    {"FQty1", data.Compute("sum(FQty1)", "true").ToString()},
                    {"FQty2", data.Compute("sum(FQty2)", "true").ToString()},
                    {"FQty3", data.Compute("sum(FQty3)", "true").ToString()},
                    {"FQty4",  data.Compute("sum(FQty4)", "true").ToString()},

                    {"FDepositSecurity",  data.Compute("sum(FDepositSecurity)", "true").ToString()},
                };

                Grid1.SummaryData = summary;
            }
            else
            {
                Grid1.DataSource = null;
                Grid1.DataBind();

                var summary = new JObject
                {
                    {"FName", "合计"},
                    {"FQty1", "0"},
                    {"FQty2", "0"},
                    {"FQty3", "0"},
                    {"FQty4", "0"},

                    {"FDepositSecurity",  "0"},

                };

                Grid1.SummaryData = summary;
            }
        }

        #endregion
    }
}       