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
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.Common
{
    public partial class WinCustomer : PageBase
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
        private CustomerService _customerService;

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
            get { return "1002"; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }

        /// <summary>
        ///     客户分类
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

                //客户分类
                LoadTreeSource();

                BindDataGrid();
            }
        }
        protected void tbxFText_OnTextChanged(object sender, EventArgs e)
        {
            BindDataGrid();
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
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Grid1.PageIndex = 0;
            BindDataGrid();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
            }
            else
            {
                StringBuilder cCode = new StringBuilder();
                StringBuilder cName = new StringBuilder();
                foreach (var rowIndex in Grid1.SelectedRowIndexArray)
                {
                    if (!string.IsNullOrWhiteSpace(Grid1.DataKeys[rowIndex][1].ToString()))
                    {
                        cCode.Append(Grid1.DataKeys[rowIndex][0] + ",");
                        cName.Append(Grid1.DataKeys[rowIndex][1] + ",");
                    }
                }

                var code = cCode.ToString().TrimEnd(',');
                var name = cName.ToString().TrimEnd(',');

                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(code, name) + ActiveWindow.GetHideReference());
            }
        }



        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            string ccode = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();
            string cname = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(ccode, cname) + ActiveWindow.GetHideReference());
            
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

            var project = ProjectItemsService.Where(p => p.FId == "2077" && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
            if (project != null)
            {
                var rootNode = new TreeNode
                {
                    Text = project.FName,
                    NodeID = project.FId.ToString(CultureInfo.InvariantCulture),
                    EnableClickEvent = true,
                    Expanded = true
                };

                trDept.Nodes.Add(rootNode);

                trDept.SelectedNodeID = "2077";

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
                        Text = string.Format(@"{0}-{1}", item.FId, item.FKey),
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
            Expression<Func<LHCustomer, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<LHCustomer> list = CustomerService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<LHCustomer, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHCustomer));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FFlag"), Expression.Constant(1, typeof(int?))));


            if (trDept.SelectedNodeID != null && !string.IsNullOrEmpty(trDept.SelectedNodeID) && trDept.SelectedNodeID != "2077")
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


            Expression<Func<LHCustomer, bool>> predicate = Expression.Lambda<Func<LHCustomer, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion
    }
}