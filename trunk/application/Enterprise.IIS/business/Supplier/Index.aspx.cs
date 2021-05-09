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

namespace Enterprise.IIS.business.Supplier
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
        private SupplierService _supplierService;

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
            get { return "2078"; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SupplierService SupplierService
        {
            get { return _supplierService ?? (_supplierService = new SupplierService()); }
            set { _supplierService = value; }
        }

        /// <summary>
        ///     供应商分类
        /// </summary>
        protected IList<LHProjectItems> ProjectItemses
        {
            get
            {
                return _projectItemses ?? (_projectItemses = ProjectItemsService
                    .Where(p => p.FCompanyId == CurrentUser.AccountComId //
                        && p.FFlag == 0 //
                        && p.FSParent == ProjectKey).ToList());
            }
            set { _projectItemses = value; }
        }

        #region Protected Method

        #region 分类
        /// <summary>
        ///     添加分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddClass_Click(object sender, EventArgs e)
        {
            try
            {
                if (trDept.SelectedNode == null)
                {
                    Alert.Show("请选择要编辑的分类节点！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = trDept.SelectedNodeID;

                    if (trDept.SelectedNode.ParentNode != null)
                    {
                        PageContext.RegisterStartupScript(
                            Window2.GetShowReference(
                                string.Format("../../Common/WinClass.aspx?FId={0}&FSParent=1003&action=1&FParentId={1}",
                                    sid, trDept.SelectedNode.ParentNode.NodeID), "分类"));
                    }
                    else
                    {
                        PageContext.RegisterStartupScript(
                           Window2.GetShowReference(
                               string.Format("../../Common/WinClass.aspx?FId={0}&FSParent=1003&action=1&FParentId={1}",
                                   sid, sid), "分类"));
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        ///     分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditClass_Click(object sender, EventArgs e)
        {
            try
            {
                if (trDept.SelectedNode == null)
                {
                    Alert.Show("请选择要编辑的分类节点！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = trDept.SelectedNodeID;

                    if (trDept.SelectedNode.ParentNode != null)
                    {
                        PageContext.RegisterStartupScript(
                            Window2.GetShowReference(
                                string.Format("../../Common/WinClass.aspx?FId={0}&FSParent=1003&action=2&FParentId={1}",
                                    sid, trDept.SelectedNode.ParentNode.NodeID), "分类"));
                    }
                    else
                    {
                        PageContext.RegisterStartupScript(
                           Window2.GetShowReference(
                               string.Format("../../Common/WinClass.aspx?FId={0}&FSParent=1003&action=2&FParentId={1}",
                                   sid, sid), "分类"));
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteClass_Click(object sender, EventArgs e)
        {
            try
            {
                if (trDept.SelectedNode == null)
                {
                    Alert.Show("请选择要删除的分类节点！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = trDept.SelectedNodeID;

                    if (trDept.SelectedNode.ParentNode != null)
                    {

                        ProjectItemsService.Delete(p => p.FId == sid //
                                                        && p.FCompanyId == CurrentUser.AccountComId);

                        Alert.Show("删除分类完成", MessageBoxIcon.Information);

                        LoadTreeSource();
                    }
                    else
                    {
                        //提未不让删除操作
                        Alert.Show("根节点无权删除操作！", MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            LoadTreeSource();
        }

        #endregion

        protected void btnRestClass_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    int[] selections = Grid1.SelectedRowIndexArray;

                    var selectIds = new StringBuilder();

                    foreach (int t in selections)
                    {
                        selectIds.AppendFormat("{0},", Grid1.DataKeys[t][0].ToString());
                    }

                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("./SetClass.aspx?FCodes={0}",
                            selectIds.ToString()), "编辑分类"));
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }

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

                //btnBatchDelete.ConfirmText = "你确定要执行删除操作吗？";

                //供应商分类
                LoadTreeSource();

                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./edit.aspx?action=1&FSubCateId={0}",
                    trDept.SelectedNodeID), "添加供应商");

                BindDataGrid();
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

            if (trDept.SelectedNodeID != "1")
            {
                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./edit.aspx?action=1&FSubCateId={0}",
                trDept.SelectedNodeID), "添加供应商");
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
            Grid1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            BindDataGrid();
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
                SupplierService.Delete(p =>p.FCompanyId==CurrentUser.AccountComId&& selectIds.Contains(p.FCode));
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();
                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("./edit.aspx?FCode={0}&FSubCateId={1}&action=2",
                            sid, trDept.SelectedNodeID), "编辑供应商"));
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     禁用/启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnabled_Click(object sender, EventArgs e)
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
                    var customer = SupplierService.FirstOrDefault(p => p.FCode == sid&&p.FCompanyId==CurrentUser.AccountComId);
                    customer.FFlag = customer.FFlag == 1 ? 0 : 1;

                    if (SupplierService.SaveChanges() > 0)
                    {
                        BindDataGrid();

                        Alert.Show("提交成功。", MessageBoxIcon.Information);

                        //记录日志
                        Log(string.Format(@"禁用帐号{0}成功。", sid));
                    }
                    else
                    {
                        Alert.Show("提交失败，请重试！", MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     引入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(string.Format("openAddFineUI('./business/Init/InitSupplier.aspx');"));
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=供应商档案{0}.xls", SequenceGuid.GetGuidReplace()));
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

            var project = ProjectItemsService.Where(p => p.FId == "2078" && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
            if (project != null)
            {
                var rootNode = new TreeNode
                {
                    Text = string.Format("{0}-{1}", project.FId, project.FName),
                    NodeID = project.FId.ToString(CultureInfo.InvariantCulture),
                    EnableClickEvent = true,
                    Expanded = true
                };

                trDept.Nodes.Add(rootNode);

                trDept.SelectedNodeID = "2078";

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
            int output;

            dynamic orderingSelector;
            Expression<Func<LHSupplier, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<LHSupplier> list = SupplierService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            Grid1.RecordCount = output;

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();

            ddlPageSize.SelectedValue = Grid1.PageSize.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<LHSupplier, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHSupplier));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            if (trDept.SelectedNodeID != null && !string.IsNullOrEmpty(trDept.SelectedNodeID) && trDept.SelectedNodeID != "2078")
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "FSubCateId"),
                        Expression.Constant(trDept.SelectedNodeID)));
            }
            if (!string.IsNullOrWhiteSpace(txtFName.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FName"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFName.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(txtFCode.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FCode"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFCode.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(txtFSpell.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FSpell"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFSpell.Text.Trim())));
            }


            Expression<Func<LHSupplier, bool>> predicate = Expression.Lambda<Func<LHSupplier, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion
    }
}