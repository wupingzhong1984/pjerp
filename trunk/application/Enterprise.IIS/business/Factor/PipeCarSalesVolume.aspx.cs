using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Enterprise.IIS.business.Factor
{
    public partial class PipeCarSalesVolume : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private TubePriceService _customerpriceService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected TubePriceService CustomerpriceService
        {
            get { return _customerpriceService ?? (_customerpriceService = new TubePriceService()); }
            set { _customerpriceService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private FactorService _factorService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected FactorService FactorService
        {
            get { return _factorService ?? (_factorService = new FactorService()); }
            set { _factorService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private WaterSpaceService _waterspaceService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected WaterSpaceService WatorSpaceService
        {
            get { return _waterspaceService ?? (_waterspaceService = new WaterSpaceService()); }
            set { _waterspaceService = value; }
        }

        /// <summary>
        ///     客户档案
        /// </summary>
        private LHCustomer _customer;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected LHCustomer Customer
        {
            get
            {
                return _customer ?? (_customer = CustomerService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim()//
              && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _customer = value; }
        }

        private decimal Totalm3 = 0;
        private decimal TotallPrice = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        protected void InitData()
        {
            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                       + Window2.GetShowReference("../../Common/WinCustomer.aspx");
            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //作业区
            GasHelper.DropDownListDeliveryMethodDataBind(ddlDeliveryMethod);
        }

        protected void BindData()
        {
            Dictionary<string, object> parms = new Dictionary<string, object>
            {
                { "@Bdate", dateBegin.SelectedDate },
                { "@Edate", dateEnd.SelectedDate },
                { "@FPoint", ddlFDistributionPoint.SelectedValue },
                { "@FCompanyCode", txtFCode.Text },
                { "@FDeliveryMethod",ddlDeliveryMethod.SelectedValue=="-1"?"":ddlDeliveryMethod.SelectedValue }
            };

            DataSet set = SqlService.ExecuteProcedureCommand("rpt_PipeCar", parms);
            DataRow newrow = set.Tables[0].NewRow();
            newrow[0] = "";
            newrow[1] = "";
            newrow[2] = "总计";
            newrow[3] = "";
            newrow[4] = "";
            newrow[5] = "";
            newrow[6] = "";
            newrow[7] = "";
            newrow[8] = "";
            newrow[9] = "";
            newrow[10] = "";
            newrow[11] = "";
            newrow[12] = "";
            newrow[13] = "";
            newrow[14] = "";
            newrow[15] = "";
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename={1}对账表{0}.xls", SequenceGuid.GetGuidReplace(), tbxFCustomer.Text));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                LHCustomer custmoer = CustomerService.Where(p => p.FName == tbxFCustomer.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFCode.Text = custmoer.FCode;
                }
            }
        }

        /// <summary>
        ///     关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            DataRowView item = e.DataItem as DataRowView;

            if (e.RowIndex != (Grid1.DataSource as DataTable).Rows.Count - 1)
            {
                string Fcode = item["FCode"].ToString();
                LHTubePrice price = CustomerpriceService.Where(p => p.FCode == txtFCode.Text && p.FBill == "销售").FirstOrDefault();


                decimal payW = string.IsNullOrWhiteSpace(item["FPayTemperature"].ToString()) ? 0 : Convert.ToDecimal(item["FPayTemperature"].ToString());
                decimal payY = string.IsNullOrWhiteSpace(item["FPayPressure"].ToString()) ? 0 : Convert.ToDecimal(item["FPayPressure"].ToString());

                decimal recW = string.IsNullOrWhiteSpace(item["FReceiveTemperature"].ToString()) ? 0 : Convert.ToDecimal(item["FReceiveTemperature"].ToString());
                decimal recY = string.IsNullOrWhiteSpace(item["FReceivePressure"].ToString()) ? 0 : Convert.ToDecimal(item["FReceivePressure"].ToString());


                double dpayW = string.IsNullOrWhiteSpace(item["FPayTemperature"].ToString()) ? 0 : Convert.ToDouble(item["FPayTemperature"].ToString());
                double dpayY = string.IsNullOrWhiteSpace(item["FPayPressure"].ToString()) ? 0 : Convert.ToDouble(item["FPayPressure"].ToString());

                double drecW = string.IsNullOrWhiteSpace(item["FReceiveTemperature"].ToString()) ? 0 : Convert.ToDouble(item["FReceiveTemperature"].ToString());
                double drecY = string.IsNullOrWhiteSpace(item["FReceivePressure"].ToString()) ? 0 : Convert.ToDouble(item["FReceivePressure"].ToString());

                decimal v = Convert.ToDecimal(item["waterspace"].ToString());

                string fmeno = item["FMemo"].ToString();
                Regex reg = new Regex("[*][0-9]{0,2}[支]");
                if (reg.IsMatch(fmeno))
                {
                    fmeno = fmeno.Replace(reg.Match(fmeno).Value, "");
                }
                if (!string.IsNullOrWhiteSpace(fmeno))
                {
                    e.Values[2] = fmeno;
                }
                //压缩因子
                string FZ1 = "0";
                string FZ2 = "0";

                //取压缩因子系数

                //客户专用压缩因子
                LHFactor startFZ = FactorService.Where(p => p.FCompanyCode == txtFCode.Text && p.FTemperature == payW && p.FBar == payY).FirstOrDefault();
                LHFactor EndFZ = FactorService.Where(p => p.FCompanyCode == txtFCode.Text && p.FTemperature == recW && p.FBar == recY).FirstOrDefault();
                //通用压缩因子
                LHFactor TSFZ = FactorService.Where(p => p.FCompanyCode == "" && p.FTemperature == payW && p.FBar == payY).FirstOrDefault();
                LHFactor TEFZ = FactorService.Where(p => p.FCompanyCode == "" && p.FTemperature == recW && p.FBar == recY).FirstOrDefault();
                //通用赋值
                if (TSFZ != null)
                {
                    FZ1 = TSFZ.FDivisor.ToString(CultureInfo.InvariantCulture);
                }
                if (TEFZ != null)
                {
                    FZ2 = TEFZ.FDivisor.ToString(CultureInfo.InvariantCulture);
                }
                //专用赋值
                if (startFZ != null)
                {
                    FZ1 = startFZ.FDivisor.ToString(CultureInfo.InvariantCulture);
                }
                if (startFZ != null)
                {
                    FZ2 = EndFZ.FDivisor.ToString(CultureInfo.InvariantCulture);
                }


                //取客户对应排管车罐号、水溶积
                LHWaterSpace waterSpace = WatorSpaceService.Where(p => p.FCompanyCode == txtFCode.Text && p.FGCode == fmeno).FirstOrDefault();
                if (waterSpace != null)
                {
                    e.Values[2] = waterSpace.FChassisNo;
                    e.Values[3] = waterSpace.FGCode;
                    e.Values[3] = waterSpace.FGCode;
                    v = waterSpace.FM3;
                }

                string[] c1 = price.FFormula.Replace("\n","").Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
                string s1 = c1[0].Replace("\n", "");
                decimal qt;
                int i = 1;
                List<object> clist = new List<object>();
                while (i < c1.Length)
                {
                    string cq="";//替换后公式
                    string[] sp = c1[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);//提取表达式及保留小数位数
                    string[] sp1;
                    if (i+1<c1.Length)
                    {
                        sp1 = c1[i + 1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);//提取表达式及保留小数位数
                        cq = sp1[0].Replace("$W3", payW.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$W4", recW.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$Y3", payY.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$YZ1", FZ1)//
                                       .Replace("$YZ2", FZ2)//
                                       .Replace("$V", v.ToString(CultureInfo.InvariantCulture))
                                       .Replace("\n", "");
                                       
                         qt = Convert.ToDecimal(FormulaCalculator.Eval(cq));
                        if (sp1[1].ToString() != "0")
                        {
                            qt = Math.Round(qt, int.Parse(sp1[1]));
                        }
                        sp[0] = string.Format(sp[0], qt);
                        //计算替换公式
                        qt = Convert.ToDecimal(FormulaCalculator.Eval(sp[0]));
                        if (sp[1].ToString() != "0")
                        {
                            qt = Math.Round(qt, int.Parse(sp[1]));
                        }
                        clist.Add(qt.ToString());
                        i = i + 2;
                    }
                    else
                    {
                        sp = c1[i + 1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);//提取表达式及保留小数位数
                        cq = sp[0].Replace("$W3", payW.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$W4", recW.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$Y3", payY.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                                       .Replace("$YZ1", FZ1)//
                                       .Replace("$YZ2", FZ2)//
                                       .Replace("$V", v.ToString(CultureInfo.InvariantCulture))
                                       .Replace("\n", "");

                        qt = Convert.ToDecimal(FormulaCalculator.Eval(cq));
                        if (sp[1].ToString() != "0")
                        {
                            qt = Math.Round(qt, int.Parse(sp[1]));
                        }
                        sp[0] = string.Format(sp[0], qt);
                        clist.Add(qt.ToString());
                    }
                }
                //   string [] sp= c1[i].Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
                //  string cp= sp[0].Replace("$W3", payW.ToString(CultureInfo.InvariantCulture))//
                //                       .Replace("$W4", recW.ToString(CultureInfo.InvariantCulture))//
                //                       .Replace("$Y3", payY.ToString(CultureInfo.InvariantCulture))//
                //                       .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                //                       .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                //                       .Replace("$YZ1", FZ1)//
                //                       .Replace("$YZ2", FZ2)//
                //                       .Replace("$V", v.ToString(CultureInfo.InvariantCulture));
                //  decimal qt = Convert.ToDecimal(FormulaCalculator.Eval(cp));
                //  if (sp[1].ToString()!="0")
                //  {
                //      qt = Math.Round(qt, int.Parse(sp[1]));
                //  }
                //  s1 = string.Format(s1,qt);
                //}

                //计算容积 最终结果
                c1[0]=string.Format(c1[0], clist.ToArray());
               string  cp = c1[0].Replace("$W3", payW.ToString(CultureInfo.InvariantCulture))//                        
                                        .Replace("$W4", recW.ToString(CultureInfo.InvariantCulture))//
                                        .Replace("$Y3", payY.ToString(CultureInfo.InvariantCulture))//
                                        .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                                        .Replace("$Y4", recY.ToString(CultureInfo.InvariantCulture))//
                                        .Replace("$YZ1", FZ1)//
                                        .Replace("$YZ2", FZ2)//
                                        .Replace("$V", v.ToString(CultureInfo.InvariantCulture));

               decimal qty = Convert.ToDecimal(FormulaCalculator.Eval(cp));
                e.Values[9] = decimal.Round(qty,2);
                Totalm3 += qty;
                //计算金额
                if (!string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    e.Values[10] = decimal.Parse(txtPrice.Text);
                }
                e.Values[11] =decimal.Round(qty * decimal.Parse(e.Values[10].ToString()),3);
                TotallPrice += decimal.Parse(e.Values[11].ToString());
                //LHCustomerPrice price = CustomerpriceService.Where(p => p.FCode == item["FCode"].ToString(),o=>o.FCreateDate,OrderingOrders.DESC).FirstOrDefault();
            }
            else
            {
                
                e.Values[9]= string.Format("<span style=\"color:{0}\">{1}</span>", "red", decimal.Round(Totalm3,2));
                e.Values[11] = string.Format("<span style=\"color:{0}\">{1}</span>", "red",decimal.Round(TotallPrice,2));
            }
            //e.Values[3] = string.Format("<span class=\"{0}\">{1}</span>", "colorred", "");
            //e.Values[4] = string.Format("<span class=\"{0}\">{1}</span>", "colorred", "");

        }
    }
}