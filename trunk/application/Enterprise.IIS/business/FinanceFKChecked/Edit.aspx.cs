﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.FinanceFKChecked
{
    public partial class Edit : PageBase
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
        private FKOrderCheckedService _fkOrderCheckedService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FKOrderCheckedService FkOrderCheckedService
        {
            get { return _fkOrderCheckedService ?? (_fkOrderCheckedService = new FKOrderCheckedService()); }
            set { _fkOrderCheckedService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private FKOrderCheckedBanksService _fkOrderCheckedBanksService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FKOrderCheckedBanksService FkOrderCheckedBanksService
        {
            get { return _fkOrderCheckedBanksService ?? (_fkOrderCheckedBanksService = new FKOrderCheckedBanksService()); }
            set { _fkOrderCheckedBanksService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private FKOrderBanksService _fkOrderBanksService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FKOrderBanksService FkOrderBanksService
        {
            get { return _fkOrderBanksService ?? (_fkOrderBanksService = new FKOrderBanksService()); }
            set { _fkOrderBanksService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private FKOrderCheckedDetailsService _fkOrderCheckedDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FKOrderCheckedDetailsService FkOrderCheckedDetailsService
        {
            get { return _fkOrderCheckedDetailsService ?? (_fkOrderCheckedDetailsService = new FKOrderCheckedDetailsService()); }
            set { _fkOrderCheckedDetailsService = value; }
        }

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
        ///     
        /// </summary>
        private LHFKOrderChecked _fkOrderChecked;

        /// <summary>
        ///     
        /// </summary>
        protected LHFKOrderChecked FkOrderChecked
        {
            get { return _fkOrderChecked ?? (_fkOrderChecked = FkOrderCheckedService.FirstOrDefault(p => p.KeyId == KeyId&&p.FCompanyId==CurrentUser.AccountComId)); }
            set { _fkOrderChecked = value; }
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var parms = new Dictionary<string, object>();
            parms.Clear();
            parms.Add("@FCode", txtFCode.Text);
            parms.Add("@companyId", CurrentUser.AccountComId);

            var data = SqlService.ExecuteProcedureCommand("proc_FinanceFKChecked", parms);

            //绑定数据源
            Grid1.DataSource = data;
            Grid1.DataBind();

            var table = data.Tables[0];
            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFAmt = 0.00M;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    sumFAmt += Convert.ToDecimal(table.Rows[i]["FAmount"]);
                }
                var summary = new JObject
                {
                    {"keyId", "合计"},
                    {"FAmount", sumFAmt},
                };
                Grid1.SummaryData = summary;
            }

            var parmsSales = new Dictionary<string, object>();
            parmsSales.Clear();
            parmsSales.Add("@FCode", txtFCode.Text);
            parmsSales.Add("@companyId", CurrentUser.AccountComId);

            var dataSales = SqlService.ExecuteProcedureCommand("proc_FinancePurchaseChecked", parmsSales);

            //绑定数据源
            Grid2.DataSource = dataSales;
            Grid2.DataBind();

            var tableSales = dataSales.Tables[0];
            if (tableSales != null && tableSales.Rows.Count > 0)
            {
                decimal sumFAmt = 0.00M;
                for (int i = 0; i < tableSales.Rows.Count; i++)
                {
                    sumFAmt += Convert.ToDecimal(tableSales.Rows[i]["FAmount"]);
                }
                var summary = new JObject
                {
                    {"FDate", "合计"},
                    {"FAmount", sumFAmt},
                };
                Grid2.SummaryData = summary;
            }

            var parmsAr = new Dictionary<string, object>();
            parmsAr.Clear();
            parmsAr.Add("@FCode", txtFCode.Text);
            parmsAr.Add("@companyId", CurrentUser.AccountComId);
            var amt = SqlService.ExecuteProcedureCommand("proc_SupplierAP", parmsAr);
            var tblAmt = amt.Tables[0];
            if (tblAmt != null && tblAmt.Rows.Count > 0)
            {
                lblFInit.Text = tblAmt.Rows[0]["FInit"].ToString();
                lblFSK.Text = tblAmt.Rows[0]["FSK"].ToString();
                lblFSales.Text = tblAmt.Rows[0]["FSales"].ToString();
                lblFSalesReturn.Text = tblAmt.Rows[0]["FSalesReturn"].ToString();
                lblFEnd.Text = tblAmt.Rows[0]["FEnd"].ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var parms = new Dictionary<string, object>();
            parms.Clear();
            parms.Add("@FCode", txtFCode.Text);
            parms.Add("@companyId", CurrentUser.AccountComId);
            var data = SqlService.ExecuteProcedureCommand("proc_FinancePurchaseChecked", parms);

            //绑定数据源
            Grid1.DataSource = data;
            Grid1.DataBind();

            var table = data.Tables[0];
            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFAmt = 0.00M;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    sumFAmt += Convert.ToDecimal(table.Rows[i]["FAmount"]);
                }
                var summary = new JObject
                {
                    {"keyId", "合计"},
                    {"FAmount", sumFAmt},
                };
                Grid1.SummaryData = summary;
            }
        }

        /// <summary>
        ///     单元格编辑与修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
        }
        /// <summary>
        ///     单元格编辑与修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid2_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
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

        protected void tbxBankCode_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
        }

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                Window1.Hidden = true;
                Window2.Hidden = true;
                Window3.Hidden = true;

                var custmoer = SupplierService.Where(p => p.FName == tbxFCustomer.Text.Trim()&&p.FCompanyId==CurrentUser.AccountComId).FirstOrDefault();
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
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (FkOrderChecked != null)
            {
                FkOrderChecked.FCode = txtFCode.Text;
                FkOrderChecked.FName = tbxFCustomer.Text;
                FkOrderChecked.FMemo = txtFMemo.Text.Trim();

                return FkOrderCheckedService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var order = FkOrderCheckedService.Where(p => p.KeyId == txtKeyId.Text.Trim()&&p.FCompanyId==CurrentUser.AccountComId).FirstOrDefault();

            if (order != null)
            {
                //核销主单据
                //--------------------------------------------------
                order.FCode = txtFCode.Text;
                order.FName = tbxFCustomer.Text;
                //--------------------------------------------------
                order.FDeleteFlag = 0;
                order.FMemo = txtFMemo.Text.Trim();
                order.CreateBy = CurrentUser.AccountName;
                order.FDate = txtFDate.SelectedDate;

                //本次实际核销的金额
                decimal checkedAmt = 0.00M;

                //收款单
                decimal sksum = 0.00M;
                int[] sks = Grid1.SelectedRowIndexArray;

                for (int i = 0; i < sks.Length; i++)
                {
                    sksum += Convert.ToDecimal(Grid1.DataKeys[Grid1.SelectedRowIndexArray[i]][2].ToString());
                }

                if (sksum <= 0)
                {
                    Alert.Show("要核销的收款单已核销完，暂无需核销单据。", MessageBoxIcon.Information);
                    return false;
                }

                //销售金额
                decimal salesum = 0.00M;
                int[] sales = Grid2.SelectedRowIndexArray;

                for (int i = 0; i < sales.Length; i++)
                {
                    salesum += Convert.ToDecimal(Grid2.DataKeys[Grid2.SelectedRowIndexArray[i]][1].ToString());
                }

                if (sksum <= 0)
                {
                    Alert.Show("要核销的发货单已核销完，暂无需核销单据。", MessageBoxIcon.Information);
                    return false;
                }

                if (sksum == salesum)
                {
                    //实际核销款
                    checkedAmt = sksum;

                    for (int i = 0; i < sks.Length; i++)
                    {
                        int id = Convert.ToInt32(Grid1.DataKeys[sks[i]][1].ToString());
                        string keyid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[i]][0].ToString();
                        var banks = new LHFKOrderCheckedBanks();
                        banks.KeyId = txtKeyId.Text;
                        banks.CBankId = id;
                        banks.FAmt = Convert.ToDecimal(Grid1.DataKeys[sks[i]][2]);
                        banks.FCompanyId = CurrentUser.AccountComId;
                        var bank = FkOrderCheckedBanksService.Where(p => p.KeyId == txtKeyId.Text && p.CBankId == id&&p.FCompanyId==CurrentUser.AccountComId).FirstOrDefault();
                        if (bank != null)
                        {
                            FkOrderCheckedBanksService.SaveChanges();
                        }
                        else
                        {
                            FkOrderCheckedBanksService.Add(banks);
                        }
                    }
                    for (int i = 0; i < sales.Length; i++)
                    {
                        string keyid = Grid2.DataKeys[Grid2.SelectedRowIndexArray[i]][0].ToString();

                        var details = new LHFKOrderCheckedDetails();
                        details.KeyId = txtKeyId.Text;
                        details.FSaleKeyId = keyid;
                        details.FAmount = Convert.ToDecimal(Grid2.DataKeys[sks[i]][1]);
                        details.FCompanyId = CurrentUser.AccountComId;
                        var detail =
                            FkOrderCheckedDetailsService.Where(p => p.KeyId == txtKeyId.Text && p.FSaleKeyId == keyid && p.FCompanyId == CurrentUser.AccountComId)
                                .FirstOrDefault();

                        if (detail != null)
                        {
                            FkOrderCheckedDetailsService.SaveChanges();
                        }
                        else
                        {
                            FkOrderCheckedDetailsService.Add(details);
                        }
                    }
                }
                else if (sksum > salesum)
                {
                    //实际核销款
                    checkedAmt = salesum;

                    //分析
                    decimal kfpje = salesum;
                    for (int i = 0; i < sks.Length; i++)
                    {
                        int id = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[i]][1].ToString());
                        //string keyid = Grid1.DataKeys[sks[i]][0].ToString();
                        /////////////////////////////////////////////////
                        var sk = Convert.ToDecimal(Grid1.DataKeys[Grid1.SelectedRowIndexArray[i]][2]);

                        decimal hxje = 0.00M;
                        decimal yszj = sk;
                        if (kfpje == 0)
                        {
                            hxje = 0;
                        }
                        else
                        {
                            decimal kyje = kfpje - yszj;

                            if (kyje > 0)
                            {
                                hxje = yszj;
                                kfpje = kyje;
                            }
                            else if (kyje == 0)
                            {
                                hxje = kfpje;
                                kfpje = kyje;
                                //continue;
                            }
                            else
                            {
                                hxje = kfpje;
                                kfpje = 0;
                                //continue;
                            }
                        }

                        /////////////////////////////////////////////////
                        var banks = new LHFKOrderCheckedBanks();
                        banks.KeyId = txtKeyId.Text;
                        banks.CBankId = id;
                        banks.FCompanyId = CurrentUser.AccountComId;

                        //本次核销款
                        banks.FAmt = hxje;

                        var bank =
                            FkOrderCheckedBanksService.Where(p => p.KeyId == txtKeyId.Text && p.CBankId == id && p.FCompanyId == CurrentUser.AccountComId)
                                .FirstOrDefault();
                        if (bank != null)
                        {
                            FkOrderCheckedBanksService.SaveChanges();
                        }
                        else
                        {
                            FkOrderCheckedBanksService.Add(banks);
                        }
                    }
                    /////////////////////////////////////////////////
                    decimal kfpje2 = salesum;
                    for (int i = 0; i < sales.Length; i++)
                    {
                        string keyid = Grid2.DataKeys[Grid2.SelectedRowIndexArray[i]][0].ToString();
                        //////////////////////////////////////////////////////////
                        var sale = Convert.ToDecimal(Grid2.DataKeys[Grid2.SelectedRowIndexArray[i]][1]);
                        decimal hxje = 0.00M;
                        decimal yszj = sale;
                        if (kfpje2 == 0)
                        {
                            hxje = 0;
                        }
                        else
                        {
                            decimal kyje = kfpje2 - yszj;

                            if (kyje > 0)
                            {
                                hxje = yszj;
                                kfpje2 = kyje;
                            }
                            else if (kyje == 0)
                            {
                                hxje = kfpje2;
                                kfpje2 = kyje;
                                //continue;
                            }
                            else
                            {
                                hxje = kfpje2;
                                kfpje = 0;
                                //continue;
                            }
                        }
                        /////////////////////////////////////////////////////////
                        var details = new LHFKOrderCheckedDetails();
                        details.KeyId = txtKeyId.Text;
                        details.FSaleKeyId = keyid;
                        details.FAmount = hxje;
                        details.FCompanyId = CurrentUser.AccountComId;
                        var detail =
                            FkOrderCheckedDetailsService.Where(p => p.KeyId == txtKeyId.Text && p.FSaleKeyId == keyid && p.FCompanyId == CurrentUser.AccountComId)
                                .FirstOrDefault();

                        if (detail != null)
                        {
                            FkOrderCheckedDetailsService.SaveChanges();
                        }
                        else
                        {
                            FkOrderCheckedDetailsService.Add(details);
                        }
                    }
                }
                else
                {
                    //实际核销款
                    checkedAmt = sksum;

                    //分析
                    decimal kfpje = sksum;
                    for (int i = 0; i < sks.Length; i++)
                    {
                        int id = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[i]][1].ToString());
                        //string keyid = Grid1.DataKeys[sks[i]][0].ToString();
                        /////////////////////////////////////////////////
                        var sk = Convert.ToDecimal(Grid1.DataKeys[Grid1.SelectedRowIndexArray[i]][2]);

                        decimal hxje = 0.00M;

                        decimal yszj = sk;

                        if (kfpje == 0)
                        {
                            hxje = 0;
                        }
                        else
                        {
                            decimal kyje = kfpje - yszj;

                            if (kyje > 0)
                            {
                                hxje = yszj;
                                kfpje = kyje;
                            }
                            else if (kyje == 0)
                            {
                                hxje = kfpje;
                                kfpje = kyje;
                                //continue;
                            }
                            else
                            {
                                hxje = kfpje;
                                kfpje = 0;
                                //continue;
                            }
                        }

                        /////////////////////////////////////////////////
                        var banks = new LHFKOrderCheckedBanks();
                        banks.KeyId = txtKeyId.Text;
                        banks.CBankId = id;
                        banks.FCompanyId = CurrentUser.AccountComId;
                        //本次核销款
                        banks.FAmt = hxje;

                        var bank = FkOrderCheckedBanksService.Where(p => p.KeyId == txtKeyId.Text && p.CBankId == id && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                        if (bank != null)
                        {
                            FkOrderCheckedBanksService.SaveChanges();
                        }
                        else
                        {
                            FkOrderCheckedBanksService.Add(banks);
                        }
                    }


                    decimal kfpje2 = sksum;
                    for (int i = 0; i < sales.Length; i++)
                    {
                        string keyid = Grid2.DataKeys[Grid2.SelectedRowIndexArray[i]][0].ToString();
                        //////////////////////////////////////////////////////////
                        var sale = Convert.ToDecimal(Grid2.DataKeys[Grid2.SelectedRowIndexArray[i]][1]);
                        decimal hxje = 0.00M;
                        decimal yszj = sale;
                        if (kfpje2 == 0)
                        {
                            hxje = 0;
                        }
                        else
                        {
                            decimal kyje = kfpje2 - yszj;

                            if (kyje > 0)
                            {
                                hxje = yszj;
                                kfpje2 = kyje;
                            }
                            else if (kyje == 0)
                            {
                                hxje = kfpje2;
                                kfpje2 = kyje;
                                //continue;
                            }
                            else
                            {
                                hxje = kfpje2;
                                kfpje = 0;
                                //continue;
                            }
                        }

                        var details = new LHFKOrderCheckedDetails();
                        details.KeyId = txtKeyId.Text;
                        details.FSaleKeyId = keyid;
                        details.FAmount = hxje;
                        details.FCompanyId = CurrentUser.AccountComId;
                        var detail =
                            FkOrderCheckedDetailsService.Where(p => p.KeyId == txtKeyId.Text && p.FSaleKeyId == keyid && p.FCompanyId == CurrentUser.AccountComId)
                                .FirstOrDefault();

                        if (detail != null)
                        {
                            FkOrderCheckedDetailsService.SaveChanges();
                        }
                        else
                        {
                            FkOrderCheckedDetailsService.Add(details);
                        }
                    }

                }


                //-----------------------------------------------------
                //实际核销款
                order.FAmount = checkedAmt;

                FkOrderCheckedService.SaveChanges();

                //-----------------------------------------------------
                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "HF", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "18");
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
                        FMemo = String.Format("单据号{0},{1}新增核销收款单据。", newKeyId, CurrentUser.AccountName)
                    };
                    GasHelper.AddBillStatus(billStatus);
                }

                return true;
            }

            return false;

        }

        /// <summary>
        ///     获取选中的发货单号集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetSalesIds()
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
        ///     获取选中的付款单号集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetSkIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var selectIds = new string[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Grid1.DataKeys[selections[i]][1].ToString();
            }
            return selectIds;
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {

            ViewState["_AppendToEnd"] = true;

            txtCreateBy.Text = CurrentUser.AccountName;

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinUnitAll.aspx");//WinSupplier

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
                    txtKeyId.Text = SequenceService.CreateSequence("TM",CurrentUser.AccountComId);
                    Region1.Title = "添加核销付款单";
                    var temp = new LHFKOrderChecked
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FAmount = 0,

                        FCate = "供应商",

                        FCompanyId = CurrentUser.AccountComId
                    };

                    //临时写入单据
                    FkOrderCheckedService.Add(temp);


                    //    var summary = new JObject
                    //{
                    //    {"FCode", "合计"},
                    //    {"FAmt", 0},
                    //};

                    //    Grid1.SummaryData = summary;

                    break;
                case WebAction.Edit:
                    Region1.Title = "编辑核销付款单";
                    txtKeyId.Text = KeyId;
                    if (FkOrderChecked != null)
                    {
                        txtFCode.Text = FkOrderChecked.FCode;
                        tbxFCustomer.Text = FkOrderChecked.FName;
                        txtFMemo.Text = FkOrderChecked.FMemo;

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