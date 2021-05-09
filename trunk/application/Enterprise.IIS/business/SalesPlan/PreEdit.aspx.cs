using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Enterprise.IIS.business.SalesPlan
{
    public partial class PreEdit : PageBase
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
        private CustomerService _customerService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private PassCardService _stockOutService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected PassCardService StockOutService
        {
            get { return _stockOutService ?? (_stockOutService = new PassCardService()); }
            set { _stockOutService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private PassCardDetailsService _stockOutDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected PassCardDetailsService StockOutDetailsService
        {
            get
            {
                return _stockOutDetailsService ?? //
                    (_stockOutDetailsService = new PassCardDetailsService());
            }
            set { _stockOutDetailsService = value; }
        }

        /// <summary>
        ///     Log
        /// </summary>
        private StockOutDetailsLogService _stockOutDetailsLogService;
        /// <summary>
        ///     Log
        /// </summary>
        protected StockOutDetailsLogService StockOutDetailsLogService
        {
            get { return _stockOutDetailsLogService ?? (_stockOutDetailsLogService = new StockOutDetailsLogService()); }
            set { _stockOutDetailsLogService = value; }
        }

        /// <summary>
        ///     Log
        /// </summary>
        private StockOutLogService _stockOutLogService;
        /// <summary>
        ///     Log
        /// </summary>
        protected StockOutLogService StockOutLogService
        {
            get { return _stockOutLogService ?? (_stockOutLogService = new StockOutLogService()); }
            set { _stockOutLogService = value; }
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
        private LHPassCard _stockOut;

        /// <summary>
        ///     
        /// </summary>
        protected LHPassCard StockOut
        {
            get
            {
                return _stockOut ?? (_stockOut = StockOutService.FirstOrDefault(p => p.KeyId == KeyId //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _stockOut = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private AttachmentService _attachmentService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected AttachmentService AttachmentService
        {
            get { return _attachmentService ?? (_attachmentService = new AttachmentService()); }
            set { _attachmentService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHAttachment _attachment;

        /// <summary>
        ///     
        /// </summary>
        protected LHAttachment Attachment
        {
            get
            {
                return _attachment ?? (_attachment = AttachmentService.FirstOrDefault(p => p.KeyId == KeyId //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _attachment = value; }
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
                return (WebAction)int.Parse(s);
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
                #region 弹窗加产品
                if (GetRequestEventArgument().Contains("reloadGrid:"))
                {
                    //查找所选商品代码，查访产品集合
                    string keys = GetRequestEventArgument().Split(':')[1];

                    string[] values = keys.Split(',');

                    string codes = string.Empty;
                    for (int i = 0; i < values.Count(); i++)
                    {
                        codes += string.Format("'{0}',", values[i]);
                    }

                    string value = codes.Substring(0, codes.Length - 1);

                    DataSet data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_SalesItem a WHERE a.FItemCode IN ({0}) and a.FCompanyId={1}", value, CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        DataTable table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            LHPassCardDetails details = new LHPassCardDetails();

                            string itemCode = table.Rows[i]["FItemCode"].ToString();

                            decimal price = GasHelper.GeCustomerPrice(txtFCode.Text.Trim(), //
                                itemCode, CurrentUser.AccountComId);

                            LHCustomerPrice info = new CustomerPriceService().FirstOrDefault(p => p.FCode == txtFCode.Text && p.FItemCode == itemCode);

                            details.FItemCode = table.Rows[i]["FItemCode"].ToString();
                            details.FPrice = price;
                            details.FQty = 1;
                            details.FAmount = price;
                            details.FBottleQty = 1;
                            details.FBottleOweQty = 0;
                            details.FCompanyId = CurrentUser.AccountComId;
                            details.KeyId = txtKeyId.Text.Trim();
                            details.FCateId = Convert.ToInt32(table.Rows[i]["FId"].ToString());
                            details.FBSpec = table.Rows[i]["FBSpec"].ToString();
                            details.FRecycleQty = 0;
                            details.FCostPrice = 0;
                            details.FBalance = 0;

                            if (info != null)
                            {
                                details.FMemo = info.FMemo;
                            }

                            //默认包装物
                            details.FBottle = table.Rows[i]["FBottle"].ToString();

                            StockOutDetailsService.AddEntity(details);

                        }

                        StockOutDetailsService.SaveChanges();

                        //重新绑定值
                        BindDataGrid();
                    }
                }
                #endregion


                if (GetRequestEventArgument().StartsWith("GridRowDblclick$"))
                {
                    string rowId = GetRequestEventArgument().Substring("GridRowDblclick$".Length);

                    GridRow row = Grid1.FindRow(rowId);

                    object[] keys = Grid1.DataKeys[row.RowIndex];

                    //var id = Convert.ToInt32(keys[1]);

                    //var details = StockOutDetailsService.FirstOrDefault(p=>p.FId==id);

                    //if (details != null)
                    {
                        string result = string.Format("第 {0} 行", row.RowIndex + 1) +
                               "<br>" +
                               string.Format("当前行数据 - 品名：{0}，规格：{1}", keys[2], keys[3]);

                        result = result.Replace("<br>", "\r\n");

                        // 非AJAX回发
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment; filename=row_" + row.RowIndex + ".txt");
                        Response.ContentType = "text/plain";
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.Write(result);
                        Response.End();
                    }
                }

                #region 更新合计

                if (Grid1.Rows.Count > 0)
                {
                    if (GetRequestEventArgument() == "UPDATE_SUMMARY")
                    {
                        // 页面要求重新计算合计行的值
                        OutputSummaryData();

                        tmsModel t = new tmsModel();
                        //写入
                        ModifiedGrid(t);

                        // 为了保持前后台上传，回发更新合计行值后，必须进行数据绑定或者提交更改
                        Grid1.CommitChanges();
                    }
                }

                #endregion
            }
        }
        protected void tbxFAddress_TriggerClick(object sender, EventArgs e)
        {
            //tbxFAddress.OnClientTriggerClick = 
            //Window3.GetSaveStateReference(tbxFAddress.ClientID)
            //        + Window3.GetShowReference(string.Format(@"../../Common/WinCustomerLink.aspx?FCode={0}", txtFCode.Text.Trim()));


            PageContext.RegisterStartupScript(Window3.GetSaveStateReference(tbxFAddress.ClientID)
                   + Window3.GetShowReference(string.Format(@"../../Common/WinCustomerLink.aspx?FCode={0}", txtFCode.Text.Trim())));

        }

        protected void tbxFAddress_TextChanged(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
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

            JObject summary = new JObject
            {
                { "FQty", sumFQty },
                { "FAmount", sumFAmount },
                { "FRecycleQty", sumFRecycleQty },
                { "FBottleQty", sumFBottleQty }
            };

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
            catch (DbEntityValidationException ex)
            {
                string msg = string.Empty;
                List<ICollection<DbValidationError>> errors = (from u in ex.EntityValidationErrors select u.ValidationErrors).ToList();
                foreach (ICollection<DbValidationError> item in errors)
                    msg += item.FirstOrDefault().ErrorMessage;
                isSucceed = false;
                Alert.Show("提交失败！" + msg, MessageBoxIcon.Error);
            }
            if (isSucceed)
            {
                PageContext.RegisterStartupScript("closeActiveTab();");
            }
            else
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            //if (filePhoto.HasFile)
            //{
            //    var fileSuffix = filePhoto.ShortFileName.Substring(filePhoto.ShortFileName.LastIndexOf('.'));

            //    var sequence = SequenceService.CreateSequence("LH", CurrentUser.AccountComId);

            //    var fileName = sequence + fileSuffix;

            //    var uploadpath = Config.GetUploadpath();

            //    var tempPath = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

            //    if (!DirFile.XFileExists(Server.MapPath(tempPath)))
            //    {
            //        DirFile.XCreateDir(Server.MapPath(tempPath));
            //    }

            //    //filePhoto.SaveAs(Server.MapPath(tempPath + fileName));

            //    hfdImage.Text = tempPath + fileName;

            //    // 清空文件上传组件
            //    //filePhoto.Reset();
            //}

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
        ///     Copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            //bool isSucceed = false;

            //try
            //{
            //    var pamrs = new Dictionary<string, object>();
            //    pamrs.Clear();

            //    pamrs.Add("@code", txtFCode.Text.Trim());
            //    pamrs.Add("@keyid", txtKeyId.Text.Trim());
            //    pamrs.Add("@companyid", CurrentUser.AccountComId);

            //    SqlService.ExecuteProcedureCommand("proc_CopyLastSales", pamrs);

            //    BindDataGrid();

            //    isSucceed = true;
            //}
            //catch (Exception)
            //{
            //    isSucceed = false;
            //}
            //finally
            //{
            //    if (isSucceed)
            //    {
            //        Alert.Show("提交成功", MessageBoxIcon.Information);
            //    }
            //    else
            //    {
            //        Alert.Show("提交失败！", MessageBoxIcon.Error);
            //    }
            //}
        }

        /// <summary>
        ///     BindDataGrid
        /// </summary>
        private void BindDataGrid()
        {
            DataSet source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_PassCardDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();

            DataTable table = source.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFQty = 0.00M;
                decimal sumFAmount = 0.00M;
                decimal sumFRecycleQty = 0.00M;
                decimal sumFBottleQty = 0.00M;

                sumFQty = Convert.ToDecimal(table.Compute("sum(FQty)", "true"));
                sumFAmount = Convert.ToDecimal(table.Compute("sum(FAmount)", "true"));
                sumFRecycleQty = Convert.ToDecimal(table.Compute("sum(FRecycleQty)", "true"));
                sumFBottleQty = Convert.ToDecimal(table.Compute("sum(FBottleQty)", "true"));

                JObject summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", sumFQty},
                    {"FAmount", sumFAmount},
                    {"FRecycleQty", sumFRecycleQty},
                    {"sumFBottleQty", sumFBottleQty},
                };

                Grid1.SummaryData = summary;
            }
            else
            {
                JObject summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", 0},
                    {"FAmount", 0},
                    {"FRecycleQty", 0},
                    {"sumFBottleQty", 0},
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
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;

            //新增行事件
            List<Dictionary<string, object>> addList = Grid1.GetNewAddedList();

            #region AddRow
            foreach (Dictionary<string, object> add in addList)
            {
                Dictionary<string, object>.ValueCollection dictValues = add.Values;

                //商品代码
                object firstOrDefault = dictValues.First();

                if (firstOrDefault != null && !string.IsNullOrEmpty(firstOrDefault.ToString()))
                {
                    DataSet dataSet = GasHelper.GetSalesItem(firstOrDefault.ToString(), CurrentUser.AccountComId);

                    DataTable table = dataSet.Tables[0];

                    if (table != null && table.Rows.Count > 0)
                    {
                        //decimal price = GasHelper.GeCustomerPrice(txtFCode.Text.Trim(),//
                        //    table.Rows[0]["FItemCode"].ToString(), CurrentUser.AccountComId);

                        table.Rows[0]["FPrice"] = 0;

                        LHPassCardDetails details = new LHPassCardDetails
                        {
                            FItemCode = table.Rows[0]["FItemCode"].ToString(),
                            FPrice = 0,
                            FQty = 1,
                            FAmount = 0,
                            FBottleQty = 0,
                            FBottleOweQty = 0,
                            FRecycleQty = 0,
                            FCompanyId = CurrentUser.AccountComId,
                            KeyId = txtKeyId.Text.Trim(),
                            FBottle = table.Rows[0]["FBottleCode"].ToString(),
                            FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString())
                        };

                        StockOutDetailsService.Add(details);
                    }
                }
            }
            #endregion

            if (addList.Count > 0)
                BindDataGrid();
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
                int datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][1]);

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
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
        }
        protected void Window3_Close(object sender, WindowCloseEventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
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

                PageContext.RegisterStartupScript(
                    Window1.GetShowReference(string.Format("../../Common/WinProductPrice.aspx?FCode={0}",
                        txtFCode.Text.Trim()), "客户合同价"));


                //PageContext.RegisterStartupScript(
                //    Window1.GetShowReference(string.Format("../../Common/WinProducReference.aspx?FCode={0}",
                //        txtFCode.Text.Trim()), "客户合同价"));
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                LHCustomer custmoer = CustomerService.Where(p => p.FName == tbxFCustomer.Text.Trim() //
                    && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    //ddlFAddress.SelectedValue = custmoer.FAddress.Trim();

                    GasHelper.DropDownListOrgDataBind(ddlFOrgName, custmoer.FCode);

                    //GasHelper.DropDownListAddressDataBind(ddlFAddress, custmoer.FCode);

                    txtFCode.Text = custmoer.FCode;

                    //tbxFAddress.Text = custmoer.FAddress.Trim();

                    txtFFreight.Text = custmoer.FFreight.ToString();
                    txtFLinkman.Text = custmoer.FLinkman;
                    txtFPhone.Text = custmoer.FPhome;


                    if (!string.IsNullOrEmpty(ddlFOrgName.SelectedValue))
                    {
                        int id = Convert.ToInt32(ddlFOrgName.SelectedValue);

                        tbxFAddress.Text = new CustomerOrgService().FirstOrDefault(p => p.FId == id).FAddress;
                    }

                    if (custmoer.FCreditFlag.Equals("不通过"))
                    {
                        Alert.Show(string.Format("信用期系统审核不通过，目前已开票未收款金额为 {0} 元。", custmoer.FCreditValue), MessageBoxIcon.Warning);
                    }

                    Window1.Hidden = true;
                    Window2.Hidden = true;
                    Window3.Hidden = true;
                }
            }
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFItemCode_OnTriggerClick(object sender, EventArgs e)
        {
        }


        protected void ddlFOrgName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlFOrgName.SelectedValue))
            {
                int id = Convert.ToInt32(ddlFOrgName.SelectedValue);

                tbxFAddress.Text = new CustomerOrgService().FirstOrDefault(p => p.FId == id).FAddress;
            }
        }


        #endregion

        #region Private Method

        /// <summary>
        ///     ModifiedGrid
        /// </summary>
        private void ModifiedGrid(tmsModel t)
        {
            List<LHPassCardDetails> lHStocks = new List<LHPassCardDetails>();
            //编辑行事件
            Dictionary<int, Dictionary<string, object>> dictModified = Grid1.GetModifiedDict();
            foreach (int rowKey in dictModified.Keys)
            {
                int datakey = Convert.ToInt32(Grid1.DataKeys[rowKey][1].ToString());

                StringBuilder sKeys = new StringBuilder();
                StringBuilder sValues = new StringBuilder();
                foreach (string key in dictModified[rowKey].Keys)
                {
                    sKeys.AppendFormat("{0},", key);
                }

                foreach (object dictValue in dictModified[rowKey].Values)
                {
                    sValues.AppendFormat("{0},", dictValue);
                }

                LHPassCardDetails details = StockOutDetailsService.Where(p => p.FId == datakey).FirstOrDefault();

                string[] keys = sKeys.ToString().Split(',');
                string[] values = sValues.ToString().Split(',');
                for (int i = 0; i < keys.Count(); i++)
                {
                    #region 修改内容

                    string key = keys[i];
                    string value = values[i];

                    if (!string.IsNullOrEmpty(key))
                    {
                        if (details != null)
                        {
                            if (key.Equals("FPrice"))
                            {
                                details.FPrice = Convert.ToDecimal(value);
                                details.FAmount = details.FPrice * details.FQty;
                            }

                            if (key.Equals("FQty"))
                            {
                                details.FQty = Convert.ToDecimal(value);
                                details.FBottleQty = Convert.ToInt32(details.FQty);
                                details.FAmount = details.FPrice * details.FQty;
                            }

                            if (key.Equals("FBottleName"))
                            {
                                details.FBottle = GasHelper.GetBottleCodeByName(value);
                            }

                            if (key.Equals("FRecycleQty"))
                            {
                                details.FRecycleQty = 0;//Convert.ToInt32(value);
                                //写入回空单
                            }

                            if (key.Equals("FBottleQty"))
                            {

                                int.TryParse(value, out int result);

                                details.FBottleQty = result;
                            }

                            if (key.Equals("FMemo"))
                            {
                                details.FMemo = value;
                            }

                            if (key.Equals("FNum"))
                            {
                                details.FNum = value;
                            }

                            if (key.Equals("FName"))
                            {
                                details.FName = value;
                                details.FCode = hfdCode.Text.Trim();
                            }

                        }
                    }

                    #endregion
                }

                lHStocks.Add(details);
                StockOutDetailsService.SaveChanges();
            }
            t.passCardDetailsList = lHStocks;
        }

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            tmsModel t = new tmsModel();
            if (StockOut != null)
            {
                ModifiedGrid(t);
                /////////////////////////////////////////////////////////////////////////////

                StockOut.FCode = txtFCode.Text;
                StockOut.FName = tbxFCustomer.Text;
                //------------------------------------------------------
                StockOut.FAddress = tbxFAddress.Text.Trim();//ddlFAddress.SelectedText;

                StockOut.FDate = txtFDate.SelectedDate;

                StockOut.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                StockOut.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                StockOut.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                StockOut.FLinkman = txtFLinkman.Text.Trim();
                StockOut.FMemo = txtFMemo.Text.Trim();
                StockOut.FPhone = txtFPhone.Text.Trim();

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyID", StockOut.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);

                decimal amt =
                   Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_SalesOrder", parms).Tables[0].Rows[0][0]);

                StockOut.FAmount = amt;

                StockOut.FSalesman = ddlFSalesman.SelectedValue;
                //StockOut.FArea = ddlFArea.SelectedValue;
                StockOut.FBill = ddlFBill.SelectedValue;

                StockOut.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());//
                //StockOut.FDiscountAmount = string.IsNullOrEmpty(txtFDiscountAmount.Text.Trim()) ? 0 : Convert.ToDecimal(txtFDiscountAmount.Text.Trim());
                StockOut.FReconciliation = txtFReconciliation.Text;
                StockOut.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                StockOut.FLogisticsNumber = txtFLogisticsNumber.Text;
                StockOut.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                StockOut.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
                StockOut.FSubjectCode = ddlSubject.SelectedValue;
                StockOut.FSubjectName = ddlSubject.SelectedText;
                StockOut.CreateBy = CurrentUser.AccountName;
                StockOut.FDistributionPoint = ddlFDistributionPoint.SelectedValue;
                StockOut.FAddress = tbxFAddress.Text.Trim();
                StockOut.FOrgName = ddlFOrgName.SelectedText;
                //StockOut.FOrgId = ddlFOrgName.SelectedValue;
                if (ddlFOrgName.SelectedValue != null)
                    StockOut.FOrgId = Convert.ToInt32(ddlFOrgName.SelectedValue);



                LHCustomer cusinfo = CustomerService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim());

                if (cusinfo != null)
                {
                    if (cusinfo.FCreditFlag.Equals("通过"))
                    {
                        StockOut.FAuditor = "系统审核";
                        StockOut.FAuditFlag = 1;
                    }
                    else
                    {
                        StockOut.FAuditor = "系统审核";
                        StockOut.FAuditFlag = 2;
                    }
                }
                else
                {
                    StockOut.FAuditor = "系统审核";
                    StockOut.FAuditFlag = 1;
                }

                StockOutService.SaveChanges();


                //var bottles = new Dictionary<string, object>();
                //bottles.Clear();

                //bottles.Add("@SalesKeyId", StockOut.KeyId);
                //bottles.Add("@companyid", CurrentUser.AccountComId);
                //bottles.Add("@date", Convert.ToDateTime(StockOut.FDate).ToString("yyyy-MM-dd"));

                //SqlService.ExecuteProcedureCommand("proc_SalesReturnBottle", bottles);

                ////收款部分
                //var parmsAuto = new Dictionary<string, object>();
                //parmsAuto.Clear();

                //parmsAuto.Add("@KeyId", StockOut.KeyId);
                //parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                //parmsAuto.Add("@FCate", "客户");
                //parmsAuto.Add("@date", Convert.ToDateTime(StockOut.FDate).ToShortDateString());
                //parmsAuto.Add("@FSKNum", string.IsNullOrEmpty(StockOut.FSKNum) ? "" : StockOut.FSKNum);

                //SqlService.ExecuteProcedureCommand("proc_SKOrderAuto", parmsAuto);
                t.passCardList = new List<LHPassCard>
                {
                    StockOut
                };
                t.passCardDetailsList = StockOutDetailsService.Where(p => p.KeyId == StockOut.KeyId).ToList();

                new HttpRequest().httpRequest(t, "open/dbo/dboData");
                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            tmsModel t = new tmsModel();
            LHPassCard stock = StockOutService.Where(p => p.KeyId == txtKeyId.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (stock != null)
            {
                ModifiedGrid(t);
                /////////////////////////////////////////////////////////////////////////////
                stock.FCode = txtFCode.Text;
                stock.FName = tbxFCustomer.Text;
                //--------------------------------------------------
                //stock.FAddress = ddlFAddress.SelectedText;
                //stock.FAddress = //txtFAddress.Text.Trim();
                stock.FFlag = 1;
                stock.FDeleteFlag = 0;
                stock.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                stock.FLinkman = txtFLinkman.Text.Trim();
                stock.FMemo = txtFMemo.Text.Trim();
                stock.FPhone = txtFPhone.Text.Trim();

                stock.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                stock.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                stock.FOutFlag = "未出库";
                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyID", stock.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);

                decimal amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_SalesOrder", parms).Tables[0].Rows[0][0]);

                stock.FAmount = amt;

                stock.FDate = txtFDate.SelectedDate;
                stock.FSalesman = ddlFSalesman.SelectedValue;
                //stock.FArea = ddlFArea.SelectedValue;

                stock.FBill = ddlFBill.SelectedValue;
                stock.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());
                //stock.FDiscountAmount = string.IsNullOrEmpty(txtFDiscountAmount.Text.Trim()) ? 0 : Convert.ToDecimal(txtFDiscountAmount.Text.Trim());
                stock.FReconciliation = txtFReconciliation.Text;
                stock.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                stock.FLogisticsNumber = txtFLogisticsNumber.Text;

                stock.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                stock.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);

                stock.FSubjectCode = ddlSubject.SelectedValue;
                stock.FSubjectName = ddlSubject.SelectedText;
                stock.FSKNum = "";
                stock.FDistributionPoint = ddlFDistributionPoint.SelectedValue;
                stock.FAddress = tbxFAddress.Text.Trim();
                stock.FOrgName = ddlFOrgName.SelectedText;
                if (ddlFOrgName.SelectedValue != null)
                    stock.FOrgId = Convert.ToInt32(ddlFOrgName.SelectedValue);

                //if (custmoer.FCreditFlag.Equals("不通过"))
                //{
                //    Alert.Show(string.Format("信用期系统审核不通过，目前已开票未收款金额为 {0} 元。", custmoer.FCreditValue), MessageBoxIcon.Warning);
                //}

                LHCustomer cusinfo = CustomerService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim());

                if (cusinfo != null)
                {
                    if (cusinfo.FCreditFlag.Equals("通过"))
                    {
                        stock.FAuditor = "系统审核";
                        stock.FAuditFlag = 1;
                    }
                    else
                    {
                        stock.FAuditor = "系统审核";
                        stock.FAuditFlag = 2;
                    }
                }
                else
                {
                    stock.FAuditor = "系统审核";
                    stock.FAuditFlag = 1;
                }

                stock.FTime1 = DateTime.Now;

                StockOutService.SaveChanges();


                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "OC", CurrentUser.AccountComId);
                    Dictionary<string, object> orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "32");
                    orderParms.Add("@companyId", CurrentUser.AccountComId);

                    SqlService.ExecuteProcedureCommand("proc_num", orderParms);
                    txtKeyId.Text = newKeyId;

                    ////新增日志
                    //var billStatus = new LHBillStatus
                    //{
                    //    KeyId = newKeyId,
                    //    FCompanyId = CurrentUser.AccountComId,
                    //    FActionName = "新增",
                    //    FDate = DateTime.Now,
                    //    FDeptId = CurrentUser.AccountOrgId,
                    //    FOperator = CurrentUser.AccountName,
                    //    FMemo = String.Format("单据号{0},{1}新增销售订单据。", newKeyId, CurrentUser.AccountName)
                    //};

                    //GasHelper.AddBillStatus(billStatus);

                    //var bottles = new Dictionary<string, object>();
                    //bottles.Clear();

                    //bottles.Add("@SalesKeyId", newKeyId);
                    //bottles.Add("@companyid", CurrentUser.AccountComId);
                    //bottles.Add("@date", Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));

                    //SqlService.ExecuteProcedureCommand("proc_SalesReturnBottle", bottles);


                    //收款部分
                    //var parmsAuto = new Dictionary<string, object>();
                    //parmsAuto.Clear();

                    //parmsAuto.Add("@KeyId", newKeyId);
                    //parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                    //parmsAuto.Add("@FCate", "客户");
                    //parmsAuto.Add("@date", Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));
                    //parmsAuto.Add("@FSKNum", stock.FSKNum);

                    //SqlService.ExecuteProcedureCommand("proc_SKOrderAuto", parmsAuto);

                    try
                    {
                        LHPassCard lHStock = StockOutService.Where(p => p.KeyId == newKeyId).FirstOrDefault();
                        List<LHPassCard> passCards = new List<LHPassCard>
                        {
                            lHStock
                        };
                        t.passCardList = passCards;
                        if (t.passCardDetailsList.Count == 0)
                        {
                            t.passCardDetailsList = StockOutDetailsService.Where(p => p.KeyId == newKeyId).ToList();
                        }
                        new HttpRequest().httpRequest(t, "open/dbo/dboData");
                    }
                    catch (Exception)
                    {

                    }

                }


                if (!string.IsNullOrEmpty(hfdImage.Text))
                {
                    LHAttachment attachment = new LHAttachment
                    {
                        FCompanyId = CurrentUser.AccountComId,
                        FPath = hfdImage.Text,
                        KeyId = txtKeyId.Text
                    };

                    AttachmentService.Add(attachment);
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

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinUnit.aspx");

            //tbxFItemCode.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFItemCode.ClientID)
            //        + Window1.GetShowReference("../../Common/WinProduct.aspx");

            //tbxFName.OnClientTriggerClick = Window2.GetSaveStateReference(hfdCode.ClientID, tbxFName.ClientID)
            //        + Window2.GetShowReference("../../Common/WinCustomer.aspx");

            //tbxFAddress.OnClientTriggerClick = Window3.GetSaveStateReference(tbxFAddress.ClientID)
            //        + Window3.GetShowReference(string.Format(@"../../Common/WinCustomerLink.aspx?FCode={0}",txtFCode.Text.Trim()));

            tbxFBottle.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFBottle.ClientID, hfdSpec.ClientID)
                    + Window1.GetShowReference("../../Common/WinBottleToGas.aspx");

            GasHelper.DropDownListDriverDataBind(ddlFDriver);

            GasHelper.DropDownListShipperDataBind(ddlFShipper);

            GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);

            //GasHelper.DropDownListBottleDataBind(tbxFBottle);

            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

            //GasHelper.DropDownListAreasDataBind(ddlFArea);

            GasHelper.DropDownListBillDataBind(ddlFBill);

            GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

            GasHelper.DropDownListDeliveryMethodDataBind(ddlDeliveryMethod);

            GasHelper.DropDownListBankSubjectDataBind(ddlSubject);

            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区

            //删除选中单元格的客户端脚本
            string deleteScript = DeleteScript();

            ////新增
            //var defaultObj = new JObject
            //{
            //    {"FItemCode", ""},
            //    {"FItemName", ""},
            //    {"FSpec", ""},
            //    {"FUnit", ""},
            //    {"FPrice", ""},
            //    {"FQty", "0"},
            //    {"FAmount", "0"},
            //    {"FBottleQty", "0"},
            //    {"FBottleOweQty", "0"},
            //    {"FCateName", ""},
            //    {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
            //        deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            //};

            //// 在第一行新增一条数据
            //btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            txtFDate.SelectedDate = DateTime.Now.AddDays(1);

            txtFFreight.Text = "0.00";
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
                    Region3.Title = "添加销售订单";

                    LHPassCard temp = new LHPassCard
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        FAuditFlag = 0,

                        //销售订单
                        FType = Convert.ToInt32(GasEnumBill.PassCard),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Pre),

                        FProgress = Convert.ToInt32(GasEnumBillStauts.Add),

                        FCate = "客户"
                    };

                    //临时写入单据
                    StockOutService.Add(temp);

                    //合计
                    JObject summary = new JObject
                    {
                        {"FItemCode", "合计"},
                        {"FQty", 0},
                        {"FAmount", 0}
                    };

                    Grid1.SummaryData = summary;

                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region3.Title = "编辑销售订单";

                    if (StockOut != null)
                    {
                        WebControlHandler.BindObjectToControls(StockOut, SimpleForm1);
                        txtFDate.SelectedDate = StockOut.FDate;
                        tbxFCustomer.Text = StockOut.FName;
                        txtFCode.Text = StockOut.FCode;

                        GasHelper.DropDownListOrgDataBind(ddlFOrgName, StockOut.FCode);

                        //GasHelper.DropDownListAddressDataBind(ddlFAddress, StockOut.FCode);

                        if (!string.IsNullOrEmpty(StockOut.FDriver))
                            ddlFDriver.SelectedValueArray = StockOut.FDriver.Split(',');
                        if (!string.IsNullOrEmpty(StockOut.FShipper))
                            ddlFShipper.SelectedValueArray = StockOut.FShipper.Split(',');
                        if (!string.IsNullOrEmpty(StockOut.FSupercargo))
                            ddlFSupercargo.SelectedValueArray = StockOut.FSupercargo.Split(',');
                        if (!string.IsNullOrEmpty(StockOut.FSalesman))
                            ddlFSalesman.SelectedValueArray = StockOut.FSalesman.Split(',');

                        ddlSubject.SelectedValue = StockOut.FSubjectCode;
                        ddlFVehicleNum.SelectedValue = StockOut.FVehicleNum;
                        ddlFBill.SelectedValue = StockOut.FBill;
                        ddlDeliveryMethod.SelectedValue = StockOut.FDeliveryMethod;
                        ddlFDistributionPoint.SelectedValue = StockOut.FDistributionPoint;
                        //ddlFAddress.SelectedValue = StockOut.FAddress;
                        ddlFOrgName.SelectedValue = StockOut.FOrgId.ToString();
                        tbxFAddress.Text = StockOut.FAddress;

                        //if (Attachment != null)
                        //{
                        //    lblfile.Text = string.Format(@"<a href='../../{0}' target='_blank'>附件</a>",//
                        //        Attachment.FPath.Replace("~/", ""));
                        //}
                        //else
                        //{
                        //    lblfile.Text = string.Empty;
                        //}

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
            return Confirm.GetShowReference("删除选中行？", string.Empty, MessageBoxIcon.Question, Grid1.GetDeleteSelectedRowsReference(), string.Empty);
        }


        #endregion


    }
}