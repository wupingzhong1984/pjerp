using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Transactions;

namespace Enterprise.IIS.business.Lease
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
        private ViewUnitService _viewUnitService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ViewUnitService ViewUnitService
        {
            get { return _viewUnitService ?? (_viewUnitService = new ViewUnitService()); }
            set { _viewUnitService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseService _leaseService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseService LeaseService
        {
            get { return _leaseService ?? (_leaseService = new LeaseService()); }
            set { _leaseService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseDetailsLogService _leaseDetailsLogService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseDetailsLogService LeaseDetailsLogService
        {
            get { return _leaseDetailsLogService ?? (_leaseDetailsLogService = new LeaseDetailsLogService()); }
            set { _leaseDetailsLogService = value; }
        }


        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseDetailsService _leaseDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseDetailsService LeaseDetailsService
        {
            get { return _leaseDetailsService ?? (_leaseDetailsService = new LeaseDetailsService()); }
            set { _leaseDetailsService = value; }
        }

        /// <summary>
        ///     客户档案
        /// </summary>
        private vm_Unit _vmUnit;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected vm_Unit Customer
        {
            get { return _vmUnit ?? (_vmUnit = ViewUnitService.FirstOrDefault(p => p.FCode == txtFCode.Text&&p.FCompanyId==CurrentUser.AccountComId)); }
            set { _vmUnit = value; }
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
        private LHLease _lease;

        /// <summary>
        ///     职员档案
        /// </summary>
        protected LHLease Lease
        {
            get { return _lease ?? (_lease = LeaseService.FirstOrDefault(p => p.KeyId == KeyId&&p.FCompanyId==CurrentUser.AccountComId)); }
            set { _lease = value; }
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
                #region reloadGrid
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

                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_SalesItem a WHERE a.FItemCode IN ({0}) and FCompanyId={1}", value,CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var details = new LHLeaseDetails();

                            decimal price = GasHelper.GeCustomerPrice(txtFCode.Text, //
                                table.Rows[i]["FItemCode"].ToString(),CurrentUser.AccountComId);

                            details.FBottle = table.Rows[i]["FItemCode"].ToString();
                            details.FPrice = price;
                            details.FBottleQty = 1;
                            details.FAmount = price;
                            details.FBottleQty = 1;
                            //details.FBottleOweQty = 0;
                            details.FCompanyId = CurrentUser.AccountComId;
                            details.KeyId = txtKeyId.Text.Trim();
                            details.FCateId = Convert.ToInt32(table.Rows[i]["FId"].ToString());
                            //每天租赁费
                            details.FRentDay = 0;
                            //押金
                            details.FAmount = price;

                            //已归还量
                            details.FReturnQty = 0;

                            LeaseDetailsService.AddEntity(details);

                            //日志
                            switch (Actions)
                            {
                                case WebAction.Add:
                                    break;
                                case WebAction.Edit:
                                    //记录一下当前新增人操作内容
                                    var detailslog = new LHLeaseDetails_Log
                                    {
                                        FUpdateBy = CurrentUser.AccountName,
                                        FUpdateDate = DateTime.Now,
                                        FBottle = details.FBottle,
                                        FPrice = price,
                                        FBottleQty = 1,
                                        FAmount = price,
                                        FCompanyId = CurrentUser.AccountComId,
                                        KeyId = txtKeyId.Text.Trim(),
                                        FStatus = "新增",
                                    };

                                    LeaseDetailsLogService.Add(detailslog);

                                    break;
                            }
                        }

                        LeaseDetailsService.SaveChanges();

                        //重新绑定值
                        BindDataGrid();
                    }
                }
                else if (GetRequestEventArgument().Contains("reloadSalesGrid:"))
                {
                    string key = GetRequestEventArgument().Split(':')[1];

                    tbxFSalesNum.Text = key;

                    var sales = new StockOutService().Where(p => p.KeyId == key&&p.FCompanyId==CurrentUser.AccountComId
                        ).FirstOrDefault();
                    if (sales != null)
                    {
                        txtFCode.Text = sales.FCode;
                        tbxFCustomer.Text = sales.FName;
                        txtFAddress.Text= sales.FAddress;
                        txtFPhone.Text = sales.FPhone;
                        txtFLinkman.Text = sales.FLinkman;
                    }

                    var data = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_SalesDetails WHERE FCateId =2000 AND keyId='{0}' and FCompanyId={1}", tbxFSalesNum.Text,CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var details = new LHLeaseDetails();

                            decimal price = GasHelper.GeCustomerPrice(txtFCode.Text, //
                                table.Rows[i]["FBottleCode"].ToString(),CurrentUser.AccountComId);

                            details.FBottle = table.Rows[i]["FBottleCode"].ToString();
                            details.FPrice = price;
                            details.FAmount = price;
                            details.FBottleQty = Convert.ToInt32(table.Rows[i]["FBottleQty"]);
                            details.FCompanyId = CurrentUser.AccountComId;
                            details.KeyId = txtKeyId.Text.Trim();

                            //钢瓶
                            details.FCateId = Convert.ToInt32(table.Rows[i]["FCateId"].ToString());
                            //每天租赁费
                            details.FRentDay = 0;
                            //押金
                            details.FAmount = price;

                            //已归还量
                            details.FReturnQty = 0;

                            LeaseDetailsService.AddEntity(details);

                            //日志
                            switch (Actions)
                            {
                                case WebAction.Add:
                                    break;
                                case WebAction.Edit:
                                    //记录一下当前新增人操作内容
                                    var detailslog = new LHLeaseDetails_Log
                                    {
                                        FUpdateBy = CurrentUser.AccountName,
                                        FUpdateDate = DateTime.Now,
                                        FBottle = details.FBottle,
                                        FPrice = price,
                                        FBottleQty = 1,
                                        FAmount = price,
                                        KeyId = txtKeyId.Text.Trim(),
                                        FCompanyId = CurrentUser.AccountComId,
                                        FStatus = "新增",
                                    };

                                    LeaseDetailsLogService.Add(detailslog);

                                    break;
                            }
                        }

                        LeaseDetailsService.SaveChanges();

                        //重新绑定值
                        BindDataGrid();
                    }
                }

                #endregion


                #region 更新合计

                //if (Grid1.Rows.Count > 0)
                //{
                //    if (GetRequestEventArgument() == "UPDATE_SUMMARY")
                //    {
                        // 页面要求重新计算合计行的值
                        OutputSummaryData();

                        //写入
                        ModifiedGrid();

                        // 为了保持前后台上传，回发更新合计行值后，必须进行数据绑定或者提交更改
                        Grid1.CommitChanges();
                //    }
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
            decimal sumFQty = 0.00M;
            decimal sumFAmount = 0.00M;

            foreach (JObject mergedRow in Grid1.GetMergedData())
            {
                JObject values = mergedRow.Value<JObject>("values");

                sumFQty += values.Value<decimal>("FBottleQty");
                sumFAmount += values.Value<decimal>("FAmount");
            }

            JObject summary = new JObject();
            summary.Add("FBottleQty", sumFQty);
            summary.Add("FAmount", sumFAmount);

            Grid1.SummaryData = summary;
        }

        #endregion

        /// <summary>
        ///      
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFBottle_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
            Window4.Hidden = true;
        }

        protected void tbxFSalesNum_OnTriggerClick(object sender, EventArgs e)
        {
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
        ///     文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                Window1.Hidden = true;
                Window2.Hidden = true;
                Window3.Hidden = true;
                Window4.Hidden = true;

                var custmoer = ViewUnitService.Where(p => p.FName == tbxFCustomer.Text.Trim()&&p.FCompanyId==CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFCode.Text = custmoer.FCode;
                    txtFAddress.Text = custmoer.FAddress.Trim();
                    txtFLinkman.Text = custmoer.FLinkman;
                    txtFPhone.Text = custmoer.FPhome;
                }
            }
        }


        protected void txtFAddress_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
            Window4.Hidden = true;
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
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_LeaseDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text,CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();

            var table = source.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFAmount = 0.00M;
                decimal sumFBottleQty = 0.00M;

                sumFAmount = Convert.ToDecimal(table.Compute("sum(FAmount)", "true"));
                sumFBottleQty = Convert.ToDecimal(table.Compute("sum(FBottleQty)", "true"));

                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FAmount", sumFAmount},
                    {"FBottleQty", sumFBottleQty},
                };

                Grid1.SummaryData = summary;
            }
            else
            {
                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FAmount", 0},
                    {"FBottleQty", 0},
                };

                Grid1.SummaryData = summary;
            }
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
            BindDataGrid();
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

                if (firstOrDefault != null&&!string.IsNullOrEmpty(firstOrDefault.ToString()))
                {
                    DataSet dataSet = GasHelper.GetSalesItem(firstOrDefault.ToString(), CurrentUser.AccountComId);

                    DataTable table = dataSet.Tables[0];

                    if (table != null && table.Rows.Count > 0)
                    {
                        decimal price = GasHelper.GeCustomerPrice(txtFCode.Text,//
                            table.Rows[0]["FItemCode"].ToString(),CurrentUser.AccountComId);

                        table.Rows[0]["FPrice"] = price;

                        var details = new LHLeaseDetails
                        {
                            FBottle = table.Rows[0]["FItemCode"].ToString(),
                            FPrice = price,
                            FBottleQty = 1,
                            FAmount = price,
                            //单日租金
                            FRentDay=1,
                            FCompanyId = CurrentUser.AccountComId,
                            KeyId = txtKeyId.Text.Trim(),
                            FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString())
                        };

                        switch (Actions)
                        {
                            case WebAction.Add:
                                break;
                            case WebAction.Edit:
                                //记录一下当前新增人操作内容
                                var detailslog = new LHLeaseDetails_Log
                                {
                                    FUpdateBy = CurrentUser.AccountName,
                                    FUpdateDate = DateTime.Now,
                                    FBottle = table.Rows[0]["FItemCode"].ToString(),
                                    FPrice = price,
                                    FBottleQty = 1,
                                    FAmount = price,
                                    KeyId = txtKeyId.Text.Trim(),
                                    FRentDay = 1,
                                    FCompanyId=CurrentUser.AccountComId,
                                    FStatus = "新增",
                                    //FMemo = string.Format(@"时间：{0} 新增人：{1}", DateTime.Now, CurrentUser.AccountName)
                                };

                                //detailslog.FCompanyId = CurrentUser.AccountComId;
                                //detailslog.FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString());

                                LeaseDetailsLogService.Add(detailslog);

                                break;
                        }

                        LeaseDetailsService.Add(details);
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

                LeaseDetailsService.Delete(p => p.FId == datakey&&p.FCompanyId==CurrentUser.AccountComId);

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

        #endregion

        #region Private Method


        /// <summary>
        ///     ModifiedGrid
        /// </summary>
        private void ModifiedGrid()
        {
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

                var details = LeaseDetailsService.Where(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

                //写入原始，通过存储过程完成明细复制
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@fid", datakey);
                parms.Add("@opr", CurrentUser.AccountName);
                parms.Add("@companyId", CurrentUser.AccountComId);
                SqlService.ExecuteProcedureCommand("proc_LeaseDetails_Log", parms);

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
                            if (key.Equals("FBottleQty"))
                            {
                                details.FBottleQty = Convert.ToInt32(value);
                                details.FAmount = details.FBottleQty * details.FPrice;
                            }

                            if (key.Equals("FRentDay"))
                            {//租金/天
                                details.FRentDay = Convert.ToDecimal(value);
                            }

                            if (key.Equals("FPrice"))
                            {//押金/个
                                details.FPrice = Convert.ToDecimal(value);
                                details.FAmount = details.FPrice * details.FBottleQty;
                            }

                            if (key.Equals("FAmount"))
                            {//押金
                                details.FAmount = details.FBottleQty * details.FPrice;
                            }
                        }

                        var detailslog = new LHLeaseDetails_Log
                        {
                            FUpdateBy = CurrentUser.AccountName,
                            FUpdateDate = DateTime.Now,
                            FBottle = details.FBottle,
                            FPrice = details.FPrice,
                            FBottleQty = details.FBottleQty,
                            FAmount = details.FAmount,
                            KeyId = details.KeyId,
                            FRentDay = details.FRentDay,
                            FStatus = "变更",
                            FCompanyId = CurrentUser.AccountComId,
                            //FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
                        };

                        LeaseDetailsLogService.Add(detailslog);
                    }
                    #endregion
                }

                LeaseDetailsService.SaveChanges();
            }
        }


        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (Lease != null)
            {
                ModifiedGrid();


                Lease.FCode = txtFCode.Text;
                Lease.FName = tbxFCustomer.Text;
                Lease.FAddress = txtFAddress.Text.Trim();
                Lease.FCompanyId = CurrentUser.AccountComId;

                Lease.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                Lease.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                Lease.FFlag = 1;
                Lease.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                Lease.FLinkman = txtFLinkman.Text.Trim();
                Lease.FMemo = txtFMemo.Text.Trim();
                Lease.FPhone = txtFPhone.Text.Trim();

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyID", Lease.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);
                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_LeaseAmt", parms).Tables[0].Rows[0][0]);

                Lease.FAmount = amt;
                Lease.FDate = txtFDate.SelectedDate;
                Lease.FSalesman = ddlFSalesman.SelectedValue;
                Lease.FArea = ddlFArea.SelectedValue;
                Lease.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());
                Lease.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                Lease.FLogisticsNumber = txtFLogisticsNumber.Text;
                Lease.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                Lease.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);

                //发货单号
                Lease.FSalesNum = tbxFSalesNum.Text;


                Lease.FSubjectCode = ddlSubject.SelectedValue;
                Lease.FSubjectName = ddlSubject.SelectedText;
                Lease.FCate = hfdClass.Text;
                Lease.CreateBy = CurrentUser.AccountName;




                LeaseService.SaveChanges();

                //收款部分
                var parmsAuto = new Dictionary<string, object>();
                parmsAuto.Clear();

                parmsAuto.Add("@KeyId", Lease.KeyId);
                parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                parmsAuto.Add("@FCate", hfdClass.Text);
                parmsAuto.Add("@date", Convert.ToDateTime(Lease.FDate).ToString("yyyyMMdd"));
                parmsAuto.Add("@FSKNum", Lease.FSKNum);
                parmsAuto.Add("@FAbstract", "收押金款");

                SqlService.ExecuteProcedureCommand("proc_SKLeaseAuto", parmsAuto);


                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var lease = LeaseService.Where(p => p.KeyId == txtKeyId.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (lease != null)
            {
                ModifiedGrid();

                lease.FCode = txtFCode.Text;
                lease.FName = tbxFCustomer.Text;
                lease.FAddress = txtFAddress.Text.Trim();
                lease.FCompanyId = CurrentUser.AccountComId;
                lease.FFlag = 1;
                lease.FDeleteFlag = 0;

                lease.FFreight = string.IsNullOrEmpty(txtFFreight.Text.Trim()) ? 0 : Convert.ToDecimal(txtFFreight.Text.Trim());

                lease.FLinkman = txtFLinkman.Text.Trim();
                lease.FMemo = txtFMemo.Text.Trim();
                lease.FPhone = txtFPhone.Text.Trim();
                lease.FDate = txtFDate.SelectedDate;
                lease.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                lease.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                var parms = new Dictionary<string, object>();
                parms.Clear();
                parms.Add("@companyId", CurrentUser.AccountComId);
                parms.Add("@keyID", lease.KeyId);
                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_LeaseAmt", parms).Tables[0].Rows[0][0]);

                lease.FAmount = amt;


                lease.FSalesman = ddlFSalesman.SelectedValue;
                lease.FArea = ddlFArea.SelectedValue;
                lease.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());
                lease.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                lease.FLogisticsNumber = txtFLogisticsNumber.Text;

                lease.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                lease.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);

                //发货单号
                lease.FSalesNum = tbxFSalesNum.Text;

                lease.FSubjectCode = ddlSubject.SelectedValue;
                lease.FSubjectName = ddlSubject.SelectedText;
                lease.FSKNum = "";
                lease.FCate = hfdClass.Text;

                LeaseService.SaveChanges();

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "ZL", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "7");
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
                        FMemo = String.Format("单据号{0},{1}新增租赁单据。", newKeyId, CurrentUser.AccountName)
                    };
                    GasHelper.AddBillStatus(billStatus);

                    //收款部分
                    var parmsAuto = new Dictionary<string, object>();
                    parmsAuto.Clear();

                    parmsAuto.Add("@KeyId", newKeyId);
                    parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                    parmsAuto.Add("@FCate", hfdClass.Text);
                    parmsAuto.Add("@date", Convert.ToDateTime(lease.FDate).ToString("yyyyMMdd"));
                    parmsAuto.Add("@FSKNum", lease.FSKNum);
                    parmsAuto.Add("@FAbstract", "收押金款");

                    SqlService.ExecuteProcedureCommand("proc_SKLeaseAuto", parmsAuto);

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

            // 钢瓶
            tbxFBottle.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFBottle.ClientID)
                    + Window1.GetShowReference("../../Common/WinBottle.aspx");

            // 客户
            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID,hfdClass.ClientID)
                    + Window2.GetShowReference("../../Common/WinUnit.aspx");

            //地址
            txtFAddress.OnClientTriggerClick = Window3.GetSaveStateReference(txtFAddress.ClientID)
                    + Window3.GetShowReference("../../Common/WinCustomerLink.aspx");

            //发货单号
            tbxFSalesNum.OnClientTriggerClick = Window4.GetSaveStateReference(tbxFSalesNum.ClientID)
                    + Window4.GetShowReference("../../Common/WinSalesList.aspx");

            GasHelper.DropDownListDriverDataBind(ddlFDriver);

            GasHelper.DropDownListShipperDataBind(ddlFShipper);

            GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);

            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

            GasHelper.DropDownListAreasDataBind(ddlFArea);

            GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

            GasHelper.DropDownListDeliveryMethodDataBind(ddlDeliveryMethod);

            GasHelper.DropDownListBankSubjectDataBind(ddlSubject);

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
                //{"FQty", "0"},
                {"FBottleQty", "0"},
                {"FAmount", "0"},
                //{"FBottleOweQty", "0"},
                {"FCateName", ""},
                {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
                    deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            };

            // 在第一行新增一条数据
            btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            //btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

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
                    Region1.Title = "添加气瓶租赁单";
                    txtKeyId.Text = SequenceService.CreateSequence("TM",CurrentUser.AccountComId);
                    var temp = new LHLease
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag=1,

                        //发货单
                        FType = Convert.ToInt32(GasEnumBill.Lease),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                        FProgress = Convert.ToInt32(GasEnumBillStauts.Add),

                    };

                    //临时写入单据
                    LeaseService.Add(temp);

                    break;
                case WebAction.Edit:
                    Region1.Title = "编辑气瓶租赁单";
                    txtKeyId.Text = KeyId;
                    if (Lease != null)
                    {
                        WebControlHandler.BindObjectToControls(Lease, SimpleForm1);
                        txtFDate.SelectedDate = Lease.FDate;
                        tbxFCustomer.Text = Lease.FName;

                        if (!string.IsNullOrEmpty(Lease.FDriver))
                            ddlFDriver.SelectedValueArray = Lease.FDriver.Split(',');
                        if (!string.IsNullOrEmpty(Lease.FShipper))
                            ddlFShipper.SelectedValueArray = Lease.FShipper.Split(',');
                        if (!string.IsNullOrEmpty(Lease.FSupercargo))
                            ddlFSupercargo.SelectedValueArray = Lease.FSupercargo.Split(',');
                        if (!string.IsNullOrEmpty(Lease.FSalesman))
                            ddlFSalesman.SelectedValueArray = Lease.FSalesman.Split(',');

                        ddlFVehicleNum.SelectedValue = Lease.FVehicleNum;
                        ddlFArea.SelectedValue = Lease.FArea;
                        ddlDeliveryMethod.SelectedValue = Lease.FDeliveryMethod;
                        hfdClass.Text = Lease.FCate;

                        tbxFSalesNum.Text = Lease.FSalesNum;

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