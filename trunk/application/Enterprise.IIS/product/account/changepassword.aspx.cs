using System;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;

namespace Enterprise.IIS.product.account
{
// ReSharper disable once InconsistentNaming
    public partial class changepassword : PageBase
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        private base_account _baseAccount;

        /// <summary>
        /// 用户操作对象
        /// </summary>
        private AccountService _accountService;

        public AccountService MainAccount
        {
            get { return _accountService ?? (_accountService = new AccountService()); }
            set { _accountService = value; }
        }

        public base_account Baseaccount
        {
            get { return _baseAccount ?? (_baseAccount = MainAccount.FirstOrDefault(p => p.account_number == CurrentUser.AccountNumber)); }
            set { _baseAccount = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //SetPermissionButtons(Toolbar1);
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();//GetHidePostBackReference
            }
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxOldPassword.Text.Trim())) 
            {
                return;
            }
            if (string.IsNullOrEmpty(tbxNewPassword.Text.Trim()))
            {
                return;
            }
            if (Baseaccount != null)
            {
                //Log(string.Format(@"修改密码操作。"));

                //if (Baseaccount.account_password.Equals(EncryptUtil.Encrypt(tbxOldPassword.Text.Trim())))
                if (Baseaccount.account_password.Equals(tbxOldPassword.Text.Trim()))
                {
                    //Baseaccount.account_password = EncryptUtil.Encrypt(tbxNewPassword.Text.Trim());
                    Baseaccount.account_password = tbxNewPassword.Text.Trim();

                    if(MainAccount.SaveChanges()>=0)
                    {
                        Alert.Show("修改成功",MessageBoxIcon.Information);
                    }
                    else
                    {
                        Alert.Show("修改失败",MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Alert.Show("密码错误", MessageBoxIcon.Warning);
                }
            }
        }
    }
}