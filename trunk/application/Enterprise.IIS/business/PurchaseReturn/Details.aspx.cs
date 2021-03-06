using System;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;


namespace Enterprise.IIS.business.PurchaseReturn
{
    public partial class Details : PageBase
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
            get { return _stockOutDetailsService ?? (_stockOutDetailsService = new StockOutDetailsService()); }
            set { _stockOutDetailsService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHStockOut _stockOut;


        /// <summary>
        ///     客户档案
        /// </summary>
        private LHSupplier _supplier;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected LHSupplier Supplier
        {
            get { return _supplier ?? (_supplier = SupplierService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim()&&p.FCompanyId==CurrentUser.AccountComId)); }
            set { _supplier = value; }
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
        ///     职员档案
        /// </summary>
        protected LHStockOut StockOut
        {
            get { return _stockOut ?? (_stockOut = StockOutService.FirstOrDefault(p => p.KeyId == KeyId&&p.FCompanyId==CurrentUser.AccountComId)); }
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
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_SalesDetails a WHERE keyId='{0}' and a.FCompanyId={1}", txtKeyId.Text,CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();

            var table = source.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFQty = 0.00M;
                decimal sumFAmount = 0.00M;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    sumFQty += Convert.ToDecimal(table.Rows[i]["FQty"]);
                    sumFAmount += Convert.ToDecimal(table.Rows[i]["FAmount"]);
                }

                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", sumFQty},
                    {"FAmount", sumFAmount}
                };

                Grid1.SummaryData = summary;
            }

            //单据状态
            //----------------------------------------------------------
            var dataStatus = SqlService.Where(string.Format(@"SELECT FId
      ,KeyId
      ,a.FCompanyId
      ,FDeptId
      ,b.org_name FDeptName
      ,FOperator
      ,FDate
      ,FActionName
      ,FMemo
  FROM dbo.LHBillStatus a
  LEFT JOIN dbo.base_orgnization b ON a.FDeptId=b.id WHERE KeyId='{0}' and a.FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));
            //绑定数据源
            Grid2.DataSource = dataStatus;
            Grid2.DataBind();

            //审批流程过程
            //----------------------------------------------------------
            var dataFlow = SqlService.Where(string.Format(@"SELECT FId
      ,KeyId
      ,a.FCompanyId
      ,FDeptId
      ,b.org_name FDeptName
      ,FOperator
      ,FDate
      ,FMemo
  FROM dbo.LHBillFlow a
  LEFT JOIN dbo.base_orgnization b ON a.FDeptId=b.id WHERE KeyId='{0}' and a.FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));
            //绑定数据源
            Grid3.DataSource = dataFlow;
            Grid3.DataBind();

            //中间变更日志
            //----------------------------------------------------------
            var log = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_SalesReturnDetailsLog a WHERE keyId='{0}' and a.FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid4.DataSource = log;
            Grid4.DataBind();

        }
        
        /// <summary>
        ///     关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }
        #endregion

        #region Private Method

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {

            GasHelper.DropDownListBottleDataBind(tbxFBottle);

        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Details:
                    txtKeyId.Text = KeyId;
                    if (StockOut != null)
                    {
                        WebControlHandler.BindObjectToControls(StockOut, SimpleForm1);

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