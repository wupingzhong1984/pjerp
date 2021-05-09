using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.IIS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Enterprise.IIS.business.Reports
{
    public partial class primeCost : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        protected void InitData()
        {
            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint);
            GasHelper.DropDownListGroupDataBind(ddlFGroup);//班组
        }

        protected void BindData()
        {
            Dictionary<string, object> parms = new Dictionary<string, object>
            {
                { "@BeginDate", dateBegin.SelectedDate },
                { "@EndDate", dateEnd.SelectedDate },
                { "@Point", ddlFDistributionPoint.SelectedValue == "-1" ? "" : ddlFDistributionPoint.SelectedValue },
                { "@FGroup", ddlFGroup.SelectedValue=="-1"?"": ddlFGroup.SelectedValue }
            };

            DataSet set = SqlService.ExecuteProcedureCommand("rpt_Cost", parms);

            DataTable price = SqlService.ExecuteProcedureCommand("rpt_lastitemprice").Tables[0];

            //领料总汇
            Dictionary<string, object> parm = new Dictionary<string, object>
            {
                { "@BeginDate", dateBegin.SelectedDate },
                { "@EndDate", dateEnd.SelectedDate },
                { "@FGroup", ddlFGroup.SelectedValue=="-1"?"": ddlFGroup.SelectedValue },
            };
            DataTable produces = SqlService.ExecuteProcedureCommand("rpt_ProducePlace", parm).Tables[0];
            foreach (DataRow item in produces.Rows)
            {
                string name = NameToEN(item["Fname"].ToString());
                item["FName"] = name;

            }
            //水电汇总
            double elect = (from x in produces.AsEnumerable()
                            where x["FName"].ToString() == "电量"
                            select double.Parse(x["FQty"].ToString())).Sum();
            double water = (from x in produces.AsEnumerable()
                            where x["FName"].ToString() == "水量"
                            select double.Parse(x["FQty"].ToString())).Sum();

            //中转化学
            foreach (DataRow item in price.Rows)
            {
                if (item["FName"].ToString().StartsWith("原料"))
                {
                    item["FName"] = NameToEN(item["FName"].ToString());
                }

            }
            //气体立方、用水电量、充装成本
            foreach (DataRow item in set.Tables[0].Rows)
            {
                double rises = 40;
                double pakgs = 13.5;
                double pab = 1;

                //升
                Regex rise = new Regex("[0-9]{0,9}(.)?[0-9]{0,9}(?=L)", RegexOptions.IgnoreCase);
                if (rise.IsMatch(item["FSpec"].ToString()))
                {
                    Match m = rise.Match(item["FSpec"].ToString());
                    rises = double.Parse(m.Value);
                }
                //压力或公斤
                Regex pakg = new Regex("([0-9]{0,9}.[0-9]{0,9})?([0-9]{0,9})?(?=mpa)", RegexOptions.IgnoreCase);
                if (pakg.IsMatch(item["FSpec"].ToString()))
                {
                    Match m = pakg.Match(item["FSpec"].ToString());
                    pakgs = double.Parse(m.Value.Replace("/", ""));
                }
                pakg = new Regex("([0-9]{0,9}.[0-9]{0,9})?([0-9]{0,9})?(?=kg)", RegexOptions.IgnoreCase);
                if (pakg.IsMatch(item["FSpec"].ToString()))
                {
                    Match m = pakg.Match(item["FSpec"].ToString());
                    pakgs = double.Parse(m.Value.Replace("/", ""));
                }
                pakg = new Regex("([0-9]{0,9}.[0-9]{0,9})?([0-9]{0,9})?(?=bar)", RegexOptions.IgnoreCase);
                if (pakg.IsMatch(item["FSpec"].ToString()))
                {
                    Match m = pakg.Match(item["FSpec"].ToString());
                    pakgs = double.Parse(m.Value.Replace("/", ""));
                }
                //瓶数
                Regex pabs = new Regex("\\*([0-9]{0,9})(?=/)?", RegexOptions.IgnoreCase);

                if (pabs.IsMatch(item["FSpec"].ToString()))
                {
                    Match m = pabs.Match(item["FSpec"].ToString());
                    pab = double.Parse(m.Value.Replace("*", "").Replace("/", ""));
                }
                if (double.Parse(item["FQty"].ToString()) > 0)
                {

                    if (item["ratio"].ToString() != "1")
                    {
                        if (item["FName"].ToString().Contains("六氟化硫") && item["FUnit"].ToString() != "公斤")
                        {
                            item["ratio"] = pakgs;
                        }
                        else
                        {
                            item["ratio"] = (pakgs * rises / 100 * pab).ToString("0.00");

                        }
                    }
                    try
                    {
                        item["volume"] = (double.Parse(item["ratio"].ToString()) * double.Parse(item["FQty"].ToString())).ToString("0.0");

                        if (item["FName"].ToString() != "液氮" || item["FName"].ToString() != "液氩")
                        {
                            if (elect != 0)
                            {
                                item["elect"] = (double.Parse(item["volume"].ToString()) / elect).ToString("0.0");
                            }

                            if (water != 0)
                            {
                                item["water"] = (double.Parse(item["volume"].ToString()) / water).ToString("0.0");
                            }

                        }



                        if (item["FName"].ToString().StartsWith("2N") || item["FName"].ToString().StartsWith("3N") || item["FName"].ToString().StartsWith("4N")
                            || item["FName"].ToString().StartsWith("5N") || item["FName"].ToString().StartsWith("6N") || item["FName"].ToString().StartsWith("4.5N") || item["FName"].ToString().StartsWith("液") || item["FName"].ToString().StartsWith("乙炔"))
                        {
                            string name = NameToEN(item["FName"].ToString());
                            item[name] = double.Parse(item["volume"].ToString()).ToString("0.0");
                        }
                        else if (item["FName"].ToString().Contains("空气"))
                        {
                            item["O2"] = (0.22 * double.Parse(item["volume"].ToString())).ToString("0.0");
                            item["N2"] = (0.78 * double.Parse(item["volume"].ToString())).ToString("0.0");
                        }
                        else
                        {
                            string[] items = item["FName"].ToString().Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                            double percent = 100;
                            foreach (string name in items)
                            {
                                if (!name.Contains("ppm") && !name.Contains("PPM"))
                                {
                                    Regex pre = new Regex("([0-9]{1,9})?[.]?([0-9]{0,9})?([0-9])?(?:%)", RegexOptions.IgnoreCase);
                                    if (pre.IsMatch(name))
                                    {
                                        Match m = pre.Match(name);
                                        double ps = double.Parse(m.Value.Replace("%", ""));
                                        string gname = name.Replace(m.Value, "");
                                        percent = percent - ps;
                                        item[gname] = ((ps / 100) * double.Parse(item["volume"].ToString())).ToString("0.0");
                                    }
                                    else
                                    {
                                        item[name] = ((double.Parse(percent.ToString()) / 100) * double.Parse(item["volume"].ToString())).ToString("0.0");
                                    }
                                }
                                else
                                {
                                    Regex ppm = new Regex("[1,9]([0,9]{0,9})?(ppm|pm)", RegexOptions.IgnoreCase);
                                    if (ppm.IsMatch(name))
                                    {
                                        Match match = ppm.Match(name);
                                        string pp = match.Value.Replace("P", "").Replace("p", "").Replace("M", "").Replace("m", "");
                                        string gasName = name.Replace(match.Value, "");
                                        if (double.Parse(pp) < 1000)
                                        {
                                            item[gasName] = "0";
                                        }
                                        else if (double.Parse(pp) >= 1000)
                                        {
                                            double pper = double.Parse(pp) * 0.00001; //气体百分比
                                            percent = percent - int.Parse((pper * 100).ToString());//剩余百分比
                                            item[gasName] = (pper * double.Parse(item["volume"].ToString())).ToString("0.0"); //气体立方 各气占比*总立方
                                        }
                                        else
                                        {
                                            item[gasName] = (percent * double.Parse(item["volume"].ToString())).ToString("0.00"); //气体立方
                                        }


    ;
                                    }

                                }

                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    item["Gsums"] = item["volume"];
                }

                //充装成本
                if (!string.IsNullOrEmpty(item["workprice"].ToString()))
                {
                    item["workprice"] = (double.Parse(item["workprice"].ToString()) * double.Parse(item["FQty"].ToString())).ToString("0.0");
                }

            }

            //耗材
            foreach (DataRow item in set.Tables[0].Rows)
            {
                try
                {
                    double supprice = 0;
                    double csums = 0;
                    string newname = "N";

                    item["volume"] = double.Parse(item["ratio"].ToString()) * double.Parse(item["FQty"].ToString());
                    if (item["FName"].ToString().StartsWith("2N") || item["FName"].ToString().StartsWith("3N") || item["FName"].ToString().StartsWith("4N")
                        || item["FName"].ToString().StartsWith("5N") || item["FName"].ToString().StartsWith("6N") || item["FName"].ToString().StartsWith("4.5N") || item["FName"].ToString().StartsWith("液") || item["FName"].ToString().StartsWith("乙炔"))
                    {
                        string name = NameToEN(item["FName"].ToString());
                        DataRow[] rowvalues = produces.Select("FName='" + name + "'");
                        DataRow[] rowprice = price.Select("FName='" + name + "'");
                        if (rowprice.Length > 0)
                        {
                            supprice = double.Parse(rowprice[0]["price"].ToString());
                        }
                        if (rowvalues.Length > 0)
                        {
                            newname += name;
                            item[newname] = (double.Parse(string.IsNullOrEmpty(item[name].ToString()) ? "0" : item[name].ToString()) * supprice).ToString("0.0");
                            csums += double.Parse(item[newname].ToString());
                        }

                    }
                    else if (item["FName"].ToString().Contains("空气"))
                    {

                        DataRow[] rowprice = price.Select("FName='O2'");
                        DataRow[] rowvalues = produces.Select("FName='O2'");
                        if (rowvalues.Length > 0)
                        {
                            if (rowprice.Length > 0)
                            {
                                supprice = double.Parse(rowprice[0]["price"].ToString());
                            }
                            item["NO2"] = (double.Parse(item["O2"].ToString()) * supprice).ToString("0.0");
                            csums += double.Parse(item["NO2"].ToString());
                        }
                        rowvalues = produces.Select("FName='N2'");
                        rowprice = price.Select("FName='N2'");
                        if (rowvalues.Length > 0)
                        {
                            if (rowprice.Length > 0)
                            {
                                supprice = double.Parse(rowprice[0]["price"].ToString());
                            }
                            item["NN2"] = (double.Parse(item["N2"].ToString()) * supprice).ToString("0.0");
                            csums += double.Parse(item["NN2"].ToString());
                        }
                    }
                    else
                    {
                        string[] items = item["FName"].ToString().Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string name in items)
                        {
                            newname = "N";
                            if (!name.Contains("ppm") && !name.Contains("PPM"))
                            {
                                Regex pre = new Regex("([0-9]{1,9})?[.]?([0-9]{0,9})?([0-9])?(?:%)", RegexOptions.IgnoreCase);
                                if (pre.IsMatch(name))
                                {
                                    Match m = pre.Match(name);
                                    double ps = double.Parse(m.Value.Replace("%", ""));
                                    string gname = name.Replace(m.Value, "");
                                    DataRow[] rowvalues = produces.Select("FName='" + gname + "'");
                                    DataRow[] rowprice = price.Select("FName='" + name + "'");
                                    if (rowprice.Length > 0)
                                    {
                                        supprice = double.Parse(rowprice[0]["price"].ToString());
                                    }
                                    if (rowvalues.Length > 0)
                                    {
                                        newname += gname;
                                        item[newname] = (double.Parse(string.IsNullOrEmpty(item[gname].ToString()) ? "0" : item[gname].ToString()) * supprice).ToString("0.0");
                                        csums += double.Parse(item[newname].ToString());
                                    }

                                }
                                else
                                {
                                    DataRow[] rowvalues = produces.Select("FName='" + name + "'");
                                    DataRow[] rowprice = price.Select("FName='" + name + "'");
                                    if (rowprice.Length > 0)
                                    {
                                        supprice = double.Parse(rowprice[0]["price"].ToString());
                                    }
                                    if (rowvalues.Length > 0)
                                    {
                                        newname += name;
                                        item[newname] = (double.Parse(string.IsNullOrEmpty(item[name].ToString()) ? "0" : item[name].ToString()) * supprice).ToString("0.0");
                                        csums += double.Parse(item[newname].ToString());
                                    }
                                }
                            }
                            else
                            {
                                Regex ppm = new Regex("[1,9]([0,9]{0,9})?(ppm|pm)", RegexOptions.IgnoreCase);
                                if (ppm.IsMatch(name))
                                {
                                    Match match = ppm.Match(name);
                                    string pp = match.Value.Replace("P", "").Replace("p", "").Replace("M", "").Replace("m", "");
                                    string gasName = name.Replace(match.Value, "");
                                    DataRow[] rowvalues = produces.Select("FName='" + gasName + "'");
                                    DataRow[] rowprice = price.Select("FName='" + name + "'");
                                    if (rowprice.Length > 0)
                                    {
                                        supprice = double.Parse(rowprice[0]["price"].ToString());
                                    }
                                    if (rowvalues.Length > 0)
                                    {
                                        newname += gasName;
                                        item[newname] = (double.Parse(string.IsNullOrEmpty(item[gasName].ToString()) ? "0" : item[gasName].ToString()) * supprice).ToString("0.0");
                                        csums += double.Parse(item[newname].ToString());

                                    }

                                }

                            }

                        }

                    }
                    item["Csums"] = csums > 0 ? csums.ToString("0.0") : "";
                }
                catch (Exception)
                {
                }
            }

            //总成本
            foreach (DataRow item in set.Tables[0].Rows)
            {
                string csums = string.IsNullOrEmpty(item["Csums"].ToString()) ? "0" : item["Csums"].ToString();
                string waterc = string.IsNullOrEmpty(item["water"].ToString()) ? "0" : item["water"].ToString();
                string electc = string.IsNullOrEmpty(item["elect"].ToString()) ? "0" : item["elect"].ToString();
                string workprice = string.IsNullOrEmpty(item["workprice"].ToString()) ? "0" : item["workprice"].ToString();
                string sums = (double.Parse(csums) + double.Parse(waterc) + double.Parse(electc) + double.Parse(workprice)).ToString("0.0");
                item["makecost"] = sums == "0.0" ? "" : sums;
                item["totalCost"] = sums == "0.0" ? "" : sums;
                if (!string.IsNullOrWhiteSpace(item["totalCost"].ToString()))
                {
                    item["unitCost"] = double.Parse(item["Gsums"].ToString()) / double.Parse(item["totalCost"].ToString());
                }

            }

            var totalFQty = (from x in set.Tables[0].AsEnumerable()
                                  select string.IsNullOrEmpty(x["FQty"].ToString()) ? 0 : double.Parse(x["FQty"].ToString())).Sum();

            var totalvolume = (from x in set.Tables[0].AsEnumerable()
                                  select string.IsNullOrEmpty(x["volume"].ToString()) ? 0 : double.Parse(x["volume"].ToString())).Sum();

            var totalGsums = (from x in set.Tables[0].AsEnumerable()
                                  select string.IsNullOrEmpty(x["Gsums"].ToString()) ? 0 : double.Parse(x["Gsums"].ToString())).Sum();

            var totalCsums = (from x in set.Tables[0].AsEnumerable()
                              select string.IsNullOrEmpty(x["Csums"].ToString()) ? 0 : double.Parse(x["Csums"].ToString())).Sum();

            var totalelect = (from x in set.Tables[0].AsEnumerable()
                              select string.IsNullOrEmpty(x["elect"].ToString()) ? 0 : double.Parse(x["elect"].ToString())).Sum();

            var totalwater = (from x in set.Tables[0].AsEnumerable()
                              select string.IsNullOrEmpty(x["water"].ToString()) ? 0 : double.Parse(x["water"].ToString())).Sum();

            var totalworkprice = (from x in set.Tables[0].AsEnumerable()
                              select string.IsNullOrEmpty(x["workprice"].ToString()) ? 0 : double.Parse(x["workprice"].ToString())).Sum();

            var totalmakecost = (from x in set.Tables[0].AsEnumerable()
                                  select string.IsNullOrEmpty(x["makecost"].ToString()) ? 0 : double.Parse(x["makecost"].ToString())).Sum();

            var totalCost = (from x in set.Tables[0].AsEnumerable()
                                  select string.IsNullOrEmpty(x["totalCost"].ToString()) ? 0 : double.Parse(x["totalCost"].ToString())).Sum();

            var totalunitCost = (from x in set.Tables[0].AsEnumerable()
                                  select string.IsNullOrEmpty(x["unitCost"].ToString()) ? 0 : double.Parse(x["unitCost"].ToString())).Sum();

            DataRow newrow = set.Tables[0].NewRow();
            newrow["FName"] = "总计";
            newrow["FQty"] = totalFQty.ToString("0.00");
            newrow["volume"] = totalvolume.ToString("0.00");
            newrow["Gsums"] = totalGsums.ToString("0.00");
            newrow["Csums"] = totalCsums.ToString("0.00");
            newrow["elect"] = totalelect.ToString("0.00");
            newrow["water"] = totalwater.ToString("0.00");
            newrow["workprice"] = totalworkprice.ToString("0.00");
            newrow["makecost"] = totalmakecost.ToString("0.00");
            newrow["totalCost"] = totalCost.ToString("0.00");
            newrow["unitCost"] = totalunitCost.ToString("0.00");
            set.Tables[0].Rows.Add(newrow);

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