using System;
using FineUI;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Framework.Web;

namespace Enterprise.IIS.product.bulletin
{
    /// <summary>
    ///     员工
    /// </summary>
    public partial class view : PageBase
    {

        /// <summary>
        ///     SequenceService
        /// </summary>
        private SequenceService _sequenceService;
        /// <summary>
        ///     SequenceService
        /// </summary>
        protected SequenceService SequenceService
        {
            get { return _sequenceService ?? (_sequenceService = new SequenceService()); }
            set { _sequenceService = value; }
        }
        /// <summary>
        ///     帐号数据服务
        /// </summary>
        private AccountService _accountService;
        /// <summary>
        ///     帐号数据服务
        /// </summary>
        protected AccountService AccountService
        {
            get { return _accountService ?? (_accountService = new AccountService()); }
            set { _accountService = value; }
        }

        /// <summary>
        ///     BulletinItemService
        /// </summary>
        private BulletinItemService _bulletinItemService;
        /// <summary>
        ///     BulletinItemService
        /// </summary>
        protected BulletinItemService BulletinItemService
        {
            get { return _bulletinItemService ?? (_bulletinItemService = new BulletinItemService()); }
            set { _bulletinItemService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private BulletinService _bulletinService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected BulletinService BulletinService
        {
            get { return _bulletinService ?? (_bulletinService = new BulletinService()); }
            set { _bulletinService = value; }
        }

        /// <summary>
        ///     当前角色对象ID
        /// </summary>
        protected string Key
        {
            get { return Request["sid"]; }
        }

        private base_bulletin_items _baseBulletinItems;

        /// <summary>
        ///      字典信息    
        /// </summary>
        protected base_bulletin_items BaseBulletinItems
        {
            get { return _baseBulletinItems ?? (_baseBulletinItems = BulletinItemService.FirstOrDefault(p => p.sequence_id == Key)); }
            set { _baseBulletinItems = value; }
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

        #region Protected Method
        #endregion

        #region Private Method
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

                    break;
                case WebAction.Details:
                    if (BaseBulletinItems != null)
                    {
                        lbltitle.Text = BaseBulletinItems.title;
                        lblvalidity_s.Text = BaseBulletinItems.validity_s.Value.ToShortDateString();
                        lblvalidity_e.Text = BaseBulletinItems.validity_e.Value.ToShortDateString();
                        lblm_content.Text = BaseBulletinItems.m_content;

                        lblfile.Text = string.Format(@"<a href='../../{0}' target='_blank'>附件</a>", BaseBulletinItems.fileupload.Replace("~/",""));

                    }
                    break;
            }
        }

        #endregion
    }
}