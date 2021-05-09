using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;
using FineUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace Enterprise.IIS.business.Vehicle
{
    public partial class InitDispatchCenter : PageBase
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
        ///     数据服务
        /// </summary>
        private StockoutDispatchServices _stockOutService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockoutDispatchServices StockOutService
        {
            get { return _stockOutService ?? (_stockOutService = new StockoutDispatchServices()); }
            set { _stockOutService = value; }
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
                SetPermissionButtons(Toolbar1);

                //单据状态
                //GasHelper.DropDownListBillStatusDataBind(ddlFStatus);

                //btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./Edit.aspx?action=1"), "发货单");

                GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区

                dateBegin.SelectedDate = DateTime.Now;

                dateEnd.SelectedDate = DateTime.Now;


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
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    if (e.CommandName == "actView")
                    {
                        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}');", sid));
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            LHStockOut item = e.DataItem as LHStockOut;

            if (item != null)
            {
                if (item.FFlag == 0)
                {
                    for (int i = 0; i < e.Values.Length; i++)
                    {
                        if (!e.Values[i].ToString().Contains("#@TPL@"))
                            e.Values[i] = string.Format("<span class=\"{0}\">{1}</span>", "colorred", e.Values[i].ToString().Contains("#@TPL@") ? "已作废" : e.Values[i]);
                    }
                }
                IQueryable<LHStockOutDetails> lHStockOutDetails = new StockOutDetailsService().Where(p => p.KeyId == item.KeyId);
                List<string> inbottle = (from x in lHStockOutDetails
                                         select x.FBottleInCode).ToList();
                e.Values[20] = string.Join("|", inbottle);

                List<string> outbottle = (from x in lHStockOutDetails
                                          select x.FBottleOutCode).ToList();
                e.Values[21] = string.Join("|", outbottle);
            }
        }

        /// <summary>
        ///     作废
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        ///     转排管车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTubeSales_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        ///     复制单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    Dictionary<string, object> parms = new Dictionary<string, object>();
                    parms.Clear();

                    parms.Add("@keyid", sid);
                    parms.Add("@Date", ServiceDateTime.ToShortDateString());
                    parms.Add("@companyId", CurrentUser.AccountComId);
                    string keyId = SqlService.ExecuteProcedureCommand("proc_CopySales", parms).Tables[0].Rows[0][0].ToString();

                    Alert.Show(string.Format("复制单据完成，新单据号：{0}", keyId), MessageBoxIcon.Information);

                    BindDataGrid();
                }
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     审核单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAudit_Click(object sender, EventArgs e)
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();
                        PageContext.RegisterStartupScript(
                        Window2.GetShowReference(string.Format("../../Common/WinAudit.aspx?KeyId={0}&action=7&Bill=1",
                            sid), "审核"));
                }
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     编辑
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    string state = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();
                    if (state.Equals("1"))
                    {
                        Alert.Show("已派送单据不能编辑！", MessageBoxIcon.Information);
                    }
                    else
                        PageContext.RegisterStartupScript(string.Format("openEditUI('{0}');", sid));
                    //无权上传该销售单据，该单据由业务员
                    //string salesman1 = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][3].ToString();
                    //string salesman2 = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][4].ToString();

                    //if (salesman1.Equals(CurrentUser.AccountName)//
                    //    || salesman2.Equals(CurrentUser.AccountName))
                    //{
                   
                    //}
                    //else
                    //{
                    //    Alert.Show("无权编辑他人销售发货单据", MessageBoxIcon.Information);
                    //    return;
                    //}
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnData_Click(object sender, EventArgs e)
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
                    //验证是否已作废
                    string flag = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();
                    if (flag.Equals("0"))
                    {
                        Alert.Show("单据已作废，不允许上传", MessageBoxIcon.Information);
                        return;
                    }

                    //验证是否已提交
                    string t6Status = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][2].ToString();
                    if (t6Status.Equals("已同步"))
                    {
                        Alert.Show("单据已同步，不允许再次上传", MessageBoxIcon.Information);
                        //return;
                    }

                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    T6Interface inInterface = new T6Interface();
                    Alert.Show(inInterface.SubmitSales(sid, CurrentUser.AccountComId), "消息提示", MessageBoxIcon.Information);

                    BindDataGrid();
                }
            }
            catch (Exception)
            {
                Alert.Show("同步失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, EventArgs e)
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("LodopPrinter('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }



        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrintSwap_Click(object sender, EventArgs e)
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("LodopPrinterSwap('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrintBlank_Click(object sender, EventArgs e)
        {
            try
            {
                PageContext.RegisterStartupScript(string.Format("LodopPrinterBlank();"));
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
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
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=预调度单{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
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
        private IEnumerable<string> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            string[] selectIds = new string[selections.Length];

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

            dynamic orderingSelector;
            int output;
            Expression<Func<LHStockOutDispatch, bool>> predicate = BuildPredicate(out  orderingSelector);

            //取数据源
            IQueryable<LHStockOutDispatch> list = StockOutService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out  output);

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
        private Expression<Func<LHStockOutDispatch, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHStockOutDispatch));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FFlag"), Expression.Constant(1, typeof(int))));

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

            if (ddlFDistributionPoint.SelectedValue != "-1")
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FDistributionPoint"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlFDistributionPoint.SelectedValue)));
            }

            
            if (!string.IsNullOrWhiteSpace(txtFKeyId.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "KeyId"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFKeyId.Text.Trim())));
            }

            Expression<Func<LHStockOutDispatch, bool>> predicate = Expression.Lambda<Func<LHStockOutDispatch, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion

        protected void btnBH_Click(object sender, EventArgs e)
        {

        }

        protected void rbtList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private string GetSelectKeyIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            StringBuilder keys = new StringBuilder();

            for (int i = 0; i < selections.Length; i++)
            {
                keys.AppendFormat("{0},", Grid1.DataKeys[selections[i]][0]);
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

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyid", CurrentUser.AccountComId);
                parms.Add("@DispatchId", sid);

                DataTable tb = SqlService.ExecuteProcedureCommand("proc_StockoutDispatch", parms).Tables[0];
                //绑定数据源
                Grid2.DataSource = tb;
                Grid2.DataBind();

                DataTable table = tb;

                if (table != null && table.Rows.Count > 0)
                {
                    decimal sumFQty = 0.00M;

                    sumFQty = Convert.ToDecimal(table.Compute("sum(FQty)", "true"));

                    JObject summary = new JObject
                {
                    {"Keyid", "合计"},
                    {"FDate", ""},
                    {"FName", ""},
                    {"pname", ""},
                    {"FQty", sumFQty},
                };

                    Grid1.SummaryData = summary;
                }
                else
                {

                    JObject summary = new JObject
                {
                    {"Keyid", "合计"},
                    {"FDate", ""},
                    {"FName", ""},
                    {"pname", ""},
                    {"FQty", 0},
                };

                    Grid1.SummaryData = summary;
                }
            }
            catch (Exception)
            {
                Alert.Show("查看失败！", MessageBoxIcon.Warning);
            }
        }

    }
}