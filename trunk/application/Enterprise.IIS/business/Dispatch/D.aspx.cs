using System;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.IIS.Common;
using System.Linq;
using System.Globalization;

namespace Enterprise.IIS.business.Dispatch
{
    public partial class D : PageBase
    {
        
        /// <summary>
        ///     数据服务
        /// </summary>
        private DispatchDetailsService _dispatchDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected DispatchDetailsService DispatchDetailsService
        {
            get { return _dispatchDetailsService ?? (_dispatchDetailsService = new DispatchDetailsService()); }
            set { _dispatchDetailsService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        protected int FId
        {
            get { return int.Parse(Request["FId"]); }
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
        ///     
        /// </summary>
        private LHDispatchDetails _dispatchDetails;

        /// <summary>
        ///     
        /// </summary>
        protected LHDispatchDetails DispatchDetails
        {
            get
            {
                return _dispatchDetails ?? (_dispatchDetails = DispatchDetailsService.FirstOrDefault(p => p.FId == FId));
            }
            set { _dispatchDetails = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //SetPermissionButtons(Toolbar1);

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
                isSucceed = SubmintEdit();
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
            if (DispatchDetails != null)
            {
                DispatchDetails.FActual = Convert.ToDecimal(txtFActual.Text.Trim());
                DispatchDetails.FDriver= txtFDriver.Text;
                DispatchDetails.FItem = txtFItem.Text;
                DispatchDetails.FMileage = Convert.ToDecimal(txtFMileage.Text.Trim());
                DispatchDetails.FNumber = Convert.ToInt32(txtFNumber.Text.Trim());
                DispatchDetails.FOperationCertificateNo =txtFOperationCertificateNo.Text.Trim();
                DispatchDetails.FRiskType = txtFRiskType.Text;
                DispatchDetails.FSupercargo = txtFSupercargo.Text;
                DispatchDetails.FTimes = Convert.ToInt32(txtFTimes.Text);
                DispatchDetails.FTonnage = txtFTonnage.Text;
                DispatchDetails.FUnit = txtFUnit.Text;

                DispatchDetailsService.SaveChanges();

                return true;
            }
            return false;
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

                    if (DispatchDetails != null)
                    {
                        txtFActual.Text= DispatchDetails.FActual.ToString();
                        txtFDriver.Text= DispatchDetails.FDriver;
                        txtFItem.Text=DispatchDetails.FItem ;
                        txtFMileage.Text=DispatchDetails.FMileage.ToString();
                        txtFNumber.Text=DispatchDetails.FNumber.ToString();
                        txtFOperationCertificateNo.Text=DispatchDetails.FOperationCertificateNo ;
                        txtFRiskType.Text=DispatchDetails.FRiskType ;
                        txtFSupercargo.Text=DispatchDetails.FSupercargo ;
                        txtFTimes.Text=DispatchDetails.FTimes.ToString();
                        txtFTonnage.Text=DispatchDetails.FTonnage.ToString();
                        txtFUnit.Text=DispatchDetails.FUnit;
                        txtFVehicleNum.Text = DispatchDetails.FVehicleNum;
                    }
                    break;
            }
        }
        #endregion


    }
}