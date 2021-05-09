using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.IIS.Common;
using FineUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Enterprise.IIS.business.Reports
{
    public partial class SalaryDriver : PageBase
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

        #region Protected Method

        /// <summary>
        ///     页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

                dpkFDateBegin.SelectedDate = DateTime.Now;

                dpkFDateEnd.SelectedDate = DateTime.Now;

                GasHelper.DropDownListCompanyDataBind(ddlCompany);

                //GasHelper.DropDownListProducerDataBind(ddlFProducer);

                //GasHelper.DropDownListWorkshopDataBind(ddlWorkShop);

                //GasHelper.DropDownListDriverDataBind(ddlFDriver);

                //GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);

                GasHelper.DropDownListVehicleClassDataBind(ddllx);


                btnExport.Hidden = true;
                Region4.Hidden = true;
                btnshowcash.Hidden = true;
            }
        }

        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SortField = string.Format(@"{0}", e.SortField);
            SortDirection = e.SortDirection;
            BindDataGrid();
        }

        /// <summary>
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
        }

        protected void tbxFName_OnTextChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(tbxFName.Text.Trim()))
            //{
            //    var bottle = GasHelper.BottleByCode(txtFCode.Text, CurrentUser.AccountComId);

            //    if (bottle != null)
            //        tbxFBottle.SelectedValue = bottle.FBottleCode;
            //}
        }

        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDataGrid();
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=运输配送计件表{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExports_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=运输配送计件工资{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid2));
            Response.End();
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }
        #endregion

        #region Private Method

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", Convert.ToInt32(ddlCompany.SelectedValue));
            parms.Add("@BeginDate", Convert.ToDateTime(dpkFDateBegin.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@EndDate", Convert.ToDateTime(dpkFDateEnd.SelectedDate).ToString("yyyy-MM-dd"));
            parms.Add("@FDriver", "-1");//ddlFDriver.SelectedValue
            parms.Add("@FSupercargo", "-1");//ddlFSupercargo.SelectedValue
            parms.Add("@FVehicleType", ddllx.SelectedValue);//ddlFSupercargo.SelectedValue
            //

            DataTable list = SqlService.ExecuteProcedureCommand("FSalary_Driver", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                //整理数据使其可准确计算

                #region 整理数据
                DataTable tb = new DataTable();
                tb.Columns.Add("FLogistics");
                tb.Columns.Add("CarNum");
                tb.Columns.Add("Driver");
                tb.Columns.Add("Cargo");
                tb.Columns.Add("StartTime");
                tb.Columns.Add("EndTime");
                tb.Columns.Add("Mileage");
                tb.Columns.Add("UnloadingPoint");
                tb.Columns.Add("Yout");
                tb.Columns.Add("Sout");
                tb.Columns.Add("Jgout");
                tb.Columns.Add("Sin");
                tb.Columns.Add("Yin");
                tb.Columns.Add("Jgin");
                tb.Columns.Add("Wait");
                tb.Columns.Add("Stay");
                tb.Columns.Add("MoreThanThree");
                tb.Columns.Add("StartMileage");
                tb.Columns.Add("EndMileage");
                tb.Columns.Add("CarType");
                DataRow oldrow = null;//上一级数据
                foreach (DataRow item in list.Rows)
                {
                    DataRow newrow = tb.NewRow();
                    newrow["FLogistics"] = item["FLogistics"];
                    string[] drivers = item["FDriver"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries
                         );
                    string[] cargos = item["FSupercargo"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries
                         );
                    if (drivers.Length > 1) ///多个司机
                    {
                        for (int i = 0; i < drivers.Length; i++)
                        {
                            decimal points = decimal.Parse(item["TaskQty"].ToString());
                            int point = Convert.ToInt32(points);
                            newrow["CarNum"] = item["FVehicleNum"] == DBNull.Value ? "" : item["FVehicleNum"].ToString();
                            newrow["Driver"] = drivers[i];
                            newrow["StartTime"] = item["FBeginDate"] == DBNull.Value ? "" : DateTime.Parse(item["FBeginDate"].ToString()).ToString("yyyy/M/d");
                            newrow["EndTime"] = item["FEndDate"] == DBNull.Value ? "" : DateTime.Parse(item["FEndDate"].ToString()).ToString("yyyy/M/d");
                            newrow["Mileage"] = item["FMileage"] == DBNull.Value ? 0 : decimal.Parse(item["FMileage"].ToString());

                            newrow["UnloadingPoint"] = item["TaskQty"] == DBNull.Value ? 0 : point;
                            newrow["Yout"] = item["FDriverYQty"] == DBNull.Value ? 0 : decimal.Parse(item["FDriverYQty"].ToString());
                            newrow["Sout"] = item["FDriverQty"] == DBNull.Value ? 0 : decimal.Parse(item["FDriverQty"].ToString());
                            newrow["Jgout"] = item["FDriverJQty"] == DBNull.Value ? 0 : decimal.Parse(item["FDriverJQty"].ToString());
                            newrow["Sin"] = 0;
                            newrow["Yin"] = 0;
                            newrow["Jgin"] = 0;
                            newrow["Wait"] = 0;
                            newrow["Stay"] = 0;
                            newrow["MoreThanThree"] = 0;
                            newrow["StartMileage"] = 0;
                            newrow["EndMileage"] = 0;

                            if (item["FVehicleNum"].ToString().Contains("0771") || item["FVehicleNum"].ToString().Contains("92052") || item["FVehicleNum"].ToString().Contains("2115") || item["FVehicleNum"].ToString().Contains("0505") || item["FVehicleNum"].ToString().Contains("6072") || item["FVehicleNum"].ToString().Contains("5330") || item["FVehicleNum"].ToString().Contains("6556") || item["FVehicleNum"].ToString().Contains("0760"))
                            {
                                newrow["CarType"] = "排管车";
                            }
                            else
                            {
                                newrow["CarType"] = "栏板车";
                            }
                            if (cargos.Length == 1 && i == 0)
                            {
                                newrow["Cargo"] = cargos[0];
                            }

                            tb.Rows.Add(newrow.ItemArray);
                        }
                    }
                    else if (cargos.Length > 1)///多个押运员
                    {
                        for (int i = 0; i < cargos.Length; i++)
                        {
                            decimal points = decimal.Parse(item["TaskQty"].ToString());
                            int point = Convert.ToInt32(points);
                            newrow["CarNum"] = item["FVehicleNum"] == DBNull.Value ? "" : item["FVehicleNum"].ToString();
                            newrow["Cargo"] = cargos[i];
                            newrow["StartTime"] = item["FBeginDate"] == DBNull.Value ? "" : DateTime.Parse(item["FBeginDate"].ToString()).ToString("yyyy/M/d");
                            newrow["EndTime"] = item["FEndDate"] == DBNull.Value ? "" : DateTime.Parse(item["FEndDate"].ToString()).ToString("yyyy/M/d");
                            newrow["Mileage"] = item["FMileage"] == DBNull.Value ? 0 : decimal.Parse(item["FMileage"].ToString());
                            newrow["UnloadingPoint"] = item["TaskQty"] == DBNull.Value ? 0 : point;
                            newrow["Yout"] = item["FDriverYQty"] == DBNull.Value ? 0 : decimal.Parse(item["FDriverYQty"].ToString());
                            newrow["Sout"] = item["FDriverQty"] == DBNull.Value ? 0 : decimal.Parse(item["FDriverQty"].ToString());
                            newrow["Jgout"] = item["FDriverJQty"] == DBNull.Value ? 0 : decimal.Parse(item["FDriverJQty"].ToString());


                            newrow["Sin"] = 0;
                            newrow["Yin"] = 0;
                            newrow["Jgin"] = 0;
                            newrow["Sin"] = 0;
                            newrow["Yin"] = 0;
                            newrow["Jgin"] = 0;
                            newrow["Wait"] = 0;
                            newrow["Stay"] = 0;
                            newrow["MoreThanThree"] = 0;
                            newrow["StartMileage"] = 0;
                            newrow["EndMileage"] = 0;

                            if (item["FVehicleNum"].ToString().Contains("0771") || item["FVehicleNum"].ToString().Contains("92052") || item["FVehicleNum"].ToString().Contains("2115") || item["FVehicleNum"].ToString().Contains("0505") || item["FVehicleNum"].ToString().Contains("6072") || item["FVehicleNum"].ToString().Contains("5330") || item["FVehicleNum"].ToString().Contains("6556") || item["FVehicleNum"].ToString().Contains("0760"))
                            {
                                newrow["CarType"] = "排管车";
                            }
                            else
                            {
                                newrow["CarType"] = "栏板车";
                            }
                            if (drivers.Length == 1 && i == 0)
                            {
                                newrow["Driver"] = drivers[0];
                            }
                            tb.Rows.Add(newrow.ItemArray);
                        }
                    }
                    else
                    {
                        //单司机 押运
                        decimal points = decimal.Parse(item["TaskQty"].ToString());
                        int point = Convert.ToInt32(points);
                        newrow["CarNum"] = item["FVehicleNum"] == DBNull.Value ? "" : item["FVehicleNum"].ToString();
                        newrow["Driver"] = item["FDriver"] == DBNull.Value ? "" : item["FVehicleNum"].ToString();
                        newrow["Cargo"] = item["FSupercargo"] == DBNull.Value ? "" : item["FSupercargo"].ToString();
                        newrow["StartTime"] = item["FBeginDate"] == DBNull.Value ? "" : DateTime.Parse(item["FBeginDate"].ToString()).ToString("yyyy/M/d");
                        newrow["EndTime"] = item["FEndDate"] == DBNull.Value ? "" : DateTime.Parse(item["FEndDate"].ToString()).ToString("yyyy/M/d");
                        newrow["Mileage"] = item["FMileage"] == DBNull.Value ? 0 : decimal.Parse(item["FMileage"].ToString());


                        newrow["UnloadingPoint"] = item["TaskQty"] == DBNull.Value ? 0 : point;
                        newrow["Yout"] = item["FDriverYQty"] == DBNull.Value ? 0 : decimal.Parse(item["FDriverYQty"].ToString());
                        newrow["Sout"] = item["FDriverQty"] == DBNull.Value ? 0 : decimal.Parse(item["FDriverQty"].ToString());
                        newrow["Jgout"] = item["FDriverJQty"] == DBNull.Value ? 0 : decimal.Parse(item["FDriverJQty"].ToString());
                        newrow["Driver"] = item["FDriver"] == DBNull.Value ? "" : item["FDriver"].ToString();

                        newrow["Sin"] = 0;
                        newrow["Yin"] = 0;
                        newrow["Jgin"] = 0;
                        newrow["Sin"] = 0;
                        newrow["Yin"] = 0;
                        newrow["Jgin"] = 0;
                        newrow["Wait"] = 0;
                        newrow["Stay"] = 0;
                        newrow["MoreThanThree"] = 0;
                        newrow["StartMileage"] = 0;
                        newrow["EndMileage"] = 0;

                        if (item["FVehicleNum"].ToString().Contains("BG2115") || item["FVehicleNum"].ToString().Contains("BQ0760") || item["FVehicleNum"].ToString().Contains("EL5330") || item["FVehicleNum"].ToString().Contains("D92052") || item["FVehicleNum"].ToString().Contains("ES6556") || item["FVehicleNum"].ToString().Contains("DC0505") || item["FVehicleNum"].ToString().Contains("EL5330") || item["FVehicleNum"].ToString().Contains("EH6072") || item["FVehicleNum"].ToString().Contains("BQ0771"))
                        {
                            newrow["CarType"] = "排管车";
                        }
                        else
                        {
                            newrow["CarType"] = "栏板车";
                        }

                        tb.Rows.Add(newrow.ItemArray);
                    }
                    oldrow = newrow;
                }
                #endregion

                var pgdriver = (from xx in tb.AsEnumerable()
                                where xx["carType"].Equals("排管车") && xx["driver"].ToString() != ""
                                group xx by new { driver = xx["driver"], time = xx["EndTime"] } into g
                                select new
                                {
                                    drivertype = "排管车驾驶员",
                                    driver = g.Key.driver,
                                    time = g.Key.time,
                                    Mileage = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString())),
                                    UnloadingPoint = g.Sum(x => Convert.ToDecimal(x["UnloadingPoint"].ToString() == "" ? "0" : x["UnloadingPoint"].ToString())),
                                    Yout = g.Sum(x => Convert.ToDecimal(x["Yout"].ToString() == "" ? "0" : x["Yout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Yin"].ToString() == "" ? "0" : x["Yin"].ToString())),
                                    Sout = g.Sum(x => Convert.ToDecimal(x["Sout"].ToString() == "" ? "0" : x["Sout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Sin"].ToString() == "" ? "0" : x["Sin"].ToString())),
                                    Jgout = g.Sum(x => Convert.ToDecimal(x["Jgout"].ToString() == "" ? "0" : x["Jgout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Jgin"].ToString() == "" ? "0" : x["Jgin"].ToString())),
                                    Wait = g.Sum(x => Convert.ToDecimal(x["Wait"].ToString() == "" ? "0" : x["Wait"].ToString())),
                                    Stay = g.Sum(x => Convert.ToDecimal(x["Stay"].ToString() == "" ? "0" : x["Stay"].ToString())),
                                    morethanthree = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) > 300 ? Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) - 300 : 0),

                                    milageprice = decimal.Parse("0.3") * g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString())),
                                    pointprice = g.Sum(x => Convert.ToDecimal(x["UnloadingPoint"].ToString() == "" ? "0" : x["UnloadingPoint"].ToString())) * decimal.Parse("60"),
                                    Yprice = decimal.Parse("0.00"),
                                    Sprice = decimal.Parse("0.00"),
                                    JgPrice = decimal.Parse("0.00"),
                                    Waitprice = 0,
                                    Stayprice = 0,
                                    threeprice = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) > 300 ? Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) - 300 : 0) * decimal.Parse("0.45") ,
                                    Yunit = "0",
                                    Sunit = "0",
                                    jgunit = "0"
                                }).Union(
                        from xx in tb.AsEnumerable()
                        where xx["carType"].Equals("排管车") && xx["cargo"].ToString() != ""
                        group xx by new { driver = xx["cargo"], time = xx["EndTime"] } into g
                        select new
                        {
                            drivertype = "排管车押运员",
                            driver = g.Key.driver,
                            time = g.Key.time,
                            Mileage = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString())),
                            UnloadingPoint = g.Sum(x => Convert.ToDecimal(x["UnloadingPoint"].ToString() == "" ? "0" : x["UnloadingPoint"].ToString())),
                            Yout = g.Sum(x => Convert.ToDecimal(x["Yout"].ToString() == "" ? "0" : x["Yout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Yin"].ToString() == "" ? "0" : x["Yin"].ToString())),
                            Sout = g.Sum(x => Convert.ToDecimal(x["Sout"].ToString() == "" ? "0" : x["Sout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Sin"].ToString() == "" ? "0" : x["Sin"].ToString())),
                            Jgout = g.Sum(x => Convert.ToDecimal(x["Jgout"].ToString() == "" ? "0" : x["Jgout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Jgin"].ToString() == "" ? "0" : x["Jgin"].ToString())),
                            Wait = g.Sum(x => Convert.ToDecimal(x["Wait"].ToString() == "" ? "0" : x["Wait"].ToString())),
                            Stay = g.Sum(x => Convert.ToDecimal(x["Stay"].ToString() == "" ? "0" : x["Stay"].ToString())),
                            morethanthree = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) > 300 ? Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) - 300 : 0),

                            milageprice = decimal.Parse("0.1") * g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString())),
                            pointprice = g.Sum(x => Convert.ToDecimal(x["UnloadingPoint"].ToString() == "" ? "0" : x["UnloadingPoint"].ToString())) * decimal.Parse("60"),
                            Yprice = decimal.Parse("0.00"),
                            Sprice = decimal.Parse("0.00"),
                            JgPrice = decimal.Parse("0.00"),
                            Waitprice = 0,
                            Stayprice = 0,
                            threeprice = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) > 300 ? Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) - 300 : 0) * decimal.Parse("0.2"),
                            Yunit = "0",
                            Sunit = "0",
                            jgunit = "0"

                        }
                    ).
                    /////栏板车
                    Union(
                    from xx in tb.AsEnumerable()
                    where xx["carType"].Equals("栏板车") && xx["driver"].ToString() != ""
                    group xx by new { driver = xx["driver"], time = xx["EndTime"] } into g
                    select new
                    {
                        drivertype = "栏板车驾驶员",
                        driver = g.Key.driver,
                        time = g.Key.time,
                        Mileage = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString())),
                        UnloadingPoint = g.Sum(x => Convert.ToDecimal(x["UnloadingPoint"].ToString() == "" ? "0" : x["UnloadingPoint"].ToString())),
                        Yout = g.Sum(x => Convert.ToDecimal(x["Yout"].ToString() == "" ? "0" : x["Yout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Yin"].ToString() == "" ? "0" : x["Yin"].ToString())),
                        Sout = g.Sum(x => Convert.ToDecimal(x["Sout"].ToString() == "" ? "0" : x["Sout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Sin"].ToString() == "" ? "0" : x["Sin"].ToString())),
                        Jgout = g.Sum(x => Convert.ToDecimal(x["Jgout"].ToString() == "" ? "0" : x["Jgout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Jgin"].ToString() == "" ? "0" : x["Jgin"].ToString())),
                        Wait = g.Sum(x => Convert.ToDecimal(x["Wait"].ToString() == "" ? "0" : x["Wait"].ToString())),
                        Stay = g.Sum(x => Convert.ToDecimal(x["Stay"].ToString() == "" ? "0" : x["Stay"].ToString())),
                        morethanthree = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) > 300 ? Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) - 300 : 0),

                        milageprice = decimal.Parse("0.25") * g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString())),
                        pointprice = g.Sum(x => Convert.ToDecimal(x["UnloadingPoint"].ToString() == "" ? "0" : x["UnloadingPoint"].ToString())) * decimal.Parse("16"),
                        Yprice = (g.Sum(x => Convert.ToDecimal(x["Yout"].ToString() == "" ? "0" : x["Yout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Yin"].ToString() == "" ? "0" : x["Yin"].ToString()))) * decimal.Parse("10.00"),
                        Sprice = (g.Sum(x => Convert.ToDecimal(x["Sout"].ToString() == "" ? "0" : x["Sout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Sin"].ToString() == "" ? "0" : x["Sin"].ToString()))) * decimal.Parse("0.60"),
                        JgPrice = (g.Sum(x => Convert.ToDecimal(x["Jgout"].ToString() == "" ? "0" : x["Jgout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Jgin"].ToString() == "" ? "0" : x["Jgin"].ToString()))) * decimal.Parse("0.20"),
                        Waitprice = 0,
                        Stayprice = 0,
                        threeprice = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) > 300 ? Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) - 300 : 0) * decimal.Parse("0.4") ,
                        Yunit = "10",
                        Sunit = "0.60",
                        jgunit = "0.20"
                    }
                    ).Union(
                    from xx in tb.AsEnumerable()
                    where xx["carType"].Equals("栏板车") && xx["cargo"].ToString() != ""
                    group xx by new { driver = xx["cargo"], time = xx["EndTime"] } into g
                    select new
                    {
                        drivertype = "栏板车押运员",
                        driver = g.Key.driver,
                        time = g.Key.time,
                        Mileage = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString())),
                        UnloadingPoint = g.Sum(x => Convert.ToDecimal(x["UnloadingPoint"].ToString() == "" ? "0" : x["UnloadingPoint"].ToString())),
                        Yout = g.Sum(x => Convert.ToDecimal(x["Yout"].ToString() == "" ? "0" : x["Yout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Yin"].ToString() == "" ? "0" : x["Yin"].ToString())),
                        Sout = g.Sum(x => Convert.ToDecimal(x["Sout"].ToString() == "" ? "0" : x["Sout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Sin"].ToString() == "" ? "0" : x["Sin"].ToString())),
                        Jgout = g.Sum(x => Convert.ToDecimal(x["Jgout"].ToString() == "" ? "0" : x["Jgout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Jgin"].ToString() == "" ? "0" : x["Jgin"].ToString())),

                        Wait = g.Sum(x => Convert.ToDecimal(x["Wait"].ToString() == "" ? "0" : x["Wait"].ToString())),
                        Stay = g.Sum(x => Convert.ToDecimal(x["Stay"].ToString() == "" ? "0" : x["Stay"].ToString())),
                        morethanthree = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) > 300 ? Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) - 300 : 0),

                        milageprice = decimal.Parse("0.05") * g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString())),
                        pointprice = g.Sum(x => Convert.ToDecimal(x["UnloadingPoint"].ToString() == "" ? "0" : x["UnloadingPoint"].ToString())) * decimal.Parse("12"),
                        Yprice = (g.Sum(x => Convert.ToDecimal(x["Yout"].ToString() == "" ? "0" : x["Yout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Yin"].ToString() == "" ? "0" : x["Yin"].ToString()))) * decimal.Parse("10.00"),
                        Sprice = (g.Sum(x => Convert.ToDecimal(x["Sout"].ToString() == "" ? "0" : x["Sout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Sin"].ToString() == "" ? "0" : x["Sin"].ToString()))) * decimal.Parse("1.10"),
                        JgPrice = (g.Sum(x => Convert.ToDecimal(x["Jgout"].ToString() == "" ? "0" : x["Jgout"].ToString())) + g.Sum(x => Convert.ToDecimal(x["Jgin"].ToString() == "" ? "0" : x["Jgin"].ToString()))) * decimal.Parse("0.60"),
                        Waitprice = 0,
                        Stayprice = 0,
                        threeprice = g.Sum(x => Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) > 300 ? Convert.ToDecimal(x["Mileage"].ToString() == "" ? "0" : x["Mileage"].ToString()) - 300 : 0) * decimal.Parse("0.35"),
                        Yunit = "10",
                        Sunit = "1.10",
                        jgunit = "0.60"

                    }
                    );
                var kk = pgdriver.ToList();

                //绑定数据源
                Grid1.DataSource = list;
                Grid1.DataBind();

                Grid2.DataSource = pgdriver;
                Grid2.DataBind();

                //合计
                JObject summary = new JObject
                    {
                        {"KeyId", "合计"},
                        {"FMileage", list.Compute("sum(FMileage)","true").ToString()},
                        {"FDriverMileageAmt", list.Compute("sum(FDriverMileageAmt)","true").ToString()},
                        {"FSupercargoMileageAmt", list.Compute("sum(FSupercargoMileageAmt)","true").ToString()},
                        {"FDiffDayAmount", list.Compute("sum(FDiffDayAmount)","true").ToString()},
                        {"TaskQty", list.Compute("sum(TaskQty)","true").ToString()},
                        {"FDriverTAmount", list.Compute("sum(FDriverTAmount)","true").ToString()},
                        {"FSupercargoTAmount", list.Compute("sum(FSupercargoTAmount)","true").ToString()},
                        {"FDriverYQty", list.Compute("sum(FDriverYQty)","true").ToString()},
                        {"FDriverYAmount", list.Compute("sum(FDriverYAmount)","true").ToString()},
                        {"FDriverQty", list.Compute("sum(FDriverQty)","true").ToString()},
                        {"FDriverAmount", list.Compute("sum(FDriverAmount)","true").ToString()},
                        {"FDriverJQty", list.Compute("sum(FDriverJQty)","true").ToString()},
                        {"FDriverJAmount", list.Compute("sum(FDriverJAmount)","true").ToString()},
                        {"FSupercargoYQty", list.Compute("sum(FSupercargoYQty)","true").ToString()},
                        {"FSupercargoYAmount", list.Compute("sum(FSupercargoYAmount)","true").ToString()},
                        {"FSupercargoQty", list.Compute("sum(FSupercargoQty)","true").ToString()},
                        {"FSupercargoAmount", list.Compute("sum(FSupercargoAmount)","true").ToString()},
                        {"FSupercargoJQty", list.Compute("sum(FSupercargoJQty)","true").ToString()},
                        {"FSupercargoJAmount", list.Compute("sum(FSupercargoJAmount)","true").ToString()},
                    };

                Grid1.SummaryData = summary;

                JObject jObject = new JObject {
                    {"driver", "合计"},
                        {"milageprice", pgdriver.Sum(x=>x.milageprice).ToString()},
                        {"pointprice", pgdriver.Sum(x=>x.pointprice).ToString()},
                        {"Yprice", pgdriver.Sum(x=>x.Yprice).ToString()},
                        {"Sprice", pgdriver.Sum(x=>x.Sprice).ToString()},
                        {"JgPrice", pgdriver.Sum(x=>x.JgPrice).ToString()},
                        {"threeprice", pgdriver.Sum(x=>x.threeprice).ToString()},
                        {"Mileage", pgdriver.Sum(x=>x.Mileage).ToString()},

                        {"Yout", pgdriver.Sum(x=>x.Yout).ToString()},
                        {"Sout", pgdriver.Sum(x=>x.Sout).ToString()},
                        {"Jgout", pgdriver.Sum(x=>x.Jgout).ToString()},
                        {"UnloadingPoint", pgdriver.Sum(x=>x.UnloadingPoint).ToString()},
                };

                Grid2.SummaryData = jObject;

            }
            else
            {
                Grid1.DataSource = null;
                //合计
                JObject summary = new JObject
                    {
                        {"KeyId", "合计"},
                        {"FMileage", "0.00"},
                    };

                Grid1.SummaryData = summary;
            }
        }

        #endregion

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);

                    return;
                }

                string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                PageContext.RegisterStartupScript(
                    Window2.GetShowReference(string.Format("../../Common/WinSalaryDriver.aspx?KeyId={0}&action=2&Bill=1",
                        sid), "修改配送"));


            }
            catch (Exception)
            {
                Alert.Show("修改配送失败！", MessageBoxIcon.Warning);
            }
        }

        protected void btnshowdetail_Click(object sender, EventArgs e)
        {

            btnExport.Hidden = false;
            Region4.Hidden = false;
            btnshowdetail.Hidden = false;
            btnshowdetail.Hidden = true;


            Button2.Hidden = true;
            btnshowcash.Hidden = true;
            Region2.Hidden = true;
            btnshowcash.Hidden = false;
        }

        protected void btnshowcash_Click(object sender, EventArgs e)
        {

            btnExport.Hidden = true;
            Region4.Hidden = true;
            btnshowdetail.Hidden = true;
            btnshowdetail.Hidden = false;

            Button2.Hidden = false;
            btnshowcash.Hidden = false;
            Region2.Hidden = false;
            btnshowcash.Hidden = true;
        }
    }
}