using Enterprise.Data;
using Enterprise.Framework.Extension;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;
using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enterprise.IIS.business.Customer
{
    public partial class Edit : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerOrgService _customerOrgService;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected CustomerOrgService CustomerOrgService
        {
            get { return _customerOrgService ?? (_customerOrgService = new CustomerOrgService()); }
            set { _customerOrgService = value; }
        }

        /// <summary>
        ///     客户档案
        /// </summary>
        private LHCustomer _member;

        /// <summary>
        ///     区域
        /// </summary>
        private ProjectItemsService _projectItemsService;

        /// <summary>
        ///     区域
        /// </summary>
        protected ProjectItemsService ProjectItemsService
        {
            get { return _projectItemsService ?? (_projectItemsService = new ProjectItemsService()); }
            set { _projectItemsService = value; }
        }

        /// <summary>
        ///     职员档案
        /// </summary>
        protected LHCustomer Customer
        {
            get { return _member ?? (_member = CustomerService.FirstOrDefault(p => p.FCode == FCode && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _member = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string FCode
        {
            get { return Request["FCode"]; }
        }

        /// <summary>
        ///     FSubCateId
        /// </summary>
        protected string FSubCateId
        {
            get { return Request["FSubCateId"]; }
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

        protected string SubMessage = "提交失败";

        #region Protected Method
        /// <summary>
        ///     TabStrip1_TabIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabStrip1_TabIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///     btnSubmit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    Alert.Show(SubMessage, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<int> selectIds = GetSelectIds();

            try
            {
                CustomerOrgService.Delete(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.FId));
                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //tbxFName.Text = string.Empty;
                //tbxFFLinkman.Text = string.Empty;
                //tbxFMemo.Text = string.Empty;
                //tbxFAddress.Text = string.Empty;
                //tbxFPhone.Text = string.Empty;

                //验证该公司下是否已有该部门存在
                if (string.IsNullOrEmpty(tbxFName.Text.Trim()))
                {
                    Alert.Show("请输入有效部门！", MessageBoxIcon.Information);
                    return;
                }

                LHCustomerOrg orginfo = CustomerOrgService.Where(p => p.FCompanyId == CurrentUser.AccountComId && p.FName == tbxFName.Text.Trim()).FirstOrDefault();

                if (orginfo != null)
                {
                    Alert.Show("该部门已存在，无需再增加该部门信息！", MessageBoxIcon.Information);
                    return;
                }

                LHCustomerOrg org = new LHCustomerOrg
                {
                    FName = tbxFName.Text.Trim(),
                    FAddress = tbxFAddress.Text.Trim(),
                    FPhone = tbxFPhone.Text.Trim(),
                    FCompanyId = CurrentUser.AccountComId,
                    FCode = FCode,
                    FMemo = tbxFMemo.Text.Trim(),
                    FFLinkman = tbxFFLinkman.Text.Trim(),
                };

                CustomerOrgService.Add(org);

                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("新增失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
            }
            else if (Grid1.SelectedRowIndexArray.Length > 1)
            {
                Alert.Show("只能选择一项！", MessageBoxIcon.Information);
            }
            else
            {
                int sid = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);

                LHCustomerOrg org = CustomerOrgService.Where(p => p.FCompanyId == CurrentUser.AccountComId && p.FId == sid).FirstOrDefault();

                if (org != null)
                {
                    tbxFAddress.Text = org.FAddress;
                    tbxFFLinkman.Text = org.FFLinkman;
                    tbxFMemo.Text = org.FMemo;
                    tbxFName.Text = org.FName;
                    tbxFPhone.Text = org.FPhone;
                    tbxFId.Text = org.FId.ToString();
                }
            }
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int sid = Convert.ToInt32(tbxFId.Text.Trim());

            LHCustomerOrg org = CustomerOrgService.Where(p => p.FCompanyId == CurrentUser.AccountComId && p.FId == sid).FirstOrDefault();

            if (org != null)
            {
                org.FAddress = tbxFAddress.Text.Trim();
                org.FFLinkman = tbxFFLinkman.Text.Trim();
                org.FMemo = tbxFMemo.Text.Trim();
                org.FName = tbxFName.Text.Trim();
                org.FPhone = tbxFPhone.Text.Trim();

                CustomerOrgService.SaveChanges();

                Alert.Show("保存成功！", MessageBoxIcon.Information);

                BindDataGrid();
            }
        }


        #region 省市县联动
        /// <summary>
        ///     市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFCity.Items.Clear();

            ProvinceService service = new ProvinceService();

            int id = Convert.ToInt32(ddlFProvince.SelectedValue);

            ddlFCity.DataSource = service.Where(p => p.p_code == id);
            ddlFCity.DataTextField = "city_name";//省
            ddlFCity.DataValueField = "id";//"id"
            ddlFCity.DataBind();

            ddlFCity.Items.Insert(0, new ListItem(@"", "-1"));

            //区域
            ddlFCounty.Items.Clear();
        }

        protected void ddlFCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFCounty.Items.Clear();

            ProvinceService service = new ProvinceService();
            int id = Convert.ToInt32(ddlFCity.SelectedValue);

            ddlFCounty.DataSource = service.Where(p => p.p_code == id);
            ddlFCounty.DataTextField = "city_name";//省
            ddlFCounty.DataValueField = "id";//"id"
            ddlFCounty.DataBind();

            ddlFCounty.Items.Insert(0, new ListItem(@"", "-1"));
        }

        #endregion

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (Customer != null)
            {
                Customer.FCode = txtFCode.Text.Trim();
                Customer.FName = txtFName.Text.Trim();
                Customer.FAddress = txtFAddress.Text.Trim();
                Customer.FLinkman = txtFLinkman.Text.Trim();
                Customer.FPhome = txtFPhome.Text.Trim();
                Customer.FMoile = txtFMoile.Text.Trim();
                Customer.FMemo = txtFMemo.Text.Trim();
                Customer.FIsPrint = ddlFIsPrint.SelectedValue.Equals("是") ? 1 : 0;
                Customer.FTipsDay = string.IsNullOrEmpty(txtFTipsDay.Text.Trim()) ? 0 : Convert.ToInt32(txtFTipsDay.Text.Trim());
                Customer.FFreight = string.IsNullOrEmpty(txtFFreight.Text.Trim()) ? 0 : Convert.ToDecimal(txtFFreight.Text.Trim());
                Customer.FCredit = string.IsNullOrEmpty(txtFCredit.Text.Trim()) ? 0 : Convert.ToDecimal(txtFCredit.Text.Trim());
                Customer.FSpell = ChineseSpell.MakeSpellCode(txtFName.Text.Trim(), "",
                    SpellOptions.FirstLetterOnly).ToUpper();
                Customer.FFlag = 1;
                Customer.FSalesman = ddlFSalesman.SelectedValue;//业务员
                Customer.FDistric = ddlDistric.SelectedValue;//区域
                Customer.FPaymentMethod = ddlFPaymentMethod.SelectedValue;
                Customer.FCompanyId = CurrentUser.AccountComId;
                Customer.FDate = txtFDate.SelectedDate;
                Customer.FGroupNo = txtFGroupNo.Text;
                Customer.FGroupNoFlag = ddlFGroupNoFlag.SelectedValue;
                Customer.FPushFlag = ddlFPushFlag.SelectedValue;
                Customer.FCoordinate = txtFCoordinate.Text.Trim();
                Customer.FLevel = ddlFGroupNoFlag.SelectedValue.Equals("是") ? 0 : 1;

                //Customer.FProvince = ddlFProvince.SelectedValue;
                //Customer.FCity = ddlFCity.SelectedValue;
                //Customer.FCounty = ddlFCounty.SelectedValue;
                Customer.FZipCode = txtFZipCode.Text;

                if (!string.IsNullOrEmpty(ddlFProvince.SelectedValue))
                {
                    Customer.FProvinceId = Convert.ToInt32(ddlFProvince.SelectedValue);
                    Customer.FProvince = ddlFProvince.SelectedText;
                }
                else
                {
                    Customer.FProvinceId = -1;
                    Customer.FProvince = "";
                }
                if (!string.IsNullOrEmpty(ddlFCity.SelectedValue))
                {
                    Customer.FCityId = Convert.ToInt32(ddlFCity.SelectedValue);
                    Customer.FCity = ddlFCity.SelectedText;
                }
                else
                {
                    Customer.FCityId = -1;
                    Customer.FCity = "";
                }
                if (!string.IsNullOrEmpty(ddlFCounty.SelectedValue))
                {
                    Customer.FCountyId = Convert.ToInt32(ddlFCounty.SelectedValue);
                    Customer.FCounty = ddlFCounty.SelectedText;
                }
                else
                {
                    Customer.FCountyId = -1;
                    Customer.FCounty = "";
                }
                return CustomerService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {

            try
            {
                LHCustomer customer = CustomerService.Where(p => p.FCode == txtFCode.Text.Trim()).FirstOrDefault();
                if (customer != null)
                {
                    SubMessage = "客户代码重复";
                    return false;
                }
                else
                {
                    LHCustomer member = new LHCustomer();
                    {
                        member.FCode = txtFCode.Text.Trim();

                        member.FGroupNo = txtFCode.Text.Trim();
                        member.FGroupNoFlag = "1";
                        member.FLevel = 0;

                        member.FName = txtFName.Text.Trim();
                        member.FAddress = txtFAddress.Text.Trim();
                        member.FLinkman = txtFLinkman.Text.Trim();
                        member.FPhome = txtFPhome.Text.Trim();
                        member.FMoile = txtFMoile.Text.Trim();
                        member.FMemo = txtFMemo.Text.Trim();
                        member.FIsPrint = ddlFIsPrint.SelectedValue.Equals("是") ? 1 : 0;
                        member.FFreight = string.IsNullOrEmpty(txtFFreight.Text.Trim())
                            ? 0
                            : Convert.ToDecimal(txtFFreight.Text.Trim());
                        member.FCredit = string.IsNullOrEmpty(txtFCredit.Text.Trim())
                            ? 0
                            : Convert.ToDecimal(txtFCredit.Text.Trim());
                        member.FSpell = ChineseSpell.MakeSpellCode(txtFName.Text.Trim(), "",
                            SpellOptions.FirstLetterOnly).ToUpper();
                        member.FFlag = 1;
                        member.FSalesman = ddlFSalesman.SelectedValue;//业务员
                        member.FDistric = ddlDistric.SelectedValue;//区域
                        member.FPaymentMethod = ddlFPaymentMethod.SelectedValue;
                        member.FCompanyId = CurrentUser.AccountComId;
                        member.FCateId = "2077";
                        member.FSubCateId = FSubCateId;
                        member.FDate = txtFDate.SelectedDate;
                        member.FIsAllot = 0;
                        member.FGroupNo = txtFGroupNo.Text;
                        member.FGroupNoFlag = ddlFGroupNoFlag.SelectedValue;
                        member.FPushFlag = ddlFPushFlag.SelectedValue;
                        member.FCoordinate = txtFCoordinate.Text.Trim();
                        member.FCreditFlag = "通过";
                        if (!string.IsNullOrEmpty(ddlFProvince.SelectedValue))
                        {
                            member.FProvinceId = Convert.ToInt32(ddlFProvince.SelectedValue);
                            member.FProvince = ddlFProvince.SelectedText;
                        }
                        else
                        {
                            member.FProvinceId = -1;
                            member.FProvince = "";
                        }

                        if (!string.IsNullOrEmpty(ddlFCity.SelectedValue))
                        {
                            member.FCityId = Convert.ToInt32(ddlFCity.SelectedValue);
                            member.FCity = ddlFCity.SelectedText;
                        }
                        else
                        {
                            member.FCityId = -1;
                            member.FCity = "";
                        }
                        if (!string.IsNullOrEmpty(ddlFCounty.SelectedValue))
                        {
                            member.FCountyId = Convert.ToInt32(ddlFCounty.SelectedValue);
                            member.FCounty = ddlFCounty.SelectedText;
                        }
                        else
                        {
                            member.FCountyId = -1;
                            member.FCounty = "";
                        }

                        member.FZipCode = txtFZipCode.Text;
                    };

                    member.FLevel = ddlFGroupNoFlag.SelectedValue.Equals("是") ? 0 : 1;

                    return CustomerService.Add(member);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            btnClose2.OnClientClick = ActiveWindow.GetHidePostBackReference();

            //区域
            GasHelper.DropDownListDistricDataBind(ddlDistric);

            //是否需要开票
            GasHelper.DropDownListDataBind(ddlFIsPrint, "1006");

            //结算方式
            GasHelper.DropDownListDataBind(ddlFPaymentMethod, "1005");

            //业务员
            GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

            //省
            GasHelper.DropDownListProvinceDataBind(ddlFProvince);

        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    //var parms = new Dictionary<string, object>();
                    //parms.Clear();

                    //parms.Add("@companyid",CurrentUser.AccountComId);
                    //parms.Add("@type", "Customer");//气体                    
                    //var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                    //txtFCode.Text = list.Rows[0][0].ToString();
                    break;
                case WebAction.Edit:

                    txtFCode.Readonly = true;


                    if (Customer != null)
                    {
                        txtFCode.Text = Customer.FCode;
                        txtFName.Text = Customer.FName;
                        txtFAddress.Text = Customer.FAddress;
                        txtFLinkman.Text = Customer.FLinkman;
                        txtFPhome.Text = Customer.FPhome;
                        txtFMoile.Text = Customer.FMoile;
                        txtFMemo.Text = Customer.FMemo;
                        //ddlFIsPrint.SelectedValue=Customer.FIsPrint.ToString();
                        txtFTipsDay.Text = Customer.FTipsDay.ToString();
                        txtFFreight.Text = Customer.FFreight.ToString();
                        txtFCredit.Text = Customer.FCredit.ToString();
                        //Customer.FFlag = 1;
                        ddlFSalesman.SelectedValue = Customer.FSalesman;//业务员
                        ddlDistric.SelectedValue = Customer.FDistric;//区域
                        ddlFPaymentMethod.SelectedValue = Customer.FPaymentMethod;
                        //CurrentUser.AccountComId=Customer.FCompanyId ;
                        txtFDate.SelectedDate = Customer.FDate;
                        ddlFIsPrint.SelectedValue = Customer.FIsPrint == 1 ? "是" : "否";

                        ddlFRebateFlag.SelectedValue = Customer.FRebateFlag;
                        ddlFPushFlag.SelectedValue = Customer.FPushFlag;
                        ddlFMonthly.SelectedValue = Customer.FMonthly;
                        txtFMonthlyDay.Text = Customer.FMonthlyDay.ToString();
                        txtFGroupNo.Text = Customer.FGroupNo;
                        ddlFGroupNoFlag.SelectedValue = Customer.FGroupNoFlag;
                        txtFZipCode.Text = Customer.FZipCode;
                        txtFCoordinate.Text = Customer.FCoordinate;

                        BindDataGrid();

                        ddlFProvince.SelectedValue = Customer.FProvinceId.ToString();

                        if (Customer.FProvince != null)
                        {
                            ProvinceService service = new ProvinceService();
                            int id = Convert.ToInt32(ddlFProvince.SelectedValue);

                            ddlFCity.DataSource = service.Where(p => p.p_code == id);
                            ddlFCity.DataTextField = "city_name";//省
                            ddlFCity.DataValueField = "id";//"id"
                            ddlFCity.DataBind();

                            ddlFCity.SelectedValue = Customer.FCityId.ToString();

                            if (Customer.FCity != null)
                            {
                                int countyId = Convert.ToInt32(ddlFCity.SelectedValue);

                                ddlFCounty.DataSource = service.Where(p => p.p_code == countyId);
                                ddlFCounty.DataTextField = "city_name";//省
                                ddlFCounty.DataValueField = "id";//"id"
                                ddlFCounty.DataBind();

                                ddlFCounty.SelectedValue = Customer.FCountyId.ToString();
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        ///     
        /// </summary>
        private void BindDataGrid()
        {
            IQueryable<LHCustomerOrg> list = CustomerOrgService.Where(p => p.FCode == txtFCode.Text.Trim());// && p.FCompanyId == CurrentUser.AccountComId

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            int[] selectIds = new int[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Convert.ToInt32(Grid1.DataKeys[selections[i]][0]);
            }
            return selectIds;
        }
        #endregion


    }
}