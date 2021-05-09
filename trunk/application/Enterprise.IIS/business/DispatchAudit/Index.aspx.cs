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
using NPOI.SS.Formula.Functions;

namespace Enterprise.IIS.business.DispatchAudit
{
    public partial class Index : PageBase
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

                //GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区

                //物流公司
                GasHelper.DropDownListLogisticsDataBind(ddlLogistics);

                GasHelper.DropDownListDeliveryMethodDataBind(ddlDeliveryMethod);

                GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

                //btnResase.ConfirmText = "确认要重新配车吗?";

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
        ///     物流选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFLogistics_SelectedIndexChanged(object sender, EventArgs e)
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
                DispatchCenterService.Update(p => p.FCompanyId == CurrentUser.AccountComId //
                && selectIds.Contains(p.KeyId), p => new LHDispatchCenter
                {
                    FFlag = 0, //
                });

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
            return keys.ToString().Substring(0, keys.Length - 1);
        }


        /// <summary>
        ///     点击任务单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid3_RowClick(object sender, GridRowClickEventArgs e)
        {
            //IEnumerable<string> selectIds = GetDispatchCenterIds();

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

                    //根据调度单号查询任务单

                    var parms = new Dictionary<string, object>();

                    parms.Clear();
                    parms.Add("@KeyId", sid);
                    parms.Add("@FCompanyId", CurrentUser.AccountComId);

                    var data = SqlService.ExecuteProcedureCommand("proc_DispatchDetails", parms).Tables[0];
                    var bottle = SqlService.ExecuteProcedureCommand("rpt_SalesBottles", parms).Tables[0];
                    if (data.Rows.Count > 0)
                    {
                        //绑定数据源
                        Grid1.DataSource = data;
                        Grid1.DataBind();
                    }
                    if (bottle.Rows.Count > 0)
                    {
                        //绑定数据源
                        Grid4.DataSource = bottle;
                        Grid4.DataBind();

                        var summary = new JObject
                    {
                        {"FBottle", "合计"},
                        {"FBottleQty", bottle.Compute("sum(FBottleQty)","true").ToString()},
                    };

                        Grid4.SummaryData = summary;
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.Show("查看失败！", MessageBoxIcon.Warning);
            }


        }

        /// <summary>
        ///     点击销售任务单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
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
            catch (Exception ex)
            {
                Alert.Show("查看失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     审核通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAuditYes_Click(object sender, EventArgs e)
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

                    var dis = DispatchCenterService.FirstOrDefault(p => p.KeyId == sid && p.FCompanyId == CurrentUser.AccountComId);

                    if (dis != null)
                    {
                        if (dis.FDeliveryMethod != null)
                        {
                            if (dis.FDeliveryMethod.Equals("物流配送"))
                            {
                                if (dis.FVehicleNum.Equals("-1") || dis.FVehicleNum.Equals(""))
                                {
                                    Alert.Show("请完善车牌号信息，再审核。", MessageBoxIcon.Information);
                                    //return;
                                }

                                if (dis.FDriver.Equals("-1") || dis.FDriver.Equals(""))
                                {
                                    Alert.Show("请完善司机信息，再审核。", MessageBoxIcon.Information);
                                    //return;
                                }

                                if (dis.FSupercargo.Equals("-1") || dis.FSupercargo.Equals(""))
                                {
                                    Alert.Show("押运员信息没有输入，请确认。", MessageBoxIcon.Information);
                                    //return;
                                }

                                if (dis.FMileage.Equals("-1") || dis.FMileage.Equals("") || dis.FMileage == 0)
                                {
                                    Alert.Show("请完善里程数信息，再审核。", MessageBoxIcon.Information);
                                    //return;
                                }
                            }
                        }

                        dis.FAuditFlag = 1;
                        dis.FAuditor = CurrentUser.AccountName;

                        DispatchCenterService.SaveChanges();

                        //写入到计件工作表中
                        var parms= new Dictionary<string,object>();
                        parms.Clear();

                        parms.Add("@FCompanyId",CurrentUser.AccountComId);
                        parms.Add("@KeyId",sid);

                        SqlService.ExecuteProcedureCommand("FSalary_DriverAuto",parms);

                        BindDataDispatchCenterGrid();
                    }

                }
            }
            catch (Exception)
            {
                Alert.Show("查看失败！", MessageBoxIcon.Warning);
            }

        }

        /// <summary>
        ///     审核不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAuditNo_Click(object sender, EventArgs e)
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

                    var dis = DispatchCenterService.FirstOrDefault(p => p.KeyId == sid && p.FCompanyId == CurrentUser.AccountComId);

                    if (dis != null)
                    {
                        dis.FAuditFlag = 2;
                        dis.FAuditor = CurrentUser.AccountName;

                        DispatchCenterService.SaveChanges();

                        //删除
                        var parms = new Dictionary<string, object>();
                        parms.Clear();

                        parms.Add("@FCompanyId", CurrentUser.AccountComId);
                        parms.Add("@KeyId", sid);

                        SqlService.ExecuteProcedureCommand("FSalary_DriverDelete", parms);


                        BindDataDispatchCenterGrid();
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
            try
            {
                if (Grid3.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);

                    return;
                }

                //验证是否审核能过
                //string auditFlag = Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][1].ToString();
                //if (Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][1]!=null //
                //    && Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][1].ToString().Equals("1"))
                //{
                //    Alert.Show("该出车任务已审，无权进行变更！", MessageBoxIcon.Information);
                //    return;
                //}

                string sid = Grid3.DataKeys[Grid3.SelectedRowIndexArray[0]][0].ToString();

                PageContext.RegisterStartupScript(
                    Window2.GetShowReference(string.Format("../../Common/WinDispatchVehicle.aspx?KeyId={0}&action=2&Bill=1",
                        sid), "补填回厂单"));


            }
            catch (Exception ex)
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
            //int output;

            //dynamic orderingSelector;
            //Expression<Func<vm_DispatchCenter, bool>> predicate = BuildPredicate(out orderingSelector);

            ////取数据源
            //IQueryable<vm_DispatchCenter> list = ViewDispatchCenterService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
            //    orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            ////设置页面大小
            //Grid1.RecordCount = output;

            ////绑定数据源
            //Grid1.DataSource = list;
            //Grid1.DataBind();

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

            //if (!string.IsNullOrWhiteSpace(txtFName.Text))
            //{
            //    expr = Expression.And(expr,
            //        Expression.Call(Expression.Property(parameter, "FName"), methodInfo,
            //        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
            //            Expression.Constant(txtFName.Text.Trim())));
            //}

            if (ddlDeliveryMethod.SelectedValue != "-1")
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FDeliveryMethod"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlDeliveryMethod.SelectedValue)));
            }

            if (!ddlFAuditFlag.SelectedValue.Equals("-1"))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FStatus"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlFAuditFlag.SelectedValue)));
            }

            //限定货流配车权限
            //var emp = new EmployeeService().FirstOrDefault(p=>p.number==CurrentUser.AccountJobNumber);
            //if (emp != null)
            //{
            //    if (!string.IsNullOrEmpty(emp.FDistributionPoint))
            //    {
            //        var expr1 =
            //        emp.FDistributionPoint.Split(',').Aggregate<string, Expression>(
            //            Expression.Constant(false),
            //            (expr2, v) =>
            //                Expression.Or(expr2, Expression.Equal(Expression.Property(parameter, "FDistributionPoint"),
            //                  Expression.Constant(v, typeof(string)))));
            //        expr = Expression.And(expr, expr1);
            //    }
            //}

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
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FFlag"), Expression.Constant(1, typeof(int?))));

            //expr = Expression.And(expr,
            //   Expression.Equal(Expression.Property(parameter, "FFlag"), Expression.Constant(1, typeof(int?))));

            if (!ddlFAuditFlag.SelectedValue.Equals("-1"))
            {
                var value = int.Parse(ddlFAuditFlag.SelectedValue);

                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "FAuditFlag"), 
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(value, typeof(int?))));
            }

            if (dateBegin.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.GreaterThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dateBegin.SelectedDate, typeof(DateTime?))));
            }

            if (ddlDeliveryMethod.SelectedValue != "-1")
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FDeliveryMethod"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlDeliveryMethod.SelectedValue)));
            }

            if (dateEnd.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.LessThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dateEnd.SelectedDate, typeof(DateTime?))));
            }

            if (!string.IsNullOrWhiteSpace(txtFKeyId.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "KeyId"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFKeyId.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(ddlFVehicleNum.SelectedValue)&& !ddlFVehicleNum.SelectedValue.Equals("-1"))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FVehicleNum"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlFVehicleNum.SelectedValue)));
            }

            Expression<Func<LHDispatchCenter, bool>> predicate = Expression.Lambda<Func<LHDispatchCenter, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, "KeyId"), parameter);

            return predicate;
        }

        #endregion

        protected void Grid3_RowDataBound(object sender, GridRowEventArgs e)
        {
            var item = e.DataItem as LHDispatchCenter;

            if (item != null)
            {
                //if (item.FFlag == 0)
                //{
                //    for (int i = 0; i < e.Values.Length; i++)
                //    {
                //        if (!e.Values[i].ToString().Contains("#@TPL@"))
                //            e.Values[i] = String.Format("<span class=\"{0}\">{1}</span>", "colorred", e.Values[i].ToString().Contains("#@TPL@") ? "已作废" : e.Values[i]);
                //    }
                //}

                //if (item.FAuditFlag == 0)
                //{
                //    for (int i = 0; i < e.Values.Length; i++)
                //    {
                //        if (!e.Values[i].ToString().Contains("#@TPL@"))
                //            e.Values[i] = String.Format("<span class=\"{0}\">{1}</span>", "colorcoral", e.Values[i].ToString().Contains("#@TPL@") ? "已作废" : e.Values[i]);
                //    }
                //}

                if (item.FAuditFlag == 1)
                {

                    //if (item.FStatus == 50)
                    //{
                    for (int i = 0; i < e.Values.Length; i++)
                    {
                        if (!e.Values[i].ToString().Contains("#@TPL@"))
                            e.Values[i] = String.Format("<span class=\"{0}\">{1}</span>", "colorgreen", e.Values[i].ToString().Contains("#@TPL@") ? "已作废" : e.Values[i]);
                    }
                    //}
                    //else
                    //{
                    //    for (int i = 0; i < e.Values.Length; i++)
                    //    {
                    //        if (!e.Values[i].ToString().Contains("#@TPL@"))
                    //            e.Values[i] = String.Format("<span class=\"{0}\">{1}</span>", "colorgreen", e.Values[i].ToString().Contains("#@TPL@") ? "已作废" : e.Values[i]);
                    //    }
                    //}
                }
                else if (item.FAuditFlag == 1)
                {
                    for (int i = 0; i < e.Values.Length; i++)
                    {
                        if (!e.Values[i].ToString().Contains("#@TPL@"))
                            e.Values[i] = String.Format("<span class=\"{0}\">{1}</span>", "colorred", e.Values[i].ToString().Contains("#@TPL@") ? "已作废" : e.Values[i]);
                    }
                }
            }
        }
    }
}