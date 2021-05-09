using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using Enterprise.Data;
using Enterprise.Framework.File;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.SalesPlanAudit
{
    public partial class Data : PageBase
    {

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
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "客户编码";
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
        ///     引入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label1.Text = string.Format(@"引入信用审核结果");
            }
        }

        #region Protected Method

        /// <summary>
        ///     关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript("closeActiveTab();");
        }

        /// <summary>
        ///     提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var errorRow = -1;
            try
            {
                CustomerService.Update(p => p.FCompanyId == CurrentUser.AccountComId, p => new Service.Base.LHCustomer
                {
                    FCreditValue = 0,
                    FCreditFlag="通过"
                });


                //using (var ts = new TransactionScope())
                //{
                var data = ViewState["dtData"] as DataTable;
                if (data == null) return;

                var stringBuilder = new StringBuilder();

                for (int i = 0; i < data.Rows.Count; i++)
                {
                    errorRow = i + 1;

                    var code = data.Rows[i]["客户编码"].ToString();
                    var creditValue = Convert.ToDecimal(data.Rows[i]["当前余额"]);
                    var creditFlag = data.Rows[i]["判断"].ToString();
                    var salesman = data.Rows[i]["业务员"].ToString();
                    var creditDays = Convert.ToInt32(data.Rows[i]["信用期"]);

                    var info = CustomerService.FirstOrDefault(p => p.FCode == code);

                    if (info != null)
                    {
                        info.FCreditDays = creditDays;
                        info.FCreditFlag = creditFlag;
                        info.FCreditValue = creditValue;
                        info.FSalesman = salesman;
                        info.FDate = DateTime.Now;

                    }
                    
                }

                CustomerService.SaveChanges();


                //写入数据库
                //CustomerService.SaveChanges();

                //ts.Complete();

                Alert.Show("引入完成。", MessageBoxIcon.Information);

                // }
            }
            catch (Exception ex)
            {
                Alert.Show(string.Format(@"引入失败，请排查模版内容是否完善，出错定位行：{0} ", errorRow), MessageBoxIcon.Information);
            }
        }

        protected void fileUpload1_FileSelected(object sender, EventArgs e)
        {
            try
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

                    Alert.Show("111", MessageBoxIcon.Information);

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
                    Alert.Show("222", MessageBoxIcon.Information);
                    var conn = new OleDbConnection(connStr);
                    conn.Open();
                    Alert.Show("333", MessageBoxIcon.Information);
                    var data = new DataTable();
                    const string cmdSql = "select * from [基础数据$]";
                    var da = new OleDbDataAdapter(cmdSql, conn);
                    da.Fill(data);
                    Alert.Show("444", MessageBoxIcon.Information);
                    ViewState["dtData"] = data;
                    Grid1.DataSource = data;
                    Grid1.DataBind();
                    Grid1.Visible = true;

                    fileUpload1.Reset();
                }
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Information);
            }
        }
        #endregion
    }
           
}