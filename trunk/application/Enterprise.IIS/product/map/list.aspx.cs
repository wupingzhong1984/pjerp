using System;
using Newtonsoft.Json.Linq;
using System.Data;
using FineUI;
using Enterprise.Data;

namespace Enterprise.IIS.product.map
{
// ReSharper disable once InconsistentNaming
    public partial class list : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
            else
            {
                if (Request.Form["__EVENTARGUMENT"] == "specialkey")
                {
                    string req = Request.Form["__EVENTTARGET"];
                    string[] point = req.Split(',');

                    if (point.Length > 1)
                    {
                        var defaultObj = new JObject();
                        defaultObj.Add("CityName", point[0]);
                        defaultObj.Add("pointX", point[1]);
                        defaultObj.Add("pointY", point[2]);
                        var dbt = (DataTable)Session["KEY_FOR_DATASOURCE_SESSION"];
                        dbt.Rows.Add(new object[] { point[0], point[1], point[2] });
                        Session["KEY_FOR_DATASOURCE_SESSION"] = dbt;
                        Grid1.DataSource = dbt;
                        Grid1.DataBind();
                    }
                }
            }
        }
        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            var dbt = (DataTable)Session["KEY_FOR_DATASOURCE_SESSION"];
            DataRow dr = dbt.Rows[e.RowIndex];
            string x = Convert.ToString(dr["pointX"]);
            string y = Convert.ToString(dr["pointY"]);
            PageContext.RegisterStartupScript("PanToCity(" + x + "," + y + ");");
        }
        private DataTable _dt;
        public void GetData()
        {
            _dt = new DataTable();
            _dt.Columns.Add("CityName");
            _dt.Columns.Add("pointX");
            _dt.Columns.Add("pointY");
            _dt.Rows.Add(new object[] { "北京市", "116.222", "39.2222" });
            Grid1.DataSource = _dt;
            Grid1.DataBind();
            Session["KEY_FOR_DATASOURCE_SESSION"] = _dt;
        }


    }
}