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

namespace Enterprise.IIS.business.PurchaseAudit
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
        private StockInService _stockInService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockInService StockInService
        {
            get { return _stockInService ?? (_stockInService = new StockInService()); }
            set { _stockInService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockInDetailsService _stockInDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockInDetailsService StockInDetailsService
        {
            get { return _stockInDetailsService ?? (_stockInDetailsService = new StockInDetailsService()); }
            set { _stockInDetailsService = value; }
        }

        /// <summary>
        ///     退货单
        /// </summary>
        private LHStockIn _stockIn;

        /// <summary>
        ///     客户档案
        /// </summary>
        private LHSupplier _supplier;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected LHSupplier Supplier
        {
            get { return _supplier ?? (_supplier = SupplierService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId)); }
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
        ///     退货单
        /// </summary>
        protected LHStockIn StockIn
        {
            get
            {
                return _stockIn ?? (_stockIn = StockInService.FirstOrDefault(p => p.KeyId == KeyId && p.FCompanyId == CurrentUser.AccountComId
              ));
            }
            set { _stockIn = value; }
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


        #region 操作日志部分
        /// <summary>
        ///     StockOutDetailsLogService
        /// </summary>
        private StockInDetailsLogService _stockInDetailsLogService;
        /// <summary>
        ///     StockOutDetailsLogService
        /// </summary>
        protected StockInDetailsLogService StockInDetailsLogService
        {
            get { return _stockInDetailsLogService ?? (_stockInDetailsLogService = new StockInDetailsLogService()); }
            set { _stockInDetailsLogService = value; }
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

        protected void tbxFLogisticsNumber_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFLogisticsNumber.Text.Trim()))
            {
                var passCard = PassCardService.Where(p => p.KeyId == tbxFLogisticsNumber.Text.Trim()).FirstOrDefault();

                if (passCard != null)
                {
                    //ddlDeliveryMethod.SelectedValue = "配送";//

                    txtFCode.Text = passCard.FCode;
                    tbxFCustomer.Text = passCard.FName;

                    var dis = new DispatchCenterService().Where(p => p.KeyId == passCard.FDispatchNum).FirstOrDefault();
                    if (dis != null)
                    {
                        if (!string.IsNullOrEmpty(dis.FDriver))
                            ddlFDriver.SelectedValueArray = dis.FDriver.Split(',');
                        //if (!string.IsNullOrEmpty(passCard.FShipper))
                        //    ddlFShipper.SelectedValueArray = passCard.FShipper.Split(',');
                        if (!string.IsNullOrEmpty(dis.FSupercargo))
                            ddlFSupercargo.SelectedValueArray = dis.FSupercargo.Split(',');
                        //if (!string.IsNullOrEmpty(StockOut.FSalesman))
                        //    ddlFSalesman.SelectedValueArray = passCard.FSalesman.Split(',');

                        ddlFVehicleNum.SelectedValue = dis.FVehicleNum;
                    }

                    var pamrs = new Dictionary<string, object>();
                    pamrs.Clear();

                    pamrs.Add("@FCode", passCard.FCode);
                    pamrs.Add("@SalesKeyId", txtKeyId.Text.Trim());
                    pamrs.Add("@FCompanyId", CurrentUser.AccountComId);
                    pamrs.Add("@KeyId", tbxFLogisticsNumber.Text.Trim());

                    SqlService.ExecuteProcedureCommand("proc_PassCardToPurchase", pamrs);

                    BindDataGrid();
                }
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

                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_PurchaseItem a WHERE a.FItemCode IN ({0}) and FCompanyId={1}", value, CurrentUser.AccountComId
                    ));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var details = new LHStockInDetails();

                            decimal price = GasHelper.GeSupplierPrice(txtFCode.Text.Trim(), //
                                table.Rows[i]["FItemCode"].ToString(), CurrentUser.AccountComId);

                            details.FItemCode = table.Rows[i]["FItemCode"].ToString();
                            details.FPrice = price;
                            details.FQty = 0;
                            details.FAmount = price;
                            details.FBottleQty = 0;
                            details.FBottleOweQty = 0;
                            details.FCompanyId = CurrentUser.AccountComId;
                            details.KeyId = txtKeyId.Text.Trim();
                            details.FCateId = Convert.ToInt32(table.Rows[i]["FId"].ToString());
                            details.FNum = table.Rows[i]["FINum"].ToString();//U8代码
                            details.FReturnQty = 0;
                            details.FRecycleQty = 0;

                            //默认包装物
                            details.FBottle = table.Rows[i]["FBottle"].ToString();

                            StockInDetailsService.AddEntity(details);

                            //日志
                            switch (Actions)
                            {
                                case WebAction.Add:
                                    break;
                                case WebAction.Edit:
                                    ////记录一下当前新增人操作内容
                                    //var detailslog = new LHStockInDetails_Log
                                    //{
                                    //    FUpdateBy = CurrentUser.AccountName,
                                    //    FUpdateDate = DateTime.Now,
                                    //    FItemCode = details.FItemCode,
                                    //    FPrice = price,
                                    //    FQty = 1,
                                    //    FAmount = price,
                                    //    FBottleQty = 1,
                                    //    FBottleOweQty = 0,
                                    //    FCompanyId = CurrentUser.AccountComId,
                                    //    KeyId = txtKeyId.Text.Trim(),
                                    //    FBottle = details.FBottle,
                                    //    FStatus = "新增",
                                    //    FMemo = string.Format(@"时间：{0} 操作人：{1}", DateTime.Now, CurrentUser.AccountName)
                                    //};

                                    //StockInDetailsLogService.Add(detailslog);

                                    break;
                            }
                        }

                        StockInDetailsService.SaveChanges();

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

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                Window1.Hidden = true;
                Window2.Hidden = true;
                //Window3.Hidden = true;

                var custmoer = SupplierService.Where(p => p.FName == tbxFCustomer.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
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
        protected void txtFAddress_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFItemCode_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            //Window3.Hidden = true;
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_PurchaseDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

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
                    DataSet dataSet = GasHelper.GetPurchaseItem(firstOrDefault.ToString(), CurrentUser.AccountComId);

                    DataTable table = dataSet.Tables[0];

                    if (table != null && table.Rows.Count > 0)
                    {
                        decimal price = GasHelper.GeSupplierPrice(txtFCode.Text.Trim(),//
                            table.Rows[0]["FItemCode"].ToString(), CurrentUser.AccountComId);

                        table.Rows[0]["FPrice"] = price;

                        var details = new LHStockInDetails
                        {
                            FItemCode = table.Rows[0]["FItemCode"].ToString(),
                            FPrice = price,
                            FQty = 0,
                            FAmount = price,
                            FBottleQty = 0,
                            FReturnQty = 0,
                            FBottleOweQty = 0,
                            FRecycleQty = 0,
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
                                var detailslog = new LHStockInDetails_Log
                                {
                                    FUpdateBy = CurrentUser.AccountName,
                                    FUpdateDate = DateTime.Now,
                                    FItemCode = table.Rows[0]["FItemCode"].ToString(),
                                    FPrice = price,
                                    FQty = 1,
                                    FAmount = price,
                                    FBottleQty = 1,
                                    FBottleOweQty = 0,
                                    KeyId = txtKeyId.Text.Trim(),
                                    FBottle = table.Rows[0]["FBottleCode"].ToString(),
                                    FStatus = "新增",
                                    FCompanyId = CurrentUser.AccountComId,
                                    FMemo = string.Format(@"时间：{0} 新增人：{1}", DateTime.Now, CurrentUser.AccountName)
                                };

                                StockInDetailsLogService.Add(detailslog);

                                break;
                        }


                        StockInDetailsService.Add(details);
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

            //            var details = StockInDetailsService.Where(p => p.FId == datakey&&p.FCompanyId==CurrentUser.AccountComId).FirstOrDefault();

            //            //写入原始，通过存储过程完成明细复制
            //            var parms = new Dictionary<string, object>();
            //            parms.Clear();

            //            parms.Add("@fid", datakey);
            //            parms.Add("@opr", CurrentUser.AccountName);
            //            parms.Add("@companyId", CurrentUser.AccountComId);
            //            SqlService.ExecuteProcedureCommand("proc_StockInDetails_Log", parms);

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

            //                var detailslog = new LHStockInDetails_Log
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
            //                    FStatus = "变更",
            //                    FCompanyId = CurrentUser.AccountComId,
            //                    FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
            //                };
            //                StockInDetailsLogService.Add(detailslog);

            //            }

            //            StockInDetailsService.SaveChanges();
            //        }
            //    }
            //}

            if (addList.Count > 0)
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

                StockInDetailsService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);

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

                var details = StockInDetailsService.Where(p => p.FId == datakey //
                    && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

                //写入原始，通过存储过程完成明细复制
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@fid", datakey);
                parms.Add("@opr", CurrentUser.AccountName);
                parms.Add("@companyId", CurrentUser.AccountComId);
                SqlService.ExecuteProcedureCommand("proc_StockOutDetails_Log", parms);

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
                                details.FBottleQty = (Int32)(Convert.ToDecimal(value));
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

                            if (key.Equals("FBottleQty"))
                            {
                                int result = 0;
                                int.TryParse(value, out result);

                                details.FBottleQty = result;

                                //details.FBottleQty = (Int32)(Convert.ToDecimal(value));
                            }
                            if (key.Equals("FT6Warehouse"))
                            {
                                details.FT6WarehouseNum = GasHelper.GetWarehouseByName(value);
                            }
                            if (key.Equals("FReturnQty"))
                            {
                                details.FReturnQty = Convert.ToDecimal(value);
                            }
                            if (key.Equals("FRecycleQty"))
                            {
                                int result = 0;
                                int.TryParse(value, out result);

                                details.FRecycleQty = result;
                                //details.FRecycleQty = (Int32)(Convert.ToDecimal(value));
                            }

                            var detailslog = new LHStockInDetails_Log
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
                                FStatus = "变更",
                                FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
                            };
                            StockInDetailsLogService.Add(detailslog);
                        }
                    }
                    #endregion
                }

                StockInDetailsService.SaveChanges();
            }
        }

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (StockIn != null)
            {
                ModifiedGrid();

                //StockIn.FClass = ddlFCate.SelectedValue;
                StockIn.FCode = txtFCode.Text;
                StockIn.FName = tbxFCustomer.Text;

                StockIn.FAddress = txtFAddress.Text.Trim();
                StockIn.FCompanyId = CurrentUser.AccountComId;

                StockIn.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                StockIn.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                StockIn.FFlag = 1;
                if (!string.IsNullOrEmpty(txtFFreight.Text.Trim()))
                {
                    StockIn.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                }
                StockIn.FLinkman = txtFLinkman.Text.Trim();
                StockIn.FMemo = txtFMemo.Text.Trim();
                StockIn.FPhone = txtFPhone.Text.Trim();

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyID", StockIn.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);
                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_PurchaseAmt", parms).Tables[0].Rows[0][0]);

                StockIn.FAmount = amt;
                StockIn.FDate = txtFDate.SelectedDate;
                StockIn.FSalesman = ddlFSalesman.SelectedValue;
                StockIn.FArea = ddlFArea.SelectedValue;
                StockIn.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());
                StockIn.FReconciliation = txtFReconciliation.Text;

                StockIn.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                StockIn.FLogisticsNumber = tbxFLogisticsNumber.Text;

                StockIn.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                StockIn.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
                StockIn.FSubjectCode = ddlSubject.SelectedValue;
                StockIn.FSubjectName = ddlSubject.SelectedText;
                //StockIn.CreateBy = CurrentUser.AccountName;


                //---------------------------------------------------------
                //收发类型
                StockIn.FT6ReceiveSendType = ddlT6ReceiveSendType.SelectedText;
                StockIn.FT6ReceiveSendTypeNum = ddlT6ReceiveSendType.SelectedValue;
                //部门名称
                StockIn.FT6Department = "";
                //部门代码
                StockIn.FT6DepartmentNum = "";
                //业务员代码
                StockIn.FT6SalesmanNum = "";
                //币种
                StockIn.FT6Currency = ddlFT6Currency.SelectedValue;
                //汇率
                StockIn.FT6ExchangeRate = Convert.ToDecimal(txtFT6ExchangeRate.Text);
                //销售类型
                StockIn.FT6PurchaseType = ddlFT6PurchaseType.SelectedText;
                StockIn.FT6PurchaseTypeNum = ddlFT6PurchaseType.SelectedValue;
                //T6同步
                //StockOut.FT6Status = "未同步";
                StockIn.FT6Warehouse = ddlWarehouse.SelectedValue;
                StockIn.FDistributionPoint = ddlFDistributionPoint.SelectedValue;


                StockInService.SaveChanges();

                //付款部分
                var parmsAuto = new Dictionary<string, object>();
                parmsAuto.Clear();

                parmsAuto.Add("@KeyId", StockIn.KeyId);
                parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                parmsAuto.Add("@FCate", "供应商");
                parmsAuto.Add("@date", Convert.ToDateTime(StockIn.FDate).ToShortDateString());
                parmsAuto.Add("@FSKNum", string.IsNullOrEmpty(StockIn.FSKNum) ? "" : StockIn.FSKNum);

                SqlService.ExecuteProcedureCommand("proc_FKOrderAuto", parmsAuto);

                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {

            var stock = StockInService.Where(p => p.KeyId == txtKeyId.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (stock != null)
            {
                ModifiedGrid();


                //stock.FClass = ddlFCate.SelectedValue;
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

                parms.Add("@keyID", stock.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);
                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_PurchaseAmt", parms).Tables[0].Rows[0][0]);

                stock.FAmount = amt;
                stock.FDate = txtFDate.SelectedDate;
                stock.FReconciliation = txtFReconciliation.Text;
                stock.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                stock.FLogisticsNumber = tbxFLogisticsNumber.Text;

                stock.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());

                stock.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                stock.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
                stock.FSubjectCode = ddlSubject.SelectedValue;
                stock.FSubjectName = ddlSubject.SelectedText;

                stock.FDate = txtFDate.SelectedDate;


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


                stock.FDistributionPoint = ddlFDistributionPoint.SelectedValue;


                StockInService.SaveChanges();

                //订单出库 
                var passCard = PassCardService.Where(p => p.KeyId == tbxFLogisticsNumber.Text.Trim()).FirstOrDefault();

                if (passCard != null)
                {
                    passCard.FOutFlag = "已出库";

                    PassCardService.SaveChanges();
                }


                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "WC", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "4");
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
                        FMemo = String.Format("单据号{0},{1}新增采购单据。", newKeyId, CurrentUser.AccountName)
                    };
                    GasHelper.AddBillStatus(billStatus);


                    //付款部分
                    var parmsAuto = new Dictionary<string, object>();
                    parmsAuto.Clear();

                    parmsAuto.Add("@KeyId", newKeyId);
                    parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                    parmsAuto.Add("@FCate", "供应商");
                    parmsAuto.Add("@date", Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));
                    parmsAuto.Add("@FSKNum", string.IsNullOrEmpty(stock.FSKNum) ? "" : stock.FSKNum);

                    SqlService.ExecuteProcedureCommand("proc_FKOrderAuto", parmsAuto);

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
            return Window1.GetShowReference(string.Format("../../Common/WinProduct.aspx"), "选择产品");
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

            tbxFLogisticsNumber.OnClientTriggerClick = Window2.GetSaveStateReference(tbxFLogisticsNumber.ClientID)
                    + Window2.GetShowReference("../../Common/WinTask.aspx?FType=45");

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

            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区

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
                    txtKeyId.Text = SequenceService.CreateSequence("TM", CurrentUser.AccountComId);
                    Region1.Title = "添加采购单";
                    txtFT6ExchangeRate.Text = "1";
                    var temp = new LHStockIn
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //发货单
                        FType = Convert.ToInt32(GasEnumBill.Purchase),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                        FProgress = Convert.ToInt32(GasEnumBillStauts.Add),

                        FCate = "供应商"

                    };

                    //临时写入单据
                    StockInService.Add(temp);

                    //合计
                    var summary = new JObject
                    {
                        {"FItemCode", "合计"},
                        {"FQty", 0},
                        {"FAmount", 0}
                    };
                    Grid1.SummaryData = summary;
                    break;
                case WebAction.Edit:
                    Region1.Title = "编辑采购单";
                    txtKeyId.Text = KeyId;
                    if (StockIn != null)
                    {
                        WebControlHandler.BindObjectToControls(StockIn, SimpleForm1);

                        txtFDate.SelectedDate = StockIn.FDate;
                        tbxFCustomer.Text = StockIn.FName;

                        if (!string.IsNullOrEmpty(StockIn.FDriver))
                            ddlFDriver.SelectedValueArray = StockIn.FDriver.Split(',');
                        if (!string.IsNullOrEmpty(StockIn.FShipper))
                            ddlFShipper.SelectedValueArray = StockIn.FShipper.Split(',');
                        if (!string.IsNullOrEmpty(StockIn.FSupercargo))
                            ddlFSupercargo.SelectedValueArray = StockIn.FSupercargo.Split(',');
                        if (!string.IsNullOrEmpty(StockIn.FSalesman))
                            ddlFSalesman.SelectedValueArray = StockIn.FSalesman.Split(',');

                        ddlSubject.SelectedValue = StockIn.FSubjectCode;
                        ddlFVehicleNum.SelectedValue = StockIn.FVehicleNum;
                        ddlFArea.SelectedValue = StockIn.FArea;
                        ddlDeliveryMethod.SelectedValue = StockIn.FDeliveryMethod;

                        ddlFT6PurchaseType.SelectedValue = StockIn.FT6PurchaseTypeNum;
                        ddlFT6Currency.SelectedValue = StockIn.FT6Currency;
                        ddlT6ReceiveSendType.SelectedValue = StockIn.FT6ReceiveSendTypeNum;
                        tbxFLogisticsNumber.Text = StockIn.FLogisticsNumber;
                        ddlWarehouse.SelectedValue = StockIn.FT6Warehouse;
                        ddlFDistributionPoint.SelectedValue = StockIn.FDistributionPoint;

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