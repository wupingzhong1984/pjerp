using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;


namespace Enterprise.IIS.Common
{
    public partial class WinEmployee : PageBase
    {
        /// <summary>
        ///     帐号数据服务
        /// </summary>
        private EmployeeService _employeeService;

        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        private CompanyService _companyService;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        private OrgnizationService _orgnizationService;

        /// <summary>
        ///     部门列表
        /// </summary>
        private IList<base_orgnization> _orgnizations;

        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "id";
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
        ///     部门组织数据服务
        /// </summary>
        protected OrgnizationService OrgnizationService
        {
            get { return _orgnizationService ?? (_orgnizationService = new OrgnizationService()); }
            set { _orgnizationService = value; }
        }

        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        protected CompanyService CompanyService
        {
            get { return _companyService ?? (_companyService = new CompanyService()); }
            set { _companyService = value; }
        }

        /// <summary>
        ///     帐号数据服务
        /// </summary>
        protected EmployeeService EmployeeService
        {
            get { return _employeeService ?? (_employeeService = new EmployeeService()); }
            set { _employeeService = value; }
        }

        /// <summary>
        ///     部门列表
        /// </summary>
        protected IList<base_orgnization> Orgnizations
        {
            get
            {
                return _orgnizations ?? (_orgnizations = OrgnizationService.Where(p => p.deleteflag == 0).ToList());
            }
            set { _orgnizations = value; }
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

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
            }
            else
            {
                var cCode = new StringBuilder();
                var cName = new StringBuilder();
                foreach (var rowIndex in Grid1.SelectedRowIndexArray)
                {
                    if (!string.IsNullOrWhiteSpace(Grid1.DataKeys[rowIndex][1].ToString()))
                    {
                        cCode.Append(Grid1.DataKeys[rowIndex][1] + ",");
                        cName.Append(Grid1.DataKeys[rowIndex][2] + ",");
                    }
                }

                var code = cCode.ToString().TrimEnd(',');
                var name = cName.ToString().TrimEnd(',');

                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(code, name) + ActiveWindow.GetHideReference());
            }
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            string ccode = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();
            string cname = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][2].ToString();
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(ccode, cname) + ActiveWindow.GetHideReference());

        }


        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     树绑定
        /// </summary>
        private void LoadTreeSource()
        {
            trDept.Nodes.Clear();

            base_company company = CompanyService.FirstOrDefault(p => p.deleteflag == 0 && p.id==CurrentUser.AccountComId);
            var rootNode = new TreeNode
            {
                Text = company.com_name,
                NodeID = company.id.ToString(CultureInfo.InvariantCulture),
                EnableClickEvent = true,
                Expanded = true
            };

            trDept.Nodes.Add(rootNode);

            //加载子部门信息
            LoadChildNodes(rootNode);
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(TreeNode node)
        {
            var nodeId = Int32.Parse(node.NodeID);

            if (OrgnizationService.Count(p => p.org_parent_id == nodeId && p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId) == 0)
            {
                node.Leaf = true;
            }
            else
            {
                node.Expanded = true;
                node.Nodes.Clear();
                foreach (
                    base_orgnization orgnization in
                        OrgnizationService.Where(p => p.org_parent_id == nodeId && p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId))
                {
                    var cNode = new TreeNode
                    {
                        Text = string.Format(@"{0}-{1}", orgnization.code, orgnization.org_name),
                        NodeID = orgnization.id.ToString(CultureInfo.InvariantCulture),
                        EnableClickEvent = true,
                        Expanded = false
                    };

                    //加载子部门信息
                    node.Nodes.Add(cNode);

                    LoadChildNodes(cNode);
                }
            }
        }

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            int output;

            dynamic orderingSelector;
            Expression<Func<base_employee, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<base_employee> list = EmployeeService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<base_employee, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_employee));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "deleteflag"), Expression.Constant(0, typeof(int?))));

            expr = Expression.And(expr,
            Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int?))));


            if (trDept.SelectedNodeID != "1" && !string.IsNullOrEmpty(trDept.SelectedNodeID))
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "orgnization_id"),
                        Expression.Constant(Convert.ToInt32(trDept.SelectedNodeID), typeof(int?))));
            }

            // 姓名
            if (!string.IsNullOrWhiteSpace(txtaccount_name.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "name"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtaccount_name.Text.Trim())));
            }

            // 登陆帐号
            if (!string.IsNullOrWhiteSpace(txtcode_cn.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "code_cn"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtcode_cn.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(txtFSpell.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "number"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFSpell.Text.Trim())));
            }

            Expression<Func<base_employee, bool>> predicate = Expression.Lambda<Func<base_employee, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion
    }
}