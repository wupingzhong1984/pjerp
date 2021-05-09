using System;
using System.Collections.Generic;
using System.Globalization;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.Liquid
{
    public partial class LiquidTo : PageBase
    {
        /// <summary>
        ///     瓶气
        /// </summary>
        protected string KeyId
        {
            get { return Request["KeyId"]; }
        }

        protected decimal Qty
        {
            get { return Convert.ToDecimal(Request["qty"]); }
        }


        /// <summary>
        ///     数据服务
        /// </summary>
        private LiquidToService _liquidToService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LiquidToService LiquidToService
        {
            get { return _liquidToService ?? (_liquidToService = new LiquidToService()); }
            set { _liquidToService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHLiquidTo _liquidTo;

        /// <summary>
        ///     
        /// </summary>
        protected LHLiquidTo To
        {
            get
            {
                return _liquidTo ?? (_liquidTo = LiquidToService.FirstOrDefault(p => p.XKeyId == KeyId //
                    && p.XCompanyId == CurrentUser.AccountComId));
            }
            set { _liquidTo = value; }
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

        protected void ddlBill_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbxFVehicleNum.Hidden = !ddlBill.SelectedValue.Equals("导车");
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (To != null)
            {
                To.XBill = ddlBill.SelectedValue;

                if (!string.IsNullOrEmpty(tbxFVehicleNum.Text))
                {
                    To.XVehicleNum = tbxFVehicleNum.Text;
                    To.XTo = tbxFVehicleNum.Text;
                }

                if (!string.IsNullOrEmpty(txtFMarginEnd.Text))
                    To.XMarginEnd = string.IsNullOrEmpty(txtFMarginEnd.Text) ? 0 : Convert.ToDecimal(txtFMarginEnd.Text);

                if (!string.IsNullOrEmpty(txtFQty.Text))
                    To.XQty = string.IsNullOrEmpty(txtFQty.Text) ? 0 : Convert.ToDecimal(txtFQty.Text);

                To.XMemo = txtFMemo.Text;

                To.XFDate = DateTime.Now;

                To.XCreateBy = CurrentUser.AccountName;

                LiquidToService.SaveChanges();


                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", CurrentUser.AccountComId);
                parms.Add("@keyid", KeyId);

                SqlService.ExecuteProcedureCommand("proc_LiquidMargin", parms);

                var parmsTo = new Dictionary<string, object>();
                parmsTo.Clear();
                parmsTo.Add("@companyId", CurrentUser.AccountComId);
                parmsTo.Add("@KeyId", KeyId);

                SqlService.ExecuteProcedureCommand("proc_LiquidTo", parmsTo);

                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var its = new LHLiquidTo
            {
                XKeyId = KeyId,
                XCompanyId = CurrentUser.AccountComId,
                XBill = ddlBill.SelectedValue
            };

            if (!string.IsNullOrEmpty(tbxFVehicleNum.SelectedValue))
            {
                its.XTo = tbxFVehicleNum.SelectedValue;
            }

            if (!string.IsNullOrEmpty(txtFMarginEnd.Text))
                its.XMarginEnd = string.IsNullOrEmpty(txtFMarginEnd.Text) ? 0 : Convert.ToDecimal(txtFMarginEnd.Text);

            if (!string.IsNullOrEmpty(txtFQty.Text))
                its.XQty = string.IsNullOrEmpty(txtFQty.Text) ? 0 : Convert.ToDecimal(txtFQty.Text);

            its.XMemo = txtFMemo.Text;

            LiquidToService.Add(its);


            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@companyId", CurrentUser.AccountComId);
            parms.Add("@keyid", KeyId);

            SqlService.ExecuteProcedureCommand("proc_LiquidMargin", parms);

            var parmsTo = new Dictionary<string, object>();
            parmsTo.Clear();
            parmsTo.Add("@companyId", CurrentUser.AccountComId);
            parmsTo.Add("@KeyId", KeyId);

            SqlService.ExecuteProcedureCommand("proc_LiquidTo", parmsTo);

            return true;

        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            GasHelper.DropDownListVehicleNumDataBind(tbxFVehicleNum);

            GasHelper.DropDownListLiquidDataBind(tbxFItemName);
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            txtKeyId.Text = KeyId;
            txtFMarginEnd.Text = Qty.ToString(CultureInfo.InvariantCulture);
            txtFQty.Text = Qty.ToString(CultureInfo.InvariantCulture);

            switch (Actions)
            {
                case WebAction.Add:
                    break;
                case WebAction.Edit:

                    if (To != null)
                    {
                        txtFMarginEnd.Text = To.XMarginEnd.ToString();
                        ddlBill.SelectedValue = To.XBill;
                        tbxFVehicleNum.Text = To.XVehicleNum;
                        txtFQty.Text = To.XQty.ToString();
                        txtFMemo.Text = To.XMemo;
                        
                    }
                    break;
            }
        }

        #endregion
    }
}