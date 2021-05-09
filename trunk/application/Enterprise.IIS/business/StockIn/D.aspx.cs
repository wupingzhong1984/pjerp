using System;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.IIS.Common;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using Enterprise.Framework.Log;

namespace Enterprise.IIS.business.StockIn
{
    public partial class D : PageBase
    {

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
        protected string KeyId
        {
            get { return Request["KeyId"]; }
        }

        /// <summary>
        ///     
        /// </summary>
        protected string FCode
        {
            get { return Request["FCode"]; }
        }

        /// <summary>
        ///     
        /// </summary>
        protected int FId
        {
            get { return int.Parse(Request["FId"]); }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockInService _stockOutService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockInService StockOutService
        {
            get { return _stockOutService ?? (_stockOutService = new StockInService()); }
            set { _stockOutService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockInLiquidService _stockInLiquidService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockInLiquidService StockInLiquidService
        {
            get { return _stockInLiquidService ?? (_stockInLiquidService = new StockInLiquidService()); }
            set { _stockInLiquidService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockInDetailsService _stockOutDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockInDetailsService StockOutDetailsService
        {
            get
            {
                return _stockOutDetailsService ?? //
              (_stockOutDetailsService = new StockInDetailsService());
            }
            set { _stockOutDetailsService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHStockIn _stockOut;

        /// <summary>
        ///     
        /// </summary>
        protected LHStockIn StockOut
        {
            get
            {
                return _stockOut ?? (_stockOut = StockOutService.FirstOrDefault(p => p.KeyId == KeyId //
              && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _stockOut = value; }
        }
        /// <summary>
        ///     
        /// </summary>
        private LHStockInLiquid _stockInLiquid;

        /// <summary>
        ///     
        /// </summary>
        protected LHStockInLiquid StockInLiquid
        {
            get
            {
                return _stockInLiquid ?? (_stockInLiquid = StockInLiquidService.FirstOrDefault(p =>  p.Rowid == FId));
            }
            set { _stockInLiquid = value; }
        }


        /// <summary>
        ///     
        /// </summary>
        private LHStockInDetails _stokcOutDetails;

        /// <summary>
        ///     
        /// </summary>
        protected LHStockInDetails StockOutDetails
        {
            get
            {
                return _stokcOutDetails ?? (_stokcOutDetails = StockOutDetailsService.FirstOrDefault(p => p.FId == FId));
            }
            set { _stokcOutDetails = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();

                LoadData();
            }
        }

        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            GasHelper.DropDownListDataBindDevice(ddlDevice);
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;
            string error = "提交失败！";
            try
            {
                isSucceed = SubmintEdit();
            }
            catch (Exception ex)
            {
                isSucceed = false;
                error = ex.Message;
            }
            finally
            {
                if (isSucceed)
                {
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show(error, MessageBoxIcon.Error);
                }
            }
        }

        protected void ddlDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = ddlDevice.SelectedValue.ToString();

            txtFDeviceValue.Text = new ProjectItemsService().FirstOrDefault(p => p.FId == id).FValue;
        }
        private bool SubmintEdit()
        {
            if (StockOutDetails != null)
            {

                decimal payY = decimal.Parse(txtFInPressure.Text)*10;
                decimal payW = decimal.Parse(txtFInTemperature.Text);


                decimal payY1 = decimal.Parse(txtoutPressure.Text)*10;
                decimal payW1 = decimal.Parse(txtoutTemperature.Text);
                //压缩因子
                string FZ1 = "0";
                string FZ2 = "0";
                //通用压缩因子
                LHFactor TSFZ = new FactorService().Where(p => p.FCompanyCode == FCode && p.FTemperature == payW && p.FBar == payY).FirstOrDefault();
                LHFactor TSFZ1 = new FactorService().Where(p => p.FCompanyCode == FCode && p.FTemperature == payW1 && p.FBar == payY1).FirstOrDefault();
                //通用赋值
                if (TSFZ != null)
                {
                    FZ1 = TSFZ.FDivisor.ToString(CultureInfo.InvariantCulture);
                }
                if (TSFZ1 != null)
                {
                    FZ2 = TSFZ1.FDivisor.ToString(CultureInfo.InvariantCulture);
                }
                StockOutDetails.FQty = Math.Abs((decimal.Parse(FZ1)-decimal.Parse(FZ2)))*decimal.Parse(txtFDeviceValue.Text);
                if (StockInLiquid == null)
                {
                    StockInLiquid = new LHStockInLiquid
                    {
                        FDevice = ddlDevice.SelectedValue,
                        FDeviceValue = decimal.Parse(txtFDeviceValue.Text),
                        companyid = CurrentUser.AccountComId.ToString(),
                        Bar = decimal.Parse(txtFInPressure.Text),
                        Temperature = decimal.Parse(txtFInTemperature.Text),
                        InBar=decimal.Parse(txtoutPressure.Text),
                        InTemperature=decimal.Parse(txtoutTemperature.Text),

                        keyid = KeyId,
                        Rowid = FId
                    };
                    StockInLiquidService.Add(StockInLiquid);

                }
                else
                {

                    StockInLiquid.FDevice = ddlDevice.SelectedValue;
                    StockInLiquid.FDeviceValue = decimal.Parse(txtFDeviceValue.Text);
                    StockInLiquid.companyid = CurrentUser.AccountComId.ToString();
                    StockInLiquid.Bar = decimal.Parse(txtFInPressure.Text);
                    StockInLiquid.Temperature = decimal.Parse(txtFInTemperature.Text);
                    StockInLiquid.InBar = decimal.Parse(txtoutPressure.Text);
                    StockInLiquid.InTemperature = decimal.Parse(txtoutTemperature.Text);
                }
                StockInLiquidService.SaveChanges();
                StockOutDetailsService.SaveChanges();

                return true;
            }
            return false;
        }
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:

                    break;
                case WebAction.Edit:
                    if (StockInLiquid != null)
                    {
                        txtFInTemperature.Text = StockInLiquid.Temperature.ToString();
                        txtFInPressure.Text = StockInLiquid.Bar.ToString();
                        ddlDevice.SelectedValue = StockInLiquid.FDevice;
                        txtFDeviceValue.Text = StockInLiquid.FDeviceValue.ToString();
                        txtoutPressure.Text = StockInLiquid.InBar.ToString();
                        txtoutTemperature.Text = StockInLiquid.InTemperature.ToString();
                    }
                    break;
            }
        }

    }
}