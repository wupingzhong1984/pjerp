using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.Vehicle
{
    public partial class DispatchCenter : PageBase
    {
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "FDeliveryMethod";
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
        private ViewDispatchCenterService _viweDispatchCenterService;

        /// <summary>
        ///     数据服务_视图
        /// </summary>
        protected ViewDispatchCenterService ViewDispatchCenterService
        {
            get { return _viweDispatchCenterService ?? (_viweDispatchCenterService = new ViewDispatchCenterService()); }
            set { _viweDispatchCenterService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private PassCardService _passCardService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected PassCardService PassCardService
        {
            get { return _passCardService ?? (_passCardService = new PassCardService()); }
            set { _passCardService = value; }
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
                //SetPermissionButtons(Toolbar1);

                Timer1.Enabled = true;

                dateBegin.SelectedDate = DateTime.Now;

                dateEnd.SelectedDate = DateTime.Now;

                //单据状态
                //GasHelper.DropDownListBillStatusDataBind(ddlFBill);

                GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区

                GasHelper.DropDownListDeliveryMethodDataBind(ddlDeliveryMethod);

                btnResase.ConfirmText = "确认要重新配车吗?";

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

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFDistributionPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        ///     追加任务单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddTask_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项追加任务单！", MessageBoxIcon.Information);
                    return;
                }

                if (Grid3.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                    return;
                }
                else if (Grid3.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string disKey = Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][0].ToString();

                    string sid = GetSelectKeyIds();

                    var parms = new Dictionary<string, object>();
                    parms.Clear();

                    parms.Add("@disKey",disKey);
                    parms.Add("@keyid", sid);
                    parms.Add("@companyId", CurrentUser.AccountComId);

                    SqlService.ExecuteProcedureCommand("proc_DispatchAddTask", parms);

                    BindDataGrid();

                }
            }
            catch (Exception)
            {
                Alert.Show("追加任务单失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDataGrid();
        }
        protected void rbtList_CheckedChanged(object sender, CheckedEventArgs e)
        {
            // 单选框按钮的CheckedChanged事件会触发两次，一次是取消选中的菜单项，另一次是选中的菜单项；
            // 不处理取消选中菜单项的事件，从而防止此函数重复执行两次
            if (!e.Checked)
            {
                return;
            }

            BindDataGrid();
        }

        protected void btnResase_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    return;
                }

                string sid = GetSelectKeyIds();

                if (!string.IsNullOrEmpty(sid))
                {
                    var parms = new Dictionary<string, object>();
                    parms.Clear();

                    parms.Add("@keyid", sid);
                    parms.Add("@companyId", CurrentUser.AccountComId);

                    SqlService.ExecuteProcedureCommand("proc_DispatchResase", parms);

                    BindDataGrid();

                }
            }
            catch (Exception)
            {
                Alert.Show("查看失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     作废
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetDispatchCenterIds();

            try
            {
              //  Log(string.Format(@"作废单据号:{0}成功。", selectIds));

                //
                //foreach (var ids in selectIds)
                //{
                    DispatchCenterService.Update(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.KeyId), p => new LHDispatchCenter
                    {
                        FFlag = 0, //
                    });
                //}

                Alert.Show("作废成功！", MessageBoxIcon.Information);

                BindDataDispatchCenterGrid();//调度作业
            }
            catch (Exception)
            {
                Alert.Show("作废失败！", MessageBoxIcon.Warning);
            }
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
            return keys.ToString().Substring(0,keys.Length-1);
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

                    var data = SqlService.ExecuteProcedureCommand("proc_DispatchCenterDetails", parms).Tables[0];


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
                string patchid = GetSelectPatchIds();

                    PageContext.RegisterStartupScript(
                        Window2.GetShowReference(string.Format("../../Common/WinDispatchVehicle.aspx?KeyId={0}&action=1&Bill=1&patchid={1}",
                            sid,patchid), "生成调度单"));


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
            try
            {
                if (Grid3.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);

                    return;
                }

                string sid = Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(
                        Window2.GetShowReference(string.Format("../../Common/WinDispatchVehicle.aspx?KeyId={0}&action=2&Bill=1",
                            sid), "补填回厂单"));


            }
            catch (Exception)
            {
                Alert.Show("生成调度单失败！", MessageBoxIcon.Warning);
            }
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=调度明细表{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        /// <summary>
        ///     打印货车出门单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid3.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid3.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("LodopPrinter('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }



        /// <summary>
        ///     打印货车出门单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTubePrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid3.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid3.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("LodopTubePrinter('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     货车运输记录单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrintDispatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid3.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid3.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("LodopPrinterDispatch('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }

        
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();

            BindDataDispatchCenterGrid();//调度作业
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();

            BindDataDispatchCenterGrid();//调度作业
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
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private string GetSelectPatchIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var keys = new StringBuilder();

            for (int i = 0; i < selections.Length && selections.Length>1; i++)
            {
                if (Grid1.DataKeys[selections[i]][1]!=null)
                {
                    if (keys != null && !keys.ToString().Contains(Grid1.DataKeys[selections[i]][1].ToString()))
                    {
                        keys.AppendFormat("{0},",
                            Grid1.DataKeys[selections[i]][1]);
                    }
                }
                
            }
            return keys.ToString();
        }

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetIds()
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
        ///     调度号
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetDispatchCenterIds()
        {
            int[] selections = Grid3.SelectedRowIndexArray;

            var selectIds = new string[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Grid3.DataKeys[selections[i]][0].ToString();
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
            Expression<Func<vm_DispatchCenter, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<vm_DispatchCenter> list = ViewDispatchCenterService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<vm_DispatchCenter, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(vm_DispatchCenter));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            
            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            //expr = Expression.And(expr,
            //   Expression.Equal(Expression.Property(parameter, "FType"), Expression.Constant(32, typeof(int?))));

            var point = ddlFDistributionPoint.SelectedValue.ToString();
            if (point != "-1")
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FDistributionPoint"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(point)));
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


            if (!string.IsNullOrEmpty(txtFKeyId.Text))
            {
                expr = Expression.And(expr,
                Expression.Call(Expression.Property(parameter, "FInitDispatch"), methodInfo, Expression.Constant(txtFKeyId.Text, typeof(string))));
            }

            //if (!string.IsNullOrWhiteSpace(txtFName.Text))
            //{
            //    expr = Expression.And(expr,
            //        Expression.Call(Expression.Property(parameter, "FName"), methodInfo,
            //        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
            //            Expression.Constant(txtFName.Text.Trim())));
            //}

            if (ddlDeliveryMethod.SelectedValue!="-1")
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FDeliveryMethod"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlDeliveryMethod.SelectedValue)));
            }

            if (!rbtList.SelectedValue.Equals("全部"))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FStatus"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(rbtList.SelectedValue)));
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

            Expression<Func<vm_DispatchCenter, bool>> predicate = Expression.Lambda<Func<vm_DispatchCenter, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }


        //调度作业------------------------------------------------------------------------------------------


        protected void Timer1_OnTickOnTick(object sender, EventArgs e)
        {
            BindDataGrid();

            BindDataDispatchCenterGrid();
        }


        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataDispatchCenterGrid()
        {
            int output;

            dynamic orderingSelector;
            Expression<Func<LHDispatchCenter, bool>> predicate = BuildPredicateDispatchCenter(out orderingSelector);

            //取数据源
            IQueryable<LHDispatchCenter> list = DispatchCenterService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            Grid3.RecordCount = output;

            //绑定数据源
            Grid3.DataSource = list;
            Grid3.DataBind();
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

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FFlag"), Expression.Constant(1, typeof(int?))));

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
            orderingSelector = Expression.Lambda(Expression.Property(parameter, "KeyId"), parameter);

            return predicate;
        }


        #endregion

        
    }
}