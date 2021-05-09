using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;
using Enterprise.Service.Base.Platform;

namespace Enterprise.IIS.Common
{
    public partial class WinTask : PageBase
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
        ///     数据服务_视图
        /// </summary>
        private ViewDispatchTaskService _viweDispatchCenterService;

        /// <summary>
        ///     数据服务_视图
        /// </summary>
        protected ViewDispatchTaskService ViewDispatchTaskService
        {
            get { return _viweDispatchCenterService ?? (_viweDispatchCenterService = new ViewDispatchTaskService()); }
            set { _viweDispatchCenterService = value; }
        }

        /// <summary>
        ///     数据服务_视图
        /// </summary>
        private DispatchCenterService _dispatchCenterService;

        /// <summary>
        ///     数据服务_视图
        /// </summary>
        protected DispatchCenterService DispatchCenterService
        {
            get { return _dispatchCenterService ?? (_dispatchCenterService = new DispatchCenterService()); }
            set { _dispatchCenterService = value; }
        }

        protected string FType
        {
            get { return Request["FType"]; }
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

                dateBegin.SelectedDate = DateTime.Now;
                dateEnd.SelectedDate = DateTime.Now;

                //单据状态
                //GasHelper.DropDownListBillStatusDataBind(ddlFBill);

                GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区

                BindDataGrid();

                BindDataDispatchCenterGrid();//调度作业
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
            }
            else
            {
                var cKeyId = new StringBuilder();
                foreach (var rowIndex in Grid1.SelectedRowIndexArray)
                {
                    if (!string.IsNullOrWhiteSpace(Grid1.DataKeys[rowIndex][0].ToString()))
                    {
                        cKeyId.Append(Grid1.DataKeys[rowIndex][0] + ",");
                    }
                }

                var code = cKeyId.ToString().TrimEnd(',');

                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(code) + ActiveWindow.GetHideReference());
            }
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
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {

        }

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private string GetSelectKeyIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            StringBuilder keys = new StringBuilder();

            for (int i = 0; i < selections.Length; i++)
            {
                keys.AppendFormat("'{0}',", Grid1.DataKeys[selections[i]][0]);
            }
            return keys.ToString().Substring(0, keys.Length - 1);
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    return;
                    //Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }

                string sid = GetSelectKeyIds();

                if (!string.IsNullOrEmpty(sid))
                {
                    var parms = new Dictionary<string, object>();
                    parms.Clear();

                    parms.Add("@keyid", sid);
                    parms.Add("@companyId", CurrentUser.AccountComId);

                    var data = SqlService.ExecuteProcedureCommand("proc_DispatchTaskDetails", parms).Tables[0];


                    if (data.Rows.Count > 0)
                    {
                        //绑定数据源
                        Grid2.DataSource = data;
                        Grid2.DataBind();

                        var summary = new JObject
                    {
                        {"FName", "合计"},
                        {"FQty", data.Compute("sum(FQty)","true").ToString()},
                    };

                        Grid2.SummaryData = summary;
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("查看失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     配车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDispatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);

                    return;
                }

                string sid = GetSelectIds();

                PageContext.RegisterStartupScript(
                    Window2.GetShowReference(string.Format("../../Common/WinDispatchVehicle.aspx?KeyId={0}&action=1&Bill=1",
                        sid), "生成调度单"));


            }
            catch (Exception)
            {
                Alert.Show("生成调度单失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     补填回厂单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Grid3.SelectedRowIndexArray.Length == 0)
            //    {
            //        Alert.Show("请至少选择一项！", MessageBoxIcon.Information);

            //        return;
            //    }

            //    string sid = Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][0].ToString();

            //    PageContext.RegisterStartupScript(
            //        Window2.GetShowReference(string.Format("../../Common/WinDispatchVehicle.aspx?KeyId={0}&action=2&Bill=1",
            //            sid), "补填回厂单"));


            //}
            //catch (Exception)
            //{
            //    Alert.Show("生成调度单失败！", MessageBoxIcon.Warning);
            //}
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
            BindDataDispatchCenterGrid();
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=调度中心{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Grid3.SelectedRowIndexArray.Length == 0)
            //    {
            //        Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
            //    }
            //    else if (Grid3.SelectedRowIndexArray.Length > 1)
            //    {
            //        Alert.Show("只能选择一项！", MessageBoxIcon.Information);
            //    }
            //    else
            //    {
            //        string sid = Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][0].ToString();

            //        PageContext.RegisterStartupScript(string.Format("LodopPrinter('{0}');", sid));
            //    }
            //}
            //catch (Exception)
            //{
            //    Alert.Show("打印失败！", MessageBoxIcon.Warning);
            //}
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFDistributionPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDataGrid();
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
        private string GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var keys = new StringBuilder();

            for (int i = 0; i < selections.Length; i++)
            {
                keys.AppendFormat("{0},",
                    Grid1.DataKeys[selections[i]][0]);
            }
            return keys.ToString();
        }

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            int output;

            dynamic orderingSelector;
            Expression<Func<vm_DispatchTask, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<vm_DispatchTask> list = ViewDispatchTaskService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<vm_DispatchTask, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(vm_DispatchTask));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            //expr = Expression.And(expr,
            //   Expression.Equal(Expression.Property(parameter, "FFlag"), Expression.Constant(1, typeof(int?))));

            var point = ddlFDistributionPoint.SelectedValue.ToString();
            if (point != "-1")
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FDistributionPoint"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(point)));
            }

            if (FType.Equals("1"))
            {
                // 单据类型
                int ftype = Convert.ToInt32(GasEnumBill.PassCard);
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "FType"), Expression.Constant(ftype, typeof(int?))));

                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FOutFlag"), methodInfo,
                // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant("未出库")));
            }
            else
            {
                // 单据类型
                int ftype = Convert.ToInt32(FType);
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "FType"), Expression.Constant(ftype, typeof(int?))));
            }

            if (dateBegin.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.GreaterThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dateBegin.SelectedDate, typeof(DateTime?))));
            }

            if (dateEnd.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.LessThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dateEnd.SelectedDate, typeof(DateTime?))));
            }

            if (!string.IsNullOrWhiteSpace(txtFName.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FName"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFName.Text.Trim())));
            }

            var emp = new EmployeeService().FirstOrDefault(p => p.number == CurrentUser.AccountJobNumber);
            if (emp != null)
            {
                if (!string.IsNullOrEmpty(emp.FDistributionPoint))
                {
                    var expr1 =
                    emp.FDistributionPoint.Split(',').Aggregate<string, Expression>(
                        Expression.Constant(false),
                        (expr2, v) =>
                            Expression.Or(expr2, Expression.Equal(Expression.Property(parameter, "FDistributionPoint"),
                              Expression.Constant(v, typeof(string)))));
                    expr = Expression.And(expr, expr1);
                }
                else
                {
                    expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FDistributionPoint"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant("无权查看")));
                }
            }
            else
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FDistributionPoint"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant("无权查看")));
            }
            if (fpg.Checked)
            {
                expr = Expression.And(expr,
                    Expression.NotEqual(Expression.Property(parameter, "fbill"), Expression.Constant("排管车", typeof(string))));
            }
            else if (pg.Checked)
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "fbill"), Expression.Constant("排管车", typeof(string))));
            }
            Expression<Func<vm_DispatchTask, bool>> predicate = Expression.Lambda<Func<vm_DispatchTask, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }


        //调度作业------------------------------------------------------------------------------------------

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataDispatchCenterGrid()
        {
            //int output;

            //dynamic orderingSelector;
            //Expression<Func<LHDispatchCenter, bool>> predicate = BuildPredicateDispatchCenter(out orderingSelector);

            ////取数据源
            //IQueryable<LHDispatchCenter> list = DispatchCenterService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
            //    orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            ////设置页面大小
            //Grid3.RecordCount = output;

            ////绑定数据源
            //Grid3.DataSource = list;
            //Grid3.DataBind();
        }

        /// <summary>
        ///     创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<LHDispatchCenter, bool>> BuildPredicateDispatchCenter(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHDispatchCenter));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));


            if (dateBegin.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.GreaterThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dateBegin.SelectedDate, typeof(DateTime?))));
            }

            if (dateEnd.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.LessThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dateEnd.SelectedDate, typeof(DateTime?))));
            }

            Expression<Func<LHDispatchCenter, bool>> predicate = Expression.Lambda<Func<LHDispatchCenter, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion

    }
}