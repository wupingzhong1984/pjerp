using System;
using System.IO;
using com.flajaxian;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.File;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;

namespace Enterprise.IIS.product.enterpriseinfo
{
// ReSharper disable once InconsistentNaming
    public partial class index : PageBase
    {
        /// <summary>
        ///     网站信息
        /// </summary>
        private base_configset _baseConfigset;

        /// <summary>
        ///     网站数据服务
        /// </summary>
        private ConfigsetService _configsetService;

        /// <summary>
        ///     网站数据服务
        /// </summary>
        protected ConfigsetService ConfigsetService
        {
            get { return _configsetService ?? (_configsetService = new ConfigsetService()); }
            set { _configsetService = value; }
        }

        /// <summary>
        ///     当前角色对象ID
        /// </summary>
        protected int Key
        {
            get { return 1; }
        }

        /// <summary>
        ///     当前画面操作项
        /// </summary>
        public WebAction Actions
        {
            get
            {
                return (WebAction)2;
            }
        }

        /// <summary>
        ///      字典信息    
        /// </summary>
        protected base_configset BaseConfigset
        {
            get { return _baseConfigset ?? (_baseConfigset = ConfigsetService.FirstOrDefault(p => p.id == Key)); }
            set { _baseConfigset = value; }
        }

        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        private CompanyService _companyService;

        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        protected CompanyService CompanyService
        {
            get { return _companyService ?? (_companyService = new CompanyService()); }
            set { _companyService = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!FileUploader1.FileIsPosted)
                {
                    //初始化控件数据
                    InitData();

                    //加载数据
                    LoadData();
                }

                // 每次页面加载都要给上传控件加入No值，确保上传控件保存文件时能够得到工单编号
                //  FileUploader1.State.Add("AttachNo", NO);
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
                    Alert.Show("提交成功！", MessageBoxIcon.Information);
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
            if (BaseConfigset != null)
            {
                BaseConfigset.name = txtName.Text.Trim();
                BaseConfigset.abbreviation = txtAbbreviation.Text.Trim();
                BaseConfigset.keywords = txtKeywords.Text.Trim();
                BaseConfigset.description = txtDescription.Text.Trim();
                BaseConfigset.deleteflag = 0;
                BaseConfigset.icp = txtICP.Text.Trim();
                BaseConfigset.cookieDomain = txtCookieDomain.Text.Trim();
                BaseConfigset.siteID = txtSiteID.Text.Trim();
                BaseConfigset.allowReg = int.Parse(rblAllowReg.SelectedValue);
                BaseConfigset.checkReg = int.Parse(rblCheckReg.SelectedValue);
                BaseConfigset.version = txtversion.Text.Trim();

                //公司
                var company = CompanyService.FirstOrDefault(p => p.deleteflag == 0);
                company.com_name = txtcom_name.Text.Trim();
                company.com_chairman = txtcom_chairman.Text;
                company.com_tel = txtcom_tel.Text;

                CompanyService.SaveChanges();
                return ConfigsetService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var dictionary = new base_configset
            {
                name = txtName.Text.Trim(),
                abbreviation = txtAbbreviation.Text.Trim(),
                keywords = txtKeywords.Text.Trim(),
                description = txtDescription.Text.Trim(),
                deleteflag = 0,
                icp = txtICP.Text.Trim(),
                cookieDomain = txtCookieDomain.Text.Trim(),
                siteID = txtSiteID.Text.Trim(),
                allowReg = int.Parse(rblAllowReg.SelectedValue),
                checkReg = int.Parse(rblCheckReg.SelectedValue),
                version = txtversion.Text.Trim(),
            };

            //公司
            var company = CompanyService.FirstOrDefault(p => p.deleteflag == 0);
            company.com_name = txtcom_name.Text.Trim();
            company.com_chairman = txtcom_chairman.Text;
            company.com_tel = txtcom_tel.Text;
            CompanyService.SaveChanges();

            return ConfigsetService.Add(dictionary);
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
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
                    if (BaseConfigset != null)
                    {
                        txtName.Text = BaseConfigset.name;
                        txtAbbreviation.Text = BaseConfigset.abbreviation;
                        txtKeywords.Text = BaseConfigset.keywords;
                        txtDescription.Text = BaseConfigset.description;
                        txtICP.Text = BaseConfigset.icp;
                        txtCookieDomain.Text = BaseConfigset.cookieDomain;
                        txtSiteID.Text = BaseConfigset.siteID;
                        rblAllowReg.SelectedValue = BaseConfigset.allowReg.ToString();
                        rblCheckReg.SelectedValue = BaseConfigset.checkReg.ToString();
                        txtversion.Text = BaseConfigset.version;
                        imgLogo.ImageUrl = BaseConfigset.logo;
                    }

                    //公司
                    var company = CompanyService.FirstOrDefault(p => p.deleteflag == 0);
                    txtcom_name.Text = company.com_name;
                    txtcom_chairman.Text = company.com_chairman;
                    txtcom_tel.Text = company.com_tel;

                    break;
            }
        }

        protected void FileUploader1_FileReceived(object sender, FileReceivedEventArgs e)
        {
            // 文件名称
            string srcFileName = Path.GetFileName(e.File.FileName);

            var fileSuffix = srcFileName.Substring(srcFileName.LastIndexOf('.'));

            var sequence = new SequenceService().CreateSequence("LH", CurrentUser.AccountComId);

            var fileName = sequence + fileSuffix;

            var uploadpath = Config.GetUploadpath();

            var tempPath = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

            if (!DirFile.XFileExists(Server.MapPath(tempPath)))
            {
                DirFile.XCreateDir(Server.MapPath(tempPath));
            }

            var logoPath = (string.Format(@"~/images/"));

            if (!DirFile.XFileExists(Server.MapPath(logoPath)))
            {
                DirFile.XCreateDir(Server.MapPath(logoPath));
            }

            e.File.SaveAs(Server.MapPath(tempPath + fileName));

            ImageHelper.LocalImage2Thumbs(Server.MapPath(tempPath + fileName),
               Server.MapPath(logoPath + fileName), 120, 50, "WH");

            if (BaseConfigset != null)
            {
                BaseConfigset.logo = logoPath + fileName;
                imgLogo.ImageUrl = logoPath + fileName;
                ConfigsetService.SaveChanges();
            }

            PageContext.Redirect("~/config/unitinfo.aspx");
        }

        #endregion
    }
}