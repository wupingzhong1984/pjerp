using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;
using FineUI;

namespace Enterprise.IIS.business.VehicleTire
{
    public partial class Edit : PageBase
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
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private VehicleTireService _vehicleTireService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected VehicleTireService VehicleTireService
        {
            get { return _vehicleTireService ?? (_vehicleTireService = new VehicleTireService()); }
            set { _vehicleTireService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHVehicleTire _vehicleTire;

        /// <summary>
        ///     
        /// </summary>
        protected LHVehicleTire VehicleTire
        {
            get { return _vehicleTire ?? (_vehicleTire = VehicleTireService.FirstOrDefault(p => p.KeyId == KeyId && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _vehicleTire = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string KeyId
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
                using (var trans = new TransactionScope())
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

                    trans.Complete();
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
                    PageContext.RegisterStartupScript("closeActiveTab();");
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
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

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (VehicleTire != null)
            {

                VehicleTire.FMemo = txtFMemo.Text.Trim();
                VehicleTire.FDate = txtFDate.SelectedDate;
                VehicleTire.FireNum1 = txtFireNum1.Text;
                VehicleTire.FireNum2 = txtFireNum2.Text;
                VehicleTire.FireNum3 = txtFireNum3.Text;
                VehicleTire.FMileage = Convert.ToDecimal(txtFMileage.Text.Trim());
                VehicleTire.FSpec = txtFSpec.Text;
                VehicleTire.FPrice = Convert.ToDecimal(txtFPrice.Text.Trim());
                VehicleTire.FVehicleNum = ddlFVehicleNum.SelectedValue;

                return VehicleTireService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var order = VehicleTireService.Where(p => p.KeyId == txtKeyId.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (order != null)
            {
                order.FDeleteFlag = 0;
                order.FMemo = txtFMemo.Text.Trim();
                order.CreateBy = CurrentUser.AccountName;
                order.FDate = txtFDate.SelectedDate;
                order.FMemo = txtFMemo.Text.Trim();
                order.FDate = txtFDate.SelectedDate;
                order.FireNum1 = txtFireNum1.Text;
                order.FireNum2 = txtFireNum2.Text;
                order.FireNum3 = txtFireNum3.Text;
                order.FMileage = Convert.ToDecimal(txtFMileage.Text.Trim());
                order.FSpec = txtFSpec.Text;
                order.FPrice = Convert.ToDecimal(txtFPrice.Text.Trim());
                order.FVehicleNum = ddlFVehicleNum.SelectedValue;


                VehicleTireService.SaveChanges();

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = new SequenceService().CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "VT", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "48");
                    orderParms.Add("@companyId", CurrentUser.AccountComId);
                    SqlService.ExecuteProcedureCommand("proc_num", orderParms);
                    txtKeyId.Text = newKeyId;

                    //新增日志
                    var billStatus = new LHBillStatus
                    {
                        KeyId = newKeyId,
                        FCompanyId = CurrentUser.AccountComId,
                        FActionName = "新增",
                        FDate = DateTime.Now,
                        FDeptId = CurrentUser.AccountOrgId,
                        FOperator = CurrentUser.AccountName,
                        FMemo = String.Format("单据号{0},{1}新增轮胎登记单据。", newKeyId, CurrentUser.AccountName)
                    };
                    GasHelper.AddBillStatus(billStatus);
                }

                return true;
            }

            return false;

        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            ViewState["_AppendToEnd"] = true;

            txtCreateBy.Text = CurrentUser.AccountName;

            txtFDate.SelectedDate = DateTime.Now;

            //GasHelper.DropDownListBankSubjectDataBind(ddlFK);

            //GasHelper.DropDownListBankSubjectDataBind(ddlSK);

            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    Region1.Title = "添加轮胎登记";
                    txtKeyId.Text = SequenceService.CreateSequence("TM", CurrentUser.AccountComId);
                    var temp = new LHVehicleTire
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        FType = Convert.ToInt32(GasEnumBill.VehicleTire),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        //FAccount = 0,

                        FCompanyId = CurrentUser.AccountComId

                    };

                    VehicleTireService.Add(temp);
                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region1.Title = "编辑轮胎登记";

                    if (VehicleTire != null)
                    {
                        txtCreateBy.Text = VehicleTire.CreateBy;
                        txtFDate.SelectedDate = VehicleTire.FDate;

                        txtFMemo.Text=VehicleTire.FMemo;
                        txtFireNum1.Text=VehicleTire.FireNum1;
                        txtFireNum2.Text=VehicleTire.FireNum2 ;
                        txtFireNum3.Text = VehicleTire.FireNum3;
                        txtFMileage.Text=VehicleTire.FMileage.ToString();
                        txtFSpec.Text=VehicleTire.FSpec;
                        txtFPrice.Text=VehicleTire.FPrice.ToString();
                        ddlFVehicleNum.SelectedValue = VehicleTire.FVehicleNum;

                    }

                    break;
            }
        }

        #endregion
    }
}