using Enterprise.Data;
using Enterprise.Framework.Extension;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using System;
using System.Linq;

namespace Enterprise.IIS.business.Device
{
    public partial class Edit : PageBase
    {
        /// <summary>
        ///     信息
        /// </summary>
        private LHDevice _device;

        /// <summary>
        ///     数据服务
        /// </summary>
        private DeviceService _deviceService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected DeviceService DeviceService
        {
            get { return _deviceService ?? (_deviceService = new DeviceService()); }
            set { _deviceService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ProjectItemsService _projectItemsService;
        /// <summary>
        ///     数据服务
        /// </summary>
        protected ProjectItemsService ProjectItemsService
        {
            get { return _projectItemsService ?? (_projectItemsService = new ProjectItemsService()); }
            set { _projectItemsService = value; }
        }

        /// <summary>
        ///     商品代码
        /// </summary>
        protected string FCode
        {
            get { return Request["FCode"]; }
        }

        /// <summary>
        ///    产品子类编码
        /// </summary>
        protected string FSubCateId
        {
            get { return Request["FSubCateId"]; }
        }

        /// <summary>
        ///     瓶气
        /// </summary>
        protected string KeyId
        {
            get { return "1015"; }
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

        /// <summary>
        ///     分类
        /// </summary>
        protected LHProjectItems ProjectItem
        {
            get
            {
                return ProjectItemsService.FirstOrDefault(p => p.FId == FSubCateId);
            }
        }

        /// <summary>
        ///    产品档案    
        /// </summary>
        protected LHDevice ProductItem
        {
            get
            {
                return _device ?? (_device = DeviceService.FirstOrDefault(p => p.FCode == FCode));
            }
            set { _device = value; }
        }
        private CustomerService _customerService;
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
        }

        #region Protected Method

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
        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                LHCustomer custmoer = CustomerService.Where(p => p.FName == tbxFCustomer.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFCCode.Text = custmoer.FCode;
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
            if (ProductItem != null)
            {
                ProductItem.FName = txtFName.Text.Trim();

                //助记码
                ProductItem.FSpell = ChineseSpell.MakeSpellCode(txtFName.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper();

                ProductItem.FSpec = txtFSpec.Text.Trim();
                ProductItem.FUnit = ddlUnit.SelectedValue.Trim();
                ProductItem.FQty = Convert.ToDecimal(txtFQty.Text.Trim());
                ProductItem.FMemo = txtFMemo.Text.Trim();//备注
                ProductItem.FCompanyId = CurrentUser.AccountComId;
                ProductItem.FFlag = 0;

                ProductItem.FInstallDate = txtFInstallDate.SelectedDate;
                ProjectItem.FFlag = 1;
                ProductItem.FParms = txtFParms.Text;
                ProductItem.FPurchaseDate = txtFPurchaseDate.SelectedDate;
                ProductItem.FStatus = Convert.ToInt32(ddlStatus.SelectedValue);
                ProductItem.FUser = txtFUser.Text;



                //产品分类
                ProductItem.FCateId = ProjectItem.FSParent;
                ProductItem.FSubCateId = ProjectItem.FId;

                return DeviceService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            LHDevice its = new LHDevice
            {
                FCode = txtFCode.Text.Trim(),
                FName = txtFName.Text.Trim(),
                FSpec = txtFSpec.Text.Trim(),
                FSpell = ChineseSpell.MakeSpellCode(txtFName.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper(),
                FFlag = 1,

                //分类
                FCateId = ProjectItem.FSParent,
                FSubCateId = ProjectItem.FId,

                //FCompanyId = CurrentUser.AccountComId,
                //FUnit = ddlUnit.SelectedValue.Trim(),
                FQty = Convert.ToDecimal(txtFQty.Text.Trim()),
                FMemo = txtFMemo.Text.Trim(),//备注
                FCompanyId = CurrentUser.AccountComId,
                //FFlag = 0,

                FInstallDate = txtFInstallDate.SelectedDate,
                //ProjectItem.FFlag = 1,
                FParms = txtFParms.Text,
                FPurchaseDate = txtFPurchaseDate.SelectedDate,
                FStatus = Convert.ToInt32(ddlStatus.SelectedValue),
                FUser = txtFUser.Text,

                //单位
                FUnit = ddlUnit.SelectedValue,

                FInspectionCycle = txtFInspectionCycle.Text,
                FInstallByName= ddlFInstallByName.SelectedValue

                //FPurchasePrice = Convert.ToDecimal(txtFPurchasePrice.Text.Trim()),
                //FSalesPrice = Convert.ToDecimal(txtFSalesPrice.Text.Trim())
            };

            return DeviceService.Add(its);
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinCustomer.aspx");
            GasHelper.DropDownListDataBind(ddlUnit, "1001");
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            lblFName.Text = string.Format("{0}-{1}", ProjectItem.FId, ProjectItem.FName);
            switch (Actions)
            {
                case WebAction.Add:
                    break;
                case WebAction.Edit:

                    txtFCode.Enabled = false;
                    txtFCode.Readonly = true;

                    if (ProductItem != null)
                    {
                        txtFCode.Text = ProductItem.FCode;
                        txtFName.Text = ProductItem.FName;
                        txtFSpec.Text = ProductItem.FSpec;
                        ddlUnit.SelectedValue = ProductItem.FUnit;
                        txtFMemo.Text = ProductItem.FMemo;

                        txtFAddress.Text = ProductItem.FAddress;
                        txtFInspectionCycle.Text = ProductItem.FInspectionCycle;
                        txtFInstallDate.SelectedDate = ProductItem.FInstallDate;
                        txtFParms.Text = ProductItem.FParms;
                        txtFPurchaseDate.SelectedDate = ProductItem.FPurchaseDate;
                        txtFQty.Text = ProductItem.FQty.ToString();


                    }
                    break;
            }
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }
        #endregion
    }
}