using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.DispatchCommission
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
        private DispatchCommissionService _dispatchCommissionService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected DispatchCommissionService DispatchCommissionService
        {
            get { return _dispatchCommissionService ?? (_dispatchCommissionService = new DispatchCommissionService()); }
            set { _dispatchCommissionService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private DispatchCommissionDetailsService _stockOutDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected DispatchCommissionDetailsService StockOutDetailsService
        {
            get
            {
                return _stockOutDetailsService ?? //
                    (_stockOutDetailsService = new DispatchCommissionDetailsService());
            }
            set { _stockOutDetailsService = value; }
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
        ///     
        /// </summary>
        private LHDispatchCommission _stockOut;

        /// <summary>
        ///     
        /// </summary>
        protected LHDispatchCommission StockOut
        {
            get
            {
                return _stockOut ?? (_stockOut = DispatchCommissionService.FirstOrDefault(p => p.KeyId == KeyId //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _stockOut = value; }
        }

        /// <summary>
        ///     FCode
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

                        //写入
                        ModifiedGrid();

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
                //using (var trans = new TransactionScope())
                //{
                    switch (Actions)
                    {
                        case WebAction.Add:
                            isSucceed = SubmintAdd();
                            break;

                        case WebAction.Edit:
                            isSucceed = SubmintEdit();
                            break;
                    }

                //    trans.Complete();
                //}
            }
            catch (Exception ex)
            {

                Alert.Show("提交失败！"+ex.Message, MessageBoxIcon.Error);
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


        protected void btnAddWorkTask_Click(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     BindDataGrid
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.LHDispatchCommissionDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();

            var table = source.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFQty = 0.00M;
                decimal sumFAmount = 0.00M;
                decimal sumFRecycleQty = 0.00M;
                decimal sumFBottleQty = 0.00M;

                //sumFQty = Convert.ToDecimal(table.Compute("sum(FQty)", "true"));
                //sumFAmount = Convert.ToDecimal(table.Compute("sum(FAmount)", "true"));
                //sumFRecycleQty = Convert.ToDecimal(table.Compute("sum(FRecycleQty)", "true"));
                //sumFBottleQty = Convert.ToDecimal(table.Compute("sum(FBottleQty)", "true"));

                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    //{"FQty", sumFQty},
                    //{"FAmount", sumFAmount},
                    //{"FRecycleQty", sumFRecycleQty},
                    //{"sumFBottleQty", sumFBottleQty},
                };

                Grid1.SummaryData = summary;
            }
            else
            {
                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    //{"FQty", 0},
                    //{"FAmount", 0},
                    //{"FRecycleQty", 0},
                    //{"sumFBottleQty", 0},
                };

                Grid1.SummaryData = summary;
            }
        }

        /// <summary>
        ///     单元格编辑与修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            //Window1.Hidden = true;
            //Window2.Hidden = true;
            //Window3.Hidden = true;

            ////新增行事件
            //var addList = Grid1.GetNewAddedList();

            //#region AddRow
            //foreach (var add in addList)
            //{
            //    var dictValues = add.Values;

            //    //商品代码
            //    var firstOrDefault = dictValues.First();

            //    if (firstOrDefault != null && !string.IsNullOrEmpty(firstOrDefault.ToString()))
            //    {
            //        DataSet dataSet = GasHelper.GetSalesItem(firstOrDefault.ToString(), CurrentUser.AccountComId);

            //        DataTable table = dataSet.Tables[0];

            //        if (table != null && table.Rows.Count > 0)
            //        {

            //        }
            //    }
            //}
            //#endregion

            //if (addList.Count > 0)
            //    BindDataGrid();
        }

        /// <summary>
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete" || e.CommandName == "Add")
            {
                var datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][1]);

                StockOutDetailsService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);

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
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
        }
        protected void Window3_Close(object sender, WindowCloseEventArgs e)
        {
        }

        protected void txtFAddress_OnTriggerClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFItemCode_OnTriggerClick(object sender, EventArgs e)
        {
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

                var details = new LHDispatchCommissionDetails();

                for (int i = 0; i < keys.Count(); i++)
                {
                    #region 修改内容

                    var key = keys[i];
                    var value = values[i];

                    if (!string.IsNullOrEmpty(key))
                    {
                        if (details != null)
                        {
                            if (key.Equals("KeyId"))
                            {
                                details.KeyId = value;
                            }

                            if (key.Equals("FDate"))
                            {
                                details.FDate = Convert.ToDateTime(value);
                            }
                            if (key.Equals("FCode"))
                            {
                                details.FCode = value;
                            }
                            if (key.Equals("FName"))
                            {
                                details.FName = value;
                            }

                            if (key.Equals("FArea"))
                            {
                                details.FArea = value;
                            }

                            if (key.Equals("FItemName"))
                            {
                                details.FItemName = value;
                            }

                            if (key.Equals("FQty"))
                            {
                                details.FQty= Convert.ToInt32(value);
                            }

                            if (key.Equals("FPrice"))
                            {
                                details.FPrice = Convert.ToDecimal(value);
                            }

                            if (key.Equals("FDriver"))
                            {
                                details.FDriver = value;
                            }

                            if (key.Equals("FDriverPrice"))
                            {
                                details.FDriverPrice = Convert.ToDecimal(value);
                            }
                            if (key.Equals("FSupercargo"))
                            {
                                details.FSupercargo = value;
                            }
                            if (key.Equals("FSupercargoPrice"))
                            {
                                details.FSupercargoPrice = Convert.ToDecimal(value);
                            }
                            if (key.Equals("FVehicleNum"))
                            {
                                details.FVehicleNum = value;
                            }

                            if (key.Equals("FMemo"))
                            {
                                details.FMemo = value;
                            }
                        }
                    }

                    #endregion
                }


                details.FCompanyId = CurrentUser.AccountComId;
                StockOutDetailsService.Add(details);
            }

            Grid1.CommitChanges();
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
                int datakey = Convert.ToInt32(Grid1.DataKeys[rowKey][1].ToString());

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

                var details = StockOutDetailsService.Where(p => p.FId == datakey).FirstOrDefault();
                
                var keys = sKeys.ToString().Split(',');
                var values = sValues.ToString().Split(',');
                for (int i = 0; i < keys.Count(); i++)
                {
                    #region 修改内容

                    var key = keys[i];
                    var value = values[i];

                    if (!string.IsNullOrEmpty(key))
                    {
                        if (details != null)
                        {
                            if (key.Equals("KeyId"))
                            {
                                details.KeyId = value;
                            }

                            if (key.Equals("FDate"))
                            {
                                details.FDate = Convert.ToDateTime(value);
                            }
                            if (key.Equals("FCode"))
                            {
                                details.FCode = value;
                            }
                            if (key.Equals("FName"))
                            {
                                details.FName = value;
                            }

                            if (key.Equals("FArea"))
                            {
                                details.FArea = value;
                            }

                            if (key.Equals("FItemName"))
                            {
                                details.FItemName = value;
                            }

                            if (key.Equals("FQty"))
                            {
                                details.FQty = Convert.ToInt32(value);
                            }

                            if (key.Equals("FPrice"))
                            {
                                details.FPrice = Convert.ToDecimal(value);
                            }
                            if (key.Equals("FDriver"))
                            {
                                details.FDriver = value;
                            }

                            if (key.Equals("FDriverPrice"))
                            {
                                details.FDriverPrice = Convert.ToDecimal(value);
                            }
                            if (key.Equals("FSupercargo"))
                            {
                                details.FSupercargo = value;
                            }
                            if (key.Equals("FSupercargoPrice"))
                            {
                                details.FSupercargoPrice = Convert.ToDecimal(value);
                            }
                            if (key.Equals("FVehicleNum"))
                            {
                                details.FVehicleNum = value;
                            }

                            if (key.Equals("FMemo"))
                            {
                                details.FMemo = value;
                            }
                        }
                    }

                    #endregion
                }

                StockOutDetailsService.SaveChanges();
            }
        }

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (StockOut != null)
            {
                AddListGrid();

                ModifiedGrid();
                /////////////////////////////////////////////////////////////////////////////
                StockOut.FDate = txtFDate.SelectedDate;
                StockOut.FMemo = txtFMemo.Text.Trim();


                return DispatchCommissionService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var stock = DispatchCommissionService.Where(p => p.KeyId == txtKeyId.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (stock != null)
            {
                AddListGrid();

                ModifiedGrid();
                /////////////////////////////////////////////////////////////////////////////
                stock.FDate = txtFDate.SelectedDate;
                stock.FMemo = txtFMemo.Text.Trim();

                DispatchCommissionService.SaveChanges();

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "TC", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "14");
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
                        FMemo = String.Format("单据号{0},{1}新增送货提成单据。", newKeyId, CurrentUser.AccountName)
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

            GasHelper.DropDownListDriverDataBind(ddlFDriver);

            GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);

            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

            //删除选中单元格的客户端脚本
            string deleteScript = DeleteScript();

            //新增
            var defaultObj = new JObject
            {
                {"KeyId", ""},
                {"FDate", DateTime.Now.ToString("yyyy-MM-dd")},
                {"FCode", ""},
                {"FName", ""},
                {"FArea", "1"},
                {"FItemName", "0"},
                {"FSp", "0"},
                {"FSpPrice", "0"},
                {"FKp", "0"},
                {"FKpPrice", "0"},
                {"FSumPrice", "0"},
                {"FDriver", ""},
                {"FDriverPrice", "0"},
                {"FSupercargo", ""},
                {"FSupercargoPrice", "0"},
                {"FVehicleNum", ""},
                {"FMemo", ""},

                {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
                    deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            };

            // 在第一行新增一条数据
            btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            txtFDate.SelectedDate = DateTime.Now;
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
                    Region3.Title = "添加送货提成单";

                    var temp = new LHDispatchCommission
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //送货提成单
                        FType = Convert.ToInt32(GasEnumBill.DispatchCommission),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                    };

                    //临时写入单据
                    DispatchCommissionService.Add(temp);

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
                    Region3.Title = "编辑送货提成单";

                    if (StockOut != null)
                    {
                        WebControlHandler.BindObjectToControls(StockOut, SimpleForm1);
                        txtFDate.SelectedDate = StockOut.FDate;

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

    }
}