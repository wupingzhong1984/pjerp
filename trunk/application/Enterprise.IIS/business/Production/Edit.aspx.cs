using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

namespace Enterprise.IIS.business.Production
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
        private LHStockIn _stockIn;

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
                if (!string.IsNullOrEmpty(txtFCode.Text))
                {
                }

                if (GetRequestEventArgument().Contains("reloadGrid:"))
                {
                    #region Grid1
                    //查找所选商品代码，查访产品集合
                    string keys = GetRequestEventArgument().Split(':')[1];

                    var values = keys.Split(',');

                    string codes = String.Empty;
                    for (int i = 0; i < values.Count(); i++)
                    {
                        codes += string.Format("'{0}',", values[i]);
                    }

                    var value = codes.Substring(0, codes.Length - 1);

                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_SalesItem a WHERE a.FItemCode IN ({0}) and FCompanyId={1}", value, CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var details = new LHStockOutDetails();

                            decimal price = GasHelper.GeSupplierPrice("", //
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

                            details.FCostPrice = 0;
                            details.FBalance = 0;

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
                                        FStatus = "新增",
                                        FCompanyId = CurrentUser.AccountComId,
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
 #endregion
                }

                #region 更新合计

                if (Grid1.Rows.Count > 0)
                {
                    if (GetRequestEventArgument() == "UPDATE_SUMMARY")
                    {
                        OutputSummaryData();

                        ModifiedGrid();

                        Grid1.CommitChanges();
                    }
                }

                #endregion
            }
        }

        #region OutputSummaryData
        /// <summary>
        ///     OutputSummaryData
        /// </summary>
        private void OutputSummaryData()
        {
            decimal sumFQty = 0.00M;
            decimal sumFAmount = 0.00M;

            foreach (JObject mergedRow in Grid1.GetMergedData())
            {
                JObject values = mergedRow.Value<JObject>("values");

                sumFQty += values.Value<decimal>("FQty");
                sumFAmount += values.Value<decimal>("FAmount");
            }

            JObject summary = new JObject();
            summary.Add("FQty", sumFQty);
            summary.Add("FAmount", sumFAmount);

            Grid1.SummaryData = summary;

            txtFCostPrice.Text = sumFAmount.ToString("####.##");
        }

        #endregion

        protected void tbxFName_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFName.Text.Trim()))
            {
                var bottle = GasHelper.BottleByCode(txtFCode.Text, CurrentUser.AccountComId);

                if (bottle != null)
                    tbxFBottle.SelectedValue = bottle.FBottleCode;

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
        }

        /// <summary>
        /// 通过配方添加原料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddFromulaMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                var parms = new Dictionary<string, object>();
                parms.Clear();
                parms.Add("@keyid",txtKeyId.Text);
                parms.Add("@companyId", CurrentUser.AccountComId);
                parms.Add("@FCode", txtFCode.Text);
                parms.Add("@num", txtFQty.Text);

                var data= SqlService.ExecuteProcedureCommand("proc_Product", parms);

                //绑定数据源
                Grid1.DataSource = data;
                Grid1.DataBind();

                var tblmaterial = data.Tables[0];

                if (tblmaterial != null && tblmaterial.Rows.Count > 0)
                {
                    decimal sumFQty = 0.00M;
                    decimal sumFAmount = 0.00M;

                    for (int i = 0; i < tblmaterial.Rows.Count; i++)
                    {
                        sumFQty += Convert.ToDecimal(tblmaterial.Rows[i]["FQty"]);
                        sumFAmount += Convert.ToDecimal(tblmaterial.Rows[i]["FAmount"]);
                    }

                    var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", sumFQty},
                    {"FAmount", sumFAmount}
                };

                    Grid1.SummaryData = summary;
                }
                else
                {
                    var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", 0},
                    {"FAmount", 0}
                };

                    Grid1.SummaryData = summary;
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFCode_OnTriggerClick(object sender, EventArgs e)
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
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var material = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_SalesDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = material;
            Grid1.DataBind();

            var tblmaterial = material.Tables[0];

            if (tblmaterial != null && tblmaterial.Rows.Count > 0)
            {
                decimal sumFQty = 0.00M;
                decimal sumFAmount = 0.00M;

                for (int i = 0; i < tblmaterial.Rows.Count; i++)
                {
                    sumFQty += Convert.ToDecimal(tblmaterial.Rows[i]["FQty"]);
                    sumFAmount += Convert.ToDecimal(tblmaterial.Rows[i]["FAmount"]);
                }

                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", sumFQty},
                    {"FAmount", sumFAmount}
                };


                txtFCostPrice.Text = sumFAmount.ToString("####.##");

                Grid1.SummaryData = summary;
            }
            else
            {
                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", 0},
                    {"FAmount", 0}
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

                if (firstOrDefault != null)
                {
                    DataSet dataSet = GasHelper.GetSalesItem(firstOrDefault.ToString(), CurrentUser.AccountComId);

                    DataTable table = dataSet.Tables[0];

                    if (table != null && table.Rows.Count > 0)
                    {
                        decimal price = GasHelper.GeSupplierPrice("",//
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

                StockOutDetailsService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);

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


        /// <summary>
        ///     ModifiedGrid
        /// </summary>
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
                            }

                            if (key.Equals("FBottleQty"))
                            {
                                details.FBottleQty = Convert.ToInt32(value);
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
            if (StockIn != null)
            {
                ModifiedGrid();

                StockIn.FCate = "内部";
                StockIn.FCode = CurrentUser.AccountComId.ToString(CultureInfo.InvariantCulture);
                StockIn.FName = "";
                StockIn.FMemo = txtFMemo.Text.Trim();
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyID", StockIn.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);
                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_PurchaseAmt", parms).Tables[0].Rows[0][0]);

                StockIn.FAmount = amt;
                StockIn.FBatchNumber = txtFBatchNumber.Text.Trim();

                StockIn.FSurveyor = !ddlFSurveyor.SelectedValue.Equals("-1") ? ddlFSurveyor.SelectedText : "";
                StockIn.FProducer = !ddlFProducer.SelectedValue.Equals("-1") ? ddlFProducer.SelectedText : "";//GasHelper.GetDropDownListArrayString(ddlFProducer.SelectedItemArray);
                StockIn.FProductionWorkshop = ddlWorkShop.SelectedValue;
                StockIn.FDate = txtFDate.SelectedDate;
                StockIn.CreateBy = CurrentUser.AccountName;
                
                StockInService.SaveChanges();

                var prod =
                    StockInDetailsService.FirstOrDefault(
                        p => p.KeyId == txtKeyId.Text && p.FCompanyId == CurrentUser.AccountComId);

                if (prod != null)
                {
                    prod.FItemCode = txtFCode.Text;
                    prod.FQty = Convert.ToDecimal(txtFQty.Text);
                    prod.FBottle = tbxFBottle.SelectedValue;
                    prod.FBottleQty = Convert.ToInt16(txtFBottleQty.Text);
                    prod.FCostPrice = Convert.ToDecimal(txtFCostPrice.Text);
                    prod.FBalance = Convert.ToDecimal(txtFBalance.Text);
                    prod.FCostPrice = Convert.ToDecimal(txtFCostPrice.Text);
                    prod.FBalance = Convert.ToDecimal(txtFBalance.Text);

                    StockInDetailsService.SaveChanges();
                }

                var orders = new Dictionary<string, object>();
                orders.Clear();
                orders.Add("@KeyId", StockIn.KeyId);
                orders.Add("@companyId", CurrentUser.AccountComId);

                SqlService.ExecuteProcedureCommand("proc_ProuctOrder", orders);

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

                var prod =
                   StockInDetailsService.FirstOrDefault(
                       p => p.KeyId == txtKeyId.Text && p.FCompanyId == CurrentUser.AccountComId);

                if (prod == null)
                {
                    //生产产品入库
                    var product = new LHStockInDetails
                    {
                        KeyId = txtKeyId.Text,
                        FItemCode = txtFCode.Text,
                        FPrice = 0,
                        FAmount = 0,
                        FQty = Convert.ToDecimal(txtFQty.Text),
                        FBottle = tbxFBottle.SelectedValue,
                        FBottleQty = Convert.ToInt16(txtFBottleQty.Text),
                        FBottleOweQty = 0,
                        FCompanyId = CurrentUser.AccountComId,
                        FCateId = 2000,
                        FMemo = "生产入库",
                        FCostPrice = Convert.ToDecimal(txtFCostPrice.Text),
                        FBalance = Convert.ToDecimal(txtFBalance.Text),
                        
                    };

                    StockInDetailsService.Add(product);
                }
                else
                {
                    prod.FItemCode = txtFCode.Text;
                    prod.FQty = Convert.ToDecimal(txtFQty.Text);
                    prod.FBottle = tbxFBottle.SelectedValue;
                    prod.FBottleQty = Convert.ToInt32(txtFBottleQty.Text);
                    prod.FCostPrice = Convert.ToDecimal(txtFCostPrice.Text);
                    prod.FBalance = Convert.ToDecimal(txtFBalance.Text);

                    StockInDetailsService.SaveChanges();
                }
                //-------------------------------------------------------------------

                stock.FCompanyId = CurrentUser.AccountComId;
                stock.FFlag = 1;
                stock.FDeleteFlag = 0;
                stock.FMemo = txtFMemo.Text.Trim();

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyID", stock.KeyId);
                parms.Add("@companyId",CurrentUser.AccountComId);
                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_PurchaseAmt", parms).Tables[0].Rows[0][0]);

                stock.FAmount = amt;

                stock.FSurveyor = !ddlFSurveyor.SelectedValue.Equals("-1") ? ddlFSurveyor.SelectedText : "";
                stock.FProducer = !ddlFProducer.SelectedValue.Equals("-1") ? ddlFProducer.SelectedText : "";//GasHelper.GetDropDownListArrayString(ddlFProducer.SelectedItemArray);
                
                stock.FBatchNumber = txtFBatchNumber.Text.Trim();

                stock.FProductionWorkshop = ddlWorkShop.SelectedValue;

                stock.FCate = "内部";
                stock.FCode = CurrentUser.AccountComId.ToString(CultureInfo.InvariantCulture);
                stock.FName = "";
                stock.FDate = txtFDate.SelectedDate;


                StockInService.SaveChanges();
               

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "PR", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "6");
                    orderParms.Add("@companyId", CurrentUser.AccountComId);
                    SqlService.ExecuteProcedureCommand("proc_num", orderParms);
                    txtKeyId.Text = newKeyId;



                    var orders = new Dictionary<string, object>();
                    orders.Clear();
                    orders.Add("@KeyId", newKeyId);
                    orders.Add("@companyId", CurrentUser.AccountComId);

                    SqlService.ExecuteProcedureCommand("proc_ProuctOrder", orders);


                    //新增日志
                    var billStatus = new LHBillStatus
                    {
                        KeyId = newKeyId,
                        FCompanyId = CurrentUser.AccountComId,
                        FActionName = "新增",
                        FDate = DateTime.Now,
                        FDeptId = CurrentUser.AccountOrgId,
                        FOperator = CurrentUser.AccountName,
                        FMemo = String.Format("单据号{0},{1}新增生产单据。", newKeyId, CurrentUser.AccountName)
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

            //原料
            tbxFItemCode.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFItemCode.ClientID)
                    + Window1.GetShowReference("../../Common/WinFormula.aspx");

            //产品
            tbxFName.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFName.ClientID, tbxFBottle.ClientID)
                    + Window2.GetShowReference("../../Common/WinProducReference.aspx");

            GasHelper.DropDownListProducerDataBind(ddlFProducer);

            GasHelper.DropDownListBottleDataBind(tbxFBottle);

            GasHelper.DropDownListSurveyorDataBind(ddlFSurveyor);

            GasHelper.DropDownListWorkshopDataBind(ddlWorkShop);

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
                {"FCateName", ""},
                {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
                    deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            };

            // 在第一行新增一条数据
            btnAddFormula.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

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

                    Region1.Title = "添加生产单";
                    txtKeyId.Text = SequenceService.CreateSequence("TM", CurrentUser.AccountComId);
                    var temp = new LHStockIn
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //发货单
                        FType = Convert.ToInt32(GasEnumBill.Production),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                        FProgress = Convert.ToInt32(GasEnumBillStauts.Add),
                    };

                    //临时写入单据
                    StockInService.Add(temp);

                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region1.Title = "编辑生产单";

                    if (StockIn != null)
                    {
                        txtFMemo.Text = StockIn.FMemo;
                        ddlFProducer.SelectedValue = StockIn.FProducer;
                        ddlFSurveyor.SelectedValue = StockIn.FSurveyor;
                        ddlWorkShop.SelectedValue = StockIn.FProductionWorkshop;
                        var product =
                            StockInDetailsService.Where(
                                p => p.KeyId == KeyId && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

                        if (product != null)
                        {
                            txtFCode.Text = product.FItemCode;
                            txtFQty.Text = product.FQty.ToString();
                            tbxFBottle.SelectedValue = product.FBottle;
                            txtFBottleQty.Text = product.FBottleQty.ToString();
                            txtFCostPrice.Text = product.FCostPrice.ToString();
                            txtFBalance.Text = product.FBalance.ToString();
                           
                            //
                            var item =
                                new ItemsService().Where(
                                    p => p.FCode == txtFCode.Text && p.FCompanyId == CurrentUser.AccountComId)
                                    .FirstOrDefault();

                            if (item != null)
                            {
                                tbxFName.Text = item.FName;
                            }

                        }

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