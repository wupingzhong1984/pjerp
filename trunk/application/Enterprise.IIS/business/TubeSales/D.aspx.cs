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

namespace Enterprise.IIS.business.TubeSales
{
    public partial class D : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private StockOutService _stockOutService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutService StockOutService
        {
            get { return _stockOutService ?? (_stockOutService = new StockOutService()); }
            set { _stockOutService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockOutDetailsService _stockOutDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutDetailsService StockOutDetailsService
        {
            get
            {
                return _stockOutDetailsService ?? //
              (_stockOutDetailsService = new StockOutDetailsService());
            }
            set { _stockOutDetailsService = value; }
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
        private LHStockOut _stockOut;

        /// <summary>
        ///     
        /// </summary>
        protected LHStockOut StockOut
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
        private LHStockOutDetails _stokcOutDetails;

        /// <summary>
        ///     
        /// </summary>
        protected LHStockOutDetails StockOutDetails
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

            txtFDeviceValue.Text = new ProjectItemsService().FirstOrDefault(p=>p.FId==id).FValue;
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (StockOutDetails != null)
            {
                StockOutDetails.FDevice = ddlDevice.SelectedValue;

                StockOutDetails.FDeviceValue = string.IsNullOrEmpty(txtFDeviceValue.Text.Trim()) ? 0 : Convert.ToDecimal(txtFDeviceValue.Text.Trim());

                StockOutDetails.FInTemperature = string.IsNullOrEmpty(txtFInTemperature.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFInTemperature.Text.Trim());
                StockOutDetails.FInPressure = string.IsNullOrEmpty(txtFInPressure.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFInPressure.Text.Trim());

                StockOutDetails.FOutTemperature = string.IsNullOrEmpty(txtFOutTemperature.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFInTemperature.Text.Trim());
                StockOutDetails.FOutPressure = string.IsNullOrEmpty(txtFOutPressure.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFOutPressure.Text.Trim());

                StockOutDetails.FPayTemperature = string.IsNullOrEmpty(txtFPayTemperature.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFPayTemperature.Text.Trim());
                StockOutDetails.FPayPressure = string.IsNullOrEmpty(txtFPayPressure.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFPayPressure.Text.Trim());

                StockOutDetails.FReceiveTemperature = string.IsNullOrEmpty(txtFReceiveTemperature.Text.Trim()) //
                                    ? 0 : Convert.ToDecimal(txtFReceiveTemperature.Text.Trim());
                StockOutDetails.FReceivePressure = string.IsNullOrEmpty(txtFReceivePressure.Text.Trim()) //
                    ? 0 : Convert.ToDecimal(txtFReceivePressure.Text.Trim());

                //数量计算

                //计算公式
                var v1 = TubePriceService.Where(p => p.FBill == "销售"//
                    && p.FCode == FCode ).FirstOrDefault();

                try
                {
                    if (v1 != null)
                    {
                        //< font color = "red" >$W1：进厂温度、$W2：出厂温度、$W3: 交付温度、$W4: 接收温度、$V: 水容积 </ font >< br />
                        //< font color = "red" >$Y1：进厂压力、$Y2：出厂压力、$Y3: 交付压力、$Y4: 接收压力 </ font >< br />
                        var inW = Convert.ToDecimal(StockOutDetails.FInTemperature).ToString(CultureInfo.InvariantCulture);
                        var inY = Convert.ToDecimal(StockOutDetails.FInPressure).ToString(CultureInfo.InvariantCulture);

                        var outW = Convert.ToDecimal(StockOutDetails.FOutTemperature).ToString(CultureInfo.InvariantCulture);
                        var outY = Convert.ToDecimal(StockOutDetails.FOutPressure).ToString(CultureInfo.InvariantCulture); ;

                        decimal payY = decimal.Parse(StockOutDetails.FPayPressure.ToString()) * 10;
                        decimal payW = decimal.Parse(StockOutDetails.FPayTemperature.ToString());
                        decimal recY = decimal.Parse(StockOutDetails.FReceivePressure.ToString()) * 10;
                        decimal recW = decimal.Parse(StockOutDetails.FReceiveTemperature.ToString());

                        decimal v = Convert.ToDecimal(StockOutDetails.FDeviceValue.ToString());

                        string fmeno = StockOutDetails.FMemo;
                        if (!string.IsNullOrWhiteSpace(fmeno))
                        {
                            Regex reg = new Regex("[*][0-9]{0,2}[支]");
                            if (reg.IsMatch(fmeno))
                            {
                                fmeno = fmeno.Replace(reg.Match(fmeno).Value, "");
                            }

                        }
                        

                        //压缩因子
                        string FZ1 = "0";
                        string FZ2 = "0";

                        //取压缩因子系数

                        //客户专用压缩因子
                        LHFactor startFZ = new FactorService().Where(p => p.FCompanyCode == FCode && p.FTemperature == payW && p.FBar == payY).FirstOrDefault();
                        LHFactor EndFZ = new FactorService().Where(p => p.FCompanyCode == FCode && p.FTemperature == recW && p.FBar == recY).FirstOrDefault();
                        //通用压缩因子
                        LHFactor TSFZ = new FactorService().Where(p => p.FCompanyCode == "" && p.FTemperature == payW && p.FBar == payY).FirstOrDefault();
                        LHFactor TEFZ = new FactorService().Where(p => p.FCompanyCode == "" && p.FTemperature == recW && p.FBar == recY).FirstOrDefault();
                        //通用赋值
                        if (TSFZ != null)
                        {
                            FZ1 = TSFZ.FDivisor.ToString(CultureInfo.InvariantCulture);
                        }
                        if (TEFZ != null)
                        {
                            FZ2 = TEFZ.FDivisor.ToString(CultureInfo.InvariantCulture);
                        }
                        //专用赋值
                        if (startFZ != null)
                        {
                            FZ1 = startFZ.FDivisor.ToString(CultureInfo.InvariantCulture);
                        }
                        if (startFZ != null)
                        {
                            FZ2 = EndFZ.FDivisor.ToString(CultureInfo.InvariantCulture);
                        }


                        //取客户对应排管车罐号、水溶积
                        LHWaterSpace waterSpace = new WaterSpaceService().Where(p => p.FCompanyCode == FCode && p.FGCode == fmeno).FirstOrDefault();
                        if (waterSpace != null)
                        {
                            v = waterSpace.FM3;
                        }



                        //计算容积

                        string s1 = v1.FFormula.Replace("$W1", inW)//
                            .Replace("$W2", outW)
                                                .Replace("$W3", payW.ToString(CultureInfo.InvariantCulture))//
                                                .Replace("$W4", recW.ToString(CultureInfo.InvariantCulture))//
                                                .Replace("$Y3", payY.ToString(CultureInfo.InvariantCulture))//
                                                .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                                                .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                                                .Replace("$YZ1", FZ1)//
                                                .Replace("$YZ2", FZ2)//
                                                .Replace("$Y1", inY)//
                            .Replace("$Y2", outY)//
                                                .Replace("$V", v.ToString(CultureInfo.InvariantCulture));

                        decimal qty = Convert.ToDecimal(FormulaCalculator.Eval(s1));
                        StockOutDetails.FQty =decimal.Round(qty,2);

                        //计算金额
                        StockOutDetails.FAmount = StockOutDetails.FQty * StockOutDetails.FPrice;



                        //var inW = Convert.ToDecimal(StockOutDetails.FInTemperature).ToString(CultureInfo.InvariantCulture);
                        //var inY = Convert.ToDecimal(StockOutDetails.FInPressure).ToString(CultureInfo.InvariantCulture);

                        //var outW = Convert.ToDecimal(StockOutDetails.FOutTemperature).ToString(CultureInfo.InvariantCulture);
                        //var outY = Convert.ToDecimal(StockOutDetails.FOutPressure).ToString(CultureInfo.InvariantCulture); ;

                        //var payW = Convert.ToDecimal(StockOutDetails.FPayTemperature).ToString(CultureInfo.InvariantCulture);
                        //var payY = Convert.ToDecimal(StockOutDetails.FPayPressure).ToString(CultureInfo.InvariantCulture);

                        //var recW = Convert.ToDecimal(StockOutDetails.FReceiveTemperature).ToString(CultureInfo.InvariantCulture);
                        //var recY = Convert.ToDecimal(StockOutDetails.FReceivePressure).ToString(CultureInfo.InvariantCulture);

                        ////水溶积
                        //var v = Convert.ToDecimal(StockOutDetails.FDeviceValue).ToString(CultureInfo.InvariantCulture);

                        ////数量
                        //var s1 = v1.FFormula.Replace("$W1", inW)//
                        //    .Replace("$W2", outW)
                        //    .Replace("$W3", payW)//
                        //    .Replace("$W4", recW)//
                        //    .Replace("$Y1", inY)//
                        //    .Replace("$Y2", outY)//
                        //    .Replace("$Y3", payY)//
                        //    .Replace("$Y4", recY)//
                        //    .Replace("$V", v);

                        ////数量 
                        //StockOutDetails.FQty = Convert.ToDecimal(FormulaCalculator.Eval(s1));
                        ////金额
                        //StockOutDetails.FAmount = StockOutDetails.FQty * StockOutDetails.FPrice;

                    }
                }
                catch (Exception ex)
                {
                    LogUtil.ErrorLog(ex);
                    //throw;
                }
                

                StockOutDetailsService.SaveChanges();

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

                    if (StockOutDetails != null)
                    {
                        //WebControlHandler.BindObjectToControls(StockOutDetails, SimpleForm1);

                        ddlDevice.SelectedValue = StockOutDetails.FDevice;

                        txtFDeviceValue.Text = StockOutDetails.FDeviceValue.ToString();

                        var code = StockOutDetails.FItemCode;
                        var item = new ItemsService().FirstOrDefault(P=>P.FCode== code);

                        if (item != null)
                        {
                            txtFItemName.Text = item.FName;
                            txtFUnit.Text = item.FUnit;
                        }

                        txtFInPressure.Text = StockOutDetails.FInPressure.ToString();
                        txtFInTemperature.Text = StockOutDetails.FInTemperature.ToString();

                        txtFOutPressure.Text = StockOutDetails.FOutPressure.ToString();
                        txtFOutTemperature.Text = StockOutDetails.FOutTemperature.ToString();

                        txtFPayPressure.Text = StockOutDetails.FPayPressure.ToString();
                        txtFPayTemperature.Text = StockOutDetails.FPayTemperature.ToString();

                        txtFReceivePressure.Text = StockOutDetails.FReceivePressure.ToString();
                        txtFReceiveTemperature.Text = StockOutDetails.FReceiveTemperature.ToString();

                    }
                    break;
            }
        }
        #endregion

        
    }
}