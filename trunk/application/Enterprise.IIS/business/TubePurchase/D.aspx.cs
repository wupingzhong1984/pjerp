using System;
using System.Linq;
using System.Globalization;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.IIS.Common;
using FineUI;

namespace Enterprise.IIS.business.TubePurchase
{
    public partial class D : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private StockInService _stockInService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockInService StockInService
        {
            get { return _stockInService ?? (_stockInService = new StockInService()); }
            set { _stockInService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockInDetailsService _stockInDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockInDetailsService StockInDetailsService
        {
            get
            {
                return _stockInDetailsService ?? //
              (_stockInDetailsService = new StockInDetailsService());
            }
            set { _stockInDetailsService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private TubePriceService _tubePriceService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected TubePriceService TubePriceService
        {
            get { return _tubePriceService ?? (_tubePriceService = new TubePriceService()); }
            set { _tubePriceService = value; }
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
        protected int FId
        {
            get { return int.Parse(Request["FId"]); }
        }
        /// <summary>
        ///     
        /// </summary>
        protected string FCode
        {
            get { return Request["FCode"]; }
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
        private LHStockIn _stockIn;

        /// <summary>
        ///     
        /// </summary>
        protected LHStockIn StockOut
        {
            get
            {
                return _stockIn ?? (_stockIn = StockInService.FirstOrDefault(p => p.KeyId == KeyId //
              && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _stockIn = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHStockInDetails _stokcInDetails;

        /// <summary>
        ///     
        /// </summary>
        protected LHStockInDetails StockInDetails
        {
            get
            {
                return _stokcInDetails ?? (_stokcInDetails = StockInDetailsService.FirstOrDefault(p => p.FId == FId));
            }
            set { _stokcInDetails = value; }
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

        protected void ddlDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = ddlDevice.SelectedValue.ToString();

            txtFDeviceValue.Text = new ProjectItemsService().FirstOrDefault(p => p.FId == id).FValue;
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (StockInDetails != null)
            {

                StockInDetails.FDevice = ddlDevice.SelectedValue;

                StockInDetails.FDeviceValue = string.IsNullOrEmpty(txtFDeviceValue.Text.Trim()) ? 0 : Convert.ToDecimal(txtFDeviceValue.Text.Trim());

                StockInDetails.FInTemperature = string.IsNullOrEmpty(txtFInTemperature.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFInTemperature.Text.Trim());
                StockInDetails.FInPressure = string.IsNullOrEmpty(txtFInPressure.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFInPressure.Text.Trim());

                StockInDetails.FOutTemperature = string.IsNullOrEmpty(txtFOutTemperature.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFInTemperature.Text.Trim());
                StockInDetails.FOutPressure = string.IsNullOrEmpty(txtFOutPressure.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFOutPressure.Text.Trim());

                StockInDetails.FPayTemperature = string.IsNullOrEmpty(txtFPayTemperature.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFPayTemperature.Text.Trim());
                StockInDetails.FPayPressure = string.IsNullOrEmpty(txtFPayPressure.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFPayPressure.Text.Trim());

                StockInDetails.FReceiveTemperature = string.IsNullOrEmpty(txtFReceiveTemperature.Text.Trim()) //
                                    ? 0 : Convert.ToDecimal(txtFReceiveTemperature.Text.Trim());
                StockInDetails.FReceivePressure = string.IsNullOrEmpty(txtFReceivePressure.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFReceivePressure.Text.Trim());

                //数量计算

                //计算公式
                var v1 = TubePriceService.Where(p => p.FBill == "采购"//
                    && p.FCode == FCode//
                    && p.FItemCode == StockInDetails.FItemCode).FirstOrDefault();

                if (v1 != null)
                {
                    //< font color = "red" >$W1：进厂温度、$W2：出厂温度、$W3: 交付温度、$W4: 接收温度、$V: 水容积 </ font >< br />
                    //< font color = "red" >$Y1：进厂压力、$Y2：出厂压力、$Y2: 交付压力、$Y2: 接收压力 </ font >< br />

                    var inW = Convert.ToDecimal(StockInDetails.FInTemperature).ToString(CultureInfo.InvariantCulture);
                    var inY = Convert.ToDecimal(StockInDetails.FInPressure).ToString(CultureInfo.InvariantCulture);

                    var outW = Convert.ToDecimal(StockInDetails.FOutTemperature).ToString(CultureInfo.InvariantCulture);
                    var outY = Convert.ToDecimal(StockInDetails.FOutPressure).ToString(CultureInfo.InvariantCulture); ;

                    var payW = Convert.ToDecimal(StockInDetails.FPayTemperature).ToString(CultureInfo.InvariantCulture);
                    var payY = Convert.ToDecimal(StockInDetails.FPayPressure).ToString(CultureInfo.InvariantCulture);

                    var recW = Convert.ToDecimal(StockInDetails.FReceiveTemperature).ToString(CultureInfo.InvariantCulture);
                    var recY = Convert.ToDecimal(StockInDetails.FReceivePressure).ToString(CultureInfo.InvariantCulture);

                    //水溶积
                    var v = Convert.ToDecimal(StockInDetails.FDeviceValue).ToString(CultureInfo.InvariantCulture);

                    //数量
                    var s1 = v1.FFormula.Replace("$W1", inW)//
                        .Replace("$W2", outW)
                        .Replace("$W3", payW)//
                        .Replace("$W4", recW)//
                        .Replace("$Y1", inY)//
                        .Replace("$Y2", outY)//
                        .Replace("$Y3", payY)//
                        .Replace("$Y4", recY)//
                        .Replace("$V", v);

                    //数量 
                    StockInDetails.FQty = Convert.ToDecimal(FormulaCalculator.Eval(s1));
                    //金额
                    StockInDetails.FAmount = StockInDetails.FQty * StockInDetails.FPrice;

                }

                return StockInDetailsService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            GasHelper.DropDownListDataBindDevice(ddlDevice);
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
                    txtFName.Text = StockOut.FName;

                    if (StockInDetails != null)
                    {
                        //WebControlHandler.BindObjectToControls(StockOutDetails, SimpleForm1);

                        ddlDevice.SelectedValue = StockInDetails.FDevice;

                        txtFDeviceValue.Text = StockInDetails.FDeviceValue.ToString();

                        var code = StockInDetails.FItemCode;
                        var item = new ItemsService().FirstOrDefault(P => P.FCode == code);

                        if (item != null)
                        {
                            txtFItemName.Text = item.FName;
                            txtFUnit.Text = item.FUnit;
                        }

                        txtFInPressure.Text = StockInDetails.FInPressure.ToString();
                        txtFInTemperature.Text = StockInDetails.FInTemperature.ToString();

                        txtFOutPressure.Text = StockInDetails.FOutPressure.ToString();
                        txtFOutTemperature.Text = StockInDetails.FOutTemperature.ToString();

                        txtFPayPressure.Text = StockInDetails.FPayPressure.ToString();
                        txtFPayTemperature.Text = StockInDetails.FPayTemperature.ToString();

                        txtFReceivePressure.Text = StockInDetails.FReceivePressure.ToString();
                        txtFReceiveTemperature.Text = StockInDetails.FReceiveTemperature.ToString();

                    }
                    break;
            }
        }
        #endregion


    }
}