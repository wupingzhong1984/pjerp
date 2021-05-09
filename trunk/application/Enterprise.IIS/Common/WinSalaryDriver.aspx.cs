using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;
using FineUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;

namespace Enterprise.IIS.Common
{
    public partial class WinSalaryDriver : PageBase
    {
        private PassCardService _passCardService;

        /// <summary>
        ///     数据服务
        /// </summary>
        private BillStatusService _billStatusService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected BillStatusService BillStatusService
        {
            get { return _billStatusService ?? (_billStatusService = new BillStatusService()); }
            set { _billStatusService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private SalaryDriverService _dispatchCenterService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SalaryDriverService DispatchCenterService
        {
            get { return _dispatchCenterService ?? (_dispatchCenterService = new SalaryDriverService()); }
            set { _dispatchCenterService = value; }
        }

        /// <summary>
        ///     KeyId
        /// </summary>
        protected string KeyId
        {
            get { return Request["KeyId"]; }
        }

        /// <summary>
        ///     单据类型
        /// </summary>
        protected string Bill
        {
            get { return Request["Bill"]; }
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
        private LHSalaryDriver _dispatchCenter;

        /// <summary>
        ///     职员档案
        /// </summary>
        protected LHSalaryDriver DispatchCenter
        {
            get
            {
                return _dispatchCenter ??
                       (_dispatchCenter =
                           DispatchCenterService.FirstOrDefault(
                               p => p.KeyId == KeyId));
            }
            set { _dispatchCenter = value; }
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



        private EmployeeService _employeeService;
        /// <summary>
        /// 
        /// </summary>
        protected EmployeeService EmployeeService
        {
            get { return _employeeService ?? (_employeeService = new EmployeeService()); }
            set { _employeeService = value; }
        }

        protected PassCardService PassCardService
        {
            get { return _passCardService ?? (_passCardService = new PassCardService()); }
            set { _passCardService = value; }
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

        /// <summary>
        ///     物流选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFLogistics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLogistics.SelectedValue))
            {
                string logistic = ddlLogistics.SelectedValue;

                //车
                ddlFVehicleNum.DataSource = // p.FLogistics == logistic &&
                    VehicleService.Where(p => p.FCompanyId == CurrentUser.AccountComId && p.FISMain == "是");
                ddlFVehicleNum.DataBind();



                //司机
                ddlFDriver.DataSource =//p.FLogistics==logistic&& 
                    EmployeeService.Where(p => p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId//
                    && p.professional.Contains("司机"));
                ddlFDriver.DataBind();

                //押运员
                ddlFSupercargo.DataSource =//p.FLogistics == logistic &&
                    EmployeeService.Where(p => p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId //
                    && p.professional.Contains("押运员"));
                ddlFSupercargo.DataBind();
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
            catch (DbEntityValidationException ex)
            {
                string msg = string.Empty;
                List<ICollection<DbValidationError>> errors = (from u in ex.EntityValidationErrors select u.ValidationErrors).ToList();
                foreach (ICollection<DbValidationError> item in errors)
                    msg += item.FirstOrDefault().ErrorMessage;
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
            if (DispatchCenter != null)
            {

                DispatchCenter.FBeginDate = dptBeginDate.SelectedDate;
                DispatchCenter.FVehicleNum = ddlFVehicleNum.SelectedValue;
                DispatchCenter.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
                DispatchCenter.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);

                DispatchCenter.FMileage = string.IsNullOrEmpty(txtFMileage.Text)
                    ? 0
                    : Convert.ToDecimal(txtFMileage.Text);

                DispatchCenter.FFrom = ddlFrom.SelectedValue;
                DispatchCenter.FTo = ddlTo.SelectedValue;

                //记录数
                DispatchCenter.TaskQty = txtTaskQty.Text == "" ? 0 : decimal.Parse(txtTaskQty.Text);
                DispatchCenter.FDriverYQty = txtFDriverYQty.Text == "" ? 0 : decimal.Parse(txtFDriverYQty.Text);
                DispatchCenter.FDriverQty = txtFDriverQty.Text == "" ? 0 : decimal.Parse(txtFDriverQty.Text);
                DispatchCenter.FDriverJQty = txtFDriverJQty.Text == "" ? 0 : decimal.Parse(txtFDriverJQty.Text);


                //司机
                DispatchCenter.FDriverYAmount = DispatchCenter.FDriverYPrice * DispatchCenter.FDriverYQty;
                DispatchCenter.FDriverAmount = DispatchCenter.FDriverPrice * DispatchCenter.FDriverQty;
                DispatchCenter.FDriverJAmount =DispatchCenter.FDriverJPrice * DispatchCenter.FDriverJQty;

                 DispatchCenter.FDriverMileageAmt= txtFDriverMileageAmt.Text == "" ? 0 : decimal.Parse(txtFDriverMileageAmt.Text);
                //押运
                DispatchCenter.FSupercargoYAmount= DispatchCenter.FSupercargoYPrice * DispatchCenter.FDriverYQty;
                DispatchCenter.FSupercargoAmount = DispatchCenter.FSupercargoPrice * DispatchCenter.FDriverQty;
                DispatchCenter.FSupercargoJAmount = DispatchCenter.FSupercargoJPrice * DispatchCenter.FDriverJQty;

                DispatchCenter.FSupercargoMileageAmt = txtFSupercargoMileageAmt.Text == "" ? 0 : decimal.Parse(txtFSupercargoMileageAmt.Text);


                //变更单据上的司机、押韵员、车牌号
                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Clear();


                parms.Add("@keyid", lblKeyId.Text);
                parms.Add("@FCompanyId", CurrentUser.AccountComId);

                DataTable data = SqlService.ExecuteProcedureCommand("proc_DispatchDetails", parms).Tables[0];

                EnumerableRowCollection<object> keys = (from x in data.AsEnumerable()
                                                        select x["keyid"]);
                foreach (string item in keys)
                {
                    parms.Clear();
                    parms.Add("@keyid", item);
                    parms.Add("@companyId", CurrentUser.AccountComId);
                    parms.Add("@FVehicleNum", ddlFVehicleNum.SelectedValue);
                    parms.Add("@FDriver", GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray));
                    parms.Add("@FSupercargo", GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray));
                    parms.Add("@FDispatchNum", lblKeyId.Text); //调度单号

                    SqlService.ExecuteProcedureCommand("proc_DispatchCenter", parms);
                }



                return DispatchCenterService.SaveChanges() >= 0;
            }

            return false;
        }


        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            //生成调度单
            LHSalaryDriver dispatch = new LHSalaryDriver();

            //调度单号生成
            string newKeyId = SequenceService.CreateSequence("PC", CurrentUser.AccountComId);
            dispatch.KeyId = newKeyId; lblKeyId.Text = newKeyId;
            dispatch.FBeginDate = dptBeginDate.SelectedDate;
            dispatch.FEndDate = dptEnd.SelectedDate;
            dispatch.FVehicleNum = ddlFVehicleNum.SelectedValue;
            dispatch.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
            dispatch.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
            dispatch.FLogistics = ddlLogistics.SelectedText;
            dispatch.FFrom = ddlFrom.SelectedValue;
            dispatch.FTo = ddlTo.SelectedValue;
            dispatch.FAuditFlag = "0";

            DispatchCenterService.Add(dispatch);


            string[] keys = KeyId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    //状态
                    LHBillStatus flow = new LHBillStatus
                    {
                        KeyId = key,
                        FCompanyId = CurrentUser.AccountComId,
                        FDeptId = CurrentUser.AccountOrgId,
                        FOperator = CurrentUser.AccountName,
                        FDate = Convert.ToDateTime(dptBeginDate.SelectedDate),

                        FActionName = "配车",

                        FMemo = string.Format(@"单据号{0}被{1}作配车业务处理。", key, CurrentUser.AccountName)
                    };

                    BillStatusService.Add(flow);

                    LHStockOut info = new StockOutService().FirstOrDefault(p => p.KeyId == key && p.FCompanyId == CurrentUser.AccountComId);

                    if (info != null)
                    {
                        Dictionary<string, object> infoParms = new Dictionary<string, object>();
                        infoParms.Clear();

                        infoParms.Add("@keyid", info.FLogisticsNumber);
                        infoParms.Add("@companyId", CurrentUser.AccountComId);
                        infoParms.Add("@FVehicleNum", ddlFVehicleNum.SelectedValue);
                        infoParms.Add("@FDriver", GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray));
                        infoParms.Add("@FSupercargo", GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray));
                        infoParms.Add("@FDispatchNum", newKeyId); //调度单号

                        SqlService.ExecuteProcedureCommand("proc_DispatchCenter", infoParms);
                    }


                    //变更单据上的司机、押韵员、车牌号
                    Dictionary<string, object> parms = new Dictionary<string, object>();
                    parms.Clear();

                    parms.Add("@keyid", key);
                    parms.Add("@companyId", CurrentUser.AccountComId);
                    parms.Add("@FVehicleNum", ddlFVehicleNum.SelectedValue);
                    parms.Add("@FDriver", GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray));
                    parms.Add("@FSupercargo", GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray));
                    parms.Add("@FDispatchNum", newKeyId); //调度单号

