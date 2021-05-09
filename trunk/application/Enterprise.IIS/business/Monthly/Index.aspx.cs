using System;
using System.Collections.Generic;
using Enterprise.Data;
using Enterprise.IIS.Common;
using Enterprise.Service.Base.Platform;
using FineUI;

namespace Enterprise.IIS.business.Monthly
{

    /// <summary>
    ///     月结
    /// </summary>
    public partial class Index : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        private CompanyService _companyService;
        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected CompanyService CompanyService
        {
            get { return _companyService ?? (_companyService = new CompanyService()); }
            set { _companyService = value; }
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

        /// <summary>
        ///     提交月结
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var company = CompanyService.FirstOrDefault(p => p.id == CurrentUser.AccountComId);

                if (company != null)
                {
                    DateTime monthbegin;
                    DateTime monthend;

                    if (company.FMonthly.Contains("是"))
                    {
                        var parms = new Dictionary<string, object>();
                        parms.Clear();

                        parms.Add("@Fdate", Convert.ToDateTime(dpkFMonth.SelectedDate).ToString("yyyy-MM-dd"));

                        var date = SqlService.ExecuteProcedureCommand("proc_MonthDay", parms);

                        monthbegin = Convert.ToDateTime(date.Tables[0].Rows[0][0]);
                        monthend = Convert.ToDateTime(date.Tables[1].Rows[0][0]);
                    }
                    else
                    {
                        monthbegin = Convert.ToDateTime(Convert.ToDateTime(dpkFMonth.SelectedDate).AddMonths(-1).ToString("yyyy-MM") + "-" + company.FMonthlyDay).AddDays(1);
                        monthend = Convert.ToDateTime(Convert.ToDateTime(dpkFMonth.SelectedDate).ToString("yyyy-MM") + "-" + company.FMonthlyDay);
                    }

                    //--------------------------------

                    var pamrs = new Dictionary<string, object>();
                    pamrs.Clear();

                    pamrs.Add("@companyId", CurrentUser.AccountComId);
                    pamrs.Add("@FMonth", Convert.ToDateTime(dpkFMonth.SelectedDate).ToString("yyyy-MM"));
                    pamrs.Add("@FMonthT", Convert.ToDateTime(dpkFMonth.SelectedDate).AddMonths(1).ToString("yyyy-MM"));
                    pamrs.Add("@CreateBy", CurrentUser.AccountName);
                    pamrs.Add("@begin", monthbegin.ToString("yyyy-MM-dd"));
                    pamrs.Add("@end", monthend.ToString("yyyy-MM-dd"));

                    SqlService.ExecuteProcedureNonQuery("proc_Month", pamrs);

                    Alert.Show(string.Format("【{0}～{1}】月结完成", monthbegin.ToString("yyyy-MM-dd"), monthend.ToString("yyyy-MM-dd")),MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                Alert.Show("月结失败",MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     提交月结
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMonthR_Click(object sender, EventArgs e)
        {
            try
            {
                var company = CompanyService.FirstOrDefault(p => p.id == CurrentUser.AccountComId);

                if (company != null)
                {
                    DateTime monthbegin;
                    DateTime monthend;

                    if (company.FMonthly.Contains("是"))
                    {
                        var parms = new Dictionary<string, object>();
                        parms.Clear();

                        parms.Add("@Fdate", Convert.ToDateTime(dpkFMonth.SelectedDate).ToString("yyyy-MM-dd"));

                        var date = SqlService.ExecuteProcedureCommand("proc_MonthDay", parms);

                        monthbegin = Convert.ToDateTime(date.Tables[0].Rows[0][0]);
                        monthend = Convert.ToDateTime(date.Tables[1].Rows[0][0]);
                    }
                    else
                    {
                        monthbegin = Convert.ToDateTime(Convert.ToDateTime(dpkFMonth.SelectedDate).AddMonths(-1).ToString("yyyy-MM") + "-" + company.FMonthlyDay).AddDays(1);
                        monthend = Convert.ToDateTime(Convert.ToDateTime(dpkFMonth.SelectedDate).ToString("yyyy-MM") + "-" + company.FMonthlyDay);
                    }

                    //--------------------------------

                    var pamrs = new Dictionary<string, object>();
                    pamrs.Clear();

                    pamrs.Add("@companyId", CurrentUser.AccountComId);
                    pamrs.Add("@FMonth", Convert.ToDateTime(dpkFMonth.SelectedDate).ToString("yyyy-MM"));
                    pamrs.Add("@FMonthT", Convert.ToDateTime(dpkFMonth.SelectedDate).AddMonths(1).ToString("yyyy-MM"));
                    pamrs.Add("@CreateBy", CurrentUser.AccountName);
                    pamrs.Add("@begin", monthbegin.ToString("yyyy-MM-dd"));
                    pamrs.Add("@end", monthend.ToString("yyyy-MM-dd"));

                    SqlService.ExecuteProcedureNonQuery("proc_MonthR", pamrs);

                    Alert.Show(string.Format("【{0}～{1}】反月结完成", monthbegin.ToString("yyyy-MM-dd"), monthend.ToString("yyyy-MM-dd")), MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                Alert.Show("反月结失败", MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Private Method


        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            //btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区

        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            dpkFMonth.SelectedDate = DateTime.Now;
        }
        #endregion
    }
}