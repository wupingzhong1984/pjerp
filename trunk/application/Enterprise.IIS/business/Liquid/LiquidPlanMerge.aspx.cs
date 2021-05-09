using System;
using System.Collections.Generic;
using System.Linq;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.Liquid
{
    public partial class LiquidPlanMerge : PageBase
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
        private LiquidPlanService _liquidPlanService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LiquidPlanService LiquidPlanService
        {
            get { return _liquidPlanService ?? (_liquidPlanService = new LiquidPlanService()); }
            set { _liquidPlanService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHLiquidPlan _liquidPlan;

        /// <summary>
        ///     
        /// </summary>
        protected LHLiquidPlan LiquidPlan
        {
            get
            {
                return _liquidPlan ?? (_liquidPlan = LiquidPlanService.FirstOrDefault(p => p.KeyId == Key //
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
            if (Vehicle != null)
            {
                tbxFMargin.Text = Vehicle.FMargin.ToString();
            }
            else
            {
                tbxFMargin.Text = "0";
            }
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
            if (LiquidPlan != null)
            {

                //    LiquidPlan.FDate = txtFDate.SelectedDate;

                //    LiquidPlan.FDate4 = FDate4.SelectedDate;

                //    LiquidPlan.FItemCode = tbxFItemName.SelectedValue;
                //    LiquidPlan.FItemName = tbxFItemName.SelectedText;

                //    LiquidPlan.FBill = tbxFBill.SelectedValue;
                //    LiquidPlan.FVehicleNum = tbxFVehicleNum.SelectedValue;
                //    LiquidPlan.FDriver = GasHelper.GetDropDownListArrayString(tbxFDriver.SelectedItemArray);
                //    LiquidPlan.FSupercargo = tbxFSupercargo.SelectedValue;
                //    LiquidPlan.FMargin = Convert.ToDecimal(tbxFMargin.Text);
                //    LiquidPlan.FMemo = txtFMemo.Text;

                //    //--------------------------------------------
                //    LiquidPlan.FSupplierCode = txtFSupplierCode.Text;
                //    LiquidPlan.FSupplierName = tbxFSupplier.Text;
                //    LiquidPlan.FPurchasedDate = txtFPurchasedDate.Text;

                //    if (!string.IsNullOrEmpty(txtFGross.Text))
                //        LiquidPlan.FGross = Convert.ToDecimal(txtFGross.Text);

                //    if (!string.IsNullOrEmpty(txtFPacked.Text))
                //        LiquidPlan.FPacked = Convert.ToDecimal(txtFPacked.Text);

                //    if (!string.IsNullOrEmpty(tbxFPurchasedPrice.Text))
                //        LiquidPlan.FPurchasedPrice = Convert.ToDecimal(tbxFPurchasedPrice.Text);

                //    if (!string.IsNullOrEmpty(tbxFPurchasedQty.Text))
                //        LiquidPlan.FPurchasedQty = Convert.ToDecimal(tbxFPurchasedQty.Text);

                //    //------------------------------------
                //    LiquidPlan.FCode1 = tbxFCode1.Text;
                //    LiquidPlan.FName1 = tbxFName1.Text;
                //    LiquidPlan.FDate1 = FDate1.SelectedDate;
                //    LiquidPlan.FTime1 = FTime1.Text;

                //    if (!string.IsNullOrEmpty(txtFGross1.Text))
                //        LiquidPlan.FGross1 = Convert.ToDecimal(txtFGross1.Text);

                //    if (!string.IsNullOrEmpty(txtFPacked1.Text))
                //        LiquidPlan.FPacked1 = Convert.ToDecimal(txtFPacked1.Text);

                //    if (!string.IsNullOrEmpty(txtFQty1.Text))
                //        LiquidPlan.FQty1 = Convert.ToDecimal(txtFQty1.Text);

                //    if (!string.IsNullOrEmpty(txtFPrice1.Text))
                //        LiquidPlan.FPrice1 = Convert.ToDecimal(txtFPrice1.Text);

                //    //------------------------------------
                //    LiquidPlan.FCode2 = tbxFCode2.Text;
                //    LiquidPlan.FName2 = tbxFName2.Text;
                //    LiquidPlan.FDate2 = FDate2.SelectedDate;
                //    LiquidPlan.FTime2 = FTime2.Text;
                //    if (!string.IsNullOrEmpty(txtFGross2.Text))
                //        LiquidPlan.FGross2 = Convert.ToDecimal(txtFGross2.Text);

                //    if (!string.IsNullOrEmpty(txtFPacked2.Text))
                //        LiquidPlan.FPacked2 = Convert.ToDecimal(txtFPacked2.Text);

                //    if (!string.IsNullOrEmpty(txtFQty2.Text))
                //        LiquidPlan.FQty2 = Convert.ToDecimal(txtFQty2.Text);

                //    if (!string.IsNullOrEmpty(txtFPrice2.Text))
                //        LiquidPlan.FPrice2 = Convert.ToDecimal(txtFPrice2.Text);

                //    //------------------------------------
                //    LiquidPlan.FCode3 = tbxFCode3.Text;
                //    LiquidPlan.FName3 = tbxFName3.Text;
                //    LiquidPlan.FDate3 = FDate3.SelectedDate;
                //    LiquidPlan.FTime3 = FTime3.Text;
                //    if (!string.IsNullOrEmpty(txtFGross3.Text))
                //        LiquidPlan.FGross3 = Convert.ToDecimal(txtFGross3.Text);

                //    if (!string.IsNullOrEmpty(txtFPacked3.Text))
                //        LiquidPlan.FPacked3 = Convert.ToDecimal(txtFPacked3.Text);

                //    if (!string.IsNullOrEmpty(txtFQty3.Text))
                //        LiquidPlan.FQty3 = Convert.ToDecimal(txtFQty3.Text);

                //    if (!string.IsNullOrEmpty(txtFPrice3.Text))
                //        LiquidPlan.FPrice3 = Convert.ToDecimal(txtFPrice3.Text);


                //    ////////////////////////////////////////////
                //    if (LiquidRefuel != null)
                //    {
                //        LiquidRefuel.FBeginAddress = txtFBeginAddress.Text;
                //        LiquidRefuel.FEndAddress = txtFEndAddress.Text;

                //        if (!string.IsNullOrEmpty(txtFBeginMileage.Text))
                //            LiquidRefuel.FBeginMileage = Convert.ToDecimal(txtFBeginMileage.Text);

                //        if (!string.IsNullOrEmpty(txtFEndMileage.Text))
                //            LiquidRefuel.FEndMileage = Convert.ToDecimal(txtFEndMileage.Text);

                //        if (!string.IsNullOrEmpty(txtFQty.Text))
                //            LiquidRefuel.FQty = Convert.ToDecimal(txtFQty.Text);

                //        if (!string.IsNullOrEmpty(txtFPrice.Text))
                //            LiquidRefuel.FPrice = Convert.ToDecimal(txtFPrice.Text);

                //        if (!string.IsNullOrEmpty(txtFAmount.Text))
                //            LiquidRefuel.FAmount = Convert.ToDecimal(txtFAmount.Text);

                //        if (!string.IsNullOrEmpty(txtFOtherAmount.Text))
                //            LiquidRefuel.FOtherAmount = Convert.ToDecimal(txtFOtherAmount.Text);

                //        LiquidRefuelService.SaveChanges();
                //    }

                //    if (!string.IsNullOrEmpty(txtFMileage.Text))
                //        LiquidPlan.FMileage = Convert.ToDecimal(txtFMileage.Text);

                //    LiquidPlan.FAmt = LiquidPlan.FPurchasedQty * LiquidPlan.FPurchasedPrice;
                //    LiquidPlan.FAmt1 = LiquidPlan.FPrice1 * LiquidPlan.FQty1;
                //    LiquidPlan.FAmt2 = LiquidPlan.FPrice2 * LiquidPlan.FQty2;
                //    LiquidPlan.FAmt3 = LiquidPlan.FPrice3 * LiquidPlan.FQty3;

                //    LiquidPlanService.SaveChanges();

                //    var parms = new Dictionary<string, object>();
                //    parms.Clear();

                //    parms.Add("@companyId", CurrentUser.AccountComId);
                //    parms.Add("@keyid", Key);

                //    SqlService.ExecuteProcedureCommand("proc_LiquidMargin", parms);
                //}
            }

            return true;
        }

        private void BindDataGrid()
        {
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@companyId", CurrentUser.AccountComId);

            var data = SqlService.ExecuteProcedureCommand("proc_LiquidPlanMerge", parms);

            var list1 = data.Tables[0];
            var list2 = data.Tables[1];


            //绑定数据源
            Grid1.DataSource = list1;
            Grid1.DataBind();

            Grid2.DataSource = list2;
            Grid2.DataBind();
        }

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private string Grid1SelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;
            
            for (int i = 0; i < selections.Length; i++)
            {
                return Grid1.DataKeys[selections[i]][0].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> Grid2SelectIds()
        {
            int[] selections = Grid2.SelectedRowIndexArray;

            var selectIds = new string[3];//selections.Length

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Grid2.DataKeys[selections[i]][0].ToString();
            }
            return selectIds;
        }


        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var liquidPlan = new LHLiquidPlan();

            string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), //
                "LP", CurrentUser.AccountComId);

            liquidPlan.FCompanyId = CurrentUser.AccountComId;
            liquidPlan.KeyId = newKeyId;
            liquidPlan.CreateBy = CurrentUser.AccountName;
            liquidPlan.FDate = txtFDate.SelectedDate;
            liquidPlan.FItemCode = tbxFItemName.SelectedValue;
            liquidPlan.FItemName = tbxFItemName.SelectedText;
            liquidPlan.FBill = tbxFBill.SelectedValue;
            liquidPlan.FVehicleNum = tbxFVehicleNum.SelectedValue;
            liquidPlan.FDriver = GasHelper.GetDropDownListArrayString(tbxFDriver.SelectedItemArray);
            liquidPlan.FSupercargo = tbxFSupercargo.SelectedValue;
            liquidPlan.FMargin = Convert.ToDecimal(tbxFMargin.Text);
            liquidPlan.FMemo = txtFMemo.Text;
            
            LiquidPlanService.Add(liquidPlan);

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@keyId",newKeyId);
            parms.Add("@companyId", CurrentUser.AccountComId);
            parms.Add("@keyId1",Grid1SelectIds());
            parms.Add("@keyId2", Grid2SelectIds().ToList()[0]);
            parms.Add("@keyId3", Grid2SelectIds().ToList()[1]);
            parms.Add("@keyId4", Grid2SelectIds().ToList()[2]);

            SqlService.ExecuteProcedureCommand("proc_LiquidPlanMergeProc",parms);


            return true;
        }

        protected void tbxFSupplier_OnTextChanged(object sender, EventArgs e)
        {
            //    if (!string.IsNullOrEmpty(tbxFSupplier.Text.Trim()))
            //    {
            //        Window2.Hidden = true;

            //        var custmoer = SupplierService.Where(p => p.FName == tbxFSupplier.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
            //        if (custmoer != null)
            //        {
            //            txtFSupplierCode.Text = custmoer.FCode;

            //            decimal price = GasHelper.GeSupplierPrice(tbxFSupplier.Text.Trim(), //
            //                      tbxFItemName.SelectedValue, CurrentUser.AccountComId);

            //            tbxFPurchasedPrice.Text = price.ToString();

            //        }
            //    }
        }

        protected void tbxFName1_OnTextChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(tbxFName1.Text.Trim()))
            //{

            //    var custmoer = CustomerService.Where(p => p.FName == tbxFName1.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
            //    if (custmoer != null)
            //    {
            //        tbxFCode1.Text = custmoer.FCode;

            //        if (!string.IsNullOrEmpty(tbxFItemName.SelectedValue))
            //        {
            //            decimal price = GasHelper.GeCustomerPrice(tbxFCode1.Text.Trim(), //
            //                    tbxFItemName.SelectedValue, CurrentUser.AccountComId);

            //            txtFPrice1.Text = price.ToString(CultureInfo.InvariantCulture);
            //        }
            //    }
            //}
        }

        protected void tbxFName2_OnTextChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(tbxFName2.Text.Trim()))
            //{
            //    var custmoer = CustomerService.Where(p => p.FName == tbxFName2.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
            //    if (custmoer != null)
            //    {
            //        tbxFCode2.Text = custmoer.FCode;


            //        if (!string.IsNullOrEmpty(tbxFItemName.SelectedValue))
            //        {
            //            decimal price = GasHelper.GeCustomerPrice(tbxFCode2.Text.Trim(), //
            //                    tbxFItemName.SelectedValue, CurrentUser.AccountComId);

            //            txtFPrice2.Text = price.ToString(CultureInfo.InvariantCulture);
            //        }
            //    }
            //}
        }

        protected void tbxFName3_OnTextChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(tbxFName3.Text.Trim()))
            //{
            //    var custmoer = CustomerService.Where(p => p.FName == tbxFName3.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
            //    if (custmoer != null)
            //    {
            //        tbxFCode3.Text = custmoer.FCode;


            //        if (!string.IsNullOrEmpty(tbxFItemName.SelectedValue))
            //        {
            //            decimal price = GasHelper.GeCustomerPrice(tbxFCode3.Text.Trim(), //
            //                    tbxFItemName.SelectedValue, CurrentUser.AccountComId);

            //            txtFPrice3.Text = price.ToString(CultureInfo.InvariantCulture);
            //        }
            //    }
            //}
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            GasHelper.DropDownListLiquidDataBind(tbxFItemName);

            GasHelper.DropDownListVehicleNumDataBind(tbxFVehicleNum);

            GasHelper.DropDownListDriverDataBind(tbxFDriver);

            GasHelper.DropDownListSupercargoDataBind(tbxFSupercargo);

            txtFDate.SelectedDate = DateTime.Now;
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            BindDataGrid();

            switch (Actions)
            {
                case WebAction.Add:

                    break;
                case WebAction.Edit:
                    if (LiquidPlan != null)
                    {
                        txtKeyId.Text = LiquidPlan.KeyId;
                        txtFDate.SelectedDate = LiquidPlan.FDate;
                        tbxFItemName.SelectedValue = LiquidPlan.FItemCode;
                        tbxFBill.SelectedValue = LiquidPlan.FBill;
                        tbxFVehicleNum.SelectedValue = LiquidPlan.FVehicleNum;
                        tbxFDriver.SelectedValue = LiquidPlan.FDriver;
                        tbxFSupercargo.SelectedValue = LiquidPlan.FSupercargo;
                        tbxFMargin.Text = LiquidPlan.FMargin.ToString();
                        txtFMemo.Text = LiquidPlan.FMemo;

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
                        //txtFSupplierCode.Text = LiquidPlan.FSupplierCode;
                        //tbxFSupplier.Text = LiquidPlan.FSupplierName;
                        //txtFPurchasedDate.Text = LiquidPlan.FPurchasedDate;
                        //txtFGross.Text = LiquidPlan.FGross.ToString();
                        //txtFPacked.Text = LiquidPlan.FPacked.ToString();
                        //tbxFPurchasedPrice.Text = LiquidPlan.FPurchasedPrice.ToString();
                        //tbxFPurchasedQty.Text = LiquidPlan.FPurchasedQty.ToString();

                        ////------------------------------------
                        //tbxFCode1.Text = LiquidPlan.FCode1;
                        //tbxFName1.Text = LiquidPlan.FName1;
                        //FDate1.SelectedDate = LiquidPlan.FDate1;
                        //txtFGross1.Text = LiquidPlan.FGross1.ToString();
                        //txtFPacked1.Text = LiquidPlan.FPacked1.ToString();
                        //txtFQty1.Text = LiquidPlan.FQty1.ToString();
                        //txtFPrice1.Text = LiquidPlan.FPrice1.ToString();

                        ////------------------------------------
                        //tbxFCode2.Text = LiquidPlan.FCode2;
                        //tbxFName2.Text = LiquidPlan.FName2;
                        //FDate2.SelectedDate = LiquidPlan.FDate2;
                        //txtFGross2.Text = LiquidPlan.FGross2.ToString();
                        //txtFPacked2.Text = LiquidPlan.FPacked2.ToString();
                        //txtFQty2.Text = LiquidPlan.FQty2.ToString();
                        //txtFPrice2.Text = LiquidPlan.FPrice2.ToString();

                        ////------------------------------------
                        //tbxFCode3.Text = LiquidPlan.FCode3;
                        //tbxFName3.Text = LiquidPlan.FName3;
                        //FDate3.SelectedDate = LiquidPlan.FDate3;
                        //txtFGross3.Text = LiquidPlan.FGross3.ToString();
                        //txtFPacked3.Text = LiquidPlan.FPacked3.ToString();
                        //txtFQty3.Text = LiquidPlan.FQty3.ToString();
                        //txtFPrice3.Text = LiquidPlan.FPrice3.ToString();
                        txtFMileage.Text = LiquidPlan.FMileage.ToString();


                        if (!string.IsNullOrEmpty(LiquidPlan.FDriver))
                            tbxFDriver.SelectedValueArray = LiquidPlan.FDriver.Split(',');
                    }

                    break;
            }
        }
        #endregion
    }
}