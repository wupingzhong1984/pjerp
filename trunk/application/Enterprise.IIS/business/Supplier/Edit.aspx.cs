using System;
using System.Collections.Generic;
using Enterprise.Data;
using Enterprise.Framework.Extension;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;


namespace Enterprise.IIS.business.Supplier
{
    public partial class Edit : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private SupplierService _supplierService;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected SupplierService SupplierService
        {
            get { return _supplierService ?? (_supplierService = new SupplierService()); }
            set { _supplierService = value; }
        }

        /// <summary>
        ///     客户档案
        /// </summary>
        private LHSupplier _supplier;

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
        protected LHSupplier Supplier
        {
            get { return _supplier ?? (_supplier = SupplierService.FirstOrDefault(p => p.FCode == FCode&&p.FCompanyId==CurrentUser.AccountComId)); }
            set { _supplier = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string FCode
        {
            get { return Request["FCode"]; }
        }

        /// <summary>
        ///     FSubCateId
        /// </summary>
        protected string FSubCateId
        {
            get { return Request["FSubCateId"]; }
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
            if (Supplier != null)
            {
               // Supplier.FCode = txtFCode.Text.Trim();
                Supplier.FName = txtFName.Text.Trim();
                Supplier.FAddress = txtFAddress.Text.Trim();
                Supplier.FLinkman = txtFLinkman.Text.Trim();
                Supplier.FPhome = txtFPhome.Text.Trim();
                Supplier.FMoile = txtFMoile.Text.Trim();
                Supplier.FMemo = txtFMemo.Text.Trim();
                Supplier.FSpell = ChineseSpell.MakeSpellCode(txtFName.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper();
                Supplier.FSalesman = ddlFSalesman.SelectedValue;


                return SupplierService.SaveChanges() >= 0;
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
                var member = new LHSupplier
                {
                    FCode = txtFCode.Text.Trim(),
                    FName = txtFName.Text.Trim(),
                    FAddress = txtFAddress.Text.Trim(),
                    FLinkman = txtFLinkman.Text.Trim(),
                    FPhome = txtFPhome.Text.Trim(),
                    FMoile = txtFMoile.Text.Trim(),
                    FMemo = txtFMemo.Text.Trim(),
                    FSpell = ChineseSpell.MakeSpellCode(txtFName.Text.Trim(), "",
                        SpellOptions.FirstLetterOnly).ToUpper(),
                    FFlag = 1,
                    FCompanyId = CurrentUser.AccountComId,
                    FCateId = "2078",
                    FIsAllot = 0,
                    FSubCateId = FSubCateId,
                    FSalesman = ddlFSalesman.SelectedValue,
                    FFreight = 0,
                    FCredit = 0,
                    
                };

                return SupplierService.Add(member);
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
            GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
           
            switch (Actions)
            {
                case WebAction.Add:
                    var parms = new Dictionary<string, object>();
                    parms.Clear();

                    parms.Add("@companyid",CurrentUser.AccountComId);
                    parms.Add("@type", "Supper");//配件                    
                    var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                    txtFCode.Text = list.Rows[0][0].ToString();
                    break;
                case WebAction.Edit:

                    txtFCode.Readonly = true;

                    if (Supplier != null)
                    {
                        txtFCode.Text = Supplier.FCode;
                        txtFName.Text = Supplier.FName;
                        txtFAddress.Text = Supplier.FAddress;
                        txtFLinkman.Text = Supplier.FLinkman ;
                        txtFPhome.Text = Supplier.FPhome;
                        txtFMoile.Text = Supplier.FMoile;
                        txtFMemo.Text = Supplier.FMemo;
                        //ddlFIsPrint.SelectedValue = Supplier.FIsPrint.ToString();
                        //txtFTipsDay.Text = Supplier.FTipsDay.ToString();
                        //txtFFreight.Text = Supplier.FFreight.ToString();
                        //txtFCredit.Text = Supplier.FCredit.ToString();
                        Supplier.FFlag = 1;
                        ddlFSalesman.SelectedValue = Supplier.FSalesman;//业务员
                        //ddlDistric.SelectedValue = Supplier.FDistric;//区域
                        //ddlFPaymentMethod.SelectedValue = Supplier.FPaymentMethod;
                        //CurrentUser.AccountComId = Supplier.FCompanyId;
                        //Supplier.FDate = txtFDate.SelectedDate;
                    }
                    break;
            }
        }

        #endregion

    }
}