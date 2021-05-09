using System;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.action
{
// ReSharper disable once InconsistentNaming
    public partial class action_edit : PageBase
    {

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

        protected int ActionId
        {
            get { return int.Parse(Request["id"]); }
        }

        private base_aciton _baseaciton;

        protected base_aciton BaseAciton
        {
            get { return _baseaciton ?? (_baseaciton = MainAction.FirstOrDefault(p => p.id == ActionId)); }
            set { _baseaciton = value; }
        }

        private AcitonService _mainAction;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected AcitonService MainAction
        {
            get { return _mainAction ?? (_mainAction = new AcitonService()); }
            set { _mainAction = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
                BindData();
            }
        }

        private void BindData()
        {
            switch (Actions)
            {
                case WebAction.Add:

                    break;
                case WebAction.Edit:
                    if (BaseAciton != null)
                    {
                        txtaction_name.Text = BaseAciton.action_name;
                        txtaction_en.Text = BaseAciton.action_en;
                        txtaction_desc.Text = BaseAciton.action_desc;
                    }
                    break;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSuessed = false;
            try
            {
                switch (Actions)
                {
                    case WebAction.Add:
                        isSuessed=SubmintAdd();
                        break;
                    case WebAction.Edit:
                        isSuessed=SubmintEdit();
                        break;
                }
            }
            catch (Exception)
            {
                isSuessed = false;
            }
            finally
            {
                if (isSuessed)
                {
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var baseaction = new base_aciton
            {
                action_name=txtaction_name.Text.Trim(),
                action_en=txtaction_en.Text.Trim(),
                action_desc=txtaction_desc.Text.Trim(),
                createdby_name = string.Format(@"{0}({1})", CurrentUser.AccountName, CurrentUser.AccountNumber),
                createdon=DateTime.Now,
                deleteflag=0
            };

            return MainAction.Add(baseaction);
        }

        private bool SubmintEdit()
        {
            if (BaseAciton != null)
            {
                BaseAciton.action_name = txtaction_name.Text.Trim();
                BaseAciton.action_en = txtaction_en.Text.Trim();
                BaseAciton.action_desc = txtaction_desc.Text.Trim();
                //BaseAciton.updatedby = string.Format(@"{0}({1})", CurrentUser.account_name, CurrentUser.account_number);
                //BaseAciton.updatedon = DateTime.Now;
                return MainAction.SaveChanges() >= 0;
            }
            return false;
        }
    }
}