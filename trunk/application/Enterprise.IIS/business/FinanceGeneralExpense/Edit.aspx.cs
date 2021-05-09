using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.FinanceGeneralExpense
{
    public partial class Edit : PageBase
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

        private bool AppendSubjectToEnd
        {
            get
            {
                return ViewState["_AppendSubjectToEnd"] != null && Convert.ToBoolean(ViewState["_AppendSubjectToEnd"]);
            }
        }

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
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerService _employeeService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected CustomerService EmployeeService
        {
            get { return _employeeService ?? (_employeeService = new CustomerService()); }
            set { _employeeService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private FKOrderDetailsService _fkOrderDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FKOrderDetailsService FKOrderDetailsService
        {
            get { return _fkOrderDetailsService ?? (_fkOrderDetailsService = new FKOrderDetailsService()); }
            set { _fkOrderDetailsService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private FKOrderService _skOrderService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FKOrderService FKOrderService
        {
            get { return _skOrderService ?? (_skOrderService = new FKOrderService()); }
            set { _skOrderService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private FKOrderBanksService _skOrderBanksService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FKOrderBanksService FKOrderBanksService
        {
            get { return _skOrderBanksService ?? (_skOrderBanksService = new FKOrderBanksService()); }
            set { _skOrderBanksService = value; }
        }

        #region 操作日志部分
        /// <summary>
        ///     StockOutDetailsLogService
        /// </summary>
        private StockOutDetailsLogService _stockOutDetailsLogService;
        /// <summary>
        ///     StockOutDetailsLogService
        /// </summary>
        protected StockOutDetailsLogService StockOutDetailsLogService
        {
            get { return _stockOutDetailsLogService ?? (_stockOutDetailsLogService = new StockOutDetailsLogService()); }
            set { _stockOutDetailsLogService = value; }
        }

        /// <summary>
        ///     StockOutLogService
        /// </summary>
        private StockOutLogService _stockOutLogService;
        /// <summary>
        ///     StockOutLogService
        /// </summary>
        protected StockOutLogService StockOutLogService
        {
            get { return _stockOutLogService ?? (_stockOutLogService = new StockOutLogService()); }
            set { _stockOutLogService = value; }
        }

        #endregion

        /// <summary>
        ///     客户档案
        /// </summary>
        private LHCustomer _employee;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected LHCustomer Employee
        {
            get { return _employee ?? (_employee = EmployeeService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _employee = value; }
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
        private LHFKOrder _skOrder;

        /// <summary>
        ///     
        /// </summary>
        protected LHFKOrder SkOrder
        {
            get { return _skOrder ?? (_skOrder = FKOrderService.FirstOrDefault(p => p.keyId == KeyId && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _skOrder = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string KeyId
        {
            get { return Request["KeyId"]; }
        }

        /// <summary>
        ///     当前画面操作项
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction)Int32.Parse(s);
            }
        }

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
                #region 银行
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

                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_Bank a WHERE a.FCode IN ({0}) and FCompanyId={1}", value, CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var banks = new LHFKOrderBanks();

                            banks.FAmt = 0;
                            banks.FCardNo = table.Rows[i]["FComment"].ToString();
                            banks.KeyId = txtKeyId.Text.Trim();
                            banks.FCode = table.Rows[i]["FCode"].ToString();
                            banks.FName = table.Rows[i]["FName"].ToString();
                            banks.FMemo = "";
                            banks.FBankNo = "";
                            banks.FBillNo = "";
                            banks.FCompanyId = CurrentUser.AccountComId;
                            //banks.FExpireDate =
                            FKOrderBanksService.AddEntity(banks);

                            ////日志
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
                            //            FItemCode = banks.FItemCode,
                            //            FPrice = price,
                            //            FQty = 1,
                            //            FAmount = price,
                            //            FBottleQty = 1,
                            //            FBottleOweQty = 0,
                            //            KeyId = txtKeyId.Text.Trim(),
                            //            FBottle = banks.FBottle,
                            //            FStatus = "新增",
                            //            FMemo = string.Format(@"时间：{0} 操作人：{1}", DateTime.Now, CurrentUser.AccountName)
                            //        };

                            //        StockOutDetailsLogService.Add(detailslog);

                            //        break;
                            //}

                        }

                        FKOrderBanksService.SaveChanges();

                        //重新绑定值
                        BindDataGrid();
                    }
                }
                #endregion

                #region 报销项目

                if (GetRequestEventArgument().Contains("reloadTree:"))
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

                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_Subject a WHERE a.FCode IN ({0}) and FCompanyId={1}", value, CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var details = new LHFKOrderDetails();

                            details.KeyId = txtKeyId.Text.Trim();
                            details.FCode = table.Rows[i]["FCode"].ToString();
                            details.FName = table.Rows[i]["FName"].ToString();
                            details.FMemo = "";
                            details.FAmount = 0;
                            details.FCompanyId = CurrentUser.AccountComId;
                            FKOrderDetailsService.AddEntity(details);

                            ////日志
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
                            //            FItemCode = banks.FItemCode,
                            //            FPrice = price,
                            //            FQty = 1,
                            //            FAmount = price,
                            //            FBottleQty = 1,
                            //            FBottleOweQty = 0,
                            //            KeyId = txtKeyId.Text.Trim(),
                            //            FBottle = banks.FBottle,
                            //            FStatus = "新增",
                            //            FMemo = string.Format(@"时间：{0} 操作人：{1}", DateTime.Now, CurrentUser.AccountName)
                            //        };

                            //        StockOutDetailsLogService.Add(detailslog);

                            //        break;
                            //}

                        }

                        FKOrderDetailsService.SaveChanges();

                        //重新绑定值
                        BindDataGrid();
                    }
                }

                #endregion

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
                PageContext.RegisterStartupScript(string.Format("LodopPrinter('{0}');", KeyId));
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }
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
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.LHFKOrderBanks WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();

            var table = source.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFAmt = 0.00M;

                sumFAmt = Convert.ToDecimal(table.Compute("sum(FAmt)", "true"));

                var summary = new JObject
                {
                    {"FCode", "合计"},
                    {"FAmt", sumFAmt},
                };

                Grid1.SummaryData = summary;
            }
            //----------------------------------------------------------------------------
            var subjects = SqlService.Where(string.Format(@"SELECT FId,KeyId,FCompanyId,FCode AS FSubjectCode,FName AS FSubjectName,FAmount,FMemo AS FMemo2  FROM dbo.LHFKOrderDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid2.DataSource = subjects;
            Grid2.DataBind();

            var details = subjects.Tables[0];

            if (details != null && details.Rows.Count > 0)
            {
                decimal sumFAmt = 0.00M;

                sumFAmt = Convert.ToDecimal(details.Compute("sum(FAmount)", "true"));

                var summary = new JObject
                {
                    {"FCode", "合计"},
                    {"FAmt", sumFAmt},
                };

                Grid2.SummaryData = summary;
            }


        }

        /// <summary>
        ///     单元格编辑与修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
            Window4.Hidden = true;

            //新增行事件
            //var addList = Grid1.GetNewAddedList();
            //foreach (var add in addList)
            //{
            //    var dictValues = add.Values;

            //    //商品代码
            //    var firstOrDefault = dictValues.First();

            //    if (firstOrDefault != null)
            //    {
            //        DataSet dataSet = GasHelper.GetSalesItem(firstOrDefault.ToString());

            //        DataTable table = dataSet.Tables[0];

            //        if (table != null && table.Rows.Count > 0)
            //        {
            //            decimal price = GasHelper.GeCustomerPrice(txtFCode.Text.Trim(),//
            //                table.Rows[0]["FItemCode"].ToString());

            //            table.Rows[0]["FPrice"] = price;

            //            var details = new LHStockOutDetails
            //            {
            //                FItemCode = table.Rows[0]["FItemCode"].ToString(),
            //                FPrice = price,
            //                FQty = 1,
            //                FAmount = price,
            //                FBottleQty = 1,
            //                FBottleOweQty = 0,
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
            //                        FBottleQty = 1,
            //                        FBottleOweQty = 0,
            //                        KeyId = txtKeyId.Text.Trim(),
            //                        FBottle = table.Rows[0]["FBottleCode"].ToString(),
            //                        FStatus = "新增",
            //                        FMemo = string.Format(@"时间：{0} 新增人：{1}", DateTime.Now, CurrentUser.AccountName)
            //                    };

            //                    //detailslog.FCompanyId = CurrentUser.AccountComId;
            //                    //detailslog.FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString());

            //                    StockOutDetailsLogService.Add(detailslog);

            //                    break;
            //            }

            //           // CollectionOrderBanksService.Add(details);
            //        }
            //    }
            //}

            //编辑行事件
            var dictModified = Grid1.GetModifiedDict();
            foreach (var index in dictModified.Keys)
            {
                int datakey = Convert.ToInt32(Grid1.DataKeys[index][1].ToString());

                foreach (var dictValue in dictModified.Values)
                {
                    foreach (KeyValuePair<string, object> keyValuePair in dictValue)
                    {
                        string key = keyValuePair.Key;
                        string value = keyValuePair.Value.ToString();

                        var banks = FKOrderBanksService.Where(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

                        //写入原始，通过存储过程完成明细复制
                        //var parms = new Dictionary<string, object>();
                        //parms.Clear();

                        //parms.Add("@fid", datakey);
                        //parms.Add("@opr", CurrentUser.AccountName);

                        //SqlService.ExecuteProcedureCommand("proc_StockOutDetails_Log", parms);

                        if (banks != null)
                        {
                            switch (key)
                            {
                                case "FAmt":
                                    banks.FAmt = Convert.ToDecimal(value);
                                    break;

                                case "FCardNo":
                                    banks.FCardNo = value;
                                    break;

                                case "FBankNo":
                                    banks.FBankNo = value;
                                    break;

                                case "FBillNo":
                                    banks.FBillNo = value;
                                    break;

                                case "FExpireDate":
                                    banks.FExpireDate = Convert.ToDateTime(value);
                                    break;

                                case "FMemo":
                                    banks.FMemo = value;
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
                            //    FStatus = "变更",
                            //    FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
                            //};

                            //StockOutDetailsLogService.Add(detailslog);

                        }

                        FKOrderBanksService.SaveChanges();
                    }
                }
            }

            BindDataGrid();
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
            Window3.Hidden = true;
            Window4.Hidden = true;

            //新增行事件
            //var addList = Grid1.GetNewAddedList();
            //foreach (var add in addList)
            //{
            //    var dictValues = add.Values;

            //    //商品代码
            //    var firstOrDefault = dictValues.First();

            //    if (firstOrDefault != null)
            //    {
            //        DataSet dataSet = GasHelper.GetSalesItem(firstOrDefault.ToString());

            //        DataTable table = dataSet.Tables[0];

            //        if (table != null && table.Rows.Count > 0)
            //        {
            //            decimal price = GasHelper.GeCustomerPrice(txtFCode.Text.Trim(),//
            //                table.Rows[0]["FItemCode"].ToString());

            //            table.Rows[0]["FPrice"] = price;

            //            var details = new LHStockOutDetails
            //            {
            //                FItemCode = table.Rows[0]["FItemCode"].ToString(),
            //                FPrice = price,
            //                FQty = 1,
            //                FAmount = price,
            //                FBottleQty = 1,
            //                FBottleOweQty = 0,
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
            //                        FBottleQty = 1,
            //                        FBottleOweQty = 0,
            //                        KeyId = txtKeyId.Text.Trim(),
            //                        FBottle = table.Rows[0]["FBottleCode"].ToString(),
            //                        FStatus = "新增",
            //                        FMemo = string.Format(@"时间：{0} 新增人：{1}", DateTime.Now, CurrentUser.AccountName)
            //                    };

            //                    //detailslog.FCompanyId = CurrentUser.AccountComId;
            //                    //detailslog.FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString());

            //                    StockOutDetailsLogService.Add(detailslog);

            //                    break;
            //            }

            //           // CollectionOrderBanksService.Add(details);
            //        }
            //    }
            //}

            //编辑行事件
            var dictModified = Grid2.GetModifiedDict();
            foreach (var index in dictModified.Keys)
            {
                int datakey = Convert.ToInt32(Grid2.DataKeys[index][1].ToString());

                foreach (var dictValue in dictModified.Values)
                {
                    foreach (KeyValuePair<string, object> keyValuePair in dictValue)
                    {
                        string key = keyValuePair.Key;
                        string value = keyValuePair.Value.ToString();

                        var details = FKOrderDetailsService.Where(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

                        //写入原始，通过存储过程完成明细复制
                        //var parms = new Dictionary<string, object>();
                        //parms.Clear();

                        //parms.Add("@fid", datakey);
                        //parms.Add("@opr", CurrentUser.AccountName);

                        //SqlService.ExecuteProcedureCommand("proc_StockOutDetails_Log", parms);

                        if (details != null)
                        {
                            switch (key)
                            {
                                case "FAmount2":
                                    details.FAmount = Convert.ToDecimal(value);
                                    break;

                                case "FMemo2":
                                    details.FMemo = value;
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
                            //    FStatus = "变更",
                            //    FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
                            //};

                            //StockOutDetailsLogService.Add(detailslog);

                        }

                        FKOrderDetailsService.SaveChanges();
                    }
                }
            }

            BindDataGrid();
        }

        /// <summary>
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete" || e.CommandName == "Add")
            {
                var datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][1]);

                FKOrderBanksService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);

                BindDataGrid();
            }
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
                var datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][1]);

                FKOrderBanksService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);

                BindDataGrid();
            }
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
        protected void Window4_Close(object sender, WindowCloseEventArgs e)
        {
        }

        protected void tbxBankCode_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
            Window4.Hidden = true;
        }

        protected void tbxFSubjectCode_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
            Window4.Hidden = true;
        }

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {

                Window1.Hidden = true;
                Window2.Hidden = true;
                Window3.Hidden = true;
                Window4.Hidden = true;

                var custmoer = EmployeeService.Where(p => p.FCode == tbxFCustomer.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
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
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
            Window4.Hidden = true;
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (SkOrder != null)
            {
                SkOrder.FCode = txtFCode.Text;
                SkOrder.FName = tbxFCustomer.Text;
                SkOrder.FMemo = txtFMemo.Text.Trim();

                return FKOrderService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var order = FKOrderService.Where(p => p.keyId == txtKeyId.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (order != null)
            {
                order.FCode = txtFCode.Text;
                order.FName = tbxFCustomer.Text;
                //--------------------------------------------------
                order.FDeleteFlag = 0;
                order.FMemo = txtFMemo.Text.Trim();
                order.CreateBy = CurrentUser.AccountName;
                order.FDate = txtFDate.SelectedDate;

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@KeyId", order.keyId);
                parms.Add("@companyId", CurrentUser.AccountComId);
                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_FKOrderAmt", parms).Tables[0].Rows[0][0]);

                order.FAmount = amt;

                FKOrderService.SaveChanges();

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "YF", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "19");
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
                        FMemo = String.Format("单据号{0},{1}新增一般费用单据。", newKeyId, CurrentUser.AccountName)
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

            tbxBankCode.OnClientTriggerClick = Window1.GetSaveStateReference(tbxBankCode.ClientID)
                    + Window1.GetShowReference("../../Common/WinBank.aspx");

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinCustomer.aspx");

            tbxFSubjectCode.OnClientTriggerClick = Window4.GetSaveStateReference(tbxFSubjectCode.ClientID)
                    + Window4.GetShowReference("../../Common/WinSubject.aspx");

            //删除选中单元格的客户端脚本
            string deleteScript = DeleteScript();
            string deleteSubject = DeleteSubject();

            //新增
            var defaultObj = new JObject
            {
                {"FItemCode", ""},
                {"FItemName", ""},
                {"FSpec", ""},
                {"FUnit", ""},
                {"FPrice", ""},
                {"FQty", "0"},
                {"FBottleQty", "0"},
                {"FBottleOweQty", "0"},
                {"FCateName", ""},
                {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
                    deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            };

            //报销项目
            var subjectObj = new JObject
            {
                {"FSubjectCode", ""},
                {"FSubjectName", ""},
                {"FAmount", ""},
                {"FMemo", ""},
                {"colDeleteSubject", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
                    deleteSubject, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            };

            // 在第一行新增一条数据
            btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            btnAddSubject.OnClientClick = Grid2.GetAddNewRecordReference(subjectObj, AppendSubjectToEnd);

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
                    Region1.Title = "添加一般费用单";
                    var temp = new LHFKOrder
                    {
                        keyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //一般费用单
                        FType = Convert.ToInt32(GasEnumBill.GeneralExpense),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FAmount = 0,

                        FCate = "内",

                        FCompanyId = CurrentUser.AccountComId
                    };

                    //临时写入单据
                    FKOrderService.Add(temp);

                    var summary = new JObject
                    {
                        {"FCode", "合计"},
                        {"FAmt", 0},
                    };

                    Grid1.SummaryData = summary;

                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region1.Title = "编辑一般费用单";
                    if (SkOrder != null)
                    {
                        txtFCode.Text = SkOrder.FCode;
                        tbxFCustomer.Text = SkOrder.FName;
                        txtFMemo.Text = SkOrder.FMemo;

                        BindDataGrid();
                    }
                    break;
            }
        }

        /// <summary>
        ///     删除选中行的脚本
        /// </summary>
        /// <returns></returns>
        private string DeleteScript()
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, Grid1.GetDeleteSelectedRowsReference(), String.Empty);
        }

        /// <summary>
        ///     报账项目
        /// </summary>
        /// <returns></returns>
        private string DeleteSubject()
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, Grid2.GetDeleteSelectedRowsReference(), String.Empty);
        }

        #endregion

    }
}