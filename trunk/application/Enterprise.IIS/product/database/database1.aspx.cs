using System;
using System.IO;
using FineUI;
using Enterprise.Data;

namespace Enterprise.IIS.product.webs
{
    public partial class database1 : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                SetPermissionButtons(Toolbar1);

                txtName.Text = "backup" + DateTime.Now.ToString("yyyyMMddHH") + ".bak";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string dbFileName = txtName.Text.Replace("'", "").Replace(";", "");
            if (!dbFileName.EndsWith(".bak"))
            {
                dbFileName += ".bak";
            }
            string _path = Server.MapPath("~/_data/databackup/" + dbFileName);
            if (File.Exists(_path))
            {

                Alert.Show("目标文件已存在。", MessageBoxIcon.Information);
                return;
            }
            else
            {
                //string dbServerIP = LHXmlReadConfig.ReadConfig("~/_config/conn", "dbServerIP");
                //string dbLoginName = LHXmlReadConfig.ReadConfig("~/_config/conn", "dbLoginName");
                //string dbLoginPass = LHXmlReadConfig.ReadConfig("~/_config/conn", "dbLoginPass");
                //string dbName = LHXmlReadConfig.ReadConfig("~/_config/conn", "dbName");
                //if (DbHelper.Sqlback(dbServerIP, dbLoginName, dbLoginPass, dbName, _path))
                //    Alert.Show("数据库备份成功。", MessageBoxIcon.Information);
                //else
                //    Alert.Show("数据库备份失败。", MessageBoxIcon.Error);
            }
        }
    }
}