                    SqlService.ExecuteProcedureCommand("proc_DispatchCenter", parms);

                    List<LHStockOut> outs = new List<LHStockOut>();
                    List<LHPassCard> cards = new List<LHPassCard>();


                }
            }

            return true;
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            //GasHelper.DropDownListDriverDataBind(ddlFDriver);
            //GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);
            //GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);


            //物流公司
            GasHelper.DropDownListLogisticsDataBind(ddlLogistics);

            GasHelper.DropDownListDistributionPointDataBind(ddlFrom);

            GasHelper.DropDownListDistributionPointDataBind(ddlTo);

            dptBeginDate.SelectedDate = DateTime.Now;

            dptEnd.SelectedDate = DateTime.Now;

            dpBeginTime.SelectedDate = DateTime.Now;

            string logistic = ddlLogistics.SelectedValue;

            //车
            ddlFVehicleNum.DataSource =
                VehicleService.Where(p => p.FLogistics == logistic && p.FCompanyId == CurrentUser.AccountComId && p.FISMain == "是");
            ddlFVehicleNum.DataBind();
            ddlFVehicleNum.Items.Insert(0, new ListItem(@"", "-1"));



            //司机
            ddlFDriver.DataSource =//p.FLogistics == logistic && 
                EmployeeService.Where(p => p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId//
                && p.professional.Contains("司机"));
            ddlFDriver.DataBind();
            ddlFDriver.Items.Insert(0, new ListItem(@"", "-1"));

            //押运员
            ddlFSupercargo.DataSource =//p.FLogistics == logistic &&
                EmployeeService.Where(p => p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId //
                && p.professional.Contains("押运员"));
            ddlFSupercargo.DataBind();
            ddlFSupercargo.Items.Insert(0, new ListItem(@"", "-1"));

        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    lblKeyId.Text = KeyId;

                    break;

                case WebAction.Edit:
                    lblKeyId.Text = KeyId;
                    if (DispatchCenter != null)
                    {
                        txtFMileage.Text = DispatchCenter.FMileage.ToString();
                        dptBeginDate.SelectedDate = DispatchCenter.FBeginDate;
                        dptEnd.SelectedDate = DispatchCenter.FEndDate;


                        //-------------------------------------------------------

                        string logistic = ddlLogistics.SelectedValue;

                        //车
                        ddlFVehicleNum.DataSource =
                            VehicleService.Where(p => p.FLogistics == logistic && p.FCompanyId == CurrentUser.AccountComId && p.FISMain == "是");
                        ddlFVehicleNum.DataBind();
                        ddlFVehicleNum.Items.Insert(0, new ListItem(@"", "-1"));

                        //挂车

                        //司机
                        ddlFDriver.DataSource =//p.FLogistics == logistic && 
                            EmployeeService.Where(p => p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId//
                            && p.professional.Contains("司机"));
                        ddlFDriver.DataBind();
                        if (ddlFDriver.Items.Select(p => p.Text == DispatchCenter.FVehicleNum).Count() <= 0)
                        {
                            ddlFDriver.Items.Insert(ddlFDriver.Items.Count + 1, new ListItem(@DispatchCenter.FVehicleNum, DispatchCenter.FVehicleNum));
                        }
                        ddlFDriver.Items.Insert(0, new ListItem(@"", "-1"));

                        //押运员
                        ddlFSupercargo.DataSource =//p.FLogistics == logistic &&
                            EmployeeService.Where(p => p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId //
                            && p.professional.Contains("押运员"));
                        ddlFSupercargo.DataBind();
                        ddlFSupercargo.Items.Insert(0, new ListItem(@"", "-1"));



                        txtTaskQty.Text = DispatchCenter.TaskQty.ToString();
                        txtFDriverYQty.Text = DispatchCenter.FDriverYQty.ToString();
                        txtFDriverQty.Text = DispatchCenter.FDriverQty.ToString();
                        txtFDriverJQty.Text = DispatchCenter.FDriverJQty.ToString();


                        txtFDriverMileageAmt.Text = DispatchCenter.FDriverMileageAmt.ToString();
                        txtFSupercargoMileageAmt.Text = DispatchCenter.FSupercargoMileageAmt.ToString();

                        //------------------------------------------------------

                        if (!string.IsNullOrEmpty(DispatchCenter.FDriver))
                            ddlFDriver.SelectedValueArray = DispatchCenter.FDriver.Split(',');
                        if (!string.IsNullOrEmpty(DispatchCenter.FSupercargo))
                            ddlFSupercargo.SelectedValueArray = DispatchCenter.FSupercargo.Split(',');

                        ddlFVehicleNum.SelectedValue = DispatchCenter.FVehicleNum;
                        ddlFrom.SelectedValue = DispatchCenter.FFrom;
                        ddlTo.SelectedValue = DispatchCenter.FTo;


                    }
                    break;
            }
        }

        #endregion
    }
}