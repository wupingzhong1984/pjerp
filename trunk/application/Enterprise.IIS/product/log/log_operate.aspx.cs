using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FineUI;
using Enterprise.Service.Logs;
using Enterprise.Service.Logs.SV;
using Enterprise.Framework.Enum;
using Enterprise.Framework.EntityRepository;
using Enterprise.Data;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.log
{
// ReSharper disable once InconsistentNaming
    public partial class log_operate : PageBase
    {
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "id";
                return sort;
            }
            set { ViewState["SortField"] = value; }
        }

        protected string SortDirection
        {
            get
            {
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "DESC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        private LogsService _logsService;
        protected LogsService LogsService
        {
            get { return _logsService ?? (_logsService = new LogsService()); }
            set { _logsService = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataGrid();

                ddlPageSize.SelectedValue = Grid1.PageSize.ToString();
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

            Expression<Func<t_logs, bool>> predicate = BuildPredicate(out orderingSelector);
            //取数据源
            IQueryable<t_logs> list = LogsService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            Grid1.RecordCount = output;

            //设置页面大小
            SetPagerdg(output);

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();
            ddlPageSize.SelectedValue = Grid1.PageSize.ToString();
        }

        /// <summary>
        ///     创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<t_logs, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(t_logs));

            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            //功能名称
            if (!string.IsNullOrWhiteSpace(txtaccount_name.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "account_name"), methodInfo,
                        Expression.Constant(txtaccount_name.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(dpBeginDate.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.GreaterThanOrEqual(Expression.Property(parameter, "action_on"),
                        Expression.Constant(DateTime.Parse(dpBeginDate.Text.Trim()), typeof(DateTime?))));
            }

            if (!string.IsNullOrWhiteSpace(dpEndDate.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.LessThanOrEqual(Expression.Property(parameter, "action_on"),
                        Expression.Constant(DateTime.Parse(dpEndDate.Text.Trim()), typeof(DateTime?))));
            }

            Expression<Func<t_logs, bool>> predicate = Expression.Lambda<Func<t_logs, bool>>(expr, parameter);

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

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            Grid1.PageIndex = 0;
            BindDataGrid();
        }
    }
        #endregion
}