using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.IIS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Enterprise.IIS.business.Reports
{
    public partial class ProductionYield : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
        

        protected void BindData()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "@Fnum", txtFnum.Text },
                { "@Bdate", dateBegin.SelectedDate },
                { "@Edate", dateEnd.SelectedDate }
            };

            DataSet set = SqlService.ExecuteProcedureCommand("p_VechlieCost", param);
            Grid1.DataSource = set.Tables[0];
            Grid1.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=成本表{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }


        protected string NameToEN(string name)
        {
            string returnname = name;
            if (name.Contains("六氟化硫"))
            {
                returnname = "SF6";
            }
            else if (name.Contains("磷烷"))
            {
                returnname = "PH3";
            }
            else if (name.Contains("氨"))
            {
                returnname = "NH3";
            }
            else if (name.Contains("氧"))
            {
                returnname = "O2";
            }
            else if (name.Contains("一氧化碳"))
            {
                returnname = "CO";
            }
            else if (name.Contains("二氧化碳"))
            {
                returnname = "CO2";
            }
            else if (name.Contains("乙炔"))
            {
                returnname = "C2H2";
            }
            else if (name.Contains("甲烷"))
            {
                returnname = "CH4";
            }
            else if (name.Contains("二氧化硫"))
            {
                returnname = "SO2";
            }
            else if (name.Contains("氙气"))
            {
                returnname = "Xe";
            }
            else if (name.Contains("氯化氢"))
            {
                returnname = "HCL";
            }
            else if (name.Contains("氖气"))
            {
                returnname = "Ne";
            }
            else if (name.Contains("四氟化碳"))
            {
                returnname = "CF4";
            }
            else if (name.Contains("液氮"))
            {
                returnname = "N2";
            }
            else if (name.Contains("一氧化氮"))
            {
                returnname = "NO";
            }
            else if (name.Contains("氦"))
            {
                returnname = "He";
            }
            else if (name.Contains("氢"))
            {
                returnname = "H2";
            }
            else if (name.Contains("氩"))
            {
                returnname = "Ar";
            }
            else if (name.Contains("氮"))
            {
                returnname = "N2";
            }
            else if (name.Contains("乙炔"))
            {
                returnname = "C2H2";
            }
            return returnname;
        }


        protected double TotalColumn(DataTable tb, string columns)
        {
            double total = 0;
            for (int j = 0; j < tb.Columns.Count; j++)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    if (tb.Columns[j].ColumnName.Equals(columns) && !string.IsNullOrEmpty(tb.Rows[i][j].ToString()))
                    {
                        total += double.Parse(tb.Rows[i][j].ToString());
                    }
                }
            }
            return total;
        }


    }
}