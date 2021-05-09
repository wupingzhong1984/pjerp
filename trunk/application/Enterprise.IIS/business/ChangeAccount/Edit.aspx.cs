using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.ChangeAccount
{
    public partial class Edit : PageBase
    {
        #region  Service
        /// <summary>
        ///     AppendToEnd
        /// </summary>
        private bool AppendToEnd
        {
            get
            {
                return ViewState["_AppendToEnd"] != null //
                    && Convert.ToBoolean(ViewState["_AppendToEnd"]);
            }
        }

        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null //
                    ? ViewState["SortField"].ToString() : "KeyId";
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
                string sort = ViewState["SortDirection"] != null //
                    ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ViewUnitAllService _viewUnitAllService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected ViewUnitAllService ViewUnitAllService
        {
            get { return _viewUnitAllService ?? (_viewUnitAllService = new ViewUnitAllService()); }
            set { _viewUnitAllService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private InitMonthServie _initMonthServie;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected InitMonthServie InitMonthServie
        {
            get { return _initMonthServie ?? (_initMonthServie = new InitMonthServie()); }
            set { _initMonthServie = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private InitMonthDetailsService _initMonthDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected InitMonthDetailsService InitMonthDetailsService
        {
            get
            {
                return _initMonthDetailsService ?? //
                    (_initMonthDetailsService = new InitMonthDetailsService());
            }
            set { _initMonthDetailsService = value; }
        }

        ///// <summary>
        /////     Log
        ///// </summary>
        //private StockOutDetailsLogService _stockOutDetailsLogService;
        ///// <summary>
        /////     Log
        ///// </summary>
        //protected StockOutDetailsLogService StockOutDetailsLogService
        //{
        //    get { return _stockOutDetailsLogService ?? (_stockOutDetailsLogService = new StockOutDetailsLogService()); }
        //    set { _stockOutDetailsLogService = value; }
        //}

        ///// <summary>
        /////     Log
        ///// </summary>
        //private StockOutLogService _stockOutLogService;
        ///// <summary>
        /////     Log
        ///// </summary>
        //protected StockOutLogService StockOutLogService
        //{
        //    get { return _stockOutLogService ?? (_stockOutLogService = new StockOutLogService()); }
        //    set { _stockOutLogService = value; }
        //}

        /// <summary>
        ///     客户档案
        /// </summary>
        private vm_UnitAll _vmUnitAll;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected vm_UnitAll UnitAll
        {
            get
            {
                return _vmUnitAll ?? (_vmUnitAll = ViewUnitAllService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim()//
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _vmUnitAll = value; }
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
        ///     
        /// </summary>
        private LHInitMonth _initMonth;

        /// <summary>
        ///     
        /// </summary>
        protected LHInitMonth StockOut
        {
            get
            {
                return _initMonth ?? (_initMonth = InitMonthServie.FirstOrDefault(p => p.KeyId == KeyId //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _initMonth = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string KeyId
        {
            get { return Request["KeyId"]; }
        }

        /// <summary>
        ///     
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction)Int32.Parse(s);
            }
        }
        #endregion

        #region Protected Method

        /// <summary>
        ///     Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
            else
            {
                #region 弹窗加产品
                //if (GetRequestEventArgument().Contains("reloadGrid:"))
                //{
                //    //查找所选商品代码，查访产品集合
                //    string keys = GetRequestEventArgument().Split(':')[1];

                //    var values = keys.Split(',');

                //    string codes = String.Empty;
                //    for (int i = 0; i < values.Count(); i++)
                //    {
                //        codes += string.Format("'{0}',", values[i]);
                //    }

                //    var value = codes.Substring(0, codes.Length - 1);

                //    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_SalesItem a WHERE a.FItemCode IN ({0}) and a.FCompanyId={1}", value, CurrentUser.AccountComId));

                //    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                //    {
                //        var table = data.Tables[0];
                //        for (int i = 0; i < table.Rows.Count; i++)
                //        {
                //            var details = new LHInitMonthDetails();

                //            details.FItemCode = table.Rows[i]["FItemCode"].ToString();
                //            details.FQty = 1;
                //            details.FCompanyId = CurrentUser.AccountComId;
                //            details.KeyId = txtKeyId.Text.Trim();
                //            details.FCateId = Convert.ToInt32(table.Rows[i]["FId"].ToString());

                //            InitMonthDetailsService.AddEntity(details);

                //            //日志
                //            //switch (Actions)
                //            //{
                //            //    case WebAction.Add:
                //            //        break;
                //            //    case WebAction.Edit:
                //            //        //记录一下当前新增人操作内容
                //            //        var detailslog = new LHStockOutDetails_Log
                //            //        {
                //            //            FUpdateBy = CurrentUser.AccountName,
                //            //            FUpdateDate = DateTime.Now,
                //            //            FItemCode = details.FItemCode,
                //            //            FPrice = price,
                //            //            FQty = 1,
                //            //            FAmount = price,
                //            //            FBottleQty = 1,
                //            //            FBottleOweQty = 0,
                //            //            KeyId = txtKeyId.Text.Trim(),
                //            //            FBottle = details.FBottle,
                //            //            FStatus = "新增",
                //            //            FCompanyId = CurrentUser.AccountComId,
                //            //            FMemo = string.Format(@"时间：{0} 操作人：{1}", DateTime.Now, CurrentUser.AccountName)
                //            //        };
                //            //        StockOutDetailsLogService.Add(detailslog);
                //            //        break;
                //            //}
                //        }

                //        InitMonthDetailsService.SaveChanges();

                //        //重新绑定值
                //        BindDataGrid();
                //    }
                //}
                #endregion

                #region 更新合计
                //if (GetRequestEventArgument() == "UPDATE_SUMMARY")
                //{
                //    // 页面要求重新计算合计行的值
                //    OutputSummaryData();

                //    //写入
                //    ModifiedGrid();

                //    // 为了保持前后台上传，回发更新合计行值后，必须进行数据绑定或者提交更改
                //    Grid1.CommitChanges();
                //}
                #endregion
            }
        }

        #region OutputSummaryData
        /// <summary>
        ///     OutputSummaryData
        /// </summary>
        private void OutputSummaryData()
        {
            //decimal sumFQty = 0.00M;
            ////decimal sumFAmount = 0.00M;
            ////decimal sumFRecycleQty = 0.00M;
            ////decimal sumFBottleQty = 0.00M;

            //foreach (JObject mergedRow in Grid1.GetMergedData())
            //{
            //    JObject values = mergedRow.Value<JObject>("values");

            //    sumFQty += values.Value<decimal>("FQty");
            //    //sumFAmount += values.Value<decimal>("FAmount");
            //    //sumFRecycleQty += values.Value<decimal>("FRecycleQty");
            //    //sumFBottleQty += values.Value<decimal>("FBottleQty");
            //}

            //JObject summary = new JObject();
            //summary.Add("FQty", sumFQty);
            ////summary.Add("FAmount", sumFAmount);
            ////summary.Add("FRecycleQty", sumFRecycleQty);
            ////summary.Add("FBottleQty", sumFBottleQty);

            //Grid1.SummaryData = summary;
        }

        #endregion

        /// <summary>
        ///     btnSubmit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                using (var trans = new TransactionScope())
                {
                    switch (Actions)
                    {
                        case WebAction.Add:
                            isSucceed = SubmintAdd();
                            break;
                        case WebAction.Edit:
                            isSucceed = SubmintEdit();
                            break;
                    }

                    trans.Complete();
                }
            }
            catch (Exception)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    PageContext.RegisterStartupScript("closeActiveTab();");
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        ///     BindDataGrid
        /// </summary>
        private void BindDataGrid()
        {
            //var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_MonthInitDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            ////绑定数据源
            //Grid1.DataSource = source;
            //Grid1.DataBind();

            //var table = source.Tables[0];

            //if (table != null && table.Rows.Count > 0)
            //{
            //    decimal sumFQty = 0.00M;
            //    //decimal sumFAmount = 0.00M;
            //    //decimal sumFRecycleQty = 0.00M;
            //    //decimal sumFBottleQty = 0.00M;

            //    sumFQty = Convert.ToDecimal(table.Compute("sum(FQty)", "true"));
            //    //sumFAmount = Convert.ToDecimal(table.Compute("sum(FAmount)", "true"));
            //    //sumFRecycleQty = Convert.ToDecimal(table.Compute("sum(FRecycleQty)", "true"));
            //    //sumFBottleQty = Convert.ToDecimal(table.Compute("sum(FBottleQty)", "true"));

            //    var summary = new JObject
            //    {
            //        {"FItemCode", "合计"},
            //        {"FQty", sumFQty},
            //        //{"FAmount", sumFAmount},
            //        //{"FRecycleQty", sumFRecycleQty},
            //        //{"sumFBottleQty", sumFBottleQty},
            //    };

            //    Grid1.SummaryData = summary;
            //}
            //else
            //{
            //    var summary = new JObject
            //    {
            //        {"FItemCode", "合计"},
            //        {"FQty", 0},
            //        //{"FAmount", 0},
            //        //{"FRecycleQty", 0},
            //        //{"sumFBottleQty", 0},
            //    };

            //    Grid1.SummaryData = summary;
            //}
        }

        /// <summary>
        ///     单元格编辑与修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            //Window1.Hidden = true;
            //Window2.Hidden = true;
            //Window3.Hidden = true;

            ////新增行事件
            //var addList = Grid1.GetNewAddedList();

            //#region AddRow
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
            //            //decimal price = GasHelper.GeCustomerPrice(txtFCode.Text.Trim(),//
            //            //    table.Rows[0]["FItemCode"].ToString(), CurrentUser.AccountComId);

            //            //table.Rows[0]["FPrice"] = price;

            //            var details = new LHInitMonthDetails()
            //            {
            //                FItemCode = table.Rows[0]["FItemCode"].ToString(),
            //                FQty = 1,
            //                FCompanyId = CurrentUser.AccountComId,
            //                KeyId = txtKeyId.Text.Trim(),
            //                FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString()),
            //                FMemo = "",
            //            };

            //            switch (Actions)
            //            {
            //                case WebAction.Add:
            //                    break;
            //                case WebAction.Edit:
            //                    //记录一下当前新增人操作内容
            //                    //var detailslog = new LHStockOutDetails_Log
            //                    //{
            //                    //    FUpdateBy = CurrentUser.AccountName,
            //                    //    FUpdateDate = DateTime.Now,
            //                    //    FItemCode = table.Rows[0]["FItemCode"].ToString(),
            //                    //    FQty = 1,
            //                    //    FAmount = price,
            //                    //    FBottleQty = 0,
            //                    //    FCompanyId = CurrentUser.AccountComId,
            //                    //    FBottleOweQty = 0,
            //                    //    KeyId = txtKeyId.Text.Trim(),
            //                    //    FBottle = table.Rows[0]["FBottleCode"].ToString(),
            //                    //    FStatus = "新增",
            //                    //    FMemo = string.Format(@"时间：{0} 新增人：{1}", DateTime.Now, CurrentUser.AccountName)
            //                    //};

            //                    //StockOutDetailsLogService.Add(detailslog);

            //                    break;
            //            }

            //            InitMonthDetailsService.Add(details);
            //        }
            //    }
            //}
            //#endregion

            //if (addList.Count > 0)
            //    BindDataGrid();
        }

        /// <summary>
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            //if (e.CommandName == "Delete" || e.CommandName == "Add")
            //{
            //    var datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][1]);

            //    InitMonthDetailsService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);

            //    BindDataGrid();
            //}
        }

        /// <summary>
        ///     关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
        }
        protected void Window3_Close(object sender, WindowCloseEventArgs e)
        {
        }

        protected void txtFAddress_OnTriggerClick(object sender, EventArgs e)
        {
        }

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                var custmoer = ViewUnitAllService.Where(p => p.FName == tbxFCustomer.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFCode.Text = custmoer.FCode;
                }
            }
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFItemCode_OnTriggerClick(object sender, EventArgs e)
        {
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     ModifiedGrid
        /// </summary>
        private void ModifiedGrid()
        {
            //编辑行事件
            //var dictModified = Grid1.GetModifiedDict();
            //foreach (var rowKey in dictModified.Keys)
            //{
            //    int datakey = Convert.ToInt32(Grid1.DataKeys[rowKey][1].ToString());

            //    var sKeys = new StringBuilder();
            //    var sValues = new StringBuilder();
            //    foreach (var key in dictModified[rowKey].Keys)
            //    {
            //        sKeys.AppendFormat("{0},", key);
            //    }

            //    foreach (var dictValue in dictModified[rowKey].Values)
            //    {
            //        sValues.AppendFormat("{0},", dictValue);
            //    }

            //    var details = InitMonthDetailsService.Where(p => p.FId == datakey).FirstOrDefault();
            //    //写入原始，通过存储过程完成明细复制
            //    //var parmsLog = new Dictionary<string, object>();
            //    //parmsLog.Clear();

            //    //parmsLog.Add("@fid", datakey);
            //    //parmsLog.Add("@opr", CurrentUser.AccountName);
            //    //parmsLog.Add("@companyId", CurrentUser.AccountComId);
            //    //SqlService.ExecuteProcedureCommand("proc_StockOutDetails_Log", parmsLog);

            //    var keys = sKeys.ToString().Split(',');
            //    var values = sValues.ToString().Split(',');
            //    for (int i = 0; i < keys.Count(); i++)
            //    {
            //        #region 修改内容

            //        var key = keys[i];
            //        var value = values[i];

            //        if (!string.IsNullOrEmpty(key))
            //        {
            //            if (details != null)
            //            {

            //                if (key.Equals("FQty"))
            //                {
            //                    details.FQty = Convert.ToDecimal(value);
            //                }

            //                if (key.Equals("FMemo"))
            //                {
            //                    details.FMemo = value;
            //                }

            //                //var detailslog = new LHStockOutDetails_Log
            //                //{
            //                //    FUpdateBy = CurrentUser.AccountName,
            //                //    FUpdateDate = DateTime.Now,
            //                //    FItemCode = details.FItemCode,
            //                //    FPrice = details.FPrice,
            //                //    FQty = details.FQty,
            //                //    FAmount = details.FAmount,
            //                //    FBottleQty = details.FBottleQty,
            //                //    FBottleOweQty = details.FBottleOweQty,
            //                //    KeyId = details.KeyId,
            //                //    FBottle = details.FBottle,
            //                //    FCompanyId = CurrentUser.AccountComId,
            //                //    FRecycleQty = details.FRecycleQty,
            //                //    FStatus = "变更",
            //                //    FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
            //                //};

            //                //StockOutDetailsLogService.Add(detailslog);
            //            }
            //        }

            //        #endregion
            //    }

            //    InitMonthDetailsService.SaveChanges();
            //}
        }

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (StockOut != null)
            {
                ModifiedGrid();
                /////////////////////////////////////////////////////////////////////////////

                StockOut.FCode = txtFCode.Text;
                StockOut.FName = tbxFCustomer.Text;
                //------------------------------------------------------
                StockOut.FCompanyId = CurrentUser.AccountComId;
                StockOut.FMemo = txtFMemo.Text.Trim();
                StockOut.FDate = txtFDate.SelectedDate;
                //var parms = new Dictionary<string, object>();
                //parms.Clear();
                //parms.Add("@keyID", StockOut.KeyId);
                //parms.Add("@companyId", CurrentUser.AccountComId);
                //var amt =
                //    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_MonthInitQty", parms).Tables[0].Rows[0][0]);

                StockOut.FAmount = Convert.ToDecimal(txtFAmount.Text);;

                return InitMonthServie.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var stock = InitMonthServie.Where(p => p.KeyId == txtKeyId.Text.Trim()).FirstOrDefault();


            if (stock != null)
            {
                ModifiedGrid();
                /////////////////////////////////////////////////////////////////////////////
                stock.FCode = txtFCode.Text;
                stock.FName = tbxFCustomer.Text;
                //--------------------------------------------------
                stock.FCompanyId = CurrentUser.AccountComId;
                stock.FFlag = 1;
                stock.FDeleteFlag = 0;
                stock.FMemo = txtFMemo.Text.Trim();

                //var parms = new Dictionary<string, object>();
                //parms.Clear();

                //parms.Add("@keyID", stock.KeyId);
                //parms.Add("@companyId", CurrentUser.AccountComId);

                //var amt =
                //    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_MonthInitQty", parms).Tables[0].Rows[0][0]);

                stock.FAmount = Convert.ToDecimal(txtFAmount.Text);

                stock.FDate = txtFDate.SelectedDate;
                stock.FAmt = 0;

                stock.FCate = hfdUnit.Text;

                InitMonthServie.SaveChanges();

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "TA", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "24");
                    orderParms.Add("@companyId", CurrentUser.AccountComId);

                    SqlService.ExecuteProcedureCommand("proc_num", orderParms);
                    txtKeyId.Text = newKeyId;

                    //新增日志
                    var billStatus = new LHBillStatus
                    {
                        KeyId = newKeyId,
                        FCompanyId = CurrentUser.AccountComId,
                        FActionName = "新增",
                        FDate = DateTime.Now,
                        FDeptId = CurrentUser.AccountOrgId,
                        FOperator = CurrentUser.AccountName,
                        FMemo = String.Format("单据号{0},{1}新增调整客户代码期初占用钢瓶单据。", newKeyId, CurrentUser.AccountName)
                    };

                    GasHelper.AddBillStatus(billStatus);

                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {

            ViewState["_AppendToEnd"] = true;

            txtCreateBy.Text = CurrentUser.AccountName;

            //tbxFItemCode.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFItemCode.ClientID)
            //        + Window1.GetShowReference("../../Common/WinBottle.aspx");

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID, hfdUnit.ClientID)
                    + Window2.GetShowReference("../../Common/WinUnitAll.aspx");

            //删除选中单元格的客户端脚本
            //string deleteScript = DeleteScript();

            ////新增
            //var defaultObj = new JObject
            //{
            //    {"FItemCode", ""},
            //    {"FItemName", ""},
            //    {"FSpec", ""},
            //    {"FUnit", ""},
            //    {"FQty", "0"},
            //    {"FCateName", ""},
            //    {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
            //        deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            //};

            // 在第一行新增一条数据
            //btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            txtFDate.SelectedDate = DateTime.Now;
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    txtKeyId.Text = SequenceService.CreateSequence("TM", CurrentUser.AccountComId);
                    Region3.Title = "调整客户代码期初占用钢瓶单";

                    var temp = new LHInitMonth
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //调整客户代码期初占用钢瓶单
                        FType = Convert.ToInt32(GasEnumBill.ChangeAccount),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                    };

                    //临时写入单据
                    InitMonthServie.Add(temp);

                    //合计
                    //var summary = new JObject
                    //{
                    //    {"FItemCode", "合计"},
                    //    {"FQty", 0},
                    //};

                    //Grid1.SummaryData = summary;

                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region3.Title = "编辑调整客户代码期初占用钢瓶单";

                    if (StockOut != null)
                    {
                        WebControlHandler.BindObjectToControls(StockOut, SimpleForm1);
                        txtFDate.SelectedDate = StockOut.FDate;
                        tbxFCustomer.Text = StockOut.FName;
                        txtFAmount.Text = StockOut.FAmount.ToString();

                        BindDataGrid();
                    }
                    break;
            }
        }

        ///// <summary>
        /////     删除选中行的脚本
        ///// </summary>
        ///// <returns></returns>
        //private string DeleteScript()
        //{
        //    //return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, Grid1.GetDeleteSelectedRowsReference(), String.Empty);
        //}

        #endregion

    }
}