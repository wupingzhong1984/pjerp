using System;
using Enterprise.Framework.Extension;
using Enterprise.IIS.Common;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Framework.Log;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.employee
{
    // ReSharper disable once InconsistentNaming
    public partial class edit : PageBase
    {
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
        ///     对象ID
        /// </summary>
        protected int Key
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
                var baseOrgnization = OrgnizationService.FirstOrDefault(p => p.id == OrgnizationId //
                    && p.FCompanyId == CurrentUser.AccountComId);
                string orgName;
                if (baseOrgnization == null)
                {
                    base_company company = (new CompanyService()).FirstOrDefault(p => p.id == OrgnizationId //
                        && p.id == CurrentUser.AccountComId);
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

        private EmployeeService _employeeService;
        /// <summary>
        /// 
        /// </summary>
        protected EmployeeService EmployeeService
        {
            get { return _employeeService ?? (_employeeService = new EmployeeService()); }
            set { _employeeService = value; }
        }


        private base_employee _baseEmployee;
        /// <summary>
        /// 
        /// </summary>
        protected base_employee BaseEmployee
        {
            get { return _baseEmployee ?? (_baseEmployee = EmployeeService.FirstOrDefault(p => p.id == Key && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _baseEmployee = value; }
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

                        LogUtil.WriteLog("职工管理", CurrentUser.AccountNumber, string.Format(@"添加职工成功！"));


                        break;
                    case WebAction.Edit:
                        isSucceed = SubmintEdit();

                        LogUtil.WriteLog("职工管理", CurrentUser.AccountNumber, string.Format(@"修改职工成功！"));
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
            if (BaseEmployee != null)
            {
                BaseEmployee.name = txtname.Text.Trim();
                if (!string.IsNullOrWhiteSpace(txtage.Text.Trim()))
                    BaseEmployee.age = Convert.ToInt32(txtage.Text.Trim());
                BaseEmployee.email = txtemail.Text.Trim();
                BaseEmployee.education = Convert.ToInt32(ddleducation.SelectedValue);
                BaseEmployee.flag = txtflag.Checked ? 1 : 2;
                BaseEmployee.flag_cause = txtflag_cause.Text.Trim();
                BaseEmployee.flag_date = dpflag_date.SelectedDate;
                BaseEmployee.bank_class = Convert.ToInt32(ddlbank_class.SelectedValue);
                BaseEmployee.birthday = dpbirthday.SelectedDate;
                BaseEmployee.flag_direction = txtflag_direction.Text.Trim();
                BaseEmployee.identity_card = txtidentity_card.Text.Trim();
                BaseEmployee.office_phone = txtoffice_phone.Text.Trim();
                BaseEmployee.office_fax = txtoffice_fax.Text.Trim();
                BaseEmployee.code_cn = ChineseSpell.MakeSpellCode(txtname.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper();
                BaseEmployee.graduate_institutions = txtgraduate_institutions.Text.Trim();
                BaseEmployee.major = txtmajor.Text.Trim();
                BaseEmployee.sex = ddlaccount_sex.SelectedValue;
                BaseEmployee.office_address = txtoffice_address.Text.Trim();
                BaseEmployee.salary_bank = txtsalary_bank.Text.Trim();
                BaseEmployee.job_date = dpjob_date.SelectedDate;
                BaseEmployee.professional = GasHelper.GetDropDownListArrayString(ddlprofessional.SelectedItemArray);

                BaseEmployee.orgnization_id = Convert.ToInt32(ddlOrgnization.SelectedValue);
                BaseEmployee.FLogistics = ddlFLogistics.SelectedValue;//物流
                BaseEmployee.FDistributionPoint = GasHelper.GetDropDownListArrayString(ddlFDistributionPoint.SelectedItemArray);
                BaseEmployee.FNo = txtFNo.Text.Trim();


                return EmployeeService.SaveChanges() >= 0;
            }



            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var sequence = new SequenceService().GH("GH", CurrentUser.AccountComId);
            var numberAuto = sequence.Split('-')[2];

            var account = new base_employee
            {
                graduate_institutions = txtgraduate_institutions.Text.Trim(),
                major = txtmajor.Text.Trim(),
                //orgnization_id = OrgnizationId,
                name = txtname.Text.Trim(),
                sex = ddlaccount_sex.SelectedValue,
                number = numberAuto,
                age = string.IsNullOrWhiteSpace(txtage.Text.Trim()) ? 0 : Convert.ToInt32(txtage.Text.Trim()),
                email = txtemail.Text.Trim(),
                education = Convert.ToInt32(ddleducation.SelectedValue),
                flag = txtflag.Checked ? 1 : 2,
                flag_cause = txtflag_cause.Text.Trim(),
                flag_date = dpflag_date.SelectedDate,
                bank_class = Convert.ToInt32(ddlbank_class.SelectedValue),
                birthday = dpbirthday.SelectedDate,
                flag_direction = txtflag_direction.Text.Trim(),
                identity_card = txtidentity_card.Text.Trim(),
                office_phone = txtoffice_phone.Text.Trim(),
                office_fax = txtoffice_fax.Text.Trim(),
                code_cn = ChineseSpell.MakeSpellCode(txtname.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper(),
                office_s_phone = txtoffice_s_phone.Text.Trim(),
                office_tel = txtoffice_tel.Text.Trim(),
                job_number = numberAuto,
                profession = txtprofession.Text.Trim(),
                qq = txtqq.Text.Trim(),
                salary_bank = txtsalary_bank.Text.Trim(),
                zip_code = txtzip_code.Text.Trim(),
                job_nature = Convert.ToInt32(ddljob_nature.SelectedValue),
                office_address = txtoffice_address.Text.Trim(),
                job_date = dpjob_date.SelectedDate ?? DateTime.Now,
                professional = GasHelper.GetDropDownListArrayString(ddlprofessional.SelectedItemArray),
                deleteflag = 0,
                FCompanyId = CurrentUser.AccountComId,
                FFlag = 1,
                orgnization_id = Convert.ToInt32(ddlOrgnization.SelectedValue),
                FLogistics = ddlFLogistics.SelectedValue,//物流
                FDistributionPoint = GasHelper.GetDropDownListArrayString(ddlFDistributionPoint.SelectedItemArray),
                FNo=txtFNo.Text.Trim(),


            };

            //if (!string.IsNullOrEmpty(BaseEmployee.FDistributionPoint))
            //    ddlFDistributionPoint.SelectedValueArray = BaseEmployee.FDistributionPoint.Split(',');

            return EmployeeService.Add(account);
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            ddleducation.DataSource =
                new DictionaryService().Where(p => p.deleteflag == 0 && p.category == "文化程度");
            ddleducation.DataBind();

            ddlprofessional.DataSource =
                new DictionaryService().Where(p => p.deleteflag == 0 && p.category == "职位管理");
            ddlprofessional.DataBind();

            ddljob_nature.DataSource =
               new DictionaryService().Where(p => p.deleteflag == 0 && p.category == "用工性质");
            ddljob_nature.DataBind();

            ddlbank_class.DataSource =
              new DictionaryService().Where(p => p.deleteflag == 0 && p.category == "银行名称");
            ddlbank_class.DataBind();

            GasHelper.DropDownListOrgnizationDataBind(ddlOrgnization);

            GasHelper.DropDownListLogisticsDataBind(ddlFLogistics);//物流公司

            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint);
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    ddlOrgnization.SelectedValue = OrgnizationId.ToString();
                    //lblorgnization_name.Text = OrgnizationName;
                    break;
                case WebAction.Edit:
                    if (BaseEmployee != null)
                    {
                        dpjob_date.SelectedDate = BaseEmployee.job_date;
                        txtmajor.Text = BaseEmployee.major;

                        if (BaseEmployee.base_orgnization != null)
                        {

                            ddlOrgnization.SelectedValue = BaseEmployee.orgnization_id.ToString();
                            //lblorgnization_name.Text = BaseEmployee.base_orgnization.org_name;
                        }
                        
                        txtname.Text = BaseEmployee.name;
                        txtage.Text = BaseEmployee.age.ToString();
                        if (BaseEmployee.birthday != null)
                            dpbirthday.SelectedDate = BaseEmployee.birthday;
                        txtoffice_address.Text = BaseEmployee.office_address;
                        txtoffice_fax.Text = BaseEmployee.office_fax;
                        txtoffice_phone.Text = BaseEmployee.office_phone;
                        txtoffice_s_phone.Text = BaseEmployee.office_s_phone;
                        txtprofession.Text = BaseEmployee.profession;
                        txtoffice_tel.Text = BaseEmployee.office_tel;
                        txtemail.Text = BaseEmployee.email;
                        txtjob_number.Text = BaseEmployee.job_number;
                        txtidentity_card.Text = BaseEmployee.identity_card;
                        txtflag_direction.Text = BaseEmployee.flag_direction;
                        txtflag_cause.Text = BaseEmployee.flag_cause;
                        txtprofession.Text = BaseEmployee.profession;
                        txtqq.Text = BaseEmployee.qq;
                        txtsalary_bank.Text = BaseEmployee.salary_bank;
                        txtzip_code.Text = BaseEmployee.zip_code;
                        txtflag.Checked = BaseEmployee.flag == 1;
                        if (BaseEmployee.sex != null)
                            ddlaccount_sex.SelectedValue = BaseEmployee.sex;
                        if (BaseEmployee.bank_class != null)
                            ddlbank_class.SelectedValue = BaseEmployee.bank_class.ToString();
                        if (BaseEmployee.education != null)
                            ddleducation.SelectedValue = BaseEmployee.education.ToString();
                        if (BaseEmployee.job_nature != null)
                            ddljob_nature.SelectedValue = BaseEmployee.job_nature.ToString();

                        txtgraduate_institutions.Text = BaseEmployee.graduate_institutions;
                        txtmajor.Text = BaseEmployee.major;
                        txtFNo.Text = BaseEmployee.FNo;
                        dpflag_date.SelectedDate = BaseEmployee.flag_date;
                        ddlFLogistics.SelectedValue = BaseEmployee.FLogistics;

                        if (!string.IsNullOrEmpty(BaseEmployee.professional))
                            ddlprofessional.SelectedValueArray = BaseEmployee.professional.Split(',');

                        if (!string.IsNullOrEmpty(BaseEmployee.FDistributionPoint))
                            ddlFDistributionPoint.SelectedValueArray = BaseEmployee.FDistributionPoint.Split(',');
                    }
                    break;
            }
        }
        #endregion
    }
}