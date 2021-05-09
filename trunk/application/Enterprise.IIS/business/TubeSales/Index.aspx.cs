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
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.TubeSales
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
        ///     数据服务
        /// </summary>
        private StockOutService _stockOutService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutService StockOutService
        {
            get { return _stockOutService ?? (_stockOutService = new StockOutService()); }
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
                GasHelper.DropDownListBillStatusDataBind(ddlFStatus);

                //btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./Edit.aspx?action=1"), "发货单");

                dateBegin.SelectedDate = DateTime.Now;
                dateEnd.SelectedDate = DateTime.Now;

                btnBatchDelete.ConfirmText = "你确定要执行作废操作吗？";

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
            var item = e.DataItem as LHStockOut;

            if (item != null)
                if (item.FFlag == 0)
                {
                    for (int i = 0; i < e.Values.Length; i++)
                    {
                        if (!e.Values[i].ToString().Contains("#@TPL@"))
                            e.Values[i] = String.Format("<span class=\"{0}\">{1}</span>", "colorred", e.Values[i].ToString().Contains("#@TPL@") ? "已作废" : e.Values[i]);
                    }
                }
        }

        /// <summary>
        ///     转排管车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTubeSales_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                Log(string.Format(@"单据转换成排管车业务:{0}成功。", selectIds));

                StockOutService.Update(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.KeyId), p => new LHStockOut
                {
                    FType = 1
                });

                foreach (string item in selectIds)
                {
                    tmsModel t = new tmsModel();
                    LHStockOut hStockOut = StockOutService.Where(p => p.KeyId == item).FirstOrDefault();
                    List<LHStockOut> lHs = new List<LHStockOut>
                        {
                            hStockOut
                        };
                    t.stockOutList = lHs;


                    new HttpRequest().httpRequest(t, "open/dbo/dboData");
                }


                Alert.Show("转发货单业务完成！", MessageBoxIcon.Information);

                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("作废失败！", MessageBoxIcon.Warning);
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

                    var parms = new Dictionary<string, object>();
                    parms.Clear();
                    parms.Add("@KeyId", ids);
                    parms.Add("@companyId", CurrentUser.AccountComId);
                    parms.Add("@Bill", 1);
                    SqlService.ExecuteProcedureNonQuery("proc_DeleteFlag", parms);

                }

                StockOutService.Update(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.KeyId), p => new LHStockOut
                {
                    FFlag = 0, //
                    FStatus = Convert.ToInt32(GasEnumBillStauts.Voided), //
                    FProgress = Convert.ToInt32(GasEnumBillStauts.Voided)
                });


                foreach (string item in selectIds)
                {

                    tmsModel t = new tmsModel();
                    LHStockOut hStockOut = StockOutService.Where(p => p.KeyId == item).FirstOrDefault();
                    List<LHStockOut> lHs = new List<LHStockOut>
                        {
                            hStockOut
                        };
                    t.stockOutList = lHs;
                    t.stockOutDetailsList = new StockOutDetailsService().Where(p => p.KeyId == item).ToList();


                    new HttpRequest().httpRequest(t, "open/dbo/dboData");

                }
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

                    PageContext.RegisterStartupScript(string.Format("openEditUI('{0}');", sid));
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
            string showmessage = "";
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    for (int i = 0; i < Grid1.SelectedRowIndexArray.Length; i++)
                    {
                        //验证是否已作废
                        string flag = Grid1.DataKeys[Grid1.SelectedRowIndexArray[i]][1].ToString();
                        if (flag.Equals("0"))
                        {
                            continue;
                        }

                        //验证是否已提交
                        string t6Status = Grid1.DataKeys[Grid1.SelectedRowIndexArray[i]][2].ToString();
                        if (t6Status.Equals("已同步"))
                        {
                            continue;
                        }

                        string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[i]][0].ToString();

                        var inInterface = new T6Interface();
                        string successCode= inInterface.SubmitSales(sid, CurrentUser.AccountComId);
                            showmessage = successCode;

                    }

                    Alert.Show(showmessage);

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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=发货单{0}.xls", SequenceGuid.GetGuidReplace()));
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
            Expression<Func<LHStockOut, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<LHStockOut> list = StockOutService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            Grid1.RecordCount = output;

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();

            //合计
            var summary = new JObject
                    {
                        {"FDistributionPoint", "合计"},
                        {"FAmount", list.AsQueryable()//
                        .Where(p=>p.FFlag==1)//
                        .Sum(p=>p.FAmount)},
                    };

            Grid1.SummaryData = summary;

            ddlPageSize.SelectedValue = Grid1.PageSize.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<LHStockOut, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHStockOut));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            // 单据类型
            int ftype = Convert.ToInt32(GasEnumBill.TubeSales);
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
                    Expression.Call(Expression.Property(parameter, "FCode"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFCode.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(txtFKeyId.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "KeyId"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFKeyId.Text.Trim())));
            }

            Expression<Func<LHStockOut, bool>> predicate = Expression.Lambda<Func<LHStockOut, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion

    }
}