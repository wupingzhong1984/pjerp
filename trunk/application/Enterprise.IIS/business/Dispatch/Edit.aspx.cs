using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.File;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;


namespace Enterprise.IIS.business.Dispatch
{
    public partial class Edit : PageBase
    {
        #region  Service
        /// <summary>
        ///     AppendToEnd
        /// </summary>
        private bool AppendToEnd
        {
            get
            {
                return ViewState["_AppendToEnd"] != null //
                    && Convert.ToBoolean(ViewState["_AppendToEnd"]);
            }
        }

        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null //
                    ? ViewState["SortField"].ToString() : "KeyId";
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
                string sort = ViewState["SortDirection"] != null //
                    ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private DispatchService _dispatchService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected DispatchService DispatchService
        {
            get { return _dispatchService ?? (_dispatchService = new DispatchService()); }
            set { _dispatchService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private DispatchDetailsService _dispatchDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected DispatchDetailsService DispatchDetailsService
        {
            get
            {
                return _dispatchDetailsService ?? //
              (_dispatchDetailsService = new DispatchDetailsService());
            }
            set { _dispatchDetailsService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHDispatch _dispatch;

        /// <summary>
        ///     
        /// </summary>
        protected LHDispatch Dispatch
        {
            get
            {
                return _dispatch ?? (_dispatch = DispatchService.FirstOrDefault(p => p.KeyId == KeyId //
              && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _dispatch = value; }
        }

        /// <summary>
        ///     KeyId
        /// </summary>
        protected string KeyId
        {
            get { return Request["KeyId"]; }
        }

        /// <summary>
        ///     
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction)Int32.Parse(s);
            }
        }
        #endregion

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
            else
            {
                #region 更新合计

                if (Grid1.Rows.Count > 0)
                {
                    if (GetRequestEventArgument() == "UPDATE_SUMMARY")
                    {
                        // 页面要求重新计算合计行的值
                        OutputSummaryData();

                        // 为了保持前后台上传，回发更新合计行值后，必须进行数据绑定或者提交更改
                        Grid1.CommitChanges();
                    }
                }

                #endregion
            }
        }

        #region OutputSummaryData
        /// <summary>
        ///     OutputSummaryData
        /// </summary>
        private void OutputSummaryData()
        {
            decimal sumFQty = 0.00M;
            decimal sumFAmount = 0.00M;
            decimal sumFRecycleQty = 0.00M;
            decimal sumFBottleQty = 0.00M;


            foreach (JObject mergedRow in Grid1.GetMergedData())
            {
                JObject values = mergedRow.Value<JObject>("values");

                sumFQty += values.Value<decimal>("FQty");
                sumFAmount += values.Value<decimal>("FAmount");
                sumFRecycleQty += values.Value<decimal>("FRecycleQty");
                sumFBottleQty += values.Value<decimal>("FBottleQty");
            }

            JObject summary = new JObject();
            summary.Add("FQty", sumFQty);
            summary.Add("FAmount", sumFAmount);
            summary.Add("FRecycleQty", sumFRecycleQty);
            summary.Add("FBottleQty", sumFBottleQty);

            Grid1.SummaryData = summary;
        }

        #endregion

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
            catch (Exception ex)
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
        ///     打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PageContext.RegisterStartupScript(string.Format("LodopPrinter('{0}');", KeyId));
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
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
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddProduct_OnClick(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     BindDataGrid
        /// </summary>
        private void BindDataGrid()
        {
            var data = DispatchDetailsService.Where(p => p.FCompanyId == CurrentUser.AccountComId && p.KeyId == txtKeyId.Text.Trim());

            //绑定数据源
            Grid1.DataSource = data;
            Grid1.DataBind();

            //var table = source.Tables[0];

            //if (table != null && table.Rows.Count > 0)
            //{
            //    decimal sumFActual = 0.00M;
            //    decimal sumFFMileage = 0.00M;
            //    decimal sumFTimes = 0.00M;

            //    sumFActual = Convert.ToDecimal(table.Compute("sum(FActual)", "true"));
            //    sumFFMileage = Convert.ToDecimal(table.Compute("sum(FMileage)", "true"));
            //    sumFTimes = Convert.ToDecimal(table.Compute("sum(FTimes)", "true"));

            //    var summary = new JObject
            //    {
            //        {"FVehicleNum", "合计"},
            //        {"FActual", sumFActual},
            //        {"FMileage", sumFFMileage},
            //        {"FTimes", sumFTimes},
            //    };

            //    Grid1.SummaryData = summary;
            //}
            //else
            //{
            //    var summary = new JObject
            //    {
            //        {"FVehicleNum", "合计"},
            //        {"FActual", 0},
            //        {"FMileage", 0},
            //        {"FTimes", 0},
            //    };

            //    Grid1.SummaryData = summary;
            //}
        }

        /// <summary>
        ///     单元格编辑与修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
        }

        /// <summary>
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][0]);
                DispatchDetailsService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);
                BindDataGrid();
            }
        }

        /// <summary>
        ///     关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
        }
        protected void Window3_Close(object sender, WindowCloseEventArgs e)
        {
        }

        /// <summary>
        ///     双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                
                int sid = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][0]);

                PageContext.RegisterStartupScript(
                    Window1.GetShowReference(string.Format("./D.aspx?FId={0}&action=2",
                        sid), "编辑调度日志"));
            }
            catch (Exception ex)
            {
                Alert.Show("打开失败：" + ex.Message, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (Dispatch != null)
            {
                Dispatch.CreateBy = CurrentUser.AccountName;
                Dispatch.FAccidentQty = Convert.ToInt32(txtFAccidentQty.Text.Trim());
                Dispatch.FDate = txtFDate.SelectedDate;
                Dispatch.FDispatcher = ddlDispatcher.SelectedValue;
                Dispatch.FHeavyTruckQty = Convert.ToDecimal(txtFHeavyTruckQty.Text.Trim());
                Dispatch.FIDepartureTime = Convert.ToInt32(txtFIDepartureTime.Text.Trim());
                Dispatch.FIQty = Convert.ToDecimal(txtFIQty.Text.Trim());
                Dispatch.FIRange = Convert.ToDecimal(txtFIRange.Text.Trim());
                Dispatch.FLogistics = ddlLogistics.SelectedValue;
                Dispatch.FLogisticsName = ddlLogistics.SelectedText;

                Dispatch.FMemo = txtFMemo.Text.Trim();
                Dispatch.FODepartureTime = Convert.ToInt32(txtFODepartureTime.Text.Trim());
                Dispatch.FOQty = Convert.ToDecimal(txtFOQty.Text.Trim());
                Dispatch.FORange = Convert.ToDecimal(txtFORange.Text.Trim());
                Dispatch.FOtherQty = Convert.ToInt32(txtFOtherQty.Text.Trim());
                Dispatch.FRepairQty = Convert.ToInt32(txtFRepairQty.Text.Trim());
                Dispatch.FTransport = Convert.ToDecimal(txtFTransport.Text.Trim());
                Dispatch.FTurnover = Convert.ToDecimal(txtFTurnover.Text.Trim());
                Dispatch.FWorkQty = Convert.ToInt32(txtFWorkQty.Text.Trim());
                Dispatch.FType = 52;

                DispatchService.SaveChanges();

                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var dispatch = DispatchService.Where(p => p.KeyId == txtKeyId.Text.Trim() //
            && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (dispatch != null)
            {
                //dispatch.KeyId = txtKeyId.Text.Trim();
                //dispatch.FCompanyId = CurrentUser.AccountComId;

                dispatch.CreateBy = CurrentUser.AccountName;
                dispatch.FAccidentQty = Convert.ToInt32(txtFAccidentQty.Text.Trim());
                dispatch.FDate = txtFDate.SelectedDate;
                dispatch.FDispatcher = ddlDispatcher.SelectedValue;
                dispatch.FHeavyTruckQty = Convert.ToDecimal(txtFHeavyTruckQty.Text.Trim());
                dispatch.FIDepartureTime = Convert.ToInt32(txtFIDepartureTime.Text.Trim());
                dispatch.FIQty = Convert.ToDecimal(txtFIQty.Text.Trim());
                dispatch.FIRange = Convert.ToDecimal(txtFIRange.Text.Trim());
                dispatch.FLogistics = ddlLogistics.SelectedValue;
                dispatch.FLogisticsName = ddlLogistics.SelectedText;

                dispatch.FMemo = txtFMemo.Text.Trim();
                dispatch.FODepartureTime = Convert.ToInt32(txtFODepartureTime.Text.Trim());
                dispatch.FOQty = Convert.ToDecimal(txtFOQty.Text.Trim());
                dispatch.FORange = Convert.ToDecimal(txtFORange.Text.Trim());
                dispatch.FOtherQty = Convert.ToInt32(txtFOtherQty.Text.Trim());
                dispatch.FRepairQty = Convert.ToInt32(txtFRepairQty.Text.Trim());
                dispatch.FTransport = Convert.ToDecimal(txtFTransport.Text.Trim());
                dispatch.FTurnover = Convert.ToDecimal(txtFTurnover.Text.Trim());
                dispatch.FWorkQty = Convert.ToInt32(txtFWorkQty.Text.Trim());
                dispatch.FDeleteFlag = 0;
                dispatch.FType = 52;

                DispatchService.SaveChanges();

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate),//
                        "DL", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "52");//Convert.ToInt32(GasEnumBill.Dispatch)
                    orderParms.Add("@companyId", CurrentUser.AccountComId);

                    SqlService.ExecuteProcedureCommand("proc_num", orderParms);
                    txtKeyId.Text = newKeyId;

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            ViewState["_AppendToEnd"] = true;
            
            txtFDate.SelectedDate = DateTime.Now;

            //调度员
            GasHelper.DropDownListDispatcherDataBind(ddlDispatcher);

            //物流公司
            GasHelper.DropDownListLogisticsDataBind(ddlLogistics);
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    txtKeyId.Text = SequenceService.CreateSequence("TM", CurrentUser.AccountComId);
                    Region3.Title = "添加调度日志";

                    var temp = new LHDispatch
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //发货单
                        FType = Convert.ToInt32(GasEnumBill.Dispatch),

                        //CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                        FProgress = Convert.ToInt32(GasEnumBillStauts.Add),
                    };

                    //临时写入单据
                    DispatchService.Add(temp);

                    //合计
                    var summary = new JObject
                    {
                        {"FItemCode", "合计"},
                        {"FQty", 0},
                        {"FAmount", 0}
                    };

                    Grid1.SummaryData = summary;

                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region3.Title = "编辑调度日志";

                    if (Dispatch != null)
                    {
                        WebControlHandler.BindObjectToControls(Dispatch, SimpleForm1);
                        txtFDate.SelectedDate = Dispatch.FDate;
                        ddlDispatcher.SelectedValue = Dispatch.FDispatcher;


                        BindDataGrid();
                    }
                    break;
            }
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

        /// <summary>
        ///     生成日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLog_Click(object sender, EventArgs e)
        {
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId",CurrentUser.AccountComId);
            parms.Add("@FLogisticsCode",ddlLogistics.SelectedValue);
            parms.Add("@FDate", Convert.ToDateTime(txtFDate.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@KeyId",txtKeyId.Text.Trim());

            SqlService.ExecuteProcedureCommand("proc_DispatchAuto",parms);

            var dispatch = DispatchService.Where(p=>p.FCompanyId==CurrentUser.AccountComId&&p.KeyId==txtKeyId.Text.Trim()).FirstOrDefault();

            if (dispatch != null)
            {
                txtFWorkQty.Text = dispatch.FWorkQty.ToString();
                txtFStopQty.Text = dispatch.FStopQty.ToString();
                txtFRepairQty.Text = dispatch.FRepairQty.ToString();
                txtFAccidentQty.Text = dispatch.FAccidentQty.ToString();
                txtFOtherQty.Text = dispatch.FOtherQty.ToString();
                txtFIDepartureTime.Text = dispatch.FIDepartureTime.ToString();
                txtFIRange.Text = dispatch.FIRange.ToString();
                txtFIQty.Text = dispatch.FIQty.ToString();
                txtFODepartureTime.Text = dispatch.FODepartureTime.ToString();
                txtFORange.Text = dispatch.FORange.ToString();
                txtFOQty.Text = dispatch.FOQty.ToString();
                txtFHeavyTruckQty.Text = dispatch.FHeavyTruckQty.ToString();
                txtFSumQty.Text = dispatch.FSumQty.ToString();
                txtFTransport.Text = dispatch.FTransport.ToString();
                txtFTurnover.Text = dispatch.FTurnover.ToString();
                txtFMemo.Text = dispatch.FMemo;

                var data = DispatchDetailsService.Where(p=>p.FCompanyId==CurrentUser.AccountComId && p.KeyId == txtKeyId.Text.Trim());

                //绑定数据源
                Grid1.DataSource = data;
                Grid1.DataBind();

            }

        }
    }
}