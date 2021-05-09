using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.role
{
// ReSharper disable once InconsistentNaming
    public partial class role_list : PageBase
    {
        private RoleService _roleService;

        /// <summary>
        ///     角色数据服务
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
        ///     角色数据服务
        /// </summary>
        protected RoleService RoleService
        {
            get { return _roleService ?? (_roleService = new RoleService()); }
            set { _roleService = value; }
        }

        #region Protected Method
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

                btnBatchDelete.ConfirmText = "你确定要执行删除操作吗？";

                BindDataGrid();

                ddlPageSize.SelectedValue = Grid1.PageSize.ToString(CultureInfo.InvariantCulture);
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
        ///     新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect(string.Format(@"role_edit.aspx?action=1&id=0"));
        }

        /// <summary>
        /// 授权
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
                    var keyId = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);
                    HttpContext.Current.Response.Redirect(string.Format(@"role_edit.aspx?id={0}&action=2", keyId));
                }
            }
            catch (Exception)
            {
                Alert.Show("提交失败！", MessageBoxIcon.Warning);
            }
        }

        protected void btnPermission_Click(object sender, EventArgs e)
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
                    var keyId = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);
                    HttpContext.Current.Response.Redirect(string.Format(@"role_permission.aspx?id={0}&action=2", keyId));
                }
            }
            catch (Exception)
            {
                Alert.Show("提交失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<int> selectIds = GetSelectIds();

            try
            {
                RoleService.Update(p => selectIds.Contains(p.id), p => new base_role { deleteflag = 1 });
                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Grid1.PageIndex = 0;
            BindDataGrid();
        }
        #endregion

        #region Private Method
        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var selectIds = new int[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Int32.Parse(Grid1.DataKeys[selections[i]][0].ToString());
            }
            return selectIds;
        }

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            int output;

            //取数据源
            dynamic orderingSelector;
            Expression<Func<base_role, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<base_role> list = RoleService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            SetPagerdg(output);

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        /// <summary>
        ///     设置数据分页大小
        /// </summary>
        /// <param name="output"></param>
        private void SetPagerdg(int output)
        {
            Grid1.RecordCount = output;
        }

        /// <summary>
        ///     收集条件
        /// </summary>
        /// <returns></returns>
        private Expression<Func<base_role, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_role));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "deleteflag"), Expression.Constant(0, typeof(int?))));

            expr = Expression.And(expr,
            Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int?))));

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "account_id"), Expression.Constant(null, typeof(int?))));

            if (!string.IsNullOrWhiteSpace(txtRole_name.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "role_name"), methodInfo,
// ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtRole_name.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(ddlFlag.SelectedValue))
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "role_flag"),
                        Expression.Constant(Int32.Parse(ddlFlag.SelectedValue), typeof(Int32?))));
            }

            Expression<Func<base_role, bool>> predicate = Expression.Lambda<Func<base_role, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }
        #endregion
    }
}