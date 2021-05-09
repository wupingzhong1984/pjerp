using System;
using System.Globalization;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Framework.Extension;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.orgnization
{
    // ReSharper disable once InconsistentNaming
    public partial class orgnization_edit : PageBase
    {
        /// <summary>
        ///     部门组织信息
        /// </summary>
        private base_orgnization _baseOrgnization;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        private OrgnizationService _orgnizationService;

        /// <summary>
        ///     组织部门ID
        /// </summary>
        protected int OrgnizationId
        {
            get { return int.Parse(Request["id"]); }
        }

        /// <summary>
        ///     组织部门父ID
        /// 新增时，代表选中当前部门下
        /// </summary>
        protected int POrgnizationId
        {
            get { return int.Parse(Request["pOrgid"]); }
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
        protected string POrgnizationName
        {
            get
            {
                base_orgnization baseOrgnization = OrgnizationService.FirstOrDefault(p => p.id == POrgnizationId);
                string orgName;
                if (baseOrgnization == null)
                {
                    base_company company = (new CompanyService()).FirstOrDefault(p => p.id == POrgnizationId && p.id == CurrentUser.AccountComId);
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
        /// 当前选中的部门信息
        /// </summary>
        protected base_orgnization POrgnization
        {
            get
            {
                base_orgnization baseOrgnization = OrgnizationService.FirstOrDefault(p => p.id == POrgnizationId);
                if (baseOrgnization == null)
                {
                    base_company company = (new CompanyService()).FirstOrDefault(p => p.id == POrgnizationId);
                    var org = new base_orgnization
                    {
                        id = company.id,
                        org_name = company.com_name,
                        cate_code = "1"
                    };
                    return org;
                }
                return baseOrgnization;
            }
        }

        /// <summary>
        ///    信息    
        /// </summary>
        protected base_orgnization BaseOrgnization
        {
            get
            {
                return _baseOrgnization ?? (_baseOrgnization = OrgnizationService.FirstOrDefault(p => p.id == OrgnizationId));
            }
            set { _baseOrgnization = value; }
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
            if (BaseOrgnization != null)
            {
                BaseOrgnization.org_name = txtorg_name.Text.Trim();
                BaseOrgnization.org_type = Int32.Parse(ddlorg_type.SelectedValue);
                BaseOrgnization.org_office_tel = txtorg_office_tel.Text.Trim();
                BaseOrgnization.org_desc = txtorg_desc.Text.Trim();
                BaseOrgnization.org_account_name = txtorg_account_name.Text.Trim();
                BaseOrgnization.org_office_fax = txtorg_office_fax.Text.Trim();
                BaseOrgnization.code_cn = ChineseSpell.MakeSpellCode(txtorg_name.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper();
                BaseOrgnization.code = txtcode.Text.Trim();
                return OrgnizationService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var catecode = POrgnizationId == 1 ? "1" : string.Format(@"{0}.{1}", POrgnization.cate_code, POrgnizationId);

            var orgnization = new base_orgnization
            {
                org_name = txtorg_name.Text.Trim(),
                org_type = Int32.Parse(ddlorg_type.SelectedValue),
                org_office_tel = txtorg_office_tel.Text.Trim(),
                org_desc = txtorg_desc.Text.Trim(),
                org_parent_id = POrgnizationId,
                cate_code = catecode,
                code = txtcode.Text.Trim(),
                org_account_name = txtorg_account_name.Text.Trim(),
                org_office_fax = txtorg_office_fax.Text,
                code_cn = ChineseSpell.MakeSpellCode(txtorg_name.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper(),
                deleteflag = 0,
                FCompanyId = CurrentUser.AccountComId,
                FFlag = 1
            };

            int listCount = OrgnizationService.Count(p => p.org_parent_id == POrgnizationId);
            orgnization.org_sort = listCount + 1;
            orgnization.createdon = DateTime.Now;

            return OrgnizationService.Add(orgnization);
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
                    lblorg_name.Text = POrgnizationName;

                    txtcode.Text = POrgnizationId == 1 ? "" : string.Format(@"{0}.",POrgnization.code);


                    break;
                case WebAction.Edit:
                    if (BaseOrgnization != null)
                    {
                        txtorg_name.Text = BaseOrgnization.org_name;
                        ddlorg_type.SelectedValue = BaseOrgnization.org_type.HasValue
                            ? BaseOrgnization.org_type.Value.ToString(CultureInfo.InvariantCulture)
                            : "";
                        txtorg_office_tel.Text = BaseOrgnization.org_office_tel;
                        txtorg_desc.Text = BaseOrgnization.org_desc;
                        lblorg_name.Text = POrgnizationName;
                        txtorg_account_name.Text = BaseOrgnization.org_account_name;
                        txtorg_office_fax.Text = BaseOrgnization.org_office_fax;
                        txtcode.Text = BaseOrgnization.code.Trim();



                    }
                    break;
            }
        }

        #endregion
    }
}