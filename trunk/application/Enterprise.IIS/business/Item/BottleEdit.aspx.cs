using System;
using System.Collections.Generic;
using Enterprise.IIS.Common;
using Enterprise.Service.Base.ERP;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Framework.Extension;
using FineUI;


namespace Enterprise.IIS.business.Item
{
    public partial class BottleEdit : PageBase
    {
        /// <summary>
        ///     产品信息
        /// </summary>
        private LHItems _product;

        /// <summary>
        ///     数据服务
        /// </summary>
        private ItemsService _itemsService;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        protected ItemsService ItemsService
        {
            get { return _itemsService ?? (_itemsService = new ItemsService()); }
            set { _itemsService = value; }
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
            get { return "2000"; }
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
        protected LHItems ProductItem
        {
            get
            {
                return _product ?? (_product = ItemsService.FirstOrDefault(p => p.FCode == FCode
                    && p.FCompanyId==CurrentUser.AccountComId));
            }
            set { _product = value; }
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

                ProductItem.FGroupNum = txtFGroupNum.Text.Trim();
                ProductItem.FSpec = txtFSpec.Text.Trim();
                ProductItem.FUnit = ddlUnit.SelectedValue.Trim();
                ProductItem.FPurchasePrice = Convert.ToDecimal(txtFPurchasePrice.Text.Trim());
                ProductItem.FSalesPrice = Convert.ToDecimal(txtFSalesPrice.Text.Trim());
                ProductItem.FMemo = txtFMemo.Text.Trim();//备注
               // ProductItem.FCompanyId = CurrentUser.AccountComId;
                ProductItem.FGroupNum = txtFGroupNum.Text.Trim();
                ProductItem.FPieceWork1 = Convert.ToDecimal(txtFPieceWork1.Text.Trim());
                ProductItem.FPieceWork2 = Convert.ToDecimal(txtFPieceWork2.Text.Trim());
                ProductItem.FPieceWork3 = Convert.ToDecimal(txtFPieceWork3.Text.Trim());
                ProductItem.FPieceWork4 = Convert.ToDecimal(txtFPieceWork4.Text.Trim());
                ProductItem.FPieceWork5 = Convert.ToDecimal(txtFPieceWork5.Text.Trim());
                ProductItem.FNetWeight = Convert.ToDecimal(txtFNetWeight.Text.Trim());//重量
                ProductItem.FRack = txtFRack.Text;


                return ItemsService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var its = new LHItems
            {
                FCode = txtFCode.Text.Trim(),
                FName = txtFName.Text.Trim(),
                FSpec = txtFSpec.Text.Trim(),
                FSpell = ChineseSpell.MakeSpellCode(txtFName.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper(),
                FFlag = 1,

                //分类
                FCateId = "2001",
                FSubCateId = ProjectItem.FId,

                FCompanyId = CurrentUser.AccountComId,
                FGroupNum = txtFGroupNum.Text,
                FMemo = txtFMemo.Text.Trim(),
                FRack = txtFRack.Text.Trim(),

                //单位
                FUnit = ddlUnit.SelectedValue,
                FPurchasePrice = Convert.ToDecimal(txtFPurchasePrice.Text.Trim()),
                FSalesPrice = Convert.ToDecimal(txtFSalesPrice.Text.Trim()),
                FPieceWork1 = Convert.ToDecimal(txtFPieceWork1.Text.Trim()),
                FPieceWork2 = Convert.ToDecimal(txtFPieceWork2.Text.Trim()),
                FPieceWork3 = Convert.ToDecimal(txtFPieceWork3.Text.Trim()),
                FPieceWork4 = Convert.ToDecimal(txtFPieceWork4.Text.Trim()),
                FPieceWork5 = Convert.ToDecimal(txtFPieceWork5.Text.Trim()),
                FNetWeight = Convert.ToDecimal(txtFNetWeight.Text.Trim()),//重量
            };

            return ItemsService.Add(its);
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

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
                    var parms = new Dictionary<string, object>();
                    parms.Clear();

                    parms.Add("@companyid",CurrentUser.AccountComId);
                    parms.Add("@type", "2001");//气体                    
                    var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                    txtFCode.Text = list.Rows[0][0].ToString();
                    break;
                case WebAction.Edit:

                    txtFCode.Enabled = false;
                    txtFCode.Readonly = true;

                    if (ProductItem != null)
                    {
                        txtFCode.Text = ProductItem.FCode;
                        txtFName.Text = ProductItem.FName;
                        txtFGroupNum.Text = ProductItem.FGroupNum;
                        txtFSpec.Text = ProductItem.FSpec;
                        ddlUnit.SelectedValue = ProductItem.FUnit;
                        txtFPurchasePrice.Text = ProductItem.FPurchasePrice.ToString();
                        txtFSalesPrice.Text = ProductItem.FSalesPrice.ToString();
                        txtFMemo.Text = ProductItem.FMemo;
                        txtFPieceWork1.Text = ProductItem.FPieceWork1.ToString();
                        txtFPieceWork2.Text = ProductItem.FPieceWork2.ToString();
                        txtFPieceWork3.Text = ProductItem.FPieceWork3.ToString();
                        txtFPieceWork4.Text = ProductItem.FPieceWork4.ToString();
                        txtFPieceWork5.Text = ProductItem.FPieceWork5.ToString();
                        txtFNetWeight.Text = ProductItem.FNetWeight.ToString();
                        txtFRack.Text = ProductItem.FRack;

                    }
                    break;
            }
        }

        #endregion
    }
}