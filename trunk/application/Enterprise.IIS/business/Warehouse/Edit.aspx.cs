using System;
using Enterprise.Service.Base.ERP;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Framework.Extension;
using FineUI;


namespace Enterprise.IIS.business.Warehouse
{
    public partial class Edit : PageBase
    {
        /// <summary>
        ///     产品信息
        /// </summary>
        private LHWarehouse _product;

        /// <summary>
        ///     数据服务
        /// </summary>
        private WarehouseService _itemsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected WarehouseService ItemsService
        {
            get { return _itemsService ?? (_itemsService = new WarehouseService()); }
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
        protected LHWarehouse ProductItem
        {
            get
            {
                return _product ?? (_product = ItemsService.FirstOrDefault(p => p.FCode == FCode
                    && p.FCompanyId == CurrentUser.AccountComId));
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

                ProductItem.FAddress = txtFAddress.Text.Trim();
                ProductItem.FLinkman = txtFLinkman.Text.Trim();
                ProductItem.FPhome = txtFPhome.Text.Trim();

                ProductItem.FMemo = txtFMemo.Text.Trim();//备注
                return ItemsService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var its = new LHWarehouse
            {
                FCode = txtFCode.Text.Trim(),
                FName = txtFName.Text.Trim(),
                FSpell = ChineseSpell.MakeSpellCode(txtFName.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper(),
                FFlag = 1,
                
                FAddress = txtFAddress.Text.Trim(),
                FLinkman = txtFLinkman.Text.Trim(),
                FPhome = txtFPhome.Text.Trim(),

                FCompanyId = CurrentUser.AccountComId,
                FMemo = txtFMemo.Text.Trim(),

            };

            return ItemsService.Add(its);
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    //var parms = new Dictionary<string, object>();
                    //parms.Clear();

                    //parms.Add("@companyid", CurrentUser.AccountComId);
                    //parms.Add("@type", "2001");//气体                    
                    //var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                    //txtFCode.Text = list.Rows[0][0].ToString();
                    break;
                case WebAction.Edit:

                    txtFCode.Enabled = false;
                    txtFCode.Readonly = true;

                    if (ProductItem != null)
                    {
                        txtFCode.Text = ProductItem.FCode;
                        txtFName.Text = ProductItem.FName;
                        



                        txtFMemo.Text = ProductItem.FMemo;
                    }
                    break;
            }
        }

        #endregion
    }
}