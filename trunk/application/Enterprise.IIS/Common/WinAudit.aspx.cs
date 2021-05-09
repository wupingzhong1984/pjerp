using System;
using System.Collections.Generic;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.Common
{
    public partial class WinAudit : PageBase
    {

        /// <summary>
        ///     数据服务
        /// </summary>
        private BillFlowServie _billFlowServie;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected BillFlowServie BillFlowServie
        {
            get { return _billFlowServie ?? (_billFlowServie = new BillFlowServie()); }
            set { _billFlowServie = value; }
        }

        /// <summary>
        ///     KeyId
        /// </summary>
        protected string KeyId
        {
            get { return Request["KeyId"]; }
        }

        /// <summary>
        ///     单据类型
        /// </summary>
        protected string Bill
        {
            get { return Request["Bill"]; }
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

        /// <summary>
        ///     是否转审
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProcessMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormRow4.Hidden = !ddlProcessMode.SelectedValue.Equals("转审");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                isSucceed = SubmintAdd();
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

        private string Url(string billType)
        {
            string url = string.Empty;
            switch (billType)
            {
                case "45"://采购请申请单
                    return string.Format(@"business/PurchaseApp/Audit.aspx?KeyId={0}&action=2", KeyId);
            }

            return string.Empty;
        }




        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var flow = new LHBillFlow
            {
                KeyId = KeyId,
                FCompanyId = CurrentUser.AccountComId,
                FDeptId = CurrentUser.AccountOrgId,
                FOperator = CurrentUser.AccountName,
                FDate = Convert.ToDateTime(dptDate.SelectedDate),

                //审核结果
                FResult = ddlProcessMode.SelectedValue,

                FMemo = txtFMemo.Text.Trim()
            };

            if (ddlProcessMode.SelectedValue.Equals("转审"))
            {
                flow.FNextAudiNum = ddlAuditor.SelectedValue;
                flow.FNextAuditName = ddlAuditor.SelectedText;
                flow.FUrl = Url(Bill);
            }

            flow.FFlag = 0;

            BillFlowServie.Add(flow);

            //审核
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@KeyId", KeyId);
            parms.Add("@BillType", Bill);
            parms.Add("@FCompanyId", CurrentUser.AccountComId);
            parms.Add("@FNum",CurrentUser.AccountJobNumber);

            SqlService.ExecuteProcedureCommand("proc_Audit", parms);

            return true;
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            //审核员
            GasHelper.DropDownListAuditorsDataBind(ddlAuditor);

            dptDate.SelectedDate = DateTime.Now;

            lblKeyId.Text = KeyId;
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            FormRow4.Hidden = true;
        }

        #endregion
    }
}