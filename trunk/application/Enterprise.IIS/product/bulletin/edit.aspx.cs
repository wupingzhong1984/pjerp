using System;
using System.Linq;
using System.Text;
using System.Transactions;
using FineUI;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Framework.Web;
using Enterprise.Framework.File;
// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.bulletin
{
    // ReSharper disable once InconsistentNaming
    public partial class edit : PageBase
    {
        private SequenceService _sequenceService;
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

        private BulletinItemService _bulletinItemService;
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
            var isSucceed = false;

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
            if (BaseBulletinItems != null)
            {
                BaseBulletinItems.m_content=txtm_content.Text;
                BaseBulletinItems.title=txttitle.Text;
                BaseBulletinItems.validity_e= Convert.ToDateTime(dpvalidity_e.Text) ;
                BaseBulletinItems.validity_s = Convert.ToDateTime(dpvalidity_s.Text);
                BaseBulletinItems.precedence=rbtnPrecedence.SelectedValue;
                BaseBulletinItems.receiver=tbAccount.Text;
                BaseBulletinItems.receiver_org=tbOrgnization.Text;
                BaseBulletinItems.receiver_role=tbRole.Text;

                var list = BulletinService.Where(p => p.sequence_id == BaseBulletinItems.sequence_id).ToList();

                foreach (var bulletin in list) 
                {

                    bulletin.m_content = txtm_content.Text;
                    bulletin.title = txttitle.Text;
                    bulletin.validity_e = Convert.ToDateTime(dpvalidity_e.Text);
                    bulletin.validity_s = Convert.ToDateTime(dpvalidity_s.Text);

                    BulletinService.SaveChanges();
                }


                return BulletinItemService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            using (var ts = new TransactionScope())
            {

                var sequence = new SequenceService().CreateSequence("LH", CurrentUser.AccountComId);
                var address = string.Empty;
                if (FileUpload1.HasFile)
                {
                    var fileSuffix = FileUpload1.ShortFileName.Substring(FileUpload1.ShortFileName.LastIndexOf('.'));

                    var fileName = sequence + fileSuffix;

                    var uploadpath = Config.GetUploadpath() + DateTime.Now.ToString("yyyyMMdd") + "/";

                    if (!DirFile.XFileExists(Server.MapPath(uploadpath)))
                    {
                        DirFile.XCreateDir(Server.MapPath(uploadpath));
                    }

                    address = uploadpath + fileName;

                    FileUpload1.SaveAs(Server.MapPath(address));
                }
                //A: base_bulletin_items
                var bulletinItems = new base_bulletin_items
                {
                    title = txttitle.Text.Trim(),
                    m_content = txtm_content.Text.Trim(),
                    pubdate = DateTime.Now,
                    deleteflag = 0,
                    precedence = rbtnPrecedence.SelectedValue,
                    sender = CurrentUser.Id,
                    validity_s = dpvalidity_s.SelectedDate,
                    validity_e = dpvalidity_e.SelectedDate,
                    receiver_role = tbRole.Text.Trim(),
                    receiver_org = tbOrgnization.Text.Trim(),
                    receiver = tbAccount.Text.Trim(),
                    isrepeal = 0,//0正常；1收回
                    fileupload = address,
                    sequence_id = sequence,
                    FCompanyId = CurrentUser.AccountComId,
                    FFlag = 1
                };
                BulletinItemService.Add(bulletinItems);
               

                #region 发送站内消息
                //B:分别给组织人发公告消息
                var orgnizations = tbOrgnization.Text.Trim();
                var roles = tbRole.Text.Trim();
                var accounts = tbAccount.Text.Trim();

                //接收对象
                var receivers = new StringBuilder();

                //判断是否选择了部门
                if (!string.IsNullOrWhiteSpace(orgnizations))
                {
                    var cOrgnizations = orgnizations.Split(',');

                    //分部给些部门的人发公告
                    for (int i = 0; i < cOrgnizations.Length; i++)
                    {
                        //取相关的人
                        var orgID = Convert.ToInt32(cOrgnizations[i]);
                        var accountFinds = AccountService.Where(p => p.account_org_id == orgID).ToList();

                        foreach (var accountFind in accountFinds)
                        {
                            var info = string.Format(@"{0}", accountFind.id);

                            if (!receivers.ToString().Contains(info))
                            {
                                receivers.AppendFormat(@"{0},", info);
                            }
                        }
                    }
                }
                //角色
                if (!string.IsNullOrWhiteSpace(roles))
                {
                    var croles = roles.Split(',');

                    //分部给些部门的人发公告
                    for (int i = 0; i < croles.Length; i++)
                    {
                        //取相关的人
                        var roleid = croles[i];
                        var accountFinds = AccountService.Where(p => p.account_role_id == roleid).ToList();

                        foreach (var accountFind in accountFinds)
                        {
                            var info = string.Format(@"{0}", accountFind.id);

                            if (!receivers.ToString().Contains(info))
                            {
                                receivers.AppendFormat(@"{0},", info);
                            }
                        }
                    }
                }
                //个人
                if (!string.IsNullOrWhiteSpace(accounts))
                {
                    var caccounts = accounts.Split(',');

                    //分部给些部门的人发公告
                    for (int i = 0; i < caccounts.Length; i++)
                    {
                        //取相关的人
                        var accountid = caccounts[i];
                        if (!receivers.ToString().Contains(accountid))
                        {
                            receivers.AppendFormat(@"{0},", accountid);
                        }
                    }
                }

                var receverItems = receivers.ToString();

                if (!string.IsNullOrWhiteSpace(receverItems))
                {
                    //分别发送通告

                    var readers = receverItems.Split(',');
                    for (int i = 0; i < readers.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(readers[i]))
                        {
                            continue;
                        }
                        var bulletin = new base_bulletin
                        {
                            title = txttitle.Text.Trim(),
                            m_content = txtm_content.Text.Trim(),
                            pubdate = DateTime.Now,
                            deleteflag = 0,
                            validity_s = dpvalidity_s.SelectedDate,
                            validity_e = dpvalidity_e.SelectedDate,
                            isrepeal = 0, //0正常；1收回
                            receiver = Convert.ToInt32(readers[i]),
                            sequence_id = sequence
                        };
                           // BulletinService.AddEntity(bulletin);

                       BulletinService.Add(bulletin);
                    }

                  //  BulletinService.SaveChanges();

                    // 清空文件上传组件
                    FileUpload1.Reset();
                    ts.Complete();
                    return true;
                }
                #endregion
            }
            return false;
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            tbOrgnization.OnClientTriggerClick = Window1.GetSaveStateReference(tbOrgnization.ClientID, HFOrgnization.ClientID)
                   + Window1.GetShowReference(string.Format("select_orgnization_list.aspx"));

            tbRole.OnClientTriggerClick = Window2.GetSaveStateReference(tbRole.ClientID, HFRole.ClientID)
                   + Window2.GetShowReference("select_role_list.aspx");

            tbAccount.OnClientTriggerClick = Window3.GetSaveStateReference(tbAccount.ClientID, HFAccount.ClientID)
                   + Window3.GetShowReference("select_account_list.aspx");
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
                    if ( BaseBulletinItems!= null)
                    {
                        txtm_content.Text = BaseBulletinItems.m_content;
                        txttitle.Text = BaseBulletinItems.title;
                        dpvalidity_e.SelectedDate = BaseBulletinItems.validity_e;
                        dpvalidity_s.SelectedDate =BaseBulletinItems.validity_s;
                        rbtnPrecedence.SelectedValue = BaseBulletinItems.precedence;
                        tbAccount.Text = BaseBulletinItems.receiver;
                        tbOrgnization.Text = BaseBulletinItems.receiver_org;
                        tbRole.Text = BaseBulletinItems.receiver_role;
                    }
                    break;
            }
        }
        
        #endregion
    }
}