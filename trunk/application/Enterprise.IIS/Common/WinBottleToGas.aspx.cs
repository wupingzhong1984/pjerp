using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.Common
{
    public partial class WinBottleToGas : PageBase
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
                //关闭
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

                //产品分类
                LoadTreeSource();

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
        ///     确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else
                {

                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();
                    string fSpec = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][2].ToString();

                    PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(sid, fSpec) + ActiveWindow.GetHideReference());
        

                    //int[] selections = Grid1.SelectedRowIndexArray;

                    //var keys = new StringBuilder();
                    //var spec = new StringBuilder();
                    //foreach (int t in selections)
                    //{
                    //    keys.AppendFormat("{0},", Grid1.DataKeys[t][1]);
                    //    //spec.AppendFormat("{0},", Grid1.DataKeys[t][2]);
                    //}

                    //// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    //string values = keys.ToString().Substring(0, keys.Length - 1);

                    //PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(values) + ActiveWindow.GetHideReference());

                    //PageContext.RegisterStartupScript(//
                    //    string.Format(@"F.getActiveWindow().window.reloadGridBootleToGas('{0}');", //
                    //     values) + ActiveWindow.GetHidePostBackReference());
                }
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();
            string fSpec = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][2].ToString();
            //PageContext.RegisterStartupScript(//
            //           string.Format(@"F.getActiveWindow().window.reloadGrid('{0}');", //
            //            sid) + ActiveWindow.GetHidePostBackReference());


            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(sid, fSpec) + ActiveWindow.GetHideReference());
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=产品档案{0}.xls", SequenceGuid.GetGuidReplace()));
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

            //钢瓶
            var project = ProjectService.Where(p => p.FId == "2001" && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
            if (project != null)
            {
                var rootNode = new TreeNode
                {
                    Text = project.FName,
                    NodeID = project.FId.ToString(CultureInfo.InvariantCulture),
                    Icon = Icon.BulletBlue,
                    EnableClickEvent = true,
                    Expanded = true
                };

                trDept.Nodes.Add(rootNode);

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
            Expression<Func<LHItems, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<LHItems> list = ItemsService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            Grid1.RecordCount = output;

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();

            //ddlPageSize.SelectedValue = Grid1.PageSize.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<LHItems, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHItems));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), //
                Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCateId"), //
                Expression.Constant("2001")));

            if (trDept.SelectedNodeID != null && !string.IsNullOrEmpty(trDept.SelectedNodeID))
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "FSubCateId"),
                        Expression.Constant(trDept.SelectedNodeID)));
            }

            if (!string.IsNullOrWhiteSpace(txtFSpell.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FSpell"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFSpell.Text.Trim())));
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

            Expression<Func<LHItems, bool>> predicate = Expression.Lambda<Func<LHItems, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion
    }
}