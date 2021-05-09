using System;

using FineUI;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Framework.Web;
using Enterprise.Framework.File;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.icon
{
// ReSharper disable once InconsistentNaming
    public partial class icon_edit :  PageBase
    {

        private SequenceService _sequenceService;
        protected SequenceService SequenceService
        {
            get { return _sequenceService ?? (_sequenceService = new SequenceService()); }
            set { _sequenceService = value; }
        }

        /// <summary>
        ///     字典信息
        /// </summary>
        private base_icon _baseIcon;

        /// <summary>
        ///     字典数据服务
        /// </summary>
        private IconService _iconService;

        /// <summary>
        ///     字典数据服务
        /// </summary>
        protected IconService IconService
        {
            get { return _iconService ?? (_iconService = new IconService()); }
            set { _iconService = value; }
        }

        /// <summary>
        ///     当前角色对象ID
        /// </summary>
        protected int Key_ID
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
        ///      字典信息    
        /// </summary>
        protected base_icon BaseDictionary
        {
            get { return _baseIcon ?? (_baseIcon = IconService.FirstOrDefault(p => p.id == Key_ID)); }
            set { _baseIcon = value; }
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

        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            if (filePhoto.HasFile)
            {
                var fileSuffix = filePhoto.ShortFileName.Substring(filePhoto.ShortFileName.LastIndexOf('.'));

                var sequence = SequenceService.CreateSequence("LH",CurrentUser.AccountComId);

                var fileName = sequence + fileSuffix;

                var uploadpath = Config.GetUploadpath();

                var tempPath = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

                if (!DirFile.XFileExists(Server.MapPath(tempPath)))
                {
                    DirFile.XCreateDir(Server.MapPath(tempPath));
                }

                var iconPath = (string.Format(@"~/icon/"));

                if (!DirFile.XFileExists(Server.MapPath(iconPath)))
                {
                    DirFile.XCreateDir(Server.MapPath(iconPath));
                }

                filePhoto.SaveAs(Server.MapPath(tempPath + fileName));


                ImageHelper.LocalImage2Thumbs(Server.MapPath(tempPath + fileName),
                   Server.MapPath(iconPath + fileName), 16, 16, "WH");

                imgPhoto.ImageUrl = iconPath + fileName;

                txticon_src.Text = imgPhoto.ImageUrl;

                // 清空文件上传组件
                filePhoto.Reset();
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
            if (BaseDictionary != null)
            {
                BaseDictionary.icon_src = txticon_src.Text.Trim();
                BaseDictionary.deleteflag = 0;

                return IconService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var dictionary = new base_icon
            {
                icon_src = txticon_src.Text.Trim(),
                deleteflag = 0
            };

            return IconService.Add(dictionary);
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
                    
                    break;
                case WebAction.Edit:
                    if (BaseDictionary != null)
                    {
                        txticon_src.Text=BaseDictionary.icon_src;
                    }
                    break;
            }
        }
        #endregion
    }
}