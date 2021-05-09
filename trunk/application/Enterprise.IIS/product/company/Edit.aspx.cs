using System;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;

namespace Enterprise.IIS.product.company
{
    public partial class Edit : PageBase
    {
        /// <summary>
        ///     部门组织信息
        /// </summary>
        private base_company _baseOrgnization;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        private CompanyService _orgnizationService;

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
                return (WebAction) int.Parse(s);
            }
        }

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        protected CompanyService OrgnizationService
        {
            get { return _orgnizationService ?? (_orgnizationService = new CompanyService()); }
            set { _orgnizationService = value; }
        }

        /// <summary>
        ///     部门名称
        /// </summary>
        protected string POrgnizationName
        {
            get
            {
                var baseOrgnization = OrgnizationService.FirstOrDefault(p => p.id == POrgnizationId);
                string orgName;
                if (baseOrgnization == null)
                {
                    var company = OrgnizationService.FirstOrDefault(p => p.id == POrgnizationId);
                    orgName = company.com_name;
                }
                else
                {
                    orgName = baseOrgnization.com_name;
                }
                return orgName;
            }
        }

        /// <summary>
        /// 当前选中的部门信息
        /// </summary>
        protected base_company POrgnization
        {
            get
            {
                var baseOrgnization = OrgnizationService.FirstOrDefault(p => p.id == POrgnizationId);
                if (baseOrgnization == null)
                {
                    return null;
                }
                return baseOrgnization;
            }
        }

        /// <summary>
        ///    信息    
        /// </summary>
        protected base_company BaseOrgnization
        {
            get
            {
                return _baseOrgnization ??
                       (_baseOrgnization = OrgnizationService.FirstOrDefault(p => p.id == OrgnizationId));
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
                BaseOrgnization.com_name = txtcom_name.Text.Trim();
                BaseOrgnization.com_code = txtcom_code.Text.Trim();
                BaseOrgnization.com_person = txtcom_person.Text.Trim();
                BaseOrgnization.com_desc = txtcom_desc.Text.Trim();
                BaseOrgnization.FMonthly = ddlMonth.SelectedValue;
                BaseOrgnization.FMonthlyDay = Convert.ToInt32(txtMonthDay.Text);
                BaseOrgnization.FAddress = txtFAddress.Text.Trim();
                BaseOrgnization.com_tel = txtcom_tel.Text.Trim();


                return OrgnizationService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var catecode = POrgnizationId == 1 ? "1" : string.Format(@"{0}.{1}", POrgnization.com_name, POrgnizationId);

            var orgnization = new base_company
            {
                com_code = txtcom_code.Text.Trim(),
                com_name = txtcom_name.Text.Trim(),
                com_desc = txtcom_desc.Text.Trim(),
                com_person = txtcom_person.Text.Trim(),
                deleteflag = 0
            };

            orgnization.FMonthly = ddlMonth.SelectedValue;
            orgnization.FMonthlyDay = Convert.ToInt32(txtMonthDay.Text);
            orgnization.prentid = POrgnizationId;
            orgnization.createdon = DateTime.Now;
            orgnization.FAddress = txtFAddress.Text.Trim();
            orgnization.com_tel = txtcom_tel.Text.Trim();


            return OrgnizationService.Add(orgnization);
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMonth.SelectedValue.Equals("否"))
            {
                txtMonthDay.Hidden = false;
            }
            else
            {
                txtMonthDay.Hidden = true;
            }
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

                    txtcom_code.Text = POrgnizationId == 1 ? "" : string.Format(@"{0}.", POrgnization.com_code);


                    break;
                case WebAction.Edit:
                    if (BaseOrgnization != null)
                    {
                        txtcom_code.Text = BaseOrgnization.com_code.Trim();
                        txtcom_name.Text = BaseOrgnization.com_name.Trim();
                        txtcom_person.Text = BaseOrgnization.com_person.Trim();
                        txtcom_desc.Text = BaseOrgnization.com_desc.Trim();
                        ddlMonth.SelectedValue = BaseOrgnization.FMonthly;
                        txtMonthDay.Text = BaseOrgnization.FMonthlyDay.ToString();


                        txtFAddress.Text=BaseOrgnization.FAddress ;
                        txtcom_tel.Text=BaseOrgnization.com_tel ;
                    }
                    break;
            }
        }

        #endregion
    }
}