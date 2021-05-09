using System;
using System.Web.UI;
using Enterprise.Framework.Web;

namespace Enterprise.IIS
{
    public partial class SystemError : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var message =Cookie.GetValue("Http_Errors");

                Literal1.Text = message;
            }
        }
    }
}