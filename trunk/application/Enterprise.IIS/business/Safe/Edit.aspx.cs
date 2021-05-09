using System;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Enterprise.IIS.Common;

namespace Enterprise.IIS.business.Safe
{
    public partial class Edit : PageBase
    {
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

        private BillSafeCheckServie _safeCheckServie;
        /// <summary>
        /// 
        /// </summary>
        protected BillSafeCheckServie SafeCheckServie
        {
            get { return _safeCheckServie ?? (_safeCheckServie = new BillSafeCheckServie()); }
            set { _safeCheckServie = value; }
        }

        private LHSafeCheck _safeCheck;
        /// <summary>
        /// 
        /// </summary>
        protected LHSafeCheck SafeCheck
        {
            get { return _safeCheck ?? (_safeCheck = SafeCheckServie.FirstOrDefault(p => p.FId == Key)); }
            set { _safeCheck = value; }
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
            if (SafeCheck != null)
            {
                SafeCheck.FOrderNo = txtOrderCode.Text.Trim();
                SafeCheck.FCheckDate = dpCheckDate.SelectedDate;
                SafeCheck.FTypeNum = ddlTypeNum.SelectedValue;
                SafeCheck.FReformDate = dpReformDate.SelectedDate;
                SafeCheck.FContext = txtContext.Text.Trim();
                SafeCheck.FReason = txtReason.Text.Trim();
                SafeCheck.FMarkContext = txtMarkContext.Text.Trim();
                SafeCheck.FFunds = string.IsNullOrWhiteSpace(txtFunds.Text.Trim()) ? 0M : Convert.ToDecimal(txtFunds.Text.Trim());
                SafeCheck.FDept = ddlDept.SelectedValue;
                SafeCheck.FPerson = ddlPerson.SelectedValue;
                SafeCheck.Updateby = CurrentUser.AccountName;
                SafeCheck.Updateon = DateTime.Now;
            };
            return SafeCheckServie.SaveChanges() >= 0;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var check = new LHSafeCheck
            {
                FOrderNo = txtOrderCode.Text.Trim(),
                FCheckDate = dpCheckDate.SelectedDate,
                FTypeNum = ddlTypeNum.SelectedValue,
                FReformDate = dpReformDate.SelectedDate,
                FContext = txtContext.Text.Trim(),
                FReason = txtReason.Text.Trim(),
                FMarkContext = txtMarkContext.Text.Trim(),
                FFunds = string.IsNullOrWhiteSpace(txtFunds.Text.Trim()) ? 0M : Convert.ToDecimal(txtFunds.Text.Trim()),
                FDept = ddlDept.SelectedValue,
                FPerson = ddlPerson.SelectedValue,
                Createon = DateTime.Now,
                Createby = CurrentUser.AccountName,
                FDeleteFlag = 0
            };
            return SafeCheckServie.Add(check);
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            ViewState["_AppendToEnd"] = true;

            GasHelper.DropDownListSurveyorDataBind(ddlPerson);
            
            //GasHelper.DropDownListDepeDataBind(ddlDept);
            
            GasHelper.DropDownListCheckDataBind(ddlTypeNum);

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
                    if (SafeCheck != null)
                    {
                        txtOrderCode.Text = SafeCheck.FOrderNo;
                        dpCheckDate.SelectedDate = SafeCheck.FCheckDate;
                        ddlTypeNum.SelectedValue = SafeCheck.FTypeNum;
                        dpReformDate.SelectedDate = SafeCheck.FReformDate;
                        txtContext.Text = SafeCheck.FContext;
                        txtReason.Text = SafeCheck.FReason;
                        txtMarkContext.Text = SafeCheck.FMarkContext;
                        txtFunds.Text = SafeCheck.FFunds + "";
                        ddlDept.SelectedValue = SafeCheck.FDept;
                        ddlPerson.SelectedValue = SafeCheck.FPerson;
                    }
                    break;
            }
        }
        #endregion
    }
}