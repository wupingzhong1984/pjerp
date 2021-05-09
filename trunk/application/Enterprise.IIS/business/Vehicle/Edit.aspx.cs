using System;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.IIS.Common;
using FineUI;

namespace Enterprise.IIS.business.Vehicle
{
    public partial class Edit : PageBase
    {
        /// <summary>
        ///     对象ID
        /// </summary>
        protected int Key
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

        private VehicleService _vehicleService;
        /// <summary>
        /// 
        /// </summary>
        protected VehicleService VehicleService
        {
            get { return _vehicleService ?? (_vehicleService = new VehicleService()); }
            set { _vehicleService = value; }
        }

        private LHVehicle _vehicle;
        /// <summary>
        /// 
        /// </summary>
        protected LHVehicle Vehicle
        {
            get { return _vehicle ?? (_vehicle = VehicleService.FirstOrDefault(p => p.FId == Key)); }
            set { _vehicle = value; }
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
            if (Vehicle != null)
            {

                Vehicle.FNum = txtcph.Text.Trim();
                Vehicle.FColor = txtColor.Text.Trim();
                Vehicle.FTypeNo = txtTypeNo.Text.Trim();
                Vehicle.FRegData = dpFRegData.SelectedDate;
                Vehicle.FLicenseNum = txtFLicenseNum.Text.Trim();
                Vehicle.FMemo = txtFMemo.Text.Trim();
                //Vehicle.FFlag = 1;
                Vehicle.FTonner = txttonner.Text.Trim();
                Vehicle.FCompanyName = txtFCompanyId.Text.Trim();

                Vehicle.FEngineNo = txtFEngineNo.Text.Trim();
                Vehicle.FVIN = txtVIN.Text.Trim();
                Vehicle.FTypeNum = ddlTypeNum.SelectedValue;
                Vehicle.FBrand = txtBrand.Text.Trim();
                Vehicle.FQuality = txtQuality.Text.Trim();
                Vehicle.FLx = ddllx.SelectedValue;
                Vehicle.FWear = Convert.ToDecimal(txtwear.Text.Trim() == string.Empty ? "0" : txtwear.Text.Trim());
                Vehicle.FFactoryDate = dpFactoryDate.SelectedDate;
                Vehicle.FEngineType = txtFEngineType.Text.Trim();
                Vehicle.Fuel = txtFuel.Text.Trim();
                Vehicle.FDesplaceMent = txtDesplaceMent.Text.Trim();
                Vehicle.FPower = txtpower.Text.Trim();
                Vehicle.FReginNo = txtReginNo.Text.Trim();
                Vehicle.FRegionCompany = txtRegionCompany.Text.Trim();
                Vehicle.FOperateNum = txtoperateNum.Text.Trim();
                Vehicle.FDrvierNum = txtDrvierNum.Text.Trim();
                Vehicle.FCertificateDate = dpCertificateDate.SelectedDate;
                Vehicle.FCertificateMoney = string.IsNullOrWhiteSpace(txtCertificateMoney.Text) ? 0M : Convert.ToDecimal(txtCertificateMoney.Text);
                Vehicle.FDrivingDate = dpDrivingDate.SelectedDate;
                Vehicle.FDrivingMoney = string.IsNullOrWhiteSpace(txtDrivingMoney.Text) ? 0M : Convert.ToDecimal(txtDrivingMoney.Text);
                Vehicle.FDeadlineDate = dpDrivingDate.SelectedDate;
                Vehicle.FDeadlineMoney = string.IsNullOrWhiteSpace(txtDeadlineMoney.Text) ? 0M : Convert.ToDecimal(txtDeadlineMoney.Text);
                Vehicle.FCommercialDate = dpCommercialDate.SelectedDate;
                Vehicle.FCommercialMoney = string.IsNullOrWhiteSpace(txtCommercialMoney.Text) ? 0M : Convert.ToDecimal(txtCommercialMoney.Text);
                Vehicle.FDangerousDate = dpDangerousDate.SelectedDate;
                Vehicle.FDangerousMoney = string.IsNullOrWhiteSpace(txtDangerousMoney.Text) ? 0M : Convert.ToDecimal(txtDangerousMoney.Text);
                Vehicle.FTankDate = dpTankDate.SelectedDate;
                Vehicle.FRegistrationDate = dpRegistrationDate.SelectedDate;
                Vehicle.TankMoney = string.IsNullOrWhiteSpace(txtTankMoney.Text) ? 0M : Convert.ToDecimal(txtTankMoney.Text);
                Vehicle.FRegistrationMoney = string.IsNullOrWhiteSpace(txtRegistrationMoney.Text) ? 0M : Convert.ToDecimal(txtRegistrationMoney.Text);
                Vehicle.FTwostageDate = dpTwostageDate.SelectedDate;
                Vehicle.FTwostageMoney = string.IsNullOrWhiteSpace(txtTwostageMoney.Text) ? 0M : Convert.ToDecimal(txtTwostageMoney.Text);
                Vehicle.FMeteredPrice = Convert.ToDecimal(txtFMeteredPrice.Text);
                Vehicle.FWaterSpace = string.IsNullOrWhiteSpace(txtFWaterSpace.Text)
                    ? 0M
                    : Convert.ToDecimal(txtFWaterSpace.Text);
                Vehicle.FRunStatus = ddlRunStatus.SelectedValue;
                Vehicle.FLogistics = ddlFLogistics.SelectedValue;//物流
                Vehicle.FISMain = ddlFISMain.SelectedValue;

            }

            return VehicleService.SaveChanges() >= 0;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var vehicle = new LHVehicle
            {
                FNum = txtcph.Text.Trim(),
                FColor = txtColor.Text.Trim(),
                FRegData = dpFRegData.SelectedDate,
                FLicenseNum = txtFLicenseNum.Text.Trim(),
                FMemo = txtFMemo.Text.Trim(),
                FTypeNo = txtTypeNo.Text.Trim(),
                FFlag = 1,
                FTonner = txttonner.Text.Trim(),
                FCompanyName = txtFCompanyId.Text.Trim(),
                FEngineNo = txtFEngineNo.Text.Trim(),
                FVIN = txtVIN.Text.Trim(),
                FTypeNum = ddlTypeNum.SelectedValue,
                FBrand = txtBrand.Text.Trim(),
                FCompanyId = CurrentUser.AccountComId,
                FQuality = txtQuality.Text.Trim(),
                FLx = ddllx.SelectedValue,
                FWear = Convert.ToDecimal(txtwear.Text.Trim() == string.Empty ? "0" : txtwear.Text.Trim()),
                FFactoryDate = dpFactoryDate.SelectedDate,
                FEngineType = txtFEngineType.Text.Trim(),
                Fuel = txtFuel.Text.Trim(),
                FDesplaceMent = txtDesplaceMent.Text.Trim(),
                FPower = txtpower.Text.Trim(),
                FReginNo = txtReginNo.Text.Trim(),
                FRegionCompany = txtRegionCompany.Text.Trim(),
                FOperateNum = txtoperateNum.Text.Trim(),
                FDrvierNum = txtDrvierNum.Text.Trim(),
                FCertificateDate = dpCertificateDate.SelectedDate,
                FCertificateMoney = string.IsNullOrWhiteSpace(txtCertificateMoney.Text) ? 0M : Convert.ToDecimal(txtCertificateMoney.Text),
                FDrivingDate = dpDrivingDate.SelectedDate,
                FDrivingMoney = string.IsNullOrWhiteSpace(txtDrivingMoney.Text) ? 0M : Convert.ToDecimal(txtDrivingMoney.Text),
                FDeadlineDate = dpDrivingDate.SelectedDate,
                FDeadlineMoney = string.IsNullOrWhiteSpace(txtDeadlineMoney.Text) ? 0M : Convert.ToDecimal(txtDeadlineMoney.Text),
                FCommercialDate = dpCommercialDate.SelectedDate,
                FCommercialMoney = string.IsNullOrWhiteSpace(txtCommercialMoney.Text) ? 0M : Convert.ToDecimal(txtCommercialMoney.Text),
                FDangerousDate = dpDangerousDate.SelectedDate,
                FDangerousMoney = string.IsNullOrWhiteSpace(txtDangerousMoney.Text) ? 0M : Convert.ToDecimal(txtDangerousMoney.Text),
                FTankDate = dpTankDate.SelectedDate,
                FRegistrationDate = dpRegistrationDate.SelectedDate,
                TankMoney = string.IsNullOrWhiteSpace(txtTankMoney.Text) ? 0M : Convert.ToDecimal(txtTankMoney.Text),
                FRegistrationMoney = string.IsNullOrWhiteSpace(txtRegistrationMoney.Text) ? 0M : Convert.ToDecimal(txtRegistrationMoney.Text),
                FTwostageDate = dpTwostageDate.SelectedDate,
                FTwostageMoney = string.IsNullOrWhiteSpace(txtTwostageMoney.Text) ? 0M : Convert.ToDecimal(txtTwostageMoney.Text),
                FMeteredPrice = Convert.ToDecimal(txtFMeteredPrice.Text),
                FWaterSpace = string.IsNullOrWhiteSpace(txtFWaterSpace.Text) ? 0M : Convert.ToDecimal(txtFWaterSpace.Text),
                FRunStatus = ddlRunStatus.SelectedValue,
                FLogistics = ddlFLogistics.SelectedValue,//物流
                FISMain=ddlFISMain.SelectedValue,
            };
            return VehicleService.Add(vehicle);
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            ViewState["_AppendToEnd"] = true;

            GasHelper.DropDownListDispatchVehicleClassDataBind(ddlTypeNum);

            GasHelper.DropDownListVehicleClassDataBind(ddllx);

            GasHelper.DropDownListRunStatusDataBind(ddlRunStatus);

            GasHelper.DropDownListLogisticsDataBind(ddlFLogistics);//物流公司

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
                    if (Vehicle != null)
                    {
                        txtcph.Text = Vehicle.FNum;
                        txtColor.Text = Vehicle.FColor;
                        ddllx.SelectedValue = Vehicle.FLx + "";
                        ddlTypeNum.SelectedValue = Vehicle.FTypeNum;
                        txtTypeNo.Text = Vehicle.FTypeNo;
                        txtBrand.Text = Vehicle.FBrand;
                        txttonner.Text = Vehicle.FTonner;
                        txtQuality.Text = Vehicle.FQuality + "";
                        txtwear.Text = Vehicle.FWear + "";
                        txtFMemo.Text = Vehicle.FMemo;
                        txtColor.Text = Vehicle.FColor;
                        dpFactoryDate.SelectedDate = Vehicle.FFactoryDate;
                        txtFEngineType.Text = Vehicle.FEngineType;
                        txtFEngineNo.Text = Vehicle.FEngineNo;
                        txtVIN.Text = Vehicle.FVIN;
                        txtFuel.Text = Vehicle.Fuel;
                        txtDesplaceMent.Text = Vehicle.FDesplaceMent;
                        txtpower.Text = Vehicle.FPower;
                        txtFCompanyId.Text = Vehicle.FCompanyName;
                        txtReginNo.Text = Vehicle.FReginNo;
                        txtRegionCompany.Text = Vehicle.FRegionCompany;
                        dpFRegData.SelectedDate = Vehicle.FRegData;
                        txtFLicenseNum.Text = Vehicle.FLicenseNum;
                        txtoperateNum.Text = Vehicle.FOperateNum;
                        txtDrvierNum.Text = Vehicle.FDrvierNum;
                        dpCertificateDate.SelectedDate = Vehicle.FCertificateDate;
                        txtCertificateMoney.Text = Vehicle.FCertificateMoney + "";
                        dpDrivingDate.SelectedDate = Vehicle.FDrivingDate;
                        txtDrivingMoney.Text = Vehicle.FDrivingMoney + "";
                        dpDeadlineDate.SelectedDate = Vehicle.FDeadlineDate;
                        txtDeadlineMoney.Text = Vehicle.FDeadlineMoney + "";
                        dpCommercialDate.SelectedDate = Vehicle.FCommercialDate;
                        txtCommercialMoney.Text = Vehicle.FCommercialMoney + "";
                        dpDangerousDate.SelectedDate = Vehicle.FDangerousDate;
                        txtDangerousMoney.Text = Vehicle.FDangerousMoney + "";
                        dpTankDate.SelectedDate = Vehicle.FTankDate;
                        txtTankMoney.Text = Vehicle.TankMoney + "";
                        dpTwostageDate.SelectedDate = Vehicle.FTwostageDate;
                        txtTwostageMoney.Text = Vehicle.FTwostageMoney + "";
                        dpRegistrationDate.SelectedDate = Vehicle.FRegistrationDate;
                        txtRegistrationMoney.Text = Vehicle.FRegistrationMoney + "";
                        txtFMeteredPrice.Text = Vehicle.FMeteredPrice + "";
                        txtFWaterSpace.Text = Vehicle.FWaterSpace.ToString();
                        ddlRunStatus.SelectedValue = Vehicle.FRunStatus;
                        ddlFLogistics.SelectedValue = Vehicle.FLogistics;
                        ddlFISMain.SelectedValue = Vehicle.FISMain;

                    }
                    break;
            }
        }
        #endregion


    }
}