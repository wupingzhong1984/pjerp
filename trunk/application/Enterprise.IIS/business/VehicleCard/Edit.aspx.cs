using System;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Enterprise.IIS.Common;

namespace Enterprise.IIS.business.VehicleCard
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

        protected string FNum
        {
            get { return Request["FNum"]; }
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

        private VehicleCardService _vehicleCardService;
        /// <summary>
        /// 
        /// </summary>
        protected VehicleCardService VehicleCardService
        {
            get { return _vehicleCardService ?? (_vehicleCardService = new VehicleCardService()); }
            set { _vehicleCardService = value; }
        }



        private LHVehicleCard _vehicleCard;
        /// <summary>
        /// 
        /// </summary>
        protected LHVehicleCard VehicleCard
        {
            get { return _vehicleCard ?? (_vehicleCard = VehicleCardService.FirstOrDefault(p => p.FId == Key)); }
            set { _vehicleCard = value; }
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
            get { return _vehicle ?? (_vehicle = new LHVehicle()); }
            set { _vehicle = value; }
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
            if (VehicleCard != null)
            {
                VehicleCard.FDeleteFlag = 0;

                VehicleCard.FNum = ddlFVehicleNum.SelectedValue.Trim();
                VehicleCard.FCertificateDate = dpCertificateDate.SelectedDate;
                VehicleCard.FCertificateMoney = string.IsNullOrWhiteSpace(txtCertificateMoney.Text) ? 0M : Convert.ToDecimal(txtCertificateMoney.Text);
                VehicleCard.FDrivingDate = dpDrivingDate.SelectedDate;
                VehicleCard.FDrivingMoney = string.IsNullOrWhiteSpace(txtDrivingMoney.Text) ? 0M : Convert.ToDecimal(txtDrivingMoney.Text);
                VehicleCard.FDeadlineDate = dpDrivingDate.SelectedDate;
                VehicleCard.FDeadlineMoney = string.IsNullOrWhiteSpace(txtDeadlineMoney.Text) ? 0M : Convert.ToDecimal(txtDeadlineMoney.Text);
                VehicleCard.FCommercialDate = dpCommercialDate.SelectedDate;
                VehicleCard.FCommercialMoney = string.IsNullOrWhiteSpace(txtCommercialMoney.Text) ? 0M : Convert.ToDecimal(txtCommercialMoney.Text);
                VehicleCard.FDangerousDate = dpDangerousDate.SelectedDate;
                VehicleCard.FDangerousMoney = string.IsNullOrWhiteSpace(txtDangerousMoney.Text) ? 0M : Convert.ToDecimal(txtDangerousMoney.Text);
                VehicleCard.FTankDate = dpTankDate.SelectedDate;
                VehicleCard.FRegistrationDate = dpRegistrationDate.SelectedDate;
                VehicleCard.FTankMoney = string.IsNullOrWhiteSpace(txtTankMoney.Text) ? 0M : Convert.ToDecimal(txtTankMoney.Text);
                VehicleCard.FRegistrationMoney = string.IsNullOrWhiteSpace(txtRegistrationMoney.Text) ? 0M : Convert.ToDecimal(txtRegistrationMoney.Text);
                VehicleCard.FTwostageDate = dpTwostageDate.SelectedDate;
                VehicleCard.FTwostageMoney = string.IsNullOrWhiteSpace(txtTwostageMoney.Text) ? 0M : Convert.ToDecimal(txtTwostageMoney.Text);
                //VehicleCard.Updateby = CurrentUser.AccountJobNumber;
                //VehicleCard.FUpdateon = DateTime.Now;

                Vehicle = VehicleService.FirstOrDefault(p => p.FNum == ddlFVehicleNum.SelectedValue);

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
                
                VehicleService.SaveChanges();
            };

            return VehicleCardService.SaveChanges() >= 0;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var vehicleCard = new LHVehicleCard
            {
                FNum = ddlFVehicleNum.SelectedValue.Trim(),
                FDeleteFlag = 0,
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
                FTankMoney = string.IsNullOrWhiteSpace(txtTankMoney.Text) ? 0M : Convert.ToDecimal(txtTankMoney.Text),
                FRegistrationMoney = string.IsNullOrWhiteSpace(txtRegistrationMoney.Text) ? 0M : Convert.ToDecimal(txtRegistrationMoney.Text),
                FTwostageDate = dpTwostageDate.SelectedDate,
                FTwostageMoney = string.IsNullOrWhiteSpace(txtTwostageMoney.Text) ? 0M : Convert.ToDecimal(txtTwostageMoney.Text),
                FContext = txtContext.Text.Trim(),
                Createdby = CurrentUser.AccountJobNumber,
                //Createdon=DateTime.Now
            };
            Vehicle = VehicleService.FirstOrDefault(p => p.FNum == ddlFVehicleNum.SelectedValue);
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
            VehicleService.SaveChanges();
            return VehicleCardService.Add(vehicleCard);

        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            ViewState["_AppendToEnd"] = true;
            
            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);
            
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
                    if (VehicleCard != null)
                    {
                        ddlFVehicleNum.SelectedValue = VehicleCard.FNum;
                        dpCertificateDate.SelectedDate = VehicleCard.FCertificateDate;
                        txtCertificateMoney.Text = VehicleCard.FCertificateMoney + "";
                        dpDrivingDate.SelectedDate = VehicleCard.FDrivingDate;
                        txtDrivingMoney.Text = VehicleCard.FDrivingMoney + "";
                        dpDeadlineDate.SelectedDate = VehicleCard.FDeadlineDate;
                        txtDeadlineMoney.Text = VehicleCard.FDeadlineMoney + "";
                        dpCommercialDate.SelectedDate = VehicleCard.FCommercialDate;
                        txtCommercialMoney.Text = VehicleCard.FCommercialMoney + "";
                        dpDangerousDate.SelectedDate = VehicleCard.FDangerousDate;
                        txtDangerousMoney.Text = VehicleCard.FDangerousMoney + "";
                        dpTankDate.SelectedDate = VehicleCard.FTankDate;
                        txtTankMoney.Text = VehicleCard.FTankMoney + "";
                        dpTwostageDate.SelectedDate = VehicleCard.FTwostageDate;
                        txtTwostageMoney.Text = VehicleCard.FTwostageMoney + "";
                        dpRegistrationDate.SelectedDate = VehicleCard.FRegistrationDate;
                        txtRegistrationMoney.Text = VehicleCard.FRegistrationMoney + "";
                    }
                    break;
            }
        }
        #endregion
    }
}