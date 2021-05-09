using System;
using System.Linq;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Framework.Web;
using Enterprise.Framework.Log;
using Enterprise.Framework.Extension;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.account
{
    // ReSharper disable once InconsistentNaming
    public partial class account_edit : PageBase
    {
        /// <summary>
        ///     账号信息
        /// </summary>
        private base_account _baseAccount;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        private AccountService _accountService;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        private OrgnizationService _orgnizationService;

        /// <summary>
        ///     当前角色对象ID
        /// </summary>
        protected int OrgnizationId
        {
            get { return int.Parse(Request["orgid"]); }
        }

        /// <summary>
        ///     当前角色对象ID
        /// </summary>
        protected int AccountId
        {
            get { return int.Parse(Request["id"]); }
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
        ///     部门组织数据服务
        /// </summary>
        protected OrgnizationService OrgnizationService
        {
            get { return _orgnizationService ?? (_orgnizationService = new OrgnizationService()); }
            set { _orgnizationService = value; }
        }

        /// <summary>
        ///     部门名称
        /// </summary>
        protected string OrgnizationName
        {
            get
            {
                var baseOrgnization = OrgnizationService.FirstOrDefault(p => p.id == OrgnizationId&&p.FCompanyId==CurrentUser.AccountComId);
                string orgName;
                if (baseOrgnization == null)
                {
                    base_company company = (new CompanyService()).FirstOrDefault(p => p.id == OrgnizationId);
                    orgName = company.com_name;
                }
                else
                {
                    orgName = baseOrgnization.org_name;
                }
                return orgName;
            }
        }

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected AccountService AccountService
        {
            get { return _accountService ?? (_accountService = new AccountService()); }
            set { _accountService = value; }
        }

        /// <summary>
        ///     账号信息    
        /// </summary>
        protected base_account BaseAccount
        {
            get { return _baseAccount ?? (_baseAccount = AccountService.FirstOrDefault(p => p.id == AccountId && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _baseAccount = value; }
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

                        LogUtil.WriteLog("帐户管理", CurrentUser.AccountNumber, string.Format(@"添加用户成功！"));
                        break;

                    case WebAction.Edit:
                        isSucceed = SubmintEdit();

                        LogUtil.WriteLog("帐户管理", CurrentUser.AccountNumber, string.Format(@"修改用户成功！"));
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

        protected void txtaccount_number_TextChanged(object sender, EventArgs e)
        {
            var account = txtaccount_number.Text.Trim();
            if (!string.IsNullOrWhiteSpace(account))
            {
                var accountObject = AccountService.Where(p => p.account_number == account && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (accountObject != null)
                    Alert.Show("帐号已被注册，请试用其它帐号进行注册！", MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (BaseAccount != null)
            {
                //操作日志
                Log(string.Format(@"修改用户基本信息{0}成功。", BaseAccount.account_number));

                BaseAccount.account_name = txtaccount_name.Text.Trim();
                BaseAccount.account_number = txtaccount_number.Text.Trim();
                BaseAccount.account_role_id = ddlaccount_role_name.SelectedValue.Trim();
                BaseAccount.account_role_name = ddlaccount_role_name.SelectedText;
                BaseAccount.account_sex = ddlaccount_sex.SelectedValue.Trim();
                BaseAccount.account_flag = Convert.ToInt32(ddlaccount_flag.SelectedValue);
                BaseAccount.account_email = txtaccount_email.Text.Trim();
                BaseAccount.account_qq = txtaccount_qq.Text.Trim();
                BaseAccount.account_mobile_phone = txtaccount_mobile_phone.Text.Trim();
                BaseAccount.code_cn = ChineseSpell.MakeSpellCode(txtaccount_name.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper();
                //BaseAccount.account_major =ddlaccount_major.SelectedValue;
                //BaseAccount.account_post = ddlaccount_post.SelectedValue;

                return AccountService.SaveChanges() >= 0;
            }



            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var account = new base_account
            {
                account_name = txtaccount_name.Text.Trim(),
                account_number = txtaccount_number.Text.Trim(),
                account_role_id = ddlaccount_role_name.SelectedValue.Trim(),
                account_role_name = ddlaccount_role_name.SelectedText,
                account_sex = ddlaccount_sex.SelectedValue.Trim(),
                account_org_id = OrgnizationId,
                //account_org_name = OrgnizationName,
                account_flag = Convert.ToInt32(ddlaccount_flag.SelectedValue),
                account_email = txtaccount_email.Text.Trim(),
                account_qq = txtaccount_qq.Text.Trim(),
                account_mobile_phone = txtaccount_mobile_phone.Text.Trim(),
                code_cn = ChineseSpell.MakeSpellCode(txtaccount_name.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper(),
                //account_major = Convert.ToInt32(ddlaccount_major.SelectedValue),
                //account_post = Convert.ToInt32(ddlaccount_post.SelectedValue),
                deleteflag = 0,
                FCompanyId = CurrentUser.AccountComId,
                FFlag = 1
            };

            int listCount = AccountService.Count(p => p.account_org_id == OrgnizationId && p.FCompanyId == CurrentUser.AccountComId);
            account.account_sort = listCount + 1;
            account.account_password = EncryptUtil.Encrypt("GAS123456");
            account.createdon = DateTime.Now;
            account.account_company_id = CurrentUser.AccountComId;

            Log(string.Format(@"添加用户基本信息{0}成功。", account.account_number));

            return AccountService.Add(account);
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            ddlaccount_role_name.DataSource = new RoleService().Where(p => p.deleteflag == 0 && p.account_id == null && p.FCompanyId == CurrentUser.AccountComId);
            ddlaccount_role_name.DataTextField = "role_name";
            ddlaccount_role_name.DataValueField = "id";
            ddlaccount_role_name.DataBind();


            //ddlaccount_post.DataSource =
            //    new DictionaryService().Where(p => p.deleteflag == 0 && p.category == "职员类别");
            //ddlaccount_post.DataBind();

            //ddlaccount_major.DataSource =
            //    new DictionaryService().Where(p => p.deleteflag == 0 && p.category == "职位管理");
            //ddlaccount_major.DataBind();
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    txtaccount_org_name.Text = OrgnizationName;
                    break;
                case WebAction.Edit:
                    if (BaseAccount != null)
                    {
                        txtaccount_name.Text = BaseAccount.account_name;
                        txtaccount_number.Text = BaseAccount.account_number;
                        ddlaccount_role_name.SelectedValue = BaseAccount.account_role_id;
                        ddlaccount_sex.SelectedValue = BaseAccount.account_sex; //性别
                        txtaccount_org_name.Text = OrgnizationName;
                        ddlaccount_flag.SelectedValue = BaseAccount.account_flag.ToString();
                        txtaccount_email.Text = BaseAccount.account_email;
                        txtaccount_qq.Text = BaseAccount.account_qq;
                        txtaccount_mobile_phone.Text = BaseAccount.account_mobile_phone;

                        if (BaseAccount.account_major != null)
                            ddlaccount_major.SelectedValue = BaseAccount.account_major.ToString();
                        if (BaseAccount.account_post != null)
                            ddlaccount_post.SelectedValue = BaseAccount.account_post.ToString();

                        //性能问题，建议不试用
                        //WebControlHandler.BindObjectToControls(BaseAccount, SimpleForm1);
                    }
                    break;
            }
        }
        #endregion
    }
}