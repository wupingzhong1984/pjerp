using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;


namespace Enterprise.IIS.business.Item
{
    public partial class GasFormulaSet : PageBase
    {
        /// <summary>
        ///     AppendToEnd
        /// </summary>
        private bool AppendToEnd
        {
            get
            {
                return ViewState["_AppendToEnd"] != null && Convert.ToBoolean(ViewState["_AppendToEnd"]);
            }
        }

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
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "DESC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ItemsService _itemsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ItemsService ItemsService
        {
            get { return _itemsService ?? (_itemsService = new ItemsService()); }
            set { _itemsService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ItemsFormulaService _itemsFormulaService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ItemsFormulaService ItemsFormulaService
        {
            get { return _itemsFormulaService ?? (_itemsFormulaService = new ItemsFormulaService()); }
            set { _itemsFormulaService = value; }
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
                ViewState["_AppendToEnd"] = true;

                SetPermissionButtons(Toolbar1);
                
                tbxFItemCode.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFItemCode.ClientID)
                                                    + Window1.GetShowReference("../../Common/WinFormula.aspx");

                //删除选中单元格的客户端脚本
                string deleteScript = DeleteScript();

                //新增
                var defaultObj = new JObject
                {
                    {"FItemCode", ""},
                    {"FItemName", ""},
                    {"FSpec", ""},
                    {"FUnit", ""},
                    {"FPrice", ""},
                    {"FQty", "0"},
                    {
                        "colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>", //
                            deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))
                    },
                };

                // 在第一行新增一条数据
                btnAdd.OnClientClick = Grid2.GetAddNewRecordReference(defaultObj, AppendToEnd);


                BindDataGrid();
            }
            else
            {
                if (GetRequestEventArgument().Contains("reloadGrid:"))
                {
                    //查找所选商品代码，查访产品集合
                    string keys = GetRequestEventArgument().Split(':')[1];

                    var values = keys.Split(',');

                    string codes = String.Empty;
                    for (int i = 0; i < values.Count(); i++)
                    {
                        codes += string.Format("'{0}',", values[i]);
                    }

                    var value = codes.Substring(0, codes.Length - 1);

                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_SalesItem a WHERE a.FItemCode IN ({0}) and a.FCompanyId={1}", value,CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];

                        if (Grid1.SelectedRowIndexArray != null //
                            && Grid1.SelectedRowIndexArray.Count() > 0 //
                            && Grid1.SelectedRowIndexArray[0] != null //
                            && Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0] != null)
                        {
                            string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                var formula = new LHItemsFormula
                                {
                                    FItemCode = table.Rows[i]["FItemCode"].ToString(),
                                    FQty = 0,
                                    FCompanyId = CurrentUser.AccountComId,
                                    FCode = sid,
                                    FFlag = 1
                                };

                                ItemsFormulaService.AddEntity(formula);

                                //日志
                                //switch (Actions)
                                //{
                                //    case WebAction.Add:
                                //        break;
                                //    case WebAction.Edit:
                                //        //记录一下当前新增人操作内容
                                //        var detailslog = new LHStockOutDetails_Log
                                //        {
                                //            FUpdateBy = CurrentUser.AccountName,
                                //            FUpdateDate = DateTime.Now,
                                //            FItemCode = details.FItemCode,
                                //            FPrice = price,
                                //            FQty = 1,
                                //            FAmount = price,
                                //            FBottleQty = 0,
                                //            FBottleOweQty = 0,
                                //            KeyId = txtKeyId.Text.Trim(),
                                //            FBottle = details.FBottle,
                                //            FStatus = "新增",
                                //            FCompanyId = CurrentUser.AccountComId,
                                //            FMemo = string.Format(@"时间：{0} 操作人：{1}", DateTime.Now, CurrentUser.AccountName)
                                //        };

                                //        StockOutDetailsLogService.Add(detailslog);

                                //        break;
                                //}
                            }

                        }
                        
                        ItemsFormulaService.SaveChanges();

