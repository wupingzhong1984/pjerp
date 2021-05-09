using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;


namespace Enterprise.IIS.business.StockIn
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
        private StockInService _stockInService;

        /// <summary>
        ///     数据服务_视图
        /// </summary>
        protected StockInService StockInService
        {
            get { return _stockInService ?? (_stockInService = new StockInService()); }
            set { _stockInService = value; }
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

                GasHelper.DropDownListBillStatusDataBind(ddlFStatus);

                dateBegin.SelectedDate = DateTime.Now;
                dateEnd.SelectedDate = DateTime.Now;

                btnBatchDelete.ConfirmText = "你确定要执行作废操作吗？";

                BindDataGrid();
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

                    var inInterface = new T6Interface();

                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    //生产入库
                    string cate = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][3].ToString();

                    if (cate.Equals("充气入仓"))
                    {
                        Alert.Show(inInterface.SubmitProductStockIn(sid, CurrentUser.AccountComId), "消息提示", MessageBoxIcon.Information);

                    }
                    else
                    {
                        Alert.Show(inInterface.SubmitStockIn(sid, CurrentUser.AccountComId), "消息提示", MessageBoxIcon.Information);

                    }

                    BindDataGrid();
                }
            }
            catch (Exception)
            {
                Alert.Show("同步失败！", MessageBoxIcon.Warning);
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

        /// <summary>
        ///     作废
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                Log(string.Format(@"作废单据号:{0}成功。", selectIds));

                //
                foreach (var ids in selectIds)
                {
                    var status = new LHBillStatus();
                    status.FCompanyId = CurrentUser.AccountComId;
                    status.FDeptId = CurrentUser.AccountOrgId;
                    status.FDate = DateTime.Now;
                    status.FOperator = CurrentUser.AccountName;
                    status.FActionName = EnumDescription.GetFieldText(GasEnumBillStauts.Voided);
                    status.KeyId = ids;
                    status.FMemo = string.Format("单据号{0}被{1}作废处理。", ids, CurrentUser.AccountName);

                    GasHelper.AddBillStatus(status);
                }

                StockInService.Update(p => selectIds.Contains(p.KeyId), p => new LHStockIn
                {
                    FFlag = 0, //
                    FStatus = Convert.ToInt32(GasEnumBillStauts.Voided), //
                    FProgress = Convert.ToInt32(GasEnumBillStauts.Voided)
                });

                Alert.Show("作废成功！", MessageBoxIcon.Information);
                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("作废失败！", MessageBoxIcon.Warning);
            }
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

                    var parms = new Dictionary<string, object>();
                    parms.Clear();

                    parms.Add("@keyid", sid);
                    parms.Add("@Date", ServiceDateTime.ToShortDateString());

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
                    //PageContext.RegisterStartupScript(
                    //    Window1.GetShowReference(string.Format("./Edit.aspx?KeyId={0}&action=2",
                    //        sid), "入仓单"));

                    PageContext.RegisterStartupScript(string.Format("openEditUI('{0}');", sid));


                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=入仓单{0}.xls", SequenceGuid.GetGuidReplace()));
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
            int output;

            dynamic orderingSelector;
            Expression<Func<LHStockIn, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<LHStockIn> list = StockInService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<LHStockIn, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHStockIn));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            // 单据类型
            int ftype = Convert.ToInt32(GasEnumBill.ProductIn);
            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FType"), Expression.Constant(ftype, typeof(int?))));

            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FDeleteFlag"), Expression.Constant(0, typeof(int?))));

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

            // 姓名
            if (!string.IsNullOrWhiteSpace(txtFName.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "CreateBy"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFName.Text.Trim())));
            }

            // 登陆帐号
            if (!string.IsNullOrWhiteSpace(txtFItemName.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FCode"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFItemName.Text.Trim())));
            }
            // 登陆帐号
            if (!string.IsNullOrWhiteSpace(txtFKeyId.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "KeyId"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFKeyId.Text.Trim())));
            }

            Expression<Func<LHStockIn, bool>> predicate = Expression.Lambda<Func<LHStockIn, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion
    }
}