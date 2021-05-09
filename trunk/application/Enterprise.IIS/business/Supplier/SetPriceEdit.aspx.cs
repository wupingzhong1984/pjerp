using System;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.Supplier
{
    public partial class SetPriceEdit : PageBase
    {
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
        private SupplierPriceService _supplierPriceService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SupplierPriceService SupplierPriceService
        {
            get { return _supplierPriceService ?? (_supplierPriceService = new SupplierPriceService()); }
            set { _supplierPriceService = value; }
        }

        /// <summary>
        ///     产品发货单价
        /// </summary>
        private LHSupplierPrice _supplierPrice;

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
        protected LHSupplierPrice SupplierPrice
        {
            get { return _supplierPrice ?? (_supplierPrice = SupplierPriceService.FirstOrDefault(p => p.FId == FId)); }
            set { _supplierPrice = value; }
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

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (SupplierPrice != null)
            {
                SupplierPrice.FPrice = Convert.ToDecimal(txtFPrice.Text.Trim());
                SupplierPrice.FItemCode = ddlItem.SelectedValue;
                return SupplierPriceService.SaveChanges() >= 0;
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
                var member = new LHSupplierPrice
                {
                    FCode = ddlCustomer.SelectedValue,
                    FItemCode = ddlItem.SelectedValue,
                    FFlag = 1,
                    FCompanyId = CurrentUser.AccountComId,
                    FCreateBy = CurrentUser.AccountName,
                    FPrice = Convert.ToDecimal(txtFPrice.Text.Trim())
                };

                return SupplierPriceService.Add(member);
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

            //供应商
            GasHelper.DropDownListSupplierDataBind(ddlCustomer);

            GasHelper.DropDownListItemDataBind(ddlItem);
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

                    if (SupplierPrice != null)
                    {
                        ddlCustomer.SelectedValue = SupplierPrice.FCode;
                        ddlItem.SelectedValue = SupplierPrice.FItemCode;

                        ddlCustomer.Readonly = true;

                        txtFPrice.Text = SupplierPrice.FPrice.ToString();
                    }
                    break;
            }
        }

        #endregion

    }
}