using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.File;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
namespace Enterprise.IIS.business.Liquid
{
    public partial class LiquidPlanPurchaseEdit : PageBase
    {
        #region  Service

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
        private LiquidPlanTaskService _liquidPlanTaskService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LiquidPlanTaskService LiquidPlanTaskService
        {
            get { return _liquidPlanTaskService ?? (_liquidPlanTaskService = new LiquidPlanTaskService()); }
            set { _liquidPlanTaskService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LiquidPlanTaskDetailsService _liquidPlanTaskDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LiquidPlanTaskDetailsService LiquidPlanTaskDetailsService
        {
            get
            {
                return _liquidPlanTaskDetailsService ?? //
                    (_liquidPlanTaskDetailsService = new LiquidPlanTaskDetailsService());
            }
            set { _liquidPlanTaskDetailsService = value; }
        }

        /// <summary>
        ///     客户档案
        /// </summary>
        private LHCustomer _customer;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected LHCustomer Customer
        {
            get
            {
                return _customer ?? (_customer = CustomerService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim()//
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _customer = value; }
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
        private LHLiquidPlanTask _liquidPlanTask;

        /// <summary>
        ///     
        /// </summary>
        protected LHLiquidPlanTask LiquidPlanTask
        {
            get
            {
                return _liquidPlanTask ?? (_liquidPlanTask = LiquidPlanTaskService.FirstOrDefault(p => p.KeyId == KeyId //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _liquidPlanTask = value; }
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
                //using (var trans = new TransactionScope())
                //{
                switch (Actions)
                {
                    case WebAction.Add:
                        isSucceed = SubmintAdd();
                        break;

                    case WebAction.Edit:
                        isSucceed = SubmintEdit();
                        break;
                }

                //    trans.Complete();
                //}
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
        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            if (filePhoto.HasFile)
            {
                var fileSuffix = filePhoto.ShortFileName.Substring(filePhoto.ShortFileName.LastIndexOf('.'));

                var sequence = SequenceService.CreateSequence("LH", CurrentUser.AccountComId);

                var fileName = sequence + fileSuffix;

                var uploadpath = Config.GetUploadpath();

                var tempPath = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

                if (!DirFile.XFileExists(Server.MapPath(tempPath)))
                {
                    DirFile.XCreateDir(Server.MapPath(tempPath));
                }

                filePhoto.SaveAs(Server.MapPath(tempPath + fileName));

                hfdImage.Text = tempPath + fileName;

                // 清空文件上传组件
                filePhoto.Reset();
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
                var custmoer = CustomerService.Where(p => p.FName == tbxFCustomer.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
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
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (LiquidPlanTask != null)
            {
                LiquidPlanTask.FCode = txtFCode.Text;
                LiquidPlanTask.FName = tbxFCustomer.Text;
                //------------------------------------------------------
                LiquidPlanTask.FAddress = txtFAddress.Text.Trim();

                LiquidPlanTask.FDate = txtFDate.SelectedDate;

                LiquidPlanTask.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                LiquidPlanTask.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                LiquidPlanTask.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                LiquidPlanTask.FLinkman = txtFLinkman.Text.Trim();
                LiquidPlanTask.FMemo = txtFMemo.Text.Trim();
                LiquidPlanTask.FPhone = txtFPhone.Text.Trim();

                LiquidPlanTask.FSalesman = ddlFSalesman.SelectedValue;
                LiquidPlanTask.FArea = ddlFArea.SelectedValue;
                LiquidPlanTask.FReconciliation = txtFReconciliation.Text;
                LiquidPlanTask.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                LiquidPlanTask.FLogisticsNumber = txtFLogisticsNumber.Text;
                LiquidPlanTask.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                LiquidPlanTask.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
                LiquidPlanTask.FQty = Convert.ToDecimal(txtFQty.Text);
                LiquidPlanTask.FPrice = Convert.ToDecimal(txtFPrice.Text);
                LiquidPlanTask.FAmount = LiquidPlanTask.FPrice * LiquidPlanTask.FQty;

                LiquidPlanTask.FItemCode = tbxFItemName.SelectedValue;
                LiquidPlanTask.FItemName = tbxFItemName.SelectedText;

                LiquidPlanTaskService.SaveChanges();

                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var stock = LiquidPlanTaskService.Where(p => p.KeyId == txtKeyId.Text.Trim() //
                && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (stock != null)
            {

                stock.FCode = txtFCode.Text;
                stock.FName = tbxFCustomer.Text;
                //--------------------------------------------------

                stock.FAddress = txtFAddress.Text.Trim();
                stock.FFlag = 1;
                stock.FDeleteFlag = 0;
                stock.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                stock.FLinkman = txtFLinkman.Text.Trim();
                stock.FMemo = txtFMemo.Text.Trim();
                stock.FPhone = txtFPhone.Text.Trim();

                stock.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                stock.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                stock.FDate = txtFDate.SelectedDate;
                stock.FSalesman = ddlFSalesman.SelectedValue;
                stock.FArea = ddlFArea.SelectedValue;
                stock.FReconciliation = txtFReconciliation.Text;
                stock.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                stock.FLogisticsNumber = txtFLogisticsNumber.Text;

                stock.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                stock.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);

                stock.FQty = Convert.ToDecimal(txtFQty.Text);
                stock.FPrice = Convert.ToDecimal(txtFPrice.Text);
                stock.FAmount = stock.FPrice * stock.FQty;
                stock.FItemCode = tbxFItemName.SelectedValue;
                stock.FItemName = tbxFItemName.SelectedText;

                LiquidPlanTaskService.SaveChanges();

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "CC", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "29");
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
                        FMemo = String.Format("单据号{0},{1}新增发货单据。", newKeyId, CurrentUser.AccountName)
                    };

                    GasHelper.AddBillStatus(billStatus);
                }


                if (!string.IsNullOrEmpty(hfdImage.Text))
                {
                    var attachment = new LHAttachment();
                    attachment.FCompanyId = CurrentUser.AccountComId;
                    attachment.FPath = hfdImage.Text;
                    attachment.KeyId = txtKeyId.Text;

                    AttachmentService.Add(attachment);
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
            txtCreateBy.Text = CurrentUser.AccountName;

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinSupplier.aspx");

            txtFAddress.OnClientTriggerClick = Window3.GetSaveStateReference(txtFAddress.ClientID)
                    + Window3.GetShowReference(string.Format(@"../../Common/WinCustomerLink.aspx"));

            GasHelper.DropDownListDriverDataBind(ddlFDriver);

            GasHelper.DropDownListShipperDataBind(ddlFShipper);

            GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);

            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

            GasHelper.DropDownListAreasDataBind(ddlFArea);

            GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

            GasHelper.DropDownListDeliveryMethodDataBind(ddlDeliveryMethod);

            GasHelper.DropDownListLiquidDataBind(tbxFItemName);

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
                    Region3.Title = "添加液体采购订单";

                    var temp = new LHLiquidPlanTask
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //发货单
                        FType = Convert.ToInt32(GasEnumBill.LiquidPurchasePlan),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                        FMergeFlag = 0
                    };

                    //临时写入单据
                    LiquidPlanTaskService.Add(temp);

                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region3.Title = "编辑液体采购订单";

                    if (LiquidPlanTask != null)
                    {
                        WebControlHandler.BindObjectToControls(LiquidPlanTask, SimpleForm1);
                        txtFDate.SelectedDate = LiquidPlanTask.FDate;
                        tbxFCustomer.Text = LiquidPlanTask.FName;

                        if (!string.IsNullOrEmpty(LiquidPlanTask.FDriver))
                            ddlFDriver.SelectedValueArray = LiquidPlanTask.FDriver.Split(',');
                        //if (!string.IsNullOrEmpty(StockOut.FShipper))
                        //    ddlFShipper.SelectedValueArray = StockOut.FShipper.Split(',');
                        if (!string.IsNullOrEmpty(LiquidPlanTask.FSupercargo))
                            ddlFSupercargo.SelectedValueArray = LiquidPlanTask.FSupercargo.Split(',');
                        if (!string.IsNullOrEmpty(LiquidPlanTask.FSalesman))
                            ddlFSalesman.SelectedValueArray = LiquidPlanTask.FSalesman.Split(',');

                        ddlFVehicleNum.SelectedValue = LiquidPlanTask.FVehicleNum;
                        ddlFArea.SelectedValue = LiquidPlanTask.FArea;
                        ddlDeliveryMethod.SelectedValue = LiquidPlanTask.FDeliveryMethod;

                        if (Attachment != null)
                        {
                            lblfile.Text = string.Format(@"<a href='../../{0}' target='_blank'>附件</a>",//
                                Attachment.FPath.Replace("~/", ""));
                        }
                        else
                        {
                            lblfile.Text = string.Empty;
                        }
                    }
                    break;
            }
        }

        #endregion

    }
}