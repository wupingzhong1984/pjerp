using System;
using System.Collections.Generic;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.Customer
{
    public partial class SetPriceEdit : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerPriceService _customerPriceService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected CustomerPriceService CustomerPriceService
        {
            get { return _customerPriceService ?? (_customerPriceService = new CustomerPriceService()); }
            set { _customerPriceService = value; }
        }


        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerPriceLogService _customerPriceLogService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected CustomerPriceLogService CustomerPriceLogService
        {
            get { return _customerPriceLogService ?? (_customerPriceLogService = new CustomerPriceLogService()); }
            set { _customerPriceLogService = value; }
        }

        /// <summary>
        ///     产品发货单价
        /// </summary>
        private LHCustomerPrice _customerPrice;

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
        protected LHCustomerPrice CustomerPrice
        {
            get { return _customerPrice ?? (_customerPrice = CustomerPriceService.FirstOrDefault(p => p.FId == FId)); }
            set { _customerPrice = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected Int32 FId
        {
            get { return Convert.ToInt32(Request["FId"]); }
        }

        /// <summary>
        ///     当前画面操作项
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction)int.Parse(s);
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
                switch (Actions)
                {
                    case WebAction.Add:
                        isSucceed = SubmintAdd();
                        break;
                    case WebAction.Edit:
                        isSucceed = SubmintEdit();
                        break;
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
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }
        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (CustomerPrice != null)
            {

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@FUpdateBy", CurrentUser.AccountName);
                parms.Add("@FId", CustomerPrice.FId);

                SqlService.ExecuteProcedureCommand("proc_LHCustomerPriceLog", parms);

                CustomerPrice.FPrice = Convert.ToDecimal(txtFPrice.Text.Trim());
                CustomerPrice.FMemo = txtFMemo.Text;


                CustomerPrice.FItemCode = txtFItemCode.Text;

                CustomerPrice.FBeginDate = dateBegin.SelectedDate;
                //CustomerPrice.FEndDate = dateEnd.SelectedDate;

                CustomerPrice.FUpdateBy = CurrentUser.AccountName;
                CustomerPrice.FUpdateDate = DateTime.Now;

                CustomerPriceService.SaveChanges();

                var priceLog = new LHCustomerPrice_Log();

                priceLog.FCode = CustomerPrice.FCode;
                priceLog.FCompanyId = CustomerPrice.FCompanyId;
                priceLog.FDate = DateTime.Now;
                priceLog.FFlag = "变更后";
                priceLog.FItemCode = CustomerPrice.FItemCode;
                priceLog.FPrice = CustomerPrice.FPrice;
                priceLog.FUpdateBy = CurrentUser.AccountName;
                priceLog.FUpdateDate = DateTime.Now;

                return CustomerPriceLogService.Add(priceLog);

            }

            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {

            try
            {
                var member = new LHCustomerPrice
                {
                    FCode = txtFCode.Text,
                    FItemCode = txtFItemCode.Text,
                    FCompanyId = CurrentUser.AccountComId,
                    FFlag = 1,
                    FMemo = txtFMemo.Text,
                    FPrice = Convert.ToDecimal(txtFPrice.Text.Trim()),

                    FBeginDate = dateBegin.SelectedDate,

                    FCreateBy = CurrentUser.AccountName,
                    FCreateDate = DateTime.Now


                };

                return CustomerPriceService.Add(member);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            Window2.Hidden = true;

            var customer = CustomerService.FirstOrDefault(p => p.FName == tbxFCustomer.Text.Trim() //
                && p.FCompanyId == CurrentUser.AccountComId);

            if (customer != null)
                txtFCode.Text = customer.FCode;

        }

        protected void tbxFName_OnTextChanged(object sender, EventArgs e)
        {
            Window2.Hidden = true;

            //var item = ItemsService.FirstOrDefault(p => p.FName == tbxFName.Text.Trim() //
            //   && p.FCompanyId == CurrentUser.AccountComId);//

            //if (item != null)
            //    txtFItemCode.Text = item.FCode;
        }


        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinCustomer.aspx", "客户档案");

            tbxFName.OnClientTriggerClick = Window2.GetSaveStateReference(txtFItemCode.ClientID, tbxFName.ClientID)
                    + Window2.GetShowReference("../../Common/WinProducReference.aspx", "产品档案");
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {


            switch (Actions)
            {
                case WebAction.Add:
                    break;
                case WebAction.Edit:

                    if (CustomerPrice != null)
                    {
                        txtFCode.Text = CustomerPrice.FCode;

                        tbxFCustomer.Text = CustomerService.FirstOrDefault(p => p.FCode == CustomerPrice.FCode //
                            && p.FCompanyId == CurrentUser.AccountComId).FName;

                        txtFItemCode.Text = CustomerPrice.FItemCode;

                        tbxFName.Text = ItemsService.FirstOrDefault(p => p.FCode == CustomerPrice.FItemCode //
                            && p.FCompanyId == CurrentUser.AccountComId).FName;


                        txtFMemo.Text = CustomerPrice.FMemo;
                        txtFPrice.Text = CustomerPrice.FPrice.ToString();

                        dateBegin.SelectedDate = CustomerPrice.FBeginDate;
                    }
                    break;
            }
        }

        #endregion

    }
}