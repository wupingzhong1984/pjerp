using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;
using System.Text;


namespace Enterprise.IIS.business.PurchaseReturn
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
        private SupplierService _supplierService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SupplierService SupplierService
        {
            get { return _supplierService ?? (_supplierService = new SupplierService()); }
            set { _supplierService = value; }
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

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockOutDetailsService _stockOutDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutDetailsService StockOutDetailsService
        {
            get { return _stockOutDetailsService ?? (_stockOutDetailsService = new StockOutDetailsService()); }
            set { _stockOutDetailsService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHStockOut _stockOut;


        /// <summary>
        ///     客户档案
        /// </summary>
        private LHSupplier _supplier;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected LHSupplier Supplier
        {
            get { return _supplier ?? (_supplier = SupplierService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim()&&p.FCompanyId==CurrentUser.AccountComId)); }
            set { _supplier = value; }
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
        ///     职员档案
        /// </summary>
        protected LHStockOut StockOut
        {
            get { return _stockOut ?? (_stockOut = StockOutService.FirstOrDefault(p => p.KeyId == KeyId && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _stockOut = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string KeyId
        {
            get { return Request["KeyId"]; }
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
        private StockInLogService _stockInLogService;
        /// <summary>
        ///     StockOutLogService
        /// </summary>
        protected StockInLogService StockInLogService
        {
            get { return _stockInLogService ?? (_stockInLogService = new StockInLogService()); }
            set { _stockInLogService = value; }
        }

        #endregion


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

                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_PurchaseItem a WHERE a.FItemCode IN ({0}) and a.FCompanyId={1}", value,CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var details = new LHStockOutDetails();

                            decimal price = GasHelper.GeSupplierPrice(txtFCode.Text.Trim(), //
                                table.Rows[i]["FItemCode"].ToString(), CurrentUser.AccountComId);

                            details.FItemCode = table.Rows[i]["FItemCode"].ToString();
                            details.FPrice = price;
                            details.FQty = 1;
                            details.FAmount = price;
                            details.FBottleQty = 1;
                            details.FBottleOweQty = 0;
                            details.FCompanyId = CurrentUser.AccountComId;
                            details.KeyId = txtKeyId.Text.Trim();
                            details.FCateId = Convert.ToInt32(table.Rows[i]["FId"].ToString());

                            //默认包装物
                            details.FBottle = table.Rows[i]["FBottle"].ToString();

                            StockOutDetailsService.AddEntity(details);


                            //日志
                            switch (Actions)
                            {
                                case WebAction.Add:
                                    break;
                                case WebAction.Edit:
                                    //记录一下当前新增人操作内容
                                    var detailslog = new LHStockOutDetails_Log
                                    {
                                        FUpdateBy = CurrentUser.AccountName,
                                        FUpdateDate = DateTime.Now,
                                        FItemCode = details.FItemCode,
                                        FPrice = price,
                                        FQty = 1,
                                        FAmount = price,
                                        FBottleQty = 1,
                                        FBottleOweQty = 0,
                                        KeyId = txtKeyId.Text.Trim(),
                                        FBottle = details.FBottle,
                                        FCompanyId = CurrentUser.AccountComId,
                                        FStatus = "新增",
                                        FMemo = string.Format(@"时间：{0} 操作人：{1}", DateTime.Now, CurrentUser.AccountName)
                                    };

                                    StockOutDetailsLogService.Add(detailslog);

                                    break;
                            }
                        }

                        StockOutDetailsService.SaveChanges();

                        //重新绑定值
                        BindDataGrid();
                    }
                }

                //更新合计
                if (GetRequestEventArgument() == "UPDATE_SUMMARY")
                {
                    // 页面要求重新计算合计行的值
                    OutputSummaryData();

                    //
                    ModifiedGrid();

                    // 为了保持前后台上传，回发更新合计行值后，必须进行数据绑定或者提交更改
                    Grid1.CommitChanges();
                }
            }
        }

        #region OutputSummaryData

        private void OutputSummaryData()
        {
            decimal chineseTotal = 0;
            decimal mathTotal = 0;
            foreach (JObject mergedRow in Grid1.GetMergedData())
            {
                JObject values = mergedRow.Value<JObject>("values");

                chineseTotal += values.Value<decimal>("FQty");
                mathTotal += values.Value<decimal>("FAmount");
            }

            JObject summary = new JObject();
            summary.Add("FQty", chineseTotal);
            summary.Add("FAmount", mathTotal);

            Grid1.SummaryData = summary;
        }

        #endregion

        /// <summary>
        ///     选择客户档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = txtFCode.Text.Trim();

            if (!string.IsNullOrEmpty(code) && !code.Equals("-1"))
            {
                txtFAddress.Text = Supplier.FAddress.Trim();
                txtFFreight.Text = Supplier.FFreight.ToString();
                txtFLinkman.Text = Supplier.FLinkman;
                txtFPhone.Text = Supplier.FPhome;
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
        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                Window1.Hidden = true;
                Window2.Hidden = true;

                var custmoer = SupplierService.Where(p => p.FName == tbxFCustomer.Text.Trim()&&p.FCompanyId==CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFCode.Text = custmoer.FCode;
                    txtFAddress.Text = custmoer.FAddress.Trim();
                    txtFFreight.Text = custmoer.FFreight.ToString();
                    txtFLinkman.Text = custmoer.FLinkman;
                    txtFPhone.Text = custmoer.FPhome;
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
        }


        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            //BindDataGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_SalesDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text,CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();

            var table = source.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFQty = 0.00M;
                decimal sumFAmount = 0.00M;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    sumFQty += Convert.ToDecimal(table.Rows[i]["FQty"]);
                    sumFAmount += Convert.ToDecimal(table.Rows[i]["FAmount"]);
                }

                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", sumFQty},
                    {"FAmount", sumFAmount}
                };

                Grid1.SummaryData = summary;
            }
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SortField = String.Format(@"{0}", e.SortField);
            SortDirection = e.SortDirection;
            //BindDataGrid();
        }




        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            var addList = Grid1.GetNewAddedList();
            foreach (var add in addList)
            {
                var dictValues = add.Values;

                //商品代码
                var firstOrDefault = dictValues.First();

                if (firstOrDefault != null && !string.IsNullOrEmpty(firstOrDefault.ToString()))
                {
                    DataSet dataSet = GasHelper.GetPurchaseItem(firstOrDefault.ToString(),CurrentUser.AccountComId);

                    DataTable table = dataSet.Tables[0];

                    if (table != null && table.Rows.Count > 0)
                    {
                        decimal price = GasHelper.GeSupplierPrice(txtFCode.Text.Trim(),//
                            table.Rows[0]["FItemCode"].ToString(), CurrentUser.AccountComId);

                        table.Rows[0]["FPrice"] = price;

                        var details = new LHStockOutDetails
                        {

                            FItemCode = table.Rows[0]["FItemCode"].ToString(),
                            FPrice = price,
                            FQty = 1,
                            FAmount = price,
                            FBottleQty = 1,
                            FBottleOweQty = 0,
                            FCompanyId = CurrentUser.AccountComId,
                            KeyId = txtKeyId.Text.Trim(),
                            FBottle = table.Rows[0]["FBottleCode"].ToString(),
                            FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString())
                        };


                        switch (Actions)
                        {
                            case WebAction.Add:
                                break;
                            case WebAction.Edit:
                                //记录一下当前新增人操作内容
                                var detailslog = new LHStockOutDetails_Log
                                {
                                    FUpdateBy = CurrentUser.AccountName,
                                    FUpdateDate = DateTime.Now,
                                    FItemCode = table.Rows[0]["FItemCode"].ToString(),
                                    FPrice = price,
                                    FQty = 1,
                                    FAmount = price,
                                    FBottleQty = 1,
                                    FBottleOweQty = 0,
                                    FCompanyId = CurrentUser.AccountComId,
                                    KeyId = txtKeyId.Text.Trim(),
                                    FBottle = table.Rows[0]["FBottleCode"].ToString(),
                                    FStatus = "新增",
                                    FMemo = string.Format(@"时间：{0} 新增人：{1}", DateTime.Now, CurrentUser.AccountName)
                                };

                                //detailslog.FCompanyId = CurrentUser.AccountComId;
                                //detailslog.FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString());

                                StockOutDetailsLogService.Add(detailslog);

                                break;
                        }


                        StockOutDetailsService.Add(details);
                    }
                }
            }

            //var dictModified = Grid1.GetModifiedDict();

            //foreach (var index in dictModified.Keys)
            //{
            //    int datakey = Convert.ToInt32(Grid1.DataKeys[index][1].ToString());

            //    foreach (var dictValue in dictModified.Values)
            //    {
            //        foreach (KeyValuePair<string, object> keyValuePair in dictValue)
            //        {
            //            string key = keyValuePair.Key;
            //            string value = keyValuePair.Value.ToString();

            //            var details = StockOutDetailsService.Where(p => p.FId == datakey&&p.FCompanyId==CurrentUser.AccountComId
            //            ).FirstOrDefault();

            //            //写入原始，通过存储过程完成明细复制
            //            var parms = new Dictionary<string, object>();
            //            parms.Clear();

            //            parms.Add("@fid", datakey);
            //            parms.Add("@opr", CurrentUser.AccountName);
            //            parms.Add("@companyId", CurrentUser.AccountComId);
            //            SqlService.ExecuteProcedureCommand("proc_StockOutDetails_Log", parms);

            //            if (details != null)
            //            {
            //                switch (key)
            //                {
            //                    case "FPrice":
            //                        details.FPrice = Convert.ToDecimal(value);
            //                        details.FAmount = details.FPrice * details.FQty;
            //                        break;

            //                    case "FQty":
            //                        details.FQty = Convert.ToDecimal(value);
            //                        details.FAmount = details.FPrice * details.FQty;
            //                        break;

            //                    case "FBottle":
            //                        details.FBottle = value;
            //                        break;

            //                    case "FBottleName":
            //                        details.FBottle = value;
            //                        break;

            //                    case "FBottleQty":
            //                        details.FBottleQty = Convert.ToInt32(value);
            //                        break;

            //                    case "FMemo":
            //                        details.FMemo = value;
            //                        break;
            //                }

            //                var detailslog = new LHStockOutDetails_Log
            //                {
            //                    FUpdateBy = CurrentUser.AccountName,
            //                    FUpdateDate = DateTime.Now,
            //                    FItemCode = details.FItemCode,
            //                    FPrice = details.FPrice,
            //                    FQty = details.FQty,
            //                    FAmount = details.FAmount,
            //                    FBottleQty = details.FBottleQty,
            //                    FBottleOweQty = details.FBottleOweQty,
            //                    KeyId = details.KeyId,
            //                    FBottle = details.FBottle,
            //                    FCompanyId = CurrentUser.AccountComId,
            //                    FStatus = "变更",
            //                    FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
            //                };
            //                StockOutDetailsLogService.Add(detailslog);

            //            }

            //            StockOutDetailsService.SaveChanges();
            //        }
            //    }
            //}

            if(addList.Count>0)
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

                StockOutDetailsService.Delete(p => p.FId == datakey&&p.FCompanyId==CurrentUser.AccountComId);

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
        #endregion

        #region Private Method


        private void ModifiedGrid()
        {
            //编辑行事件
            var dictModified = Grid1.GetModifiedDict();
            foreach (var rowKey in dictModified.Keys)
            {
                int datakey = Convert.ToInt32(Grid1.DataKeys[rowKey][1].ToString());

                var sKeys = new StringBuilder();
                var sValues = new StringBuilder();
                foreach (var key in dictModified[rowKey].Keys)
                {
                    sKeys.AppendFormat("{0},", key);
                }

                foreach (var dictValue in dictModified[rowKey].Values)
                {
                    sValues.AppendFormat("{0},", dictValue);
                }

                var details = StockOutDetailsService.Where(p => p.FId == datakey).FirstOrDefault();
                //写入原始，通过存储过程完成明细复制
                var parmsLog = new Dictionary<string, object>();
                parmsLog.Clear();

                parmsLog.Add("@fid", datakey);
                parmsLog.Add("@opr", CurrentUser.AccountName);
                parmsLog.Add("@companyId", CurrentUser.AccountComId);
                SqlService.ExecuteProcedureCommand("proc_StockOutDetails_Log", parmsLog);

                var keys = sKeys.ToString().Split(',');
                var values = sValues.ToString().Split(',');
                for (int i = 0; i < keys.Count(); i++)
                {
                    #region 修改内容

                    var key = keys[i];
                    var value = values[i];

                    if (!string.IsNullOrEmpty(key))
                    {
                        if (details != null)
                        {
                            if (key.Equals("FPrice"))
                            {
                                details.FPrice = Convert.ToDecimal(value);
                                details.FAmount = details.FPrice * details.FQty;
                            }

                            if (key.Equals("FQty"))
                            {
                                details.FQty = Convert.ToDecimal(value);
                                details.FBottleQty = Convert.ToInt32(details.FQty);
                                details.FAmount = details.FPrice * details.FQty;
                            }

                            if (key.Equals("FBottle"))
                            {
                                details.FBottle = value;
                            }

                            if (key.Equals("FBottleName"))
                            {
                                details.FBottle = value;
                            }

                            if (key.Equals("FRecycleQty"))
                            {
                                details.FRecycleQty = Convert.ToInt32(value);
                                //写入回空单
                            }

                            if (key.Equals("FBottleQty"))
                            {
                                details.FBottleQty = Convert.ToInt32(value);
                            }
                            if (key.Equals("FT6Warehouse"))
                            {
                                details.FT6WarehouseNum = GasHelper.GetWarehouseByName(value);
                            }
                            if (key.Equals("FMemo"))
                            {
                                details.FMemo = value;
                            }

                            var detailslog = new LHStockOutDetails_Log
                            {
                                FUpdateBy = CurrentUser.AccountName,
                                FUpdateDate = DateTime.Now,
                                FItemCode = details.FItemCode,
                                FPrice = details.FPrice,
                                FQty = details.FQty,
                                FAmount = details.FAmount,
                                FBottleQty = details.FBottleQty,
                                FBottleOweQty = details.FBottleOweQty,
                                KeyId = details.KeyId,
                                FBottle = details.FBottle,
                                FCompanyId = CurrentUser.AccountComId,
                                FRecycleQty = details.FRecycleQty,
                                FStatus = "变更",
                                FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
                            };

                            StockOutDetailsLogService.Add(detailslog);
                        }
                    }

                    #endregion
                }
                StockOutDetailsService.SaveChanges();
            }
        }

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (StockOut != null)
            {
                ModifiedGrid();


                StockOut.FCode = txtFCode.Text;
                StockOut.FName = tbxFCustomer.Text;
                StockOut.FAddress = txtFAddress.Text.Trim();
                StockOut.FCompanyId = CurrentUser.AccountComId;

                StockOut.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                StockOut.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                StockOut.FFlag = 1;

                if (!string.IsNullOrEmpty(txtFFreight.Text.Trim()))
                {
                    StockOut.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                }
                StockOut.FLinkman = txtFLinkman.Text.Trim();
                StockOut.FMemo = txtFMemo.Text.Trim();
                StockOut.FPhone = txtFPhone.Text.Trim();

                var parms = new Dictionary<string, object>();
                parms.Clear();
                parms.Add("@companyId", CurrentUser.AccountComId);
                parms.Add("@keyID", StockOut.KeyId);
                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_SalesAmt", parms).Tables[0].Rows[0][0]);

                StockOut.FAmount = amt;


                StockOut.FSalesman = ddlFSalesman.SelectedValue;
                StockOut.FArea = ddlFArea.SelectedValue;
                StockOut.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());
                StockOut.FReconciliation = txtFReconciliation.Text;

                StockOut.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                StockOut.FLogisticsNumber = txtFLogisticsNumber.Text;
                StockOut.FDate = txtFDate.SelectedDate;
                StockOut.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                StockOut.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
                StockOut.FSubjectCode = ddlSubject.SelectedValue;
                StockOut.FSubjectName = ddlSubject.SelectedText;
                StockOut.FDate = txtFDate.SelectedDate;
                StockOut.CreateBy = CurrentUser.AccountName;

                //---------------------------------------------------------
                //收发类型
                StockOut.FT6ReceiveSendType = ddlT6ReceiveSendType.SelectedText;
                StockOut.FT6ReceiveSendTypeNum = ddlT6ReceiveSendType.SelectedValue;
                //部门名称
                StockOut.FT6Department = "";
                //部门代码
                StockOut.FT6DepartmentNum = "";
                //业务员代码
                StockOut.FT6SalesmanNum = "";
                //币种
                StockOut.FT6Currency = ddlFT6Currency.SelectedValue;
                //汇率
                StockOut.FT6ExchangeRate = Convert.ToDecimal(txtFT6ExchangeRate.Text);
                //销售类型
                StockOut.FT6PurchaseType = ddlFT6PurchaseType.SelectedText;
                StockOut.FT6PurchaseTypeNum = ddlFT6PurchaseType.SelectedValue;
                //T6同步
                //StockOut.FT6Status = "未同步";
                StockOut.FT6Warehouse = ddlWarehouse.SelectedValue;
                StockOutService.SaveChanges();

                //收款部分
                var parmsAuto = new Dictionary<string, object>();
                parmsAuto.Clear();

                parmsAuto.Add("@KeyId", StockOut.KeyId);
                parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                parmsAuto.Add("@FCate", "供应商");
                parmsAuto.Add("@date", Convert.ToDateTime(StockOut.FDate).ToShortDateString());
                parmsAuto.Add("@FSKNum", string.IsNullOrEmpty(StockOut.FSKNum) ? "" : StockOut.FSKNum);

                SqlService.ExecuteProcedureCommand("proc_SKOrderAuto", parmsAuto);

                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var stock = StockOutService.Where(p => p.KeyId == txtKeyId.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (stock != null)
            {
                ModifiedGrid();

                stock.FCode = txtFCode.Text;
                stock.FName = tbxFCustomer.Text;
                stock.FAddress = txtFAddress.Text.Trim();
                stock.FCompanyId = CurrentUser.AccountComId;
                stock.FFlag = 1;
                stock.FDeleteFlag = 0;

                if (!string.IsNullOrEmpty(txtFFreight.Text.Trim()))
                {
                    stock.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                }

                stock.FLinkman = txtFLinkman.Text.Trim();
                stock.FMemo = txtFMemo.Text.Trim();
                stock.FPhone = txtFPhone.Text.Trim();

                stock.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                stock.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";


                var parms = new Dictionary<string, object>();
                parms.Clear();
                parms.Add("@companyId", CurrentUser.AccountComId);
                parms.Add("@keyID", stock.KeyId);
                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_SalesAmt", parms).Tables[0].Rows[0][0]);

                stock.FAmount = amt;

                stock.FSalesman = ddlFSalesman.SelectedValue;
                stock.FArea = ddlFArea.SelectedValue;
                stock.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());
                stock.FReconciliation = txtFReconciliation.Text;
                stock.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                stock.FLogisticsNumber = txtFLogisticsNumber.Text;
                stock.FDate = txtFDate.SelectedDate;
                stock.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                stock.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
                stock.FSubjectCode = ddlSubject.SelectedValue;
                stock.FSubjectName = ddlSubject.SelectedText;
                stock.FSKNum = "";



                #region T6 对接接口
                //---------------------------------------------------------
                //收发类型
                stock.FT6ReceiveSendType = ddlT6ReceiveSendType.SelectedText;
                stock.FT6ReceiveSendTypeNum = ddlT6ReceiveSendType.SelectedValue;
                //部门名称
                stock.FT6Department = "";
                //部门代码
                stock.FT6DepartmentNum = "";
                //业务员代码
                stock.FT6SalesmanNum = "";
                //币种
                stock.FT6Currency = ddlFT6Currency.SelectedValue;
                //汇率
                stock.FT6ExchangeRate = Convert.ToDecimal(txtFT6ExchangeRate.Text);
                //销售类型
                stock.FT6PurchaseType = ddlFT6PurchaseType.SelectedText;
                stock.FT6PurchaseTypeNum = ddlFT6PurchaseType.SelectedValue;
                //T6同步
                stock.FT6Status = "未同步";
                stock.FT6Warehouse = ddlWarehouse.SelectedValue;
                //---------------------------------------------------------
                #endregion


                StockOutService.SaveChanges();

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "CT", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "5");
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
                        FMemo = String.Format("单据号{0},{1}新增采购退货单据。", newKeyId, CurrentUser.AccountName)
                    };
                    GasHelper.AddBillStatus(billStatus);


                    //收款部分
                    var parmsAuto = new Dictionary<string, object>();
                    parmsAuto.Clear();

                    parmsAuto.Add("@KeyId", newKeyId);
                    parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                    parmsAuto.Add("@FCate", "供应商");
                    parmsAuto.Add("@date", Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));
                    parmsAuto.Add("@FSKNum", stock.FSKNum);

                    SqlService.ExecuteProcedureCommand("proc_SKOrderAuto", parmsAuto);


                }

                return true;
            }
            return false;

        }

        /// <summary>
        ///     选择产品
        /// </summary>
        /// <returns></returns>
        private string SearchScript()
        {
            return Window1.GetShowReference("../../Common/WinProduct.aspx", "选择产品");
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            ViewState["_AppendToEnd"] = true;

            txtCreateBy.Text = CurrentUser.AccountName;

            tbxFItemCode.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFItemCode.ClientID)
                    + Window1.GetShowReference("../../Common/WinProduct.aspx");

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinSupplier.aspx");

            GasHelper.DropDownListDriverDataBind(ddlFDriver);

            GasHelper.DropDownListShipperDataBind(ddlFShipper);

            GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);

            GasHelper.DropDownListBottleDataBind(tbxFBottle);

            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

            GasHelper.DropDownListAreasDataBind(ddlFArea);

            GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

            GasHelper.DropDownListDeliveryMethodDataBind(ddlDeliveryMethod);

            GasHelper.DropDownListBankSubjectDataBind(ddlSubject);

            GasHelper.DropDownListDataBindPurchaseType(ddlFT6PurchaseType);

            GasHelper.DropDownListDataBindReceiveSendType(ddlT6ReceiveSendType);

            GasHelper.DropDownListWarehouseDataBind(tbxFWarehouse);

            GasHelper.DropDownListDataBindCurrencyType(ddlFT6Currency);

            GasHelper.DropDownListWarehouseDataBind(ddlWarehouse);

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
                {"FAmount", "0"},
                {"FBottleQty", "0"},
                {"FBottleOweQty", "0"},
                {"FCateName", ""},
                {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
                    deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            };

            // 在第一行新增一条数据
            btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            //btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            txtFDate.SelectedDate = DateTime.Now;

            txtFFreight.Text = "0.00";

        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    txtKeyId.Text = SequenceService.CreateSequence("TM",CurrentUser.AccountComId);
                    Region1.Title = "添加采购退货单";
                    var temp = new LHStockOut
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //采购退货
                        FType = Convert.ToInt32(GasEnumBill.PurchaseReturn),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                        FProgress = Convert.ToInt32(GasEnumBillStauts.Add),

                        FCate = "供应商"


                    };

                    //临时写入单据
                    StockOutService.Add(temp);

                    var summary = new JObject
                    {
                        {"FItemCode", "合计"},
                        {"FQty", 0},
                        {"FAmount", 0}
                    };

                    Grid1.SummaryData = summary;

                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region1.Title = "编辑采购退货单";
                    if (StockOut != null)
                    {
                        WebControlHandler.BindObjectToControls(StockOut, SimpleForm1);
                        txtFDate.SelectedDate = StockOut.FDate;
                        tbxFCustomer.Text = StockOut.FName;

                        if (!string.IsNullOrEmpty(StockOut.FDriver))
                            ddlFDriver.SelectedValueArray = StockOut.FDriver.Split(',');
                        if (!string.IsNullOrEmpty(StockOut.FShipper))
                            ddlFShipper.SelectedValueArray = StockOut.FShipper.Split(',');
                        if (!string.IsNullOrEmpty(StockOut.FSupercargo))
                            ddlFSupercargo.SelectedValueArray = StockOut.FSupercargo.Split(',');
                        if (!string.IsNullOrEmpty(StockOut.FSalesman))
                            ddlFSalesman.SelectedValueArray = StockOut.FSalesman.Split(',');

                        ddlSubject.SelectedValue = StockOut.FSubjectCode;
                        ddlFVehicleNum.SelectedValue = StockOut.FVehicleNum;
                        ddlFArea.SelectedValue = StockOut.FArea;
                        ddlDeliveryMethod.SelectedValue = StockOut.FDeliveryMethod;

                        ddlFT6PurchaseType.SelectedValue = StockOut.FT6PurchaseTypeNum;
                        ddlFT6Currency.SelectedValue = StockOut.FT6Currency;
                        ddlT6ReceiveSendType.SelectedValue = StockOut.FT6ReceiveSendTypeNum;
                        ddlWarehouse.SelectedValue = StockOut.FT6Warehouse;

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

        #endregion

    }
}