
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.Common
{
    public partial class WinUnitLeaseBottle : PageBase
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
        ///     分类Key
        /// </summary>
        protected int ProjectKey
        {
            get { return 1002; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ViewLeaseBottleARService _viewLeaseBottleArService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ViewLeaseBottleARService ViewLeaseBottleARService
        {
            get { return _viewLeaseBottleArService ?? (_viewLeaseBottleArService = new ViewLeaseBottleARService()); }
            set { _viewLeaseBottleArService = value; }
        }

        /// <summary>
        ///     客户代码
        /// </summary>
        protected string UnitCode
        {
            //get { return Cookie.GetValue("UnitCode"); }
            get { return Request["unitcode"]; }
        }

        /// <summary>
        ///     客户代码
        /// </summary>
        protected string FDate
        {
            //get { return Cookie.GetValue("UnitCode"); }
            get { return Request["FDate"]; }
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
                //客户分类
                //LoadTreeSource();
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

                BindDataGrid();
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
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Grid1.PageIndex = 0;
            BindDataGrid();
        }


        protected void tbxFText_OnTextChanged(object sender, EventArgs e)
        {
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
                int[] selections = Grid1.SelectedRowIndexArray;

                var keys = new StringBuilder();

                foreach (int t in selections)
                {
                    keys.AppendFormat("{0},", Grid1.DataKeys[t][0]);
                }

                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                string values = keys.ToString().Substring(0, keys.Length - 1);

                PageContext.RegisterStartupScript(//
                    string.Format(@"F.getActiveWindow().window.reloadGrid('{0}');", //
                     values) + ActiveWindow.GetHidePostBackReference());
            }
        }


        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

            PageContext.RegisterStartupScript(//
                       string.Format(@"F.getActiveWindow().window.reloadGrid('{0}');", //
                        sid) + ActiveWindow.GetHidePostBackReference());
        }



        protected void Window1_Close(object sender, WindowCloseEventArgs e)
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
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            //int output;

            //dynamic orderingSelector;
            //Expression<Func<vm_LeaseBottleAR, bool>> predicate = BuildPredicate(out orderingSelector);

            ////取数据源
            //IQueryable<vm_LeaseBottleAR> list = ViewLeaseBottleARService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
            //    orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            ////设置页面大小
            //Grid1.RecordCount = output;

            var parms = new Dictionary<string, object>();

            parms.Clear();
            parms.Add("@FCompanyId",CurrentUser.AccountComId);
            parms.Add("@FCode", UnitCode);
            parms.Add("@FDate",FDate);


            var list = SqlService.ExecuteProcedureCommand("proc_LeaseBottleRec",parms);


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
        private Expression<Func<vm_LeaseBottleAR, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(vm_LeaseBottleAR));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int?))));

            //客户代码
            expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FCode"), methodInfo,
                // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(UnitCode)));

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
                    Expression.Call(Expression.Property(parameter, "FBottle"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFCode.Text.Trim())));
            }

            Expression<Func<vm_LeaseBottleAR, bool>> predicate = Expression.Lambda<Func<vm_LeaseBottleAR, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion
    }
}