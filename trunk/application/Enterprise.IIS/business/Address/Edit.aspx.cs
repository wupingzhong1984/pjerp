using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Enterprise.Data;
using Enterprise.Framework.Extension;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.Address
{
    public partial class Edit : PageBase
    {
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
        private CustomerLinkService _customerLinkService;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected CustomerLinkService CustomerLinkService
        {
            get { return _customerLinkService ?? (_customerLinkService = new CustomerLinkService()); }
            set { _customerLinkService = value; }
        }
        /// <summary>
        ///     客户档案
        /// </summary>
        private LHCustomerLink _supplier;

        /// <summary>
        ///     区域
        /// </summary>
        private ProjectItemsService _projectItemsService;

        /// <summary>
        ///     区域
        /// </summary>
        protected ProjectItemsService ProjectItemsService
        {
            get { return _projectItemsService ?? (_projectItemsService = new ProjectItemsService()); }
            set { _projectItemsService = value; }
        }

        /// <summary>
        ///     职员档案
        /// </summary>
        protected LHCustomerLink Link
        {
            get { return _supplier ?? (_supplier = CustomerLinkService.FirstOrDefault(p => p.FId == FId && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _supplier = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected int FId
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

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (Link != null)
            {
                Link.FCode = txtFCode.Text.Trim();
                Link.FName = tbxFCustomer.Text.Trim();
                Link.FAddress = txtFAddress.Text.Trim();
                Link.FLinkman = txtFLinkman.Text.Trim();
                Link.FPhome = txtFPhome.Text.Trim();
                Link.FMoile = txtFMoile.Text.Trim();
                Link.FMemo = txtFMemo.Text.Trim();
                Link.FSpell = ChineseSpell.MakeSpellCode(tbxFCustomer.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper();
                Link.FCity = txtFCity.Text.Trim();
                Link.FZip = txtFZip.Text.Trim();

                return CustomerLinkService.SaveChanges() >= 0;
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
                var member = new LHCustomerLink
                {
                    FCode = txtFCode.Text.Trim(),
                    FName = tbxFCustomer.Text.Trim(),
                    FAddress = txtFAddress.Text.Trim(),
                    FLinkman = txtFLinkman.Text.Trim(),
                    FPhome = txtFPhome.Text.Trim(),
                    FMoile = txtFMoile.Text.Trim(),
                    FMemo = txtFMemo.Text.Trim(),
                    FSpell = ChineseSpell.MakeSpellCode(tbxFCustomer.Text.Trim(), "",
                        SpellOptions.FirstLetterOnly).ToUpper(),
                    FCompanyId = CurrentUser.AccountComId,
                    FCity = txtFCity.Text.Trim(),
                    FZip = txtFZip.Text.Trim(),
            };

                return CustomerLinkService.Add(member);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            //区域
            //GasHelper.DropDownListDataBind(ddlDistric, 1004);

            //是否需要开票
            //GasHelper.DropDownListDataBind(ddlFIsPrint, 1006);

            //结算方式
            //GasHelper.DropDownListDataBind(ddlFPaymentMethod, 1005);

            //业务员
            //GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinCustomer.aspx");
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

                    txtFCode.Readonly = true;

                    if (Link != null)
                    {
                        txtFCode.Text = Link.FCode;
                        tbxFCustomer.Text = Link.FName;
                        txtFAddress.Text = Link.FAddress;
                        txtFLinkman.Text = Link.FLinkman;
                        txtFPhome.Text = Link.FPhome;
                        txtFMoile.Text = Link.FMoile;
                        txtFMemo.Text = Link.FMemo;
                        txtFCity.Text = Link.FCity;
                        txtFZip.Text = Link.FZip;
                    }
                    break;
            }
        }

        #endregion

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                var custmoer = CustomerService.Where(p => p.FName == tbxFCustomer.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFCode.Text = custmoer.FCode;
                    txtFAddress.Text = custmoer.FAddress.Trim();
                    //txtFFreight.Text = custmoer.FFreight.ToString();
                    txtFLinkman.Text = custmoer.FLinkman;
                    //txtFPhone.Text = custmoer.FPhome;
                }
            }
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
          
        }
    }
}