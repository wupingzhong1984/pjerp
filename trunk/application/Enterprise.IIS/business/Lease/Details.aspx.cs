using System;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;


namespace Enterprise.IIS.business.Lease
{
    public partial class Details : PageBase
    {
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
        private ViewUnitService _viewUnitService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ViewUnitService ViewUnitService
        {
            get { return _viewUnitService ?? (_viewUnitService = new ViewUnitService()); }
            set { _viewUnitService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseService _leaseService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseService LeaseService
        {
            get { return _leaseService ?? (_leaseService = new LeaseService()); }
            set { _leaseService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseDetailsService _leaseDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseDetailsService LeaseDetailsService
        {
            get { return _leaseDetailsService ?? (_leaseDetailsService = new LeaseDetailsService()); }
            set { _leaseDetailsService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHLease _stockOut;

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
        protected LHLease Lease
        {
            get { return _stockOut ?? (_stockOut = LeaseService.FirstOrDefault(p => p.KeyId == KeyId&&p.FCompanyId==CurrentUser.AccountComId)); }
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
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_LeaseDetails a WHERE keyId='{0}' and FCompanyId={1}", lblKeyId.Text,CurrentUser.AccountComId));

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
                    sumFQty += Convert.ToDecimal(table.Rows[i]["FBottleQty"]);
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
  LEFT JOIN dbo.base_orgnization b ON a.FDeptId=b.id WHERE KeyId='{0}' and a.FCompanyId={1}", lblKeyId.Text,CurrentUser.AccountComId));
            //绑定数据源
            Grid2.DataSource = dataStatus;
            Grid2.DataBind();
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
  LEFT JOIN dbo.base_orgnization b ON a.FDeptId=b.id WHERE KeyId='{0}' and a.FCompanyId={1}", lblKeyId.Text, CurrentUser.AccountComId));
            //绑定数据源
            Grid3.DataSource = dataFlow;
            Grid3.DataBind();
            //----------------------------------------------------------

            var log = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_LeaseDetailsLog a WHERE keyId='{0}' and a.FCompanyId={1}", lblKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid4.DataSource = log;
            Grid4.DataBind();
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SortField = String.Format(@"{0}", e.SortField);
            SortDirection = e.SortDirection;
        }

        /// <summary>
        ///     
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
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Details:
                    lblKeyId.Text = KeyId;
                    if (Lease != null)
                    {
                        lblCustomer.Text = Lease.FName;
                        lblFAddress.Text = Lease.FAddress;
                        lblFFreight.Text = Lease.FFreight.ToString();
                        lblFLinkman.Text = Lease.FLinkman;
                        lblFMemo.Text = Lease.FMemo;
                        lblFPhone.Text = Lease.FPhone;
                        lblFDriver.Text = Lease.FDriver;
                        lblFShipper.Text = Lease.FShipper;
                        lblFSupercargo.Text = Lease.FSupercargo;
                        lblFVehicleNum.Text = Lease.FVehicleNum;
                        lblFDate.Text = Convert.ToDateTime(Lease.FDate).ToString("yyyy-MM-dd");

                        //发货单据
                        BindDataGrid();

                    }
                    break;
            }
        }

        #endregion

    }
}