                        //重新绑定值
                        BindDataFormulaGrid();
                    }
                }
            }
        }

        private void BindDataFormulaGrid()
        {
            if (Grid1.SelectedRowIndexArray.Count() > 0)
            {
                string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();
                if (!string.IsNullOrEmpty(sid))
                {
                    var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_Formula a WHERE a.FCode='{0}' and a.FCompanyId={1}", sid, CurrentUser.AccountComId));

                    //绑定数据源
                    Grid2.DataSource = source;
                    Grid2.DataBind();

                    var table = source.Tables[0];

                    if (table != null && table.Rows.Count > 0)
                    {
                        decimal sumFQty = 0.00M;
                        //decimal sumFAmount = 0.00M;

                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            sumFQty += Convert.ToDecimal(table.Rows[i]["FQty"]);
                            //sumFAmount += Convert.ToDecimal(table.Rows[i]["FAmount"]);
                        }

                        var summary = new JObject
                    {
                        {"FItemCode", "合计"},
                        {"FQty", sumFQty},
                        //{"FAmount", sumFAmount}
                    };
                        Grid2.SummaryData = summary;
                    }
                }
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
        }

        /// <summary>
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete" || e.CommandName == "Add")
            {
                var datakey = Convert.ToInt32(Grid2.DataKeys[e.RowIndex][0]);

                ItemsFormulaService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);

                BindDataFormulaGrid();
            }
        }

        protected void Grid1_RowSelect(object sender, GridRowSelectEventArgs e)
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

                    BindDataFormulaGrid();
                }
            }
            catch (Exception)
            {
                Alert.Show("加载失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     单元格编辑与修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid2_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;

            //新增行事件
            //var addList = Grid1.GetNewAddedList();
            //foreach (var add in addList)
            //{
            //    var dictValues = add.Values;

            //    //商品代码
            //    var firstOrDefault = dictValues.First();

            //    if (firstOrDefault != null && !string.IsNullOrEmpty(firstOrDefault.ToString()))
            //    {
            //        DataSet dataSet = GasHelper.GetSalesItem(firstOrDefault.ToString(), CurrentUser.AccountComId);

            //        DataTable table = dataSet.Tables[0];

            //        if (table != null && table.Rows.Count > 0)
            //        {
            //            decimal price = GasHelper.GeCustomerPrice(txtFCode.Text.Trim(),//
            //                table.Rows[0]["FItemCode"].ToString(), CurrentUser.AccountComId);

            //            table.Rows[0]["FPrice"] = price;

            //            var details = new LHStockOutDetails
            //            {
            //                FItemCode = table.Rows[0]["FItemCode"].ToString(),
            //                FPrice = price,
            //                FQty = 1,
            //                FAmount = price,
            //                FBottleQty = 0,
            //                FBottleOweQty = 0,
            //                FRecycleQty = 0,
            //                FCompanyId = CurrentUser.AccountComId,
            //                KeyId = txtKeyId.Text.Trim(),
            //                FBottle = table.Rows[0]["FBottleCode"].ToString(),
            //                FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString())
            //            };

            //            switch (Actions)
            //            {
            //                case WebAction.Add:
            //                    break;
            //                case WebAction.Edit:
            //                    //记录一下当前新增人操作内容
            //                    var detailslog = new LHStockOutDetails_Log
            //                    {
            //                        FUpdateBy = CurrentUser.AccountName,
            //                        FUpdateDate = DateTime.Now,
            //                        FItemCode = table.Rows[0]["FItemCode"].ToString(),
            //                        FPrice = price,
            //                        FQty = 1,
            //                        FAmount = price,
            //                        FBottleQty = 0,
            //                        FCompanyId = CurrentUser.AccountComId,
            //                        FBottleOweQty = 0,
            //                        KeyId = txtKeyId.Text.Trim(),
            //                        FBottle = table.Rows[0]["FBottleCode"].ToString(),
            //                        FStatus = "新增",
            //                        FMemo = string.Format(@"时间：{0} 新增人：{1}", DateTime.Now, CurrentUser.AccountName)
            //                    };

            //                    StockOutDetailsLogService.Add(detailslog);

            //                    break;
            //            }

            //            StockOutDetailsService.Add(details);
            //        }
            //    }
            //}

            //编辑行事件
            var dictModified = Grid2.GetModifiedDict();
            foreach (var index in dictModified.Keys)
            {
                int datakey = Convert.ToInt32(Grid2.DataKeys[index][0].ToString());

                foreach (var dictValue in dictModified.Values)
                {
                    foreach (KeyValuePair<string, object> keyValuePair in dictValue)
                    {
                        string key = keyValuePair.Key;
                        string value = keyValuePair.Value.ToString();

                        var details = ItemsFormulaService.Where(p => p.FId == datakey).FirstOrDefault();

                        //写入原始，通过存储过程完成明细复制
                        //var parms = new Dictionary<string, object>();
                        //parms.Clear();

                        //parms.Add("@fid", datakey);
                        //parms.Add("@opr", CurrentUser.AccountName);
                        //parms.Add("@companyId", CurrentUser.AccountComId);
                        //SqlService.ExecuteProcedureCommand("proc_StockOutDetails_Log", parms);

                        if (details != null)
                        {
                            switch (key)
                            {

                                case "FQty":
                                    details.FQty = Convert.ToDecimal(value);
                                    break;
                                
                            }

                            //var detailslog = new LHStockOutDetails_Log
                            //{
                            //    FUpdateBy = CurrentUser.AccountName,
                            //    FUpdateDate = DateTime.Now,
                            //    FItemCode = details.FItemCode,
                            //    FPrice = details.FPrice,
                            //    FQty = details.FQty,
                            //    FAmount = details.FAmount,
                            //    FBottleQty = details.FBottleQty,
                            //    FBottleOweQty = details.FBottleOweQty,
                            //    KeyId = details.KeyId,
                            //    FBottle = details.FBottle,
                            //    FCompanyId = CurrentUser.AccountComId,
                            //    FRecycleQty = details.FRecycleQty,
                            //    FStatus = "变更",
                            //    FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
                            //};

                            //StockOutDetailsLogService.Add(detailslog);

                        }

                        ItemsFormulaService.SaveChanges();
                    }
                }
            }

            BindDataFormulaGrid();
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDispatch_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFItemCode_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=产品配方{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 保存配方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

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
            Expression<Func<LHItems, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<LHItems> list = ItemsService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<LHItems, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHItems));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCateId"), Expression.Constant("2000")));

            //expr = Expression.And(expr,
            //        Expression.Call(Expression.Property(parameter, "FUnit"), methodInfo,
            //    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
            //            Expression.Constant("瓶")));

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

            if (!string.IsNullOrWhiteSpace(txtFSpell.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FSpell"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFSpell.Text.Trim())));
            }

            Expression<Func<LHItems, bool>> predicate = Expression.Lambda<Func<LHItems, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        /// <summary>
        ///     删除选中行的脚本
        /// </summary>
        /// <returns></returns>
        private string DeleteScript()
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, Grid1.GetDeleteSelectedRowsReference(), String.Empty);
        }

        #endregion
    }
}