using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.TubeInvo
{
    /// <summary>
    ///     氢气进销
    /// </summary>
    public partial class TubePlan : PageBase
    {
        /// <summary>
        ///     AppendToEnd
        /// </summary>
        private bool AppendToEnd
        {
            get
            {
                return ViewState["_AppendToEnd"] != null && Convert.ToBoolean(ViewState["_AppendToEnd"]);
            }
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
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "KeyId";
                return sort;
            }
            set { ViewState["SortField"] = value; }
        }

        /// <summary>
        ///     排序方向
        /// </summary>
        protected string SortDirection
        {
            get
            {
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "DESC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
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
        ///     客户档案
        /// </summary>
        private LHCustomer _customer;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected LHCustomer Customer
        {
            get { return _customer ?? (_customer = CustomerService.FirstOrDefault(p => p.FCompanyId == CurrentUser.AccountComId)); }
            set { _customer = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ItemsService _itemsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ItemsService ItemsService
        {
            get { return _itemsService ?? (_itemsService = new ItemsService()); }
            set { _itemsService = value; }
        }

        /// <summary>
        ///     计算公式
        /// </summary>
        private TubePriceService _tubePriceService;

        /// <summary>
        ///     计算公式
        /// </summary>
        protected TubePriceService TubePriceService
        {
            get { return _tubePriceService ?? (_tubePriceService = new TubePriceService()); }
            set { _tubePriceService = value; }
        }

        /// <summary>
        ///     当前画面操作项
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction)Int32.Parse(s);
            }
        }

        #region Protected Method

        /// <summary>
        ///     Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///     btnSubmit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                isSucceed = SubmintAdd();

            }
            catch (Exception)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    BindDataGrid();

                    Alert.Show("保存成功！", MessageBoxIcon.Information);
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDataGrid();
        }

        /// <summary>
        ///     新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
                Alert.Show("新增失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=氢气进销{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        ///// <summary>
        /////     分页
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        //{
        //    Grid1.PageIndex = e.NewPageIndex;
        //    BindDataGrid();
        //}

        ///// <summary>
        /////     分页
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Grid1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        //    BindDataGrid();
        //}

        ///// <summary>
        /////     排序
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Grid1_Sort(object sender, GridSortEventArgs e)
        //{
        //    SortField = string.Format(@"{0}", e.SortField);
        //    SortDirection = e.SortDirection;
        //    BindDataGrid();
        //}


        protected void btnTo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("./LiquidTo.aspx?id={0}&action=2",
                            sid), "导气"));
                }
            }
            catch (Exception)
            {
                Alert.Show("导气失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     BindDataGrid
        /// </summary>
        private void BindDataGrid()
        {
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@companyId", CurrentUser.AccountComId);
            parms.Add("@KeyId", string.IsNullOrEmpty(txtFKeyId.Text.Trim()) ? "-1" : txtFKeyId.Text.Trim());
            parms.Add("@begin", Convert.ToDateTime(dateBegin.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@end", Convert.ToDateTime(dateEnd.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@ItemName", string.IsNullOrEmpty(ddlFName.SelectedText.Trim()) ? "-1" : ddlFName.SelectedText.Trim());
            parms.Add("@FBill", ddlFBillType.SelectedValue);
            parms.Add("@FDriver", string.IsNullOrEmpty(ddlFDriver.SelectedValue) ? "-1" : ddlFDriver.SelectedValue);
            parms.Add("@FSupercargo", string.IsNullOrEmpty(ddlFSupercargo.SelectedValue) ? "-1" : ddlFSupercargo.SelectedValue);
            parms.Add("@FVehicleNum", string.IsNullOrEmpty(ddlFVehicleNum.SelectedValue) ? "-1" : ddlFVehicleNum.SelectedValue);
            parms.Add("@FSupplierName", string.IsNullOrEmpty(ddlSuper.SelectedText.Trim()) ? "-1" : ddlSuper.SelectedText.Trim());
            parms.Add("@FName", string.IsNullOrEmpty(this.ddlCustomer.SelectedText.Trim()) ? "-1" : ddlCustomer.SelectedText.Trim());

            var list = SqlService.ExecuteProcedureCommand("proc_TubePlanList", parms).Tables[0];

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        /// <summary>
        ///     单元格编辑与修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            AddListGrid();

            ModifiedGrid();

            BindDataGrid();
        }

        /// <summary>
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            var commandName = e.CommandName;
            var datakey = Grid1.DataKeys[e.RowIndex][0].ToString();

            switch (commandName)
            {
                case "Delete":
                    TubePlanService.Delete(p => p.KeyId == datakey && p.FCompanyId == CurrentUser.AccountComId);

                    Alert.Show("删除成功。", MessageBoxIcon.Information);

                    break;

                case "Pound":

                    //PageContext.RegisterStartupScript(
                    //    Window1.GetShowReference(string.Format("./PoundEdit.aspx?KeyId={0}&action=2",
                    //        datakey), "填写磅单"));

                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("./PoundEdit.aspx?KeyId={0}&action=2",
                            datakey), "填写磅单"));

                    BindDataGrid();

                    break;

                case "Submit":

                    var tube =
                        TubePlanService.Where(p => p.FCompanyId == CurrentUser.AccountComId && p.KeyId == datakey)
                            .FirstOrDefault();

                    if (tube != null)
                    {
                        //产品
                        var dataitemcode = Grid1.DataKeys[e.RowIndex][2].ToString();
                        //供货商
                        var datasuppliercode = Grid1.DataKeys[e.RowIndex][3].ToString();
                        //客户
                        var datacustomercode = Grid1.DataKeys[e.RowIndex][4].ToString();

                        if (string.IsNullOrEmpty(dataitemcode))
                        {
                            Alert.Show("请选择有效产品。", MessageBoxIcon.Information);
                            return;
                        }

                        if (!string.IsNullOrEmpty(datasuppliercode))
                        {
                            //计算公式
                            var v1 = TubePriceService.Where(p => p.FBill == "采购"//
                                && p.FCode == datasuppliercode//
                                && p.FItemCode == dataitemcode).FirstOrDefault();

                            var inW = Convert.ToDecimal(tube.FSupplierInW).ToString(CultureInfo.InvariantCulture);
                            var inY = Convert.ToDecimal(tube.FSupplierInY).ToString(CultureInfo.InvariantCulture);

                            var outW = Convert.ToDecimal(tube.FSupplierOutW).ToString(CultureInfo.InvariantCulture);
                            var outY = Convert.ToDecimal(tube.FSupplierOutY).ToString(CultureInfo.InvariantCulture); ;

                            var payW = Convert.ToDecimal(tube.FSupplierPayW).ToString(CultureInfo.InvariantCulture);
                            var payY = Convert.ToDecimal(tube.FSupplierPayY).ToString(CultureInfo.InvariantCulture);

                            var recW = Convert.ToDecimal(tube.FSupplierRecW).ToString(CultureInfo.InvariantCulture);
                            var recY = Convert.ToDecimal(tube.FSupplierRecY).ToString(CultureInfo.InvariantCulture);

                            //水溶积
                            var v = Convert.ToDecimal(tube.FWaterSpace).ToString(CultureInfo.InvariantCulture);
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

                                var qty=  Convert.ToDecimal(FormulaCalculator.Eval(s1));

                                tube.FPurchasedQty = qty;
                                tube.FPurchasedAmt = tube.FPurchasedPrice*qty;
                            }
                        }


                        if (!string.IsNullOrEmpty(datacustomercode))
                        {
                            //计算公式
                            var v1 = TubePriceService.Where(p => p.FBill == "销售"//
                                && p.FCode == datacustomercode//
                                && p.FItemCode == dataitemcode).FirstOrDefault();

                            var inW = Convert.ToDecimal(tube.F1InW).ToString(CultureInfo.InvariantCulture);
                            var inY = Convert.ToDecimal(tube.F1InY).ToString(CultureInfo.InvariantCulture);

                            var outW = Convert.ToDecimal(tube.F1OutW).ToString(CultureInfo.InvariantCulture);
                            var outY = Convert.ToDecimal(tube.F1OutY).ToString(CultureInfo.InvariantCulture); ;

                            var payW = Convert.ToDecimal(tube.F1PayW).ToString(CultureInfo.InvariantCulture);
                            var payY = Convert.ToDecimal(tube.F1PayY).ToString(CultureInfo.InvariantCulture);

                            var recW = Convert.ToDecimal(tube.F1RecW).ToString(CultureInfo.InvariantCulture);
                            var recY = Convert.ToDecimal(tube.F1RecY).ToString(CultureInfo.InvariantCulture);

                            //水溶积
                            var v = Convert.ToDecimal(tube.FWaterSpace).ToString(CultureInfo.InvariantCulture);
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

                                tube.F1Qty = qty;
                                tube.F1Amt = tube.F1Price * qty;
                            }
                        }

                        //提交
                        TubePlanService.SaveChanges();

                        //自动生成采购单、发货单
                        var parms = new Dictionary<string, object>();
                        parms.Clear();

                        parms.Add("@companyId", CurrentUser.AccountComId);
                        parms.Add("@keyId", datakey);

                        SqlService.ExecuteProcedureCommand("proc_TubePlan", parms);

                        Alert.Show("磅单成功。", MessageBoxIcon.Information);

                    }
                    
                    break;


                //case "To":
                //    var dataqty = Grid1.DataKeys[e.RowIndex][1].ToString();
                //    var to =
                //        LiquidToService.FirstOrDefault(
                //            p => p.XCompanyId == CurrentUser.AccountComId && p.XKeyId == datakey);
                //    if (to != null)
                //    {
                //        PageContext.RegisterStartupScript(
                //            Window4.GetShowReference(string.Format("./LiquidTo.aspx?KeyId={0}&qty={1}&action=2",
                //                datakey, dataqty), "导气"));
                //    }
                //    else
                //    {
                //        PageContext.RegisterStartupScript(
                //            Window4.GetShowReference(string.Format("./LiquidTo.aspx?KeyId={0}&qty={1}&action=1",
                //                datakey, dataqty), "导气"));
                //    }
                //    break;

            }

            BindDataGrid();
        }

        /// <summary>
        ///     关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
        }
        protected void Window3_Close(object sender, WindowCloseEventArgs e)
        {
        }

        protected void tbxBankCode_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
        }

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
        }

        //protected void tbxFVehicleNum_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var num = tbxFVehicleNum.SelectedValue;

        //        if (num != null && !string.IsNullOrEmpty(num))
        //        {
        //            var vehicle = new VehicleService().FirstOrDefault(p => p.FNum == num);

        //            Grid1.Rows[Grid1.SelectedRowIndex].Values[10] = Convert.ToDecimal(vehicle.FMargin);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Alert.Show("选择车辆失败！", MessageBoxIcon.Warning);
        //    }
        //}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFItemCode_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     AddListGrid
        /// </summary>
        private void AddListGrid()
        {
            //新增行事件
            var dictList = Grid1.GetNewAddedList();
            foreach (var rowKey in dictList)
            {
                var sKeys = new StringBuilder();
                var sValues = new StringBuilder();
                foreach (var key in rowKey.Keys)
                {
                    sKeys.AppendFormat("{0},", key);
                }

                foreach (var dictValue in rowKey.Values)
                {
                    sValues.AppendFormat("{0},", dictValue);
                }

                var keys = sKeys.ToString().Split(',');
                var values = sValues.ToString().Split(',');

                var details = new LHTubePlan();

                for (int i = 0; i < keys.Count(); i++)
                {
                    #region 修改内容

                    var key = keys[i];
                    var value = values[i];

                    if (!string.IsNullOrEmpty(key) && key.Length > 0)
                    {
                        if (details != null)
                        {
                            #region Add

                            if (key.Equals("KeyId"))
                            {
                                details.KeyId = value;
                            }

                            if (key.Equals("FDate"))
                            {
                                details.FDate = Convert.ToDateTime(value);
                            }

                            if (key.Equals("FItemName"))//产品
                            {
                                details.FItemCode = value;

                                if (!string.IsNullOrEmpty(value))
                                    details.FItemName =
                                        ItemsService.FirstOrDefault(
                                            p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            }

                            if (key.Equals("FBill"))
                            {
                                details.FBill = value;
                            }

                            if (key.Equals("FSupercargo"))
                            {
                                details.FSupercargo = value;
                            }

                            if (key.Equals("FVehicleNum"))
                            {
                                details.FVehicleNum = value;

                                if (value != null && !string.IsNullOrEmpty(value))
                                {
                                    var vehicle = new VehicleService().FirstOrDefault(p => p.FNum == value);

                                    details.FMargin = Convert.ToDecimal(vehicle.FMargin);
                                }
                            }

                            if (key.Equals("FPurchasedDate"))
                            {
                                details.FPurchasedDate = value;
                            }

                            if (key.Equals("FSupplierName"))
                            {
                                details.FSupplierCode = value;
                                if (!string.IsNullOrEmpty(value))
                                    details.FSupplierName =
                                        SupplierService.FirstOrDefault(
                                            p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            }

                            if (key.Equals("FPurchasedPrice"))
                            {
                                details.FPurchasedPrice = Convert.ToDecimal(value);
                                //details.FAmt = details.FPurchasedPrice * details.FPurchasedQty;
                            }

                            if (key.Equals("FPurchasedQty"))
                            {
                                details.FPurchasedQty = Convert.ToDecimal(value);
                                //details.FAmt = details.FPurchasedPrice * details.FPurchasedQty;
                            }

                            //--------------------------------------

                            //if (key.Equals("FDate1"))
                            //{
                            //    details.FDate1 = Convert.ToDateTime(value);
                            //}

                            //if (key.Equals("FName1"))
                            //{
                            //    details.FCode1 = value;
                            //    if (!string.IsNullOrEmpty(value))
                            //        details.FName1 =
                            //            CustomerService.FirstOrDefault(
                            //                p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            //}

                            //if (key.Equals("FPrice1"))
                            //{
                            //    details.FPrice1 = Convert.ToDecimal(value);
                            //    details.FAmt1 = details.FPrice1 * details.FQty1;
                            //}

                            //if (key.Equals("FQty1"))
                            //{
                            //    details.FQty1 = Convert.ToDecimal(value);
                            //    details.FAmt1 = details.FPrice1 * details.FQty1;
                            //}

                            ////--------------------

                            //if (key.Equals("FDate2"))
                            //{
                            //    details.FDate2 = Convert.ToDateTime(value);
                            //}

                            //if (key.Equals("FName2"))
                            //{
                            //    details.FCode2 = value;
                            //    if (!string.IsNullOrEmpty(value))
                            //        details.FName2 =
                            //            CustomerService.FirstOrDefault(
                            //                p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            //}

                            //if (key.Equals("FPrice2"))
                            //{
                            //    details.FPrice2 = Convert.ToDecimal(value);
                            //    details.FAmt2 = details.FPrice2 * details.FQty2;
                            //}

                            //if (key.Equals("FQty2"))
                            //{
                            //    details.FQty2 = Convert.ToDecimal(value);
                            //    details.FAmt2 = details.FPrice2 * details.FQty2;
                            //}

                            ////--------------------

                            //if (key.Equals("FDat3"))
                            //{
                            //    details.FDate3 = Convert.ToDateTime(value);
                            //}

                            //if (key.Equals("FName3"))
                            //{
                            //    details.FCode3 = value;
                            //    if (!string.IsNullOrEmpty(value))
                            //        details.FName3 =
                            //            CustomerService.FirstOrDefault(
                            //                p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            //}

                            //if (key.Equals("FPrice3"))
                            //{
                            //    details.FPrice3 = Convert.ToDecimal(value);
                            //    details.FAmt3 = details.FPrice3 * details.FQty3;
                            //}

                            //if (key.Equals("FQty3"))
                            //{
                            //    details.FQty3 = Convert.ToDecimal(value);
                            //    details.FAmt3 = details.FPrice3 * details.FQty3;
                            //}
                            //--------------------

                            if (key.Equals("FMarginEnd"))
                            {
                                details.FMarginEnd = Convert.ToDecimal(value);
                            }

                            if (key.Equals("FMemo"))
                            {
                                details.FMemo = value;
                            }


                            #endregion
                        }
                    }

                    #endregion
                }

                string keyId = SequenceService.CreateSequence(Convert.ToDateTime(details.FDate), "TP", CurrentUser.AccountComId);
                details.KeyId = keyId;
                details.FDeleteFlag = 0;
                details.FType = Convert.ToInt32(GasEnumBill.TubePlan);
                details.FFlag = 1;
                details.CreateBy = CurrentUser.AccountName;
                details.FStatus = 10;

                details.FCompanyId = CurrentUser.AccountComId;
                TubePlanService.Add(details);

                var refuel = new LHLiquidRefuel
                {
                    FFlag = 1,
                    CreateBy = CurrentUser.AccountName,
                    FDate = details.FDate,
                    KeyId = details.KeyId,
                    FCompanyId = CurrentUser.AccountComId

                };
                LiquidRefuelService.Add(refuel);
            }

            Grid1.CommitChanges();
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            var item = e.DataItem as DataRowView;

            if (item != null)
                if (item["FStatus"].ToString() == "100")
                {
                    e.Values[5] = String.Format("<span class=\"{0}\">{1}</span>", "colorred", item["KeyId"]);
                }
        }


        /// <summary>
        ///     ModifiedGrid
        /// </summary>
        private void ModifiedGrid()
        {
            //编辑行事件
            var dictModified = Grid1.GetModifiedDict();
            foreach (var rowKey in dictModified.Keys)
            {
                string datakey = Grid1.DataKeys[rowKey][0].ToString();

                var sKeys = new StringBuilder();
                var sValues = new StringBuilder();
                foreach (var key in dictModified[rowKey].Keys)
                {
                    sKeys.AppendFormat("{0},", key);
                }

                foreach (var dictValue in dictModified[rowKey].Values)
                {
                    sValues.AppendFormat("{0},", dictValue);
                }

                if (sValues.ToString().Contains("-1"))
                {
                    Alert.Show("请输入有效信息！", MessageBoxIcon.Warning);
                    return;
                }

                var details = TubePlanService.Where(p => p.KeyId == datakey && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

                var keys = sKeys.ToString().Split(',');
                var values = sValues.ToString().Split(',');
                for (int i = 0; i < keys.Count(); i++)
                {
                    #region 修改内容

                    var key = keys[i];
                    var value = values[i];

                    if (!string.IsNullOrEmpty(key) && key.Length > 0)
                    {
                        if (details != null)
                        {
                            #region Edit

                            if (key.Equals("KeyId"))
                            {
                                details.KeyId = value;
                            }

                            if (key.Equals("FDate"))
                            {
                                details.FDate = Convert.ToDateTime(value);
                            }

                            if (key.Equals("FItemName"))//产品
                            {
                                details.FItemCode = value;

                                if (!string.IsNullOrEmpty(value))
                                    details.FItemName =
                                        ItemsService.FirstOrDefault(
                                            p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            }

                            if (key.Equals("FBill"))
                            {
                                details.FBill = value;
                            }

                            //if (key.Equals("FDriver"))
                            //{
                            //    details.FDriver = value;
                            //}

                            if (key.Equals("FSupercargo"))
                            {
                                details.FSupercargo = value;
                            }

                            if (key.Equals("FVehicleNum"))
                            {
                                details.FVehicleNum = value;

                                if (value != null && !string.IsNullOrEmpty(value))
                                {
                                    var vehicle = new VehicleService().FirstOrDefault(p => p.FNum == value);

                                    details.FMargin = Convert.ToDecimal(vehicle.FMargin);
                                }
                            }

                            //if (key.Equals("FMargin"))
                            //{
                            //    details.FMargin = Convert.ToDecimal(value);
                            //}

                            if (key.Equals("FPurchasedDate"))
                            {
                                details.FPurchasedDate = value;
                            }

                            if (key.Equals("FSupplierName"))
                            {
                                details.FSupplierCode = value;
                                if (!string.IsNullOrEmpty(value))
                                    details.FSupplierName =
                                        SupplierService.FirstOrDefault(
                                            p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            }

                            if (key.Equals("FPurchasedPrice"))
                            {
                                details.FPurchasedPrice = Convert.ToDecimal(value);
                            }

                            if (key.Equals("FPurchasedQty"))
                            {
                                details.FPurchasedQty = Convert.ToDecimal(value);
                            }


                            //if (key.Equals("FPurchasedPQty"))
                            //{
                            //    details.FPurchasedPQty = Convert.ToDecimal(value);
                            //}


                            //--------------------------------------

                            //if (key.Equals("FDate1"))
                            //{
                            //    details.FDate1 = Convert.ToDateTime(value);
                            //}

                            //if (key.Equals("FName1"))
                            //{
                            //    details.FCode1 = value;
                            //    if (!string.IsNullOrEmpty(value))
                            //        details.FName1 =
                            //            CustomerService.FirstOrDefault(
                            //                p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            //}

                            //if (key.Equals("FPrice1"))
                            //{
                            //    details.FPrice1 = Convert.ToDecimal(value);
                            //}

                            //if (key.Equals("FPQty1"))
                            //{
                            //    details.FPQty1 = Convert.ToDecimal(value);
                            //}

                            //if (key.Equals("FQty1"))
                            //{
                            //    details.FQty1 = Convert.ToDecimal(value);
                            //}

                            //--------------------

                            //if (key.Equals("FDate2"))
                            //{
                            //    details.FDate2 = Convert.ToDateTime(value);
                            //}

                            //if (key.Equals("FName2"))
                            //{
                            //    details.FCode2 = value;
                            //    if (!string.IsNullOrEmpty(value))
                            //        details.FName2 =
                            //            CustomerService.FirstOrDefault(
                            //                p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            //}

                            //if (key.Equals("FPrice2"))
                            //{
                            //    details.FPrice2 = Convert.ToDecimal(value);
                            //}

                            //if (key.Equals("FQty2"))
                            //{
                            //    details.FQty2 = Convert.ToDecimal(value);
                            //}

                            //if (key.Equals("FPQty2"))
                            //{
                            //    details.FPQty2 = Convert.ToDecimal(value);
                            //}

                            //--------------------

                            //if (key.Equals("FDat3"))
                            //{
                            //    details.FDate3 = Convert.ToDateTime(value);
                            //}

                            //if (key.Equals("FName3"))
                            //{
                            //    details.FCode3 = value;
                            //    if (!string.IsNullOrEmpty(value))
                            //        details.FName3 =
                            //            CustomerService.FirstOrDefault(
                            //                p => p.FCode == value && p.FCompanyId == CurrentUser.AccountComId).FName;
                            //}

                            //if (key.Equals("FPrice3"))
                            //{
                            //    details.FPrice3 = Convert.ToDecimal(value);
                            //}

                            //if (key.Equals("FPQty3"))
                            //{
                            //    details.FPQty3 = Convert.ToDecimal(value);
                            //}

                            //if (key.Equals("FQty3"))
                            //{
                            //    details.FQty3 = Convert.ToDecimal(value);
                            //}
                            //--------------------

                            if (key.Equals("FMarginEnd"))
                            {
                                details.FMarginEnd = Convert.ToDecimal(value);
                            }

                            if (key.Equals("FMemo"))
                            {
                                details.FMemo = value;
                            }


                            #endregion
                        }
                    }

                    #endregion
                }

                TubePlanService.SaveChanges();

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", CurrentUser.AccountComId);
                parms.Add("@keyid", datakey);

                SqlService.ExecuteProcedureCommand("proc_LiquidMargin", parms);
            }
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            ModifiedGrid();

            return true;
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            GasHelper.DropDownListTubeDataBind(tbxFItemName);

            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

            GasHelper.DropDownListDriverDataBind(ddlFDriver);

            GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);

            GasHelper.DropDownListVehicleNumDataBind(tbxFVehicleNum);

            GasHelper.DropDownListSupercargoDataBind(tbxFSupercargo);

            GasHelper.DropDownListSupplierDataBind(ddlSuper);

            GasHelper.DropDownListSupplierDataBind(tbxFSupplierName);

            GasHelper.DropDownListCustomerDataBind(ddlCustomer);

            GasHelper.DropDownListTubeDataBind(ddlFName);

            GasHelper.DropDownListCustomerDataBind(tbxFName1);

            //GasHelper.DropDownListCustomerDataBind(tbxFName2);

            //GasHelper.DropDownListCustomerDataBind(tbxFName3);


            //删除选中单元格的客户端脚本
            string deleteScript = DeleteScript();

            //新增
            var defaultObj = new JObject
            {
                {"KeyId", ""},
                {"FDate", DateTime.Now.ToString("yyyy-MM-dd")},
                {"FItemName", ""},
                {"FBill", "进销"},
                {"FDriver", ""},
                {"FSupercargo", ""},
                {"FMargin", "0"},
                {"FPurchasedDate", ""},
                {"FSupplierName", ""},
                {"FPurchasedQty", "0"},
                {"FPurchasedPQty", "0"},
                {"FPurchasedPrice", "0"},

                {"F1Date", DateTime.Now.ToString("yyyy-MM-dd")},
                {"F1Name", ""},
                {"F1PQty", "0"},
                {"F1Qty", "0"},
                {"F1Price", "0"},

                //{"FDate2", DateTime.Now.ToString("yyyy-MM-dd")},
                //{"FName2", ""},
                //{"FPQty2", "0"},
                //{"FQty2", "0"},
                //{"FPrice2", "0"},

                //{"FDate3", DateTime.Now.ToString("yyyy-MM-dd")},
                //{"FName3", ""},
                //{"FPQty3", "0"},
                //{"FQty3", "0"},
                //{"FPrice3", "0"},

                {"FMarginEnd", "0"},
                {"FMemo", ""},

                {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
                    deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            };

            // 在第一行新增一条数据
            btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            //btnMerge.OnClientClick = Window1.GetShowReference(string.Format("./LiquidPlanMerge.aspx?action=1"), "进销合并操作");

            dateBegin.SelectedDate = DateTime.Now.AddDays(-3);
            dateEnd.SelectedDate = DateTime.Now.AddDays(3);
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            BindDataGrid();
        }

        /// <summary>
        ///     删除选中行的脚本
        /// </summary>
        /// <returns></returns>
        private string DeleteScript()
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, Grid1.GetDeleteSelectedRowsReference(), String.Empty);
        }
        #endregion
    }
}