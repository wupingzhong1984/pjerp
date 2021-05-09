using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Enterprise.IIS.Common
{
    /// <summary>
    /// TMSReBackInfos 的摘要说明
    /// </summary>
    public class TMSReBackInfos : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string backouts = context.Request["params"];

            TmsReturnModel returnModel = new TmsReturnModel();
            StockOutDetailsService service = new StockOutDetailsService();
            DispatchCenterService centerService = new DispatchCenterService();
            StockInDetailsService stockInDetails = new StockInDetailsService();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    tmsModel outs = JsonConvert.DeserializeObject<tmsModel>(backouts);
                    if (outs.stockOutDetailsList != null)
                    {
                        foreach (LHStockOutDetails items in outs.stockOutDetailsList)
                        {
                            LHStockOutDetails details = service.Where(p => p.KeyId == items.KeyId && p.FId == items.FId ).FirstOrDefault();

                            if (details != null)
                            {
                                details.FRecycleQty = items.FRecycleQty;
                                details.FReturnQty = items.FReturnQty;

                                details.FDevice = items.FDevice;

                                details.FDeviceValue = string.IsNullOrEmpty(items.FDeviceValue.ToString()) ? 0 : Convert.ToDecimal(items.FDeviceValue);

                                details.FInTemperature = string.IsNullOrEmpty(items.FInTemperature.ToString()) //
                                    ? 0 : Convert.ToDecimal(items.FInTemperature.ToString());
                                details.FInPressure = string.IsNullOrEmpty(items.FInPressure.ToString()) //
                                    ? 0 : Convert.ToDecimal(items.FInPressure.ToString());

                                details.FOutTemperature = string.IsNullOrEmpty(items.FOutTemperature.ToString()) //
                                    ? 0 : Convert.ToDecimal(items.FOutTemperature.ToString());
                                details.FOutPressure = string.IsNullOrEmpty(items.FOutPressure.ToString()) //
                                    ? 0 : Convert.ToDecimal(items.FOutPressure.ToString());

                                details.FPayTemperature = string.IsNullOrEmpty(items.FPayTemperature.ToString()) //
                                    ? 0 : Convert.ToDecimal(items.FPayTemperature.ToString());
                                details.FPayPressure = string.IsNullOrEmpty(items.FPayPressure.ToString()) //
                                    ? 0 : Convert.ToDecimal(items.FPayPressure.ToString());

                                details.FReceiveTemperature = string.IsNullOrEmpty(items.FReceiveTemperature.ToString()) //
                                                    ? 0 : Convert.ToDecimal(items.FReceiveTemperature.ToString());
                                details.FReceivePressure = string.IsNullOrEmpty(items.FReceivePressure.ToString()) //
                                    ? 0 : Convert.ToDecimal(items.FReceivePressure.ToString());

                                //数量计算
                                LHStockOut StockOut = new StockOutService().Where(p => p.KeyId == items.KeyId).FirstOrDefault();
                                string FCode = StockOut.FCode;
                                //计算公式
                                LHTubePrice v1 = new TubePriceService().Where(p => p.FBill == "销售"//
                                    && p.FCode == FCode//
                                    && p.FItemCode == details.FItemCode).FirstOrDefault();

                                if (v1 != null)
                                {
                                    //< font color = "red" >$W1：进厂温度、$W2：出厂温度、$W3: 交付温度、$W4: 接收温度、$V: 水容积 </ font >< br />
                                    //< font color = "red" >$Y1：进厂压力、$Y2：出厂压力、$Y3: 交付压力、$Y4: 接收压力 </ font >< br />

                                    string inW = Convert.ToDecimal(details.FInTemperature).ToString(CultureInfo.InvariantCulture);
                                    string inY = Convert.ToDecimal(details.FInPressure).ToString(CultureInfo.InvariantCulture);

                                    string outW = Convert.ToDecimal(details.FOutTemperature).ToString(CultureInfo.InvariantCulture);
                                    string outY = Convert.ToDecimal(details.FOutPressure).ToString(CultureInfo.InvariantCulture); ;

                                    string payW = Convert.ToDecimal(details.FPayTemperature).ToString(CultureInfo.InvariantCulture);
                                    string payY = Convert.ToDecimal(details.FPayPressure).ToString(CultureInfo.InvariantCulture);

                                    string recW = Convert.ToDecimal(details.FReceiveTemperature).ToString(CultureInfo.InvariantCulture);
                                    string recY = Convert.ToDecimal(details.FReceivePressure).ToString(CultureInfo.InvariantCulture);

                                    //水溶积
                                    string v = Convert.ToDecimal(details.FDeviceValue).ToString(CultureInfo.InvariantCulture);

                                    //数量
                                    string s1 = v1.FFormula.Replace("$W1", inW)//
                                        .Replace("$W2", outW)
                                        .Replace("$W3", payW)//
                                        .Replace("$W4", recW)//
                                        .Replace("$Y1", inY)//
                                        .Replace("$Y2", outY)//
                                        .Replace("$Y3", payY)//
                                        .Replace("$Y4", recY)//
                                        .Replace("$V", v);

                                    //数量 
                                    details.FQty = Convert.ToDecimal(FormulaCalculator.Eval(s1));
                                    //金额
                                    details.FAmount = details.FQty * details.FPrice;

                                }


                                service.SaveChanges();
                            }
                        }
                    }
                    if (outs.dispatchCenterList != null)
                    {
                        foreach (LHDispatchCenter item in outs.dispatchCenterList)
                        {
                            LHDispatchCenter lH = centerService.Where(p => p.KeyId == item.KeyId & p.FAuditFlag==0).FirstOrDefault();
                            if (lH!=null)
                            {
                                lH.FDriver = item.FDriver;
                                lH.FSupercargo = item.FSupercargo;
                                lH.FMileage = item.FMileage;
                            }
                            
                            centerService.SaveChanges();
                        }
                    }
                    trans.Complete();
                }

                returnModel.code = "200";
                returnModel.msg = "成功";
            }
            catch (Exception ex)
            {
                returnModel.code = "500";
                returnModel.msg = "失败";
                returnModel.msg = ex.Message;
            }



            context.Response.Write(JsonConvert.SerializeObject(returnModel));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}