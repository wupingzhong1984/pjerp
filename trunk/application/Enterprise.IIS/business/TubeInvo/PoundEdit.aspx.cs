using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.TubeInvo
{
    public partial class PoundEdit : PageBase
    {
        /// <summary>
        ///     对象ID
        /// </summary>
        protected string Key
        {
            get { return Request["KeyId"]; }
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
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LiquidRefuelService _liquidRefuelService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LiquidRefuelService LiquidRefuelService
        {
            get { return _liquidRefuelService ?? (_liquidRefuelService = new LiquidRefuelService()); }
            set { _liquidRefuelService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private SupplierService _supplierService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SupplierService SupplierService
        {
            get { return _supplierService ?? (_supplierService = new SupplierService()); }
            set { _supplierService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private VehicleService _vehicleService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected VehicleService VehicleService
        {
            get { return _vehicleService ?? (_vehicleService = new VehicleService()); }
            set { _vehicleService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private TubePlanService _tubePlanService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected TubePlanService TubePlanService
        {
            get { return _tubePlanService ?? (_tubePlanService = new TubePlanService()); }
            set { _tubePlanService = value; }
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
        private LHTubePlan _liquidPlan;

        /// <summary>
        ///     
        /// </summary>
        protected LHTubePlan TubePlan
        {
            get
            {
                return _liquidPlan ?? (_liquidPlan = TubePlanService.FirstOrDefault(p => p.KeyId == Key //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _liquidPlan = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHVehicle _vehicle;

        /// <summary>
        ///     
        /// </summary>
        protected LHVehicle Vehicle
        {
            get
            {
                return _vehicle ?? (_vehicle = VehicleService.FirstOrDefault(p => p.FNum == tbxFVehicleNum.SelectedValue //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _vehicle = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHLiquidRefuel _liquidRefuel;

        /// <summary>
        ///     
        /// </summary>
        protected LHLiquidRefuel LiquidRefuel
        {
            get
            {
                return _liquidRefuel ?? (_liquidRefuel = LiquidRefuelService.FirstOrDefault(p => p.KeyId == Key //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _liquidRefuel = value; }
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

        protected void tbxFVehicleNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbxFMargin.Text = Vehicle != null ? Vehicle.FMargin.ToString() : "0";
        }

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

        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (TubePlan != null)
            {
                TubePlan.CreateBy = ddlFSalesman.SelectedValue;
                TubePlan.FDate = txtFDate.SelectedDate;

                TubePlan.FItemCode = tbxFItemName.SelectedValue;
                TubePlan.FItemName = tbxFItemName.SelectedText;

                TubePlan.FBill = tbxFBill.SelectedValue;
                TubePlan.FVehicleNum = tbxFVehicleNum.SelectedValue;
                TubePlan.FDriver = GasHelper.GetDropDownListArrayString(tbxFDriver.SelectedItemArray);
                TubePlan.FSupercargo = tbxFSupercargo.SelectedValue;
                TubePlan.FMargin = Convert.ToDecimal(tbxFMargin.Text);
                TubePlan.FMemo = txtFMemo.Text;
                TubePlan.FT6Warehouse =ddlFWarehouse.SelectedValue;
                TubePlan.FWaterSpace = Convert.ToDecimal(txtFWaterSpace.Text);

                //--------------------------------------------
                TubePlan.FSupplierCode = txtFSupplierCode.Text;
                TubePlan.FSupplierName = tbxFSupplier.Text;
                TubePlan.FPurchasedDate = txtFPurchasedDate.Text;

                //if (!string.IsNullOrEmpty(txtFGross.Text))
                //    TubePlan.FGross = Convert.ToDecimal(txtFGross.Text);

                //if (!string.IsNullOrEmpty(txtFPacked.Text))
                //    TubePlan.FPacked = Convert.ToDecimal(txtFPacked.Text);

                if (!string.IsNullOrEmpty(tbxFPurchasedPrice.Text))
                    TubePlan.FPurchasedPrice = Convert.ToDecimal(tbxFPurchasedPrice.Text);

                //if (!string.IsNullOrEmpty(tbxFPurchasedQty.Text))
                //    TubePlan.FPurchasedQty = Convert.ToDecimal(tbxFPurchasedQty.Text);

                //------------------------------------
                TubePlan.F1Code = tbxF1Code.Text;
                TubePlan.F1Name = tbxF1Name.Text;
                TubePlan.F1Date = txbF1Date.SelectedDate;
                TubePlan.F1Time = tbxF1Time.Text;

                //if (!string.IsNullOrEmpty(txtFGross1.Text))
                //    TubePlan.FGross1 = Convert.ToDecimal(txtFGross1.Text);

                //if (!string.IsNullOrEmpty(txtFPacked1.Text))
                //    TubePlan.FPacked1 = Convert.ToDecimal(txtFPacked1.Text);

                //if (!string.IsNullOrEmpty(txtFQty1.Text))
                //    TubePlan.FQty1 = Convert.ToDecimal(txtFQty1.Text);

                if (!string.IsNullOrEmpty(tbxF1Price.Text))
                    TubePlan.F1Price = Convert.ToDecimal(tbxF1Price.Text);

                ////////////////////////////////////////////
                //if (LiquidRefuel != null)
                //{
                //    LiquidRefuel.FBeginAddress = txtFBeginAddress.Text;
                //    LiquidRefuel.FEndAddress = txtFEndAddress.Text;

                //    if (!string.IsNullOrEmpty(txtFBeginMileage.Text))
                //        LiquidRefuel.FBeginMileage = Convert.ToDecimal(txtFBeginMileage.Text);

                //    if (!string.IsNullOrEmpty(txtFEndMileage.Text))
                //        LiquidRefuel.FEndMileage = Convert.ToDecimal(txtFEndMileage.Text);

                //    if (!string.IsNullOrEmpty(txtFQty.Text))
                //        LiquidRefuel.FQty = Convert.ToDecimal(txtFQty.Text);

                //    if (!string.IsNullOrEmpty(txtFPrice.Text))
                //        LiquidRefuel.FPrice = Convert.ToDecimal(txtFPrice.Text);

                //    if (!string.IsNullOrEmpty(txtFAmount.Text))
                //        LiquidRefuel.FAmount = Convert.ToDecimal(txtFAmount.Text);

                //    if (!string.IsNullOrEmpty(txtFOtherAmount.Text))
                //        LiquidRefuel.FOtherAmount = Convert.ToDecimal(txtFOtherAmount.Text);

                //    LiquidRefuelService.SaveChanges();
                //}

                if (!string.IsNullOrEmpty(txtFMileage.Text))
                    TubePlan.FMileage = Convert.ToDecimal(txtFMileage.Text);

                //TubePlan.FAmt = TubePlan.FPurchasedQty * TubePlan.FPurchasedPrice;
                //TubePlan.FAmt1 = TubePlan.FPrice1 * TubePlan.FQty1;
                //TubePlan.FAmt2 = TubePlan.FPrice2 * TubePlan.FQty2;
                //TubePlan.FAmt3 = TubePlan.FPrice3 * TubePlan.FQty3;


                TubePlan.FSupplierInW = Convert.ToDecimal(tbxFSupplierInW.Text);
                TubePlan.FSupplierInY = Convert.ToDecimal(tbxFSupplierInY.Text);
                TubePlan.FSupplierOutW = Convert.ToDecimal(tbxFSupplierOutW.Text);
                TubePlan.FSupplierOutY = Convert.ToDecimal(tbxFSupplierOutY.Text);
                TubePlan.FSupplierRecW = Convert.ToDecimal(tbxFSupplierRecW.Text);
                TubePlan.FSupplierRecY = Convert.ToDecimal(tbxFSupplierRecY.Text);
                TubePlan.FSupplierPayW = Convert.ToDecimal(tbxFSupplierPayW.Text);
                TubePlan.FSupplierPayY = Convert.ToDecimal(tbxFSupplierPayY.Text);

                //-------------------------------------------------------------------------
                TubePlan.F1InW = Convert.ToDecimal(tbxF1InW.Text);
                TubePlan.F1InY = Convert.ToDecimal(tbxF1InY.Text);
                TubePlan.F1OutW = Convert.ToDecimal(tbxF1OutW.Text);
                TubePlan.F1OutY = Convert.ToDecimal(tbxF1OutY.Text);
                TubePlan.F1RecW = Convert.ToDecimal(tbxF1RecW.Text);
                TubePlan.F1RecY = Convert.ToDecimal(tbxF1RecY.Text);
                TubePlan.F1PayW = Convert.ToDecimal(tbxF1PayW.Text);
                TubePlan.F1PayY = Convert.ToDecimal(tbxF1PayY.Text);
                //-------------------------------------------------------------------------
                TubePlan.F1Address = tbxF1Address.Text.Trim();
                ////余量
                //LiquidPlan.FMarginEnd = Convert.ToDecimal(tbxFMargin.Text) + Convert.ToDecimal(tbxFPurchasedQty.Text) //
                //                        - Convert.ToDecimal(txtFQty1.Text) - Convert.ToDecimal(txtFQty2.Text) -
                //                        Convert.ToDecimal(txtFQty3.Text);
                TubePlan.FDistributionPoint = ddlFDistributionPoint.SelectedValue;


                TubePlan.FSupplierPay = string.IsNullOrEmpty(ddlFSupplierPay.SelectedText)?0: Convert.ToInt32(ddlFSupplierPay.SelectedValue);
                TubePlan.F1Way = string.IsNullOrEmpty(ddlF1Way.SelectedText) ? 0 : Convert.ToInt32(ddlF1Way.SelectedValue);

                TubePlanService.SaveChanges();

                //var parms = new Dictionary<string, object>();
                //parms.Clear();

                //parms.Add("@companyId", CurrentUser.AccountComId);
                //parms.Add("@keyid", Key);

                //SqlService.ExecuteProcedureCommand("proc_LiquidMargin", parms);

                return true;
            }

            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var liquidPlan = new LHTubePlan
            {
                CreateBy = CurrentUser.AccountName
            };

            return TubePlanService.Add(liquidPlan);
        }

        protected void tbxFSupplier_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFSupplier.Text.Trim()))
            {
                Window2.Hidden = true;

                var custmoer = SupplierService.Where(p => p.FName == tbxFSupplier.Text.Trim()//
                    && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFSupplierCode.Text = custmoer.FCode;

                    decimal price = GasHelper.GeSupplierPrice(custmoer.FCode, //
                              tbxFItemName.SelectedValue, CurrentUser.AccountComId);

                    tbxFPurchasedPrice.Text = price.ToString(CultureInfo.InvariantCulture);

                    GasHelper.DropDownListTubePriceDataBind(ddlFSupplierPay, tbxFSupplier.Text.Trim(),"采购");

                    var datasuppliercode = custmoer.FCode;

                    if (!string.IsNullOrEmpty(txtFSupplierCode.Text.Trim()))
                    {
                        //计算公式
                        var v1 = TubePriceService.Where(p => p.FBill == "采购"//
                            && p.FCode == datasuppliercode//
                            && p.FItemCode == tbxFItemName.SelectedValue).FirstOrDefault();

                        var inW = Convert.ToDecimal(tbxFSupplierInW.Text).ToString(CultureInfo.InvariantCulture);
                        var inY = Convert.ToDecimal(tbxFSupplierInY.Text).ToString(CultureInfo.InvariantCulture);

                        var outW = Convert.ToDecimal(tbxFSupplierOutW.Text).ToString(CultureInfo.InvariantCulture);
                        var outY = Convert.ToDecimal(tbxFSupplierOutY.Text).ToString(CultureInfo.InvariantCulture); ;

                        var payW = Convert.ToDecimal(tbxFSupplierPayW.Text).ToString(CultureInfo.InvariantCulture);
                        var payY = Convert.ToDecimal(tbxFSupplierPayY.Text).ToString(CultureInfo.InvariantCulture);

                        var recW = Convert.ToDecimal(tbxFSupplierRecW.Text).ToString(CultureInfo.InvariantCulture);
                        var recY = Convert.ToDecimal(tbxFSupplierRecY.Text).ToString(CultureInfo.InvariantCulture);

                        //水溶积
                        var v = Convert.ToDecimal(txtFWaterSpace.Text).ToString(CultureInfo.InvariantCulture);
                        //<p>$W1：进厂温度、$W2：出厂温度、$W3:交付温度、$W4:接收温度</p>
                        //<p>$Y1：进厂压力、$Y2：出厂压力、$Y2:交付压力、$Y2:接收压力</p>
                        //<p>$V:水容积</p>

                        if (v1 != null)
                        {
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

                            var qty = Convert.ToDecimal(FormulaCalculator.Eval(s1));

                            tbxPurchasedQty.Text = qty.ToString();
                            //tube.FPurchasedAmt = tube.FPurchasedPrice * qty;
                        }
                    }
                }
            }
        }

        protected void tbxF1Name_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxF1Name.Text.Trim()))
            {
                var custmoer = CustomerService.Where(p => p.FName == tbxF1Name.Text.Trim() //
                    && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

                if (custmoer != null)
                {
                    tbxF1Code.Text = custmoer.FCode;

                    if (!string.IsNullOrEmpty(tbxFItemName.SelectedValue))
                    {
                        decimal price = GasHelper.GeCustomerPrice(custmoer.FCode, //
                                tbxFItemName.SelectedValue, CurrentUser.AccountComId);

                        tbxF1Price.Text = price.ToString(CultureInfo.InvariantCulture);

                        GasHelper.DropDownListTubePriceDataBind(ddlF1Way, tbxF1Code.Text.Trim(), "销售");

                        //产品
                        var dataitemcode = tbxFItemName.SelectedValue;
                        
                        if (string.IsNullOrEmpty(dataitemcode))
                        {
                            Alert.Show("请选择有效产品。", MessageBoxIcon.Information);
                            return;
                        }

                        var datacustomercode = tbxF1Code.Text.Trim();
                        if (!string.IsNullOrEmpty(datacustomercode))
                        {
                            //计算公式
                            var v1 = TubePriceService.Where(p => p.FBill == "销售"//
                                && p.FCode == datacustomercode//
                                && p.FItemCode == dataitemcode).FirstOrDefault();

                            var inW = Convert.ToDecimal(tbxF1InW).ToString(CultureInfo.InvariantCulture);
                            var inY = Convert.ToDecimal(tbxF1InY).ToString(CultureInfo.InvariantCulture);

                            var outW = Convert.ToDecimal(tbxF1OutW).ToString(CultureInfo.InvariantCulture);
                            var outY = Convert.ToDecimal(tbxF1OutY).ToString(CultureInfo.InvariantCulture); ;

                            var payW = Convert.ToDecimal(tbxF1PayW).ToString(CultureInfo.InvariantCulture);
                            var payY = Convert.ToDecimal(tbxF1PayY).ToString(CultureInfo.InvariantCulture);

                            var recW = Convert.ToDecimal(tbxF1RecW).ToString(CultureInfo.InvariantCulture);
                            var recY = Convert.ToDecimal(tbxF1RecY).ToString(CultureInfo.InvariantCulture);

                            //水溶积
                            var v = Convert.ToDecimal(txtFWaterSpace.Text).ToString(CultureInfo.InvariantCulture);
                            //<p>$W1：进厂温度、$W2：出厂温度、$W3:交付温度、$W4:接收温度</p>
                            //<p>$Y1：进厂压力、$Y2：出厂压力、$Y2:交付压力、$Y2:接收压力</p>
                            //<p>$V:水容积</p>

                            if (v1 != null)
                            {
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

                                var qty = Convert.ToDecimal(FormulaCalculator.Eval(s1));

                                tbxF1Qty.Text = qty.ToString(CultureInfo.InvariantCulture);
                                //tube.F1Amt = tube.F1Price * qty;
                            }
                        }
                    }
                }
            }
        }

        //protected void tbxFName2_OnTextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(tbxFName2.Text.Trim()))
        //    {
        //        var custmoer = CustomerService.Where(p => p.FName == tbxFName2.Text.Trim() //
        //            && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
        //        if (custmoer != null)
        //        {
        //            tbxFCode2.Text = custmoer.FCode;

        //            if (!string.IsNullOrEmpty(tbxFItemName.SelectedValue))
        //            {
        //                decimal price = GasHelper.GeCustomerPrice(tbxFCode2.Text.Trim(), //
        //                        tbxFItemName.SelectedValue, CurrentUser.AccountComId);

        //                txtFPrice2.Text = price.ToString(CultureInfo.InvariantCulture);
        //            }
        //        }
        //    }
        //}

        //protected void tbxFName3_OnTextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(tbxFName3.Text.Trim()))
        //    {
        //        var custmoer = CustomerService.Where(p => p.FName == tbxFName3.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
        //        if (custmoer != null)
        //        {
        //            tbxFCode3.Text = custmoer.FCode;


        //            if (!string.IsNullOrEmpty(tbxFItemName.SelectedValue))
        //            {
        //                decimal price = GasHelper.GeCustomerPrice(tbxFCode3.Text.Trim(), //
        //                        tbxFItemName.SelectedValue, CurrentUser.AccountComId);

        //                txtFPrice3.Text = price.ToString(CultureInfo.InvariantCulture);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            GasHelper.DropDownListTubeDataBind(tbxFItemName);

            GasHelper.DropDownListVehicleNumDataBind(tbxFVehicleNum);

            GasHelper.DropDownListDriverDataBind(tbxFDriver);

            GasHelper.DropDownListSupercargoDataBind(tbxFSupercargo);

            GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

            //GasHelper.DropDownListSupplierDataBind(tbxFSupplierName);

            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区

            //GasHelper.DropDownListCustomerDataBind(tbxFName1);

            //GasHelper.DropDownListCustomerDataBind(tbxFName2);

            //GasHelper.DropDownListCustomerDataBind(tbxFName3);

            GasHelper.DropDownListWarehouseInfoDataBind(ddlFWarehouse);

            tbxFSupplier.OnClientTriggerClick = Window1.GetSaveStateReference(txtFSupplierCode.ClientID, tbxFSupplier.ClientID)
                    + Window1.GetShowReference("../../Common/WinSupplier.aspx");

            tbxF1Name.OnClientTriggerClick = Window2.GetSaveStateReference(tbxF1Code.ClientID, tbxF1Name.ClientID)
                    + Window2.GetShowReference("../../Common/WinCustomer.aspx");

            //tbxFName2.OnClientTriggerClick = Window2.GetSaveStateReference(tbxFCode2.ClientID, tbxFName2.ClientID)
            //        + Window2.GetShowReference("../../Common/WinCustomer.aspx");

            //tbxFName3.OnClientTriggerClick = Window2.GetSaveStateReference(tbxFCode3.ClientID, tbxFName3.ClientID)
            //        + Window2.GetShowReference("../../Common/WinCustomer.aspx");

            txbF1Date.SelectedDate = DateTime.Now;
            //FDate2.SelectedDate = DateTime.Now;
            //FDate3.SelectedDate = DateTime.Now;

            tbxF1InW.Text = "0";
            tbxF1InY.Text = "0";
            tbxF1OutW.Text = "0";
            tbxF1OutY.Text = "0";
            tbxF1PayW.Text = "0";
            tbxF1PayY.Text = "0";
            tbxF1RecW.Text = "0";
            tbxF1RecY.Text = "0";

            tbxFSupplierInW.Text = "0";
            tbxFSupplierInY.Text = "0";
            tbxFSupplierOutY.Text = "0";
            tbxFSupplierPayW.Text = "0";
            tbxFSupplierPayY.Text = "0";
            tbxFSupplierRecW.Text = "0";
            tbxFSupplierRecY.Text = "0";

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
                    if (TubePlan != null)
                    {
                        ddlFSalesman.SelectedValue = TubePlan.CreateBy;
                        tbxF1Time.Text = TubePlan.F1Time;

                        txtKeyId.Text = TubePlan.KeyId;
                        txtFDate.SelectedDate = TubePlan.FDate;
                        tbxFItemName.SelectedValue = TubePlan.FItemCode;
                        tbxFBill.SelectedValue = TubePlan.FBill;
                        tbxFVehicleNum.SelectedValue = TubePlan.FVehicleNum;
                        tbxFDriver.SelectedValue = TubePlan.FDriver;
                        tbxFSupercargo.SelectedValue = TubePlan.FSupercargo;
                        tbxFMargin.Text = TubePlan.FMargin.ToString();
                        txtFMemo.Text = TubePlan.FMemo;

                        //-------------
                        if (LiquidRefuel != null)
                        {
                            txtFBeginAddress.Text = LiquidRefuel.FBeginAddress;
                            txtFEndAddress.Text = LiquidRefuel.FEndAddress;
                            txtFBeginMileage.Text = LiquidRefuel.FBeginMileage.ToString();
                            txtFEndMileage.Text = LiquidRefuel.FEndMileage.ToString();
                            txtFQty.Text = LiquidRefuel.FEndMileage.ToString();
                            txtFPrice.Text = LiquidRefuel.FEndMileage.ToString();
                            txtFAmount.Text = LiquidRefuel.FEndMileage.ToString();
                            txtFOtherAmount.Text = LiquidRefuel.FEndMileage.ToString();
                        }

                        //--------------------------------------------
                        txtFSupplierCode.Text = TubePlan.FSupplierCode;
                        tbxFSupplier.Text = TubePlan.FSupplierName;
                        txtFPurchasedDate.Text = TubePlan.FPurchasedDate;
                        tbxFPurchasedPrice.Text = TubePlan.FPurchasedPrice.ToString();
                        //------------------------------------
                        tbxF1Code.Text = TubePlan.F1Code;
                        tbxF1Name.Text = TubePlan.F1Name;
                        txbF1Date.SelectedDate = TubePlan.F1Date;
                        tbxF1Price.Text = TubePlan.F1Price.ToString();

                        //------------------------------------
                        txtFMileage.Text = TubePlan.FMileage.ToString();

                        if (!string.IsNullOrEmpty(TubePlan.FDriver))
                            tbxFDriver.SelectedValueArray = TubePlan.FDriver.Split(',');

                        tbxF1Address.Text = TubePlan.F1Address;
                        tbxF1InW.Text = TubePlan.F1InW==null?"0": TubePlan.F1InW.ToString();
                        tbxF1InY.Text = TubePlan.F1InY == null ? "0" : TubePlan.F1InY.ToString();
                        tbxF1OutW.Text = TubePlan.F1OutW == null ? "0" : TubePlan.F1OutW.ToString();
                        tbxF1OutY.Text = TubePlan.F1OutY == null ? "0" : TubePlan.F1OutY.ToString();
                        tbxF1RecW.Text = TubePlan.F1RecW == null ? "0" : TubePlan.F1RecW.ToString();
                        tbxF1RecY.Text = TubePlan.F1RecY == null ? "0" : TubePlan.F1RecY.ToString();
                        tbxF1PayW.Text = TubePlan.F1PayW == null ? "0" : TubePlan.F1PayW.ToString();
                        tbxF1PayY.Text = TubePlan.F1PayY == null ? "0" : TubePlan.F1PayY.ToString();

                        tbxFSupplierInW.Text = TubePlan.FSupplierInW == null ? "0" : TubePlan.FSupplierInW.ToString();
                        tbxFSupplierInY.Text = TubePlan.FSupplierInY == null ? "0" : TubePlan.FSupplierInY.ToString();
                        tbxFSupplierOutW.Text = TubePlan.FSupplierOutW == null ? "0" : TubePlan.FSupplierOutW.ToString();
                        tbxFSupplierOutY.Text = TubePlan.FSupplierOutY == null ? "0" : TubePlan.FSupplierOutY.ToString();
                        tbxFSupplierRecW.Text = TubePlan.FSupplierRecW == null ? "0" : TubePlan.FSupplierRecW.ToString();
                        tbxFSupplierRecY.Text = TubePlan.FSupplierRecY == null ? "0" : TubePlan.FSupplierRecY.ToString();
                        tbxFSupplierPayW.Text = TubePlan.FSupplierPayW == null ? "0" : TubePlan.FSupplierPayW.ToString();
                        tbxFSupplierPayY.Text = TubePlan.FSupplierPayY == null ? "0" : TubePlan.FSupplierPayY.ToString();
                        txtFWaterSpace.Text = TubePlan.FWaterSpace.ToString();

                        ddlFDistributionPoint.SelectedValue = TubePlan.FDistributionPoint;
                        ddlFWarehouse.SelectedValue = TubePlan.FT6Warehouse;

                        GasHelper.DropDownListTubePriceDataBind(ddlF1Way, tbxF1Code.Text.Trim(), "销售");
                        GasHelper.DropDownListTubePriceDataBind(ddlFSupplierPay, tbxFSupplier.Text.Trim(), "采购");

                        ddlF1Way.SelectedValue = TubePlan.F1Way.ToString();
                        ddlFSupplierPay.SelectedValue = TubePlan.FSupplierPay.ToString();

                    }


                    break;
            }
        }
        #endregion
    }
}