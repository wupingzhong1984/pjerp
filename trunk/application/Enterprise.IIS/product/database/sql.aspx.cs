using System;
using System.Web;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.File;
using Enterprise.Framework.Web;
namespace Enterprise.IIS.product.webs
{
    public partial class sql : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ajaxExecuteSql();
        }

        private void ajaxExecuteSql()
        {
            int _canExecuteSQL = 0;
            if (IpHelper.ClientIp == Request.ServerVariables["LOCAl_ADDR"])
                _canExecuteSQL = 1;
            else
                _canExecuteSQL = 0;
            if (_canExecuteSQL == 1)
            {
                string _SQLContent = Request.Form["UEditor1"].ToString().Trim()//
                    .Replace("<br/>", "").Replace("<br>", "").Replace("&nbsp", "").Replace("</p>", "").Replace("<p>","").Replace(";","");
                if (_SQLContent.Length == 0)
                {
                    Alert.Show("SQL脚本有错。", MessageBoxIcon.Warning);
                    return;
                }
                string _tmpFile = "/_data/sql/" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".sql";
                DirFile.SaveFile(_SQLContent, _tmpFile);
                if (ExecuteSqlInFile(Server.MapPath(_tmpFile)))
                     Alert.Show("脚本执行成功。", MessageBoxIcon.Information);
                else
                 Alert.Show("脚本执行错误。", MessageBoxIcon.Warning);
            }
        }

        private bool ExecuteSqlInFile(string pathToScriptFile)
        {
            return ExecuteSqlBlock.Go("1", HttpContext.Current.Application["HENANLUHENG_dbConnStr"].ToString(),
                pathToScriptFile);
        }
    }
}