using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using FineUI;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Framework.Enum;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.IIS.product.online
{
    /// <summary>
    /// 在线用户统计
    /// </summary>
// ReSharper disable once InconsistentNaming
    public partial class index : PageBase
    {
        /// <summary>
        /// 排序
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
        /// 排序
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
        /// 在线用户
        /// </summary>
        private AccountService _accountService;

        /// <summary>
        /// 在线用户
        /// </summary>
        protected AccountService AccountService
        {
            get { return _accountService ?? (_accountService = new AccountService()); }
            set { _accountService = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataGrid();

                ddlPageSize.SelectedValue = Grid1.PageSize.ToString(CultureInfo.InvariantCulture);
            }
        }

        #region LoadData

        /// <summary>
        /// 绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            int output;

            dynamic orderingSelector;

            Expression<Func<base_account, bool>> predicate = BuildPredicate(out orderingSelector);
            //取数据源
            IQueryable<base_account> list = AccountService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            Grid1.RecordCount = output;

            //设置页面大小
            SetPagerdg(output);

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
        private Expression<Func<base_account, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_account));

            var time = DateTime.Now.AddHours(-2).ToString(CultureInfo.InvariantCulture);
            expr = Expression.And(expr,
                    Expression.GreaterThanOrEqual(Expression.Property(parameter, "account_last_date"),
                        Expression.Constant(DateTime.Parse(time), typeof(DateTime?))));


            var time2 = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            expr = Expression.And(expr,
                   Expression.LessThanOrEqual(Expression.Property(parameter, "account_last_date"),
                       Expression.Constant(DateTime.Parse(time2), typeof(DateTime?))));

            Expression<Func<base_account, bool>> predicate = Expression.Lambda<Func<base_account, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        /// <summary>
        /// 设置数据分页大小
        /// </summary>
        /// <param name="output"></param>
        private void SetPagerdg(int output)
        {
            Grid1.RecordCount = output;
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tgbSearch_OnTriggerClick(object sender, EventArgs e)
        {
            BindDataGrid();
        }
        #endregion

        #region Events
        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            Grid1.PageIndex = 0;
            BindDataGrid();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindDataGrid();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            BindDataGrid();
        }

        /// <summary>
        /// RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {

        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SortField = string.Format(@"{0}", e.SortField);
            SortDirection = e.SortDirection;
            BindDataGrid();
        }
        #endregion
    }
}