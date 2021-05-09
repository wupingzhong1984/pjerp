using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.File;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Service.Base.ERP;

using FineUI;

namespace Enterprise.IIS.business.VehicleETC
{
    public partial class Import : PageBase
    {

        /// <summary>
        ///     字典数据服务
        /// </summary>
        private DictionaryService _dictionaryService;

        /// <summary>
        ///     字典数据服务
        /// </summary>
        protected DictionaryService DictionaryService
        {
            get { return _dictionaryService ?? (_dictionaryService = new DictionaryService()); }
            set { _dictionaryService = value; }
        }

        private CompanyService _companyService;
        protected CompanyService CompanyService
        {
            get { return _companyService ?? (_companyService = new CompanyService()); }
            set { _companyService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private VehicleEtcService _vehicleEtcService;

        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "FId";
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
        ///     
        /// </summary>
        protected VehicleEtcService VehicleEtcService
        {
            get { return _vehicleEtcService ?? (_vehicleEtcService = new VehicleEtcService()); }
            set { _vehicleEtcService = value; }
        }

        private ProvinceService _provinceService;

        protected ProvinceService ProvinceService
        {
            get { return _provinceService ?? (_provinceService = new ProvinceService()); }
            set { _provinceService = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label1.Text = string.Format(@"引入月通卡");

                GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);
            }
        }

        #region Protected Method

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript("closeActiveTab();");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var errorRow = -1;
            try
            {
                using (var ts = new TransactionScope())
                {
                    var date = ViewState["dtData"] as DataTable;
                    if (date == null) return;

                    StringBuilder stringBuilder = new StringBuilder();

                    for (int i = 0; i < date.Rows.Count; i++)
                    {

                        errorRow = i + 1;

                        var etc = new LHVehicleEtc
                        {
                            FVehicleNum = ddlFVehicleNum.SelectedValue,
                            FDateI = Convert.ToDateTime(date.Rows[i]["入口时间"]),
                            FPortalEntry = date.Rows[i]["入口站"].ToString(),
                            FDateO = Convert.ToDateTime(date.Rows[i]["出口时间"]),
                            FPortalExit = date.Rows[i]["出口站"].ToString(),
                            FProvince = date.Rows[i]["交易省份"].ToString(),
                            FNature = date.Rows[i]["交易性质"].ToString(),
                            FExpenditure = Convert.ToDecimal(date.Rows[i]["支出"]),
                            FDeposit = Convert.ToDecimal(date.Rows[i]["存入"]),
                            FRebate = Convert.ToDecimal(date.Rows[i]["通行应返还金额"]),
                            FFlag = 1,
                            CreateBy = CurrentUser.AccountName,
                            FDate = DateTime.Now

                        };


                        VehicleEtcService.Add(etc);
                    }

                    ts.Complete();

                    Alert.Show("导入成功。", MessageBoxIcon.Information);

                    if (stringBuilder.ToString().Length > 0)
                    {
                        Alert.Show(string.Format(@"导入时，出错的数据部分有：{0}", stringBuilder.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.Show(string.Format(@"导入失败，错误数据行 {0} 进行排查。 ", errorRow), MessageBoxIcon.Information);
            }
        }

        protected void fileUpload1_FileSelected(object sender, EventArgs e)
        {
            if (fileUpload1.HasFile)
            {
                var fileSuffix = fileUpload1.ShortFileName.Substring(fileUpload1.ShortFileName.LastIndexOf('.'));

                var sequence = SequenceService.CreateSequence("LH", CurrentUser.AccountComId);

                var fileName = sequence + fileSuffix;

                var uploadpath = Config.GetUploadpath();

                //该目录设定死，最好不要修改
                var fileTemp = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

                if (!DirFile.XFileExists(Server.MapPath(fileTemp)))
                {
                    DirFile.XCreateDir(Server.MapPath(fileTemp));
                }

                var address = fileTemp + fileName;

                fileUpload1.SaveAs(Server.MapPath(address));

                //导入
                var connStr = string.Empty;//数据连接字符串
                if (fileSuffix == ".xls")
                {
                    connStr = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + Server.MapPath(address) + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1'";
                }
                else if (fileSuffix == ".xlsx")
                {
                    connStr = "Provider=Microsoft.ACE.OleDb.12.0;" + "data source=" + Server.MapPath(address) + ";Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";
                }

                var conn = new OleDbConnection(connStr);
                conn.Open();

                var data = new DataTable();
                const string cmdSql = "select * from [Sheet1$]";
                var da = new OleDbDataAdapter(cmdSql, conn);
                da.Fill(data);

                ViewState["dtData"] = data;
                Grid1.DataSource = data;
                Grid1.DataBind();
                Grid1.Visible = true;

                fileUpload1.Reset();
            }
        }
        #endregion
    }
}