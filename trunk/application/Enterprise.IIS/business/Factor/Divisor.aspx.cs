using Enterprise.Data;
using Enterprise.Framework.File;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using System;
using System.Data;
using System.Data.OleDb;

namespace Enterprise.IIS.business.Factor
{
    public partial class Divisor : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private FactorService _itemsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FactorService ItemsService
        {
            get { return _itemsService ?? (_itemsService = new FactorService()); }
            set { _itemsService = value; }
        }

        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "商品代码";
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
                Label1.Text = string.Format(@"引入压缩因子表");
                tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinUnit.aspx");
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

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {

            }
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            Window2.Hidden = true;
        }
        /// <summary>
        ///     提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int errorRow = -1;
            try
            {
                //using (TransactionScope ts = new TransactionScope())
                {
                    DataTable data = ViewState["dtData"] as DataTable;
                    if (data == null) return;

                    //var stringBuilder = new StringBuilder();

                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        errorRow = i + 1;
                        if (!string.IsNullOrWhiteSpace(data.Rows[i][2].ToString()))
                        {
                            LHFactor item = new LHFactor
                            {
                                FCompanyCode = tbxFCustomer.Text,
                                FBar = decimal.Parse(data.Rows[i][0].ToString()),
                                FTemperature = decimal.Parse(data.Rows[i][1].ToString()),
                                FDivisor = data.Rows[i][2].ToString(),
                            };

                            ItemsService.AddEntity(item);
                        }
                        else
                        {
                            break;
                        }
                        
                    }

                    //写入数据库
                    ItemsService.SaveChanges();

                    //ts.Complete();

                    Alert.Show("引入完成。", MessageBoxIcon.Information);

                }
            }
            catch (DBConcurrencyException ex)
            {
                Alert.Show(string.Format(@"引入失败，请排查模版内容是否完善，出错定位行：{0} ", ex.Message), MessageBoxIcon.Information);
            }
        }

        protected void fileUpload1_FileSelected(object sender, EventArgs e)
        {
            if (fileUpload1.HasFile)
            {
                string fileSuffix = fileUpload1.ShortFileName.Substring(fileUpload1.ShortFileName.LastIndexOf('.'));

                string sequence = SequenceService.CreateSequence("LH", CurrentUser.AccountComId);

                string fileName = sequence + fileSuffix;

                string uploadpath = Config.GetUploadpath();

                //该目录设定死，最好不要修改
                string fileTemp = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

                if (!DirFile.XFileExists(Server.MapPath(fileTemp)))
                {
                    DirFile.XCreateDir(Server.MapPath(fileTemp));
                }

                string address = fileTemp + fileName;

                fileUpload1.SaveAs(Server.MapPath(address));

                //导入
                string connStr = string.Empty;//数据连接字符串
                if (fileSuffix == ".xls")
                {
                    connStr = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + Server.MapPath(address) + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1'";
                }
                else if (fileSuffix == ".xlsx")
                {
                    connStr = "Provider=Microsoft.ACE.OleDb.12.0;" + "data source=" + Server.MapPath(address) + ";Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";
                }

                OleDbConnection conn = new OleDbConnection(connStr);
                conn.Open();

                DataTable data = new DataTable();
                const string cmdSql = "select * from [Sheet1$]";
                OleDbDataAdapter da = new OleDbDataAdapter(cmdSql, conn);
                da.Fill(data);
                conn.Close();
                
                DataTable table = new DataTable();
                table.Columns.Add("压力");
                table.Columns.Add("温度");
                table.Columns.Add("因子系数");
                for (int i = 1; i < data.Columns.Count; i++)
                {
                    for (int j = 0; j < data.Rows.Count; j++)
                    {
                        DataRow row = table.NewRow();
                        row[0] = data.Columns[i].ColumnName;
                        row[1] = data.Rows[j][0];
                        row[2] = data.Rows[j][i];
                        table.Rows.Add(row);
                    }
                }
                ViewState["dtData"] = table;
                Grid1.DataSource = table;
                Grid1.DataBind();
                Grid1.Visible = true;

                fileUpload1.Reset();
            }
        }
        #endregion
    }
}