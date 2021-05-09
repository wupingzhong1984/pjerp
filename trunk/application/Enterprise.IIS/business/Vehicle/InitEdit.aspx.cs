using Enterprise.Data;
using Enterprise.Framework.File;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Enterprise.IIS.business.Vehicle
{
    public partial class InitEdit : PageBase
    {
        private string keynum = "";

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
        private CustomerService _customerService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
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
        private PassCardService _passCardService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected PassCardService PassCardService
        {
            get { return _passCardService ?? (_passCardService = new PassCardService()); }
            set { _passCardService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockoutDispatchServices _stockOutDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockoutDispatchServices StockOutDetailsService
        {
            get
            {
                return _stockOutDetailsService ?? //
              (_stockOutDetailsService = new StockoutDispatchServices());
            }
            set { _stockOutDetailsService = value; }
        }

        /// <summary>
        ///     Log
        /// </summary>
        private StockOutDetailsLogService _stockOutDetailsLogService;
        /// <summary>
        ///     Log
        /// </summary>
        protected StockOutDetailsLogService StockOutDetailsLogService
        {
            get { return _stockOutDetailsLogService ?? (_stockOutDetailsLogService = new StockOutDetailsLogService()); }
            set { _stockOutDetailsLogService = value; }
        }

        /// <summary>
        ///     Log
        /// </summary>
        private StockOutLogService _stockOutLogService;
        /// <summary>
        ///     Log
        /// </summary>
        protected StockOutLogService StockOutLogService
        {
            get { return _stockOutLogService ?? (_stockOutLogService = new StockOutLogService()); }
            set { _stockOutLogService = value; }
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
        private LHStockOutDispatch _stockOut;

        /// <summary>
        ///     
        /// </summary>
        protected LHStockOutDispatch StockOut
        {
            get
            {
                return _stockOut ?? (_stockOut = StockoutDispatchService.FirstOrDefault(p => p.KeyId == KeyId //
              && p.FCompanyid == CurrentUser.AccountComId));
            }
            set { _stockOut = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private AttachmentService _attachmentService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected AttachmentService AttachmentService
        {
            get { return _attachmentService ?? (_attachmentService = new AttachmentService()); }
            set { _attachmentService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHAttachment _attachment;

        /// <summary>
        ///     
        /// </summary>
        protected LHAttachment Attachment
        {
            get
            {
                return _attachment ?? (_attachment = AttachmentService.FirstOrDefault(p => p.KeyId == KeyId //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _attachment = value; }
        }



        /// <summary>
        ///     数据服务
        /// </summary>
        private StockoutDispatchServices _stockoutDispatchService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected StockoutDispatchServices StockoutDispatchService
        {
            get { return _stockoutDispatchService ?? (_stockoutDispatchService = new StockoutDispatchServices()); }
            set { _stockoutDispatchService = value; }
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
                return (WebAction)int.Parse(s);
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
                if (GetRequestEventArgument().Contains("reloadGrid:"))
                {
                    //查找所选商品代码，查访产品集合
                    string keys = GetRequestEventArgument().Split(':')[1];

                    string[] values = keys.Split(',');

                    string codes = string.Empty;
                    for (int i = 0; i < values.Count(); i++)
                    {
                        string val = values[i];
                        LHStockOut outs = StockOutService.Where(t=>t.KeyId==val).FirstOrDefault();
                        if (outs!=null)
                        {
                            
                        //outs.FInitDispatch = txtKeyId.Text.Trim();
                        StockOutService.SaveChanges();
                        }

                        LHStockIn ins = StockInService.Where(t=>t.KeyId== val).FirstOrDefault();
                        if (ins!=null)
                        {
                            ins.FInitDispatch = txtKeyId.Text.Trim();
                            StockInService.SaveChanges();
                        }
                        
                    }

                   

                        //重新绑定值
                        BindDataGrid();
                    
                }
                #endregion

                #region 更新合计


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
            decimal sumFRecycleQty = 0.00M;
            decimal sumFBottleQty = 0.00M;

            foreach (JObject mergedRow in Grid1.GetMergedData())
            {
                JObject values = mergedRow.Value<JObject>("values");

                sumFQty += values.Value<decimal>("FQty");
                sumFAmount += values.Value<decimal>("FAmount");
                sumFRecycleQty += values.Value<decimal>("FRecycleQty");
                sumFBottleQty += values.Value<decimal>("FBottleQty");
            }

            JObject summary = new JObject
            {
                { "FQty", sumFQty },
                { "FAmount", sumFAmount },
                { "FRecycleQty", sumFRecycleQty },
                { "FBottleQty", sumFBottleQty }
            };

            Grid1.SummaryData = summary;
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
            LHStockOut stockOut = null;
            string errmsg = "提交失败！";
            try
            {
                if (stockOut == null || Actions == WebAction.Edit)
                {

                    using (TransactionScope trans = new TransactionScope())
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
                else
                {
                    errmsg = "已存在预调度单！";
                    isSucceed = false;
                }

            }
            catch (DbEntityValidationException ex)
            {
                isSucceed = false;
                errmsg = ex.Message + "--" + ex.StackTrace;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    //try
                    //{
                    //    tmsModel t = new tmsModel();
                    //    LHStockOut hStockOut = StockOutService.Where(p => p.KeyId == txtKeyId.Text).FirstOrDefault();
                    //    List<LHStockOut> lHs = new List<LHStockOut>
                    //    {
                    //        hStockOut
                    //    };
                    //    t.stockOutList = lHs;
                    //    t.stockOutDetailsList = StockOutDetailsService.Where(p => p.KeyId == txtKeyId.Text).ToList();
                    //    t.passCardList = new List<LHPassCard>();
                    //    if (!string.IsNullOrWhiteSpace(tbxFLogisticsNumber.Text))
                    //    {

                    //        IQueryable<LHPassCard> lHPassCards = PassCardService.Where(p => p.KeyId == tbxFLogisticsNumber.Text);
                    //        if (lHPassCards != null && lHPassCards.Count() > 0)
                    //        {
                    //            t.passCardList = lHPassCards.ToList();
                    //            t.passCardDetailsList = new List<LHPassCardDetails>();
                    //            IQueryable<LHPassCardDetails> lHPassCardDetails = new PassCardDetailsService().Where(p => p.KeyId == tbxFLogisticsNumber.Text);
                    //            if (lHPassCardDetails != null && lHPassCardDetails.Count() > 0)
                    //            {
                    //                t.passCardDetailsList = lHPassCardDetails.ToList();
                    //            }
                    //        }
                    //    }


                    //    t.stockInList = new StockInService().Where(p => p.FLogisticsNumber == txtKeyId.Text).ToList();

                    //    t.stockInDetailsList = new List<LHStockInDetails>();
                    //    foreach (LHStockIn item in t.stockInList)
                    //    {
                    //        List<LHStockInDetails> list = new StockInDetailsService().Where(p => p.KeyId == item.KeyId).ToList();
                    //        foreach (LHStockInDetails inDetails in list)
                    //        {
                    //            t.stockInDetailsList.Add(inDetails);
                    //        }
                    //    }

                    //    new HttpRequest().httpRequest(t, "open/dbo/dboData");
                    //}
                    //catch (Exception)
                    //{

                    //}


                    PageContext.RegisterStartupScript("closeActiveTab();");
                }
                else
                {
                    Alert.Show(errmsg, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        ///     调度生成发货单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPassCardToSales_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {

                Dictionary<string, object> pamrs = new Dictionary<string, object>();
                pamrs.Clear();


                BindDataGrid();

                isSucceed = true;
            }
            catch (Exception)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    Alert.Show("提交成功", MessageBoxIcon.Information);
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
        protected void filePhoto_FileSelected(object sender, EventArgs e)
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
        ///     新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                PageContext.RegisterStartupScript(
                    Window1.GetShowReference(string.Format("../../Common/WinProductPrice.aspx?FCode={0}",
                       ""), "客户合同价"));



            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddProduct_OnClick(object sender, EventArgs e)
        {
            try
            {
                PageContext.RegisterStartupScript(
                   Window1.GetShowReference("../../Common/WinSalesTask.aspx?FType=1", "发货单"));
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     Copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                Dictionary<string, object> pamrs = new Dictionary<string, object>();
                pamrs.Clear();


                BindDataGrid();

                isSucceed = true;
            }
            catch (Exception)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    Alert.Show("提交成功", MessageBoxIcon.Information);
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
            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@companyid", CurrentUser.AccountComId);
            parms.Add("@DispatchId", txtKeyId.Text.Trim());

            DataTable tb = SqlService.ExecuteProcedureCommand("proc_StockoutDispatch", parms).Tables[0];
            //绑定数据源
           Grid1.DataSource = tb;
            Grid1.DataBind();

            DataTable table = tb;

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFQty = 0.00M;

                sumFQty = Convert.ToDecimal(table.Compute("sum(FQty)", "true"));

                JObject summary = new JObject
                {
                    {"Keyid", "合计"},
                    {"FDate", ""},
                    {"FName", ""},
                    {"pname", ""},
                    {"FQty", sumFQty},
                };

                Grid1.SummaryData = summary;
            }
            else
            {

                JObject summary = new JObject
                {
                    {"Keyid", "合计"},
                    {"FDate", ""},
                    {"FName", ""},
                    {"pname", ""},
                    {"FQty", 0},
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
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;

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
                string datakey = Grid1.DataKeys[e.RowIndex][0].ToString();

               LHStockOut lk= StockOutService.Where(t => t.KeyId == datakey).FirstOrDefault();
               lk.FInitDispatch = "";
               StockOutService.SaveChanges();
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

        protected void txtFAddress_OnTriggerClick(object sender, EventArgs e)
        {
        }

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
        }

        protected void tbxFLogisticsNumber_OnTextChanged(object sender, EventArgs e)
        {
            
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


                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            LHStockOutDispatch stock = StockoutDispatchService.Where(p => p.KeyId == txtKeyId.Text.Trim() && p.FCompanyid == CurrentUser.AccountComId).FirstOrDefault();
            stock.FDate = txtFDate.SelectedDate;
            stock.FMemo = txtFMemo.Text.Trim();
            stock.DeliveryMethod = ddlDeliveryMethod.SelectedValue;
            stock.FDistributionPoint = ddlFDistributionPoint.SelectedValue;
            stock.FFlag = 1;
            try
            {
                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "XPC", CurrentUser.AccountComId);
                    keynum = newKeyId;
                    Dictionary<string, object> orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "100");
                    orderParms.Add("@companyId", CurrentUser.AccountComId);

                    SqlService.ExecuteProcedureCommand("proc_num", orderParms);
                    txtKeyId.Text = newKeyId;
                    stock.KeyId = newKeyId;
                    //新增日志
                    LHBillStatus billStatus = new LHBillStatus
                    {
                        KeyId = newKeyId,
                        FCompanyId = CurrentUser.AccountComId,
                        FActionName = "新增",
                        FDate = DateTime.Now,
                        FDeptId = CurrentUser.AccountOrgId,
                        FOperator = CurrentUser.AccountName,
                        FMemo = string.Format("单据号{0},{1}新增预调度单据。", newKeyId, CurrentUser.AccountName)
                    };
                }
                StockoutDispatchService.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            ViewState["_AppendToEnd"] = true;

            txtCreateBy.Text = CurrentUser.AccountName;


            //tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
            //        + Window2.GetShowReference("../../Common/WinCustomer.aspx");

            //txtFAddress.OnClientTriggerClick = Window3.GetSaveStateReference(txtFAddress.ClientID)
            //        + Window3.GetShowReference(string.Format(@"../../Common/WinCustomerLink.aspx"));

            //tbxFBottle.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFBottle.ClientID, hfdSpec.ClientID)
            //        + Window1.GetShowReference("../../Common/WinBottleToGas.aspx");

            //tbxFLogisticsNumber.OnClientTriggerClick = Window4.GetSaveStateReference(tbxFLogisticsNumber.ClientID)
            //        + Window4.GetShowReference("../../Common/WinTask.aspx?FType=1");

            //GasHelper.DropDownListDriverDataBind(ddlFDriver);

            //GasHelper.DropDownListGodownKeeperDataBind(ddlFShipper);

            //GasHelper.DropDownListGodownKeeperDataBind(ddlFReceiver);

            //GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);

            //GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

            ////GasHelper.DropDownListAreasDataBind(ddlFArea);

            //GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

            GasHelper.DropDownListDeliveryMethodDataBind(ddlDeliveryMethod);

            //GasHelper.DropDownListBankSubjectDataBind(ddlSubject);

            //GasHelper.DropDownListDataBindSaleType(ddlFT6SaleType);

            //GasHelper.DropDownListDataBindReceiveSendType(ddlT6ReceiveSendType);


            //GasHelper.DropDownListDataBindCurrencyType(ddlFT6Currency);

            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区

            //ddlFT6SaleType.SelectedValue = "01";//普通销售

            //ddlT6ReceiveSendType.SelectedValue = "0202";//销售出库


            //删除选中单元格的客户端脚本
            string deleteScript = DeleteScript();

            //新增
            //var defaultObj = new JObject
            //{
            //    {"FItemCode", ""},
            //    {"FItemName", ""},
            //    {"FSpec", ""},
            //    {"FUnit", ""},
            //    {"FPrice", ""},
            //    {"FQty", "0"},
            //    {"FAmount", "0"},
            //    {"FBottleQty", "0"},
            //    {"FBottleOweQty", "0"},
            //    {"FCateName", ""},
            //    {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
            //        deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            //};

            //// 在第一行新增一条数据
            //btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            txtFDate.SelectedDate = DateTime.Now;

            //txtFFreight.Text = "0.00";

           // ddlFShipper.SelectedValue = CurrentUser.AccountName;
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
                    Region3.Title = "添加预调度单";

                    LHStockOutDispatch temp = new LHStockOutDispatch
                    {
                        KeyId = txtKeyId.Text,

                       FCompanyid=CurrentUser.AccountComId,

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FFlag=0,

                        FState=0
                       
                    };

                    //临时写入单据
                    StockoutDispatchService.Add(temp);

                    //合计
                    JObject summary = new JObject
                    {
                        {"FItemCode", "合计"},
                        {"FQty", 0},
                        {"FAmount", 0}
                    };

                    Grid1.SummaryData = summary;

                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region3.Title = "编辑预调度单";
                    keynum = KeyId;
                    if (StockOut != null)
                    {
                        WebControlHandler.BindObjectToControls(StockOut, SimpleForm1);
                        txtFDate.SelectedDate = StockOut.FDate;
                       
                        //ddlFArea.SelectedValue = StockOut.FArea;
                        ddlDeliveryMethod.SelectedValue = StockOut.DeliveryMethod;

                        txtCreateBy.Text = StockOut.CreateBy;

                        txtFMemo.Text = StockOut.FMemo;

                        ddlFDistributionPoint.SelectedValue = StockOut.FDistributionPoint;

                        

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
            return Confirm.GetShowReference("删除选中行？", string.Empty, MessageBoxIcon.Question, Grid1.GetDeleteSelectedRowsReference(), string.Empty);
        }

        #endregion

    }
}