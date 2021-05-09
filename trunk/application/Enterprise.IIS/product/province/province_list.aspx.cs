using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Data;
using FineUI;

namespace Enterprise.IIS.product.province
{
// ReSharper disable once InconsistentNaming
    public partial class province_list : PageBase
    {
        private ProvinceService _provinceService;
        protected ProvinceService ProvinceService
        {
            get { return _provinceService ?? (_provinceService = new ProvinceService()); }
            set { _provinceService = value; }
        }

        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "codeid";
                return sort;
            }
            set { ViewState["SortField"] = value; }
        }

        protected string SortDirection
        {
            get
            {
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        } 
        #endregion

        #region 查询
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Grid1.PageIndex = 0;
            BindData();
        } 

        #endregion

        #region grid

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindData();
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
            BindData();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            BindData();
        }  
        #endregion

        private void BindData()
        {
            int output = 0;

            dynamic orderingSelector;

            Expression<Func<base_province, bool>> predicate = BuildPredicate(out orderingSelector);
            //取数据源
            IQueryable<base_province> list = ProvinceService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        /// 设置数据分页大小
        /// </summary>
        /// <param name="output"></param>
        private void SetPagerdg(int output)
        {
            Grid1.RecordCount = output;
        }

        private Expression<Func<base_province, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_province));

            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            
            //名称
            if (!string.IsNullOrWhiteSpace(txtcity_name.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "city_name"), methodInfo,
                        Expression.Constant(txtcity_name.Text.Trim())));
            }

            //编码
            if (!string.IsNullOrWhiteSpace(txtcodeid.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "codeid"), methodInfo,
                        Expression.Constant(txtcodeid.Text.Trim())));
            }

            Expression<Func<base_province, bool>> predicate = Expression.Lambda<Func<base_province, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }
    }
}