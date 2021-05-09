using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Linq;
using Enterprise.DataAccess.SQLServer;
using Enterprise.Framework.FormsAuth;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;

namespace Enterprise.IIS.Common
{
    /// <summary>
    /// AjaxPrinter 业务单据凭证打印
    /// </summary>
    public class AjaxPrinter : IHttpHandler
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private PassCardService _passCardService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected PassCardService PassCardService
        {
            get { return _passCardService ?? (_passCardService = new PassCardService()); }
            set { _passCardService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private PassCardDetailsService _passCardDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected PassCardDetailsService PassCardDetailsService
        {
            get
            {
                return _passCardDetailsService ?? //
                    (_passCardDetailsService = new PassCardDetailsService());
            }
            set { _passCardDetailsService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockOutService _stockOutService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutService StockOutService
        {
            get { return _stockOutService ?? (_stockOutService = new StockOutService()); }
            set { _stockOutService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockInService _stockInService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockInService StockInService
        {
            get { return _stockInService ?? (_stockInService = new StockInService()); }
            set { _stockInService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseService _leaseService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseService LeaseService
        {
            get { return _leaseService ?? (_leaseService = new LeaseService()); }
            set { _leaseService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseReturnService _leaseReturnService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseReturnService LeaseReturnService
        {
            get { return _leaseReturnService ?? (_leaseReturnService = new LeaseReturnService()); }
            set { _leaseReturnService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockOutDetailsService _stockOutDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutDetailsService StockOutDetailsService
        {
            get { return _stockOutDetailsService ?? (_stockOutDetailsService = new StockOutDetailsService()); }
            set { _stockOutDetailsService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private SKOrderService _skOrderService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SKOrderService SkOrderService
        {
            get { return _skOrderService ?? (_skOrderService = new SKOrderService()); }
            set { _skOrderService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private FKOrderService _fkOrderService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FKOrderService FkOrderService
        {
            get { return _fkOrderService ?? (_fkOrderService = new FKOrderService()); }
            set { _fkOrderService = value; }
        }

        private DispatchCenterService _dispathCenterService;
        public DispatchCenterService DispathCenterService
        {
            get { return _dispathCenterService ?? (_dispathCenterService = new DispatchCenterService()); }
            set { _dispathCenterService = value; }
        }

        private DispatchService _dispatchService;
        public DispatchService DispatchService
        {
            get { return _dispatchService ?? (_dispatchService = new DispatchService()); }
            set { _dispatchService = value; }
        }

        private DispatchDetailsService _dispatchDetailsService;
        public DispatchDetailsService DispatchDetailsService
        {
            get { return _dispatchDetailsService ?? (_dispatchDetailsService = new DispatchDetailsService()); }
            set { _dispatchDetailsService = value; }
        }
        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerLinkService _customerLinkService;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected CustomerLinkService CustomerLinkService
        {
            get { return _customerLinkService ?? (_customerLinkService = new CustomerLinkService()); }
            set { _customerLinkService = value; }
        }
        private string _operType = string.Empty;
        private string _response = string.Empty;

        /// <summary>
        ///     ProcessRequest
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            _operType = context.Request["oper"] ?? "";
            switch (_operType)
            {
                case "ajaxPrintSales"://发货单
                    AjaxPrintSales(context);
                    break;

                case "ajaxPrintAllotTrans"://调拨出库单
                    AjaxPrintAllotTrans(context);
                    break;

                case "ajaxPrintAllotPlan"://调拨申请单
                    AjaxPrintAllotPlan(context);
                    break;

                case "ajaxPrintTubeSales"://氢气发货单
                    AjaxPrintTubeSales(context);
                    break;

                case "ajaxPrintTubeVehicle"://排管车出门证
                    AjaxPrintTubeVehicle(context);
                    break;

                //ajaxPrintSwap
                case "ajaxPrintSwap"://换货单
                    AjaxPrintSwap(context);
                    break;

                //ajaxPrintBlank
                case "ajaxPrintBlank"://空白单
                    AjaxPrintBlank(context);
                    break;

                case "ajaxPrintSales2"://发货单
                    AjaxPrintSales2(context);
                    break;

                case "ajaxPrintPassCard"://销售订单

                    AjaxPrintPassCard(context);
                    break;

                case "ajaxPrintSalesRec"://发货单
                    AjaxPrintSalesRec(context);
                    break;

                case "ajaxPrintSalesReturn"://销售退货单
                    AjaxPrintSalesReturn(context);
                    break;

                case "ajaxPrintPurchase"://采购单
                    AjaxPrintPurchase(context);
                    break;

                case "ajaxPrintPurchaseReturn"://采购退货
                    AjaxPrintPurchaseReturn(context);
                    break;

                case "ajaxPrintStockIn"://其它入库单
                    AjaxPrintStockIn(context);
                    break;

                case "ajaxPrintStockOut"://其它出库单
                    AjaxPrintStockOut(context);
                    break;

                case "ajaxPrintAllot"://调拨单
                    AjaxPrintAllot(context);
                    break;

                case "ajaxPrintAllotDept"://调拨单
                    AjaxPrintAllotDept(context);
                    break;

                case "ajaxPrintProduction"://充气入库单
                    AjaxPrintProduction(context);
                    break;

                case "ajaxPrintLease"://气瓶押金单
                    AjaxPrintLease(context);
                    break;

                case "ajaxPrintLeaseB"://气瓶押金单
                    AjaxPrintLeaseB(context);
                    break;

                case "ajaxPrintLeaseReturn"://气瓶租金收款单
                    AjaxPrintLeaseReturn(context);
                    break;

                case "ajaxPrintLossess"://盘亏单
                    AjaxPrintLosses(context);
                    break;

                case "ajaxPrintProfit"://盘盈单
                    AjaxPrintProfit(context);
                    break;

                case "ajaxPrintSK"://收款单
                    AjaxPrintSK(context);
                    break;

                case "ajaxPrintFK"://付款单
                    AjaxPrintFK(context);
                    break;

                case "ajaxPrintBottleReturn"://回瓶入库
                    AjaxPrintBottleReturn(context);
                    break;

                case "ajaxPrintVehicle"://出门单
                    AjaxPrintVehicle(context);
                    break;

                case "ajaxPrintDispatch":// 货车运输记录单
                    AjaxPrintDispatch(context);
                    break;

                case "ajaxPrintVehiclePass"://
                    AjaxPrintVehiclePass(context);
                    break;

                case "ajaxPrintBottleTo"://
                    AjaxPrintBottleTo(context);
                    break;

                case "ajaxPrintLogDoc"://运管监管行车记录日志
                    AjaxPrintLogDoc(context);
                    break;

                case "ajaxPrintPost"://运管监管行车记录日志
                    AjaxPrintPost(context);
                    break;
            }
            context.Response.Write(_response);
        }

        private void AjaxPrintPost(HttpContext context)
        {
            var keyid = Convert.ToInt32(context.Request["FId"]);
            var stock = CustomerLinkService.FirstOrDefault(p => p.FId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();
            if (stock != null)
            {
                print.Append("<br/>");
                print.Append("<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/>");
                print.Append("<table style='font-size:15px;' align='left' width='750' cellspacing='0' border='0'");
                print.AppendFormat("<tr height='22'><td width='60px'></td><td width='110px'>{0}</td><td>{1}</td></tr>", stock.FLinkman,stock.FMoile+"/"+stock.FPhome);

                //if (stock.FName.Length >= 15)
                //{
                    print.AppendFormat("<tr height='22'><td></td><td colspan=2>{0}</td></tr>", stock.FName.Substring(0,15)+"<br/>"+ stock.FName);//.Substring(16, stock.FName.Length-16));
                //}
                //else
                //{
                  //  print.AppendFormat("<tr height='22'><td></td><td colspan=2>{0}</td></tr>", stock.FName);
                //}

                print.AppendFormat("<tr height='22'><td></td><td colspan=2>{0}</td></tr>", stock.FAddress);
                print.Append("<tr height='20'><td></td><td></td><td></td></tr>");
                //if (stock.FName.Length >= 15)
                //{
                //}else
                //{
                    print.Append("<tr height='20'><td></td><td></td><td></td></tr>");
                //}
                //print.Append("<tr height='22'><td></td><td></td><td></td></tr>");
                print.AppendFormat("<tr height='20'><td></td><td>{0}</td><td align='left'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{1}</td></tr>", stock.FCity ,stock.FZip);
            }

            stock.FDate=DateTime.Now;

            CustomerLinkService.SaveChanges();

            _response = print.ToString();

        }

        /// <summary>
        ///     运管监管行车记录日志
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintLogDoc(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = DispatchService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                #region 表头

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>危险货物道路运输车辆调度日志</title></head>
<body>
    <table width='1634' height='331' border='1'>
        <tr>
            <td width='88'>表22：</td>
            <td colspan='18' align='center'><strong>危险货物道路运输车辆调度日志</strong></td>
        </tr>
        <tr>
            <td height='26' align='center'>单位</td>
            <td colspan='3' align='center'>{0}</td>
            <td width='45' align='center'>日期</td>
            <td colspan='9' align='center'>{1}</td>
            <td width='53' align='center'>调度</td>
            <td colspan='4' align='center'>{2}</td>
        </tr>
        <tr>
            <td rowspan='4' align='center'>车辆技术状况</td>
            <td height='26' colspan='3' align='center'>完好车数</td>
            <td colspan='3' align='center'>非完好车数</td>
            <td width='78' rowspan='4' align='center'>车辆运行状况</td>
            <td colspan='2' align='center'>市内运行</td>
            <td colspan='2' align='center'>外省市运行</td>
            <td width='85' rowspan='4' align='center'>运输量统计</td>
            <td width='75' rowspan='2' align='center'>重车行程</td>
            <td rowspan='2' align='center'>{16}</td>
            <td width='99' rowspan='2' align='center'>&nbsp;</td>
            <td width='152' rowspan='2' align='center'>货物运量（吨）</td>
            <td width='137' rowspan='2' align='center'>{17}</td>
            <td width='70' rowspan='4' align='center'>&nbsp;</td>
        </tr>
        <tr>
            <td width='102' rowspan='3' align='center'>其中</td>
            <td width='90' align='center'>工作（辆）</td>
            <td width='40' align='center'>{3}</td>
            <td rowspan='3' align='center'>其中</td>
            <td width='99' align='center'>修理（辆）</td>
            <td width='67' align='center'>{4}</td>
            <td width='58' align='center'>辆次</td>
            <td width='44' align='center'>{5}</td>
            <td width='67' align='center'>辆次</td>
            <td width='67' align='center'>{6}</td>
        </tr>
        <tr>
            <td align='center'>停驶（辆）</td>
            <td align='center'>{7}</td>
            <td align='center'>事故（辆）</td>
            <td align='center'>{8}</td>
            <td align='center'>里程</td>
            <td align='center'>{9}</td>
            <td align='center'>里程</td>
            <td align='center'>{10}</td>
            <td rowspan='2' align='center'>总行程</td>
            <td rowspan='2' align='center'>{11}</td>
            <td rowspan='2' align='center'>&nbsp;</td>
            <td rowspan='2' align='center'>货运周转量（吨公里）</td>
            <td rowspan='2' align='center'>{12}</td>
        </tr>
        <tr>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>其他（辆）</td>
            <td align='center'>{13}</td>
            <td align='center'>货运量</td>
            <td align='center'>{14}</td>
            <td align='center'>货运量</td>
            <td align='center'>{15}</td>
        </tr><tr>
            <td rowspan='11' align='center'>单车  实际 调度 记录</td>
            <td height='33' align='center'>序号</td>
            <td align='center'>车号</td>
            <td align='center'>车型</td>
            <td align='center'>吨位</td>
            <td align='center'>营运证号</td>
            <td align='center'>危险类别</td>
            <td align='center'>货物名     称</td>
            <td align='center'>包装</td>
            <td align='center'>件        数</td>
            <td align='center'>实载吨位</td>
            <td align='center'>运行起       点</td>
            <td align='center'>运行止点</td>
            <td align='center'>里程</td>
            <td align='center'>驾驶员</td>
            <td align='center'>押运员</td>
            <td align='center'>出车车次</td>
            <td align='center'>托运单位托运人）</td>
            <td rowspan='11' align='center'>
                <table cellspacing='0' cellpadding='0'>
                    <col width='64' />
                    <tr>
                        <td width='64' rowspan='12' valign='top'>工作    记事</td>
                    </tr>
                    <tr> </tr>
                    <tr> </tr>
                    <tr> </tr>
                    <tr> </tr>
                    <tr> </tr>
                    <tr> </tr>
                    <tr> </tr>
                    <tr> </tr>
                    <tr> </tr>
                    <tr> </tr>
                    <tr> </tr>
                </table>
            </td>
        </tr>",
        stock.FLogisticsName,//0
        Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"),//1
        stock.FDispatcher,//2
        stock.FWorkQty,//3
        stock.FRepairQty,//4修理数量
        stock.FIDepartureTime,//5
        stock.FODepartureTime,//6//省外量次
        stock.FStopQty,//7
        stock.FAccidentQty,//8事故
        stock.FIRange,//9省内量程
        stock.FORange,//10
        stock.FSumQty,//11总量程
        stock.FTurnover,//12货运周转量
        stock.FOtherQty,//13其他非完好
        stock.FIQty,//14货运量
        stock.FOQty,//15货运量
        stock.FHeavyTruckQty,//重车行程
        stock.FTransport//货物运量

         );
                #endregion

                #region 表体
                IQueryable<LHDispatchDetails> list = DispatchDetailsService.Where(p => p.KeyId == keyid);

                int i = 1;

                foreach (var item in list)
                {
                    print.AppendFormat(@"<tr>
            <td align='center'>{0}</td>
            <td align='center'>{1}</td>
            <td align='center'>{2}</td>
            <td align='center'>{3}</td>
            <td align='center'>{4}</td>
            <td align='center'>{5}</td>
            <td align='center'>{6}</td>
            <td align='center'>{7}</td>
            <td align='center'>{8}</td>
            <td align='center'>{9}</td>
            <td align='center'>{11}</td>
            <td align='center'>{12}</td>
            <td align='center'>{13}</td>
            <td align='center'>{14}</td>
            <td align='center'>{15}</td>
            <td align='center'>{16}</td>
            <td align='center'>{17}</td>
        </tr>",
        i++,//序号0
        item.FVehicleNum,//车辆1
        item.FVehicleType,//车型2
        item.FTonnage,//吨位3
        item.FOperationCertificateNo,//证号4
        item.FRiskType,//5
        item.FItem,//6
        "",//包7
        item.FNumber,//8
        item.FActual,//9
        item.FFrom,//10
        item.FTo,//11
        item.FMileage,//12
        item.FDriver,//13
        item.FSupercargo,//14
        item.FTimes,//15
        item.FLogistics,//16
        ""
);
            }

                if (i < 10)
                {
                    int m = 10 - i;
                    for (int o = 0; o <= m; o++)
                    {
                        print.AppendFormat(@"<tr>
            <td align='center'>{0}</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
        </tr>",i++);
                    }
                }


                #endregion

                #region 表尾

                print.AppendFormat(@"<tr>
            <td align='center'>&nbsp;</td>
            <td colspan='3' align='center'>合计</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>{0}</td>
            <td align='center'>{1}</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>{2}</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
            <td align='center'>&nbsp;</td>
        </tr></table>
</body >
</html > ",list.Sum(p=>p.FMileage),
list.Sum(p => p.FMileage),
list.Sum(p => p.FMileage)

);

                #endregion

                //    details.Compute("SUM(FQty)", "true"),
                //    details.Compute("SUM(FBottleQty)", "true")
            }

            _response = print.ToString();
        }

        /// <summary>
        ///     货车运输记录单
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintDispatch(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = DispathCenterService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {

                var logistic = new ProjectItemsService().FirstOrDefault(p => p.FSParent == "1028" && p.FValue == stock.FLogisticsCode);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:560px;height:440px;border:0px solid red' >
<center><b><font face='隶书'>货车运输记录单             ID： {8}</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='550'>
<tr height='22'><td align='left'>承运方:</td><td colspan=3>{0}</td><td align='left'>运输车辆:</td><td>{1}</td></tr>
<tr height='22'><td align='left'>日期:</td><td>{2}</td><td align='left'>驾驶员:</td><td>{3}</td><td align='left'></td><td>{4}</td></tr>
<tr height='22'><td align='left'>任务数:</td><td>{5}</td><td align='left'>押运员:</td><td>{6}</td><td align='left'>随车装卸:</td><td>{7}</td></tr>
</table>", //

        //物流公司全称
        logistic != null ? logistic.FEx03 : "上海坦申物流有限公司",
        stock.FVehicleNum,//
        Convert.ToDateTime(stock.FBeginDate).ToString("yyyy-MM-dd"),
        stock.FDriver,//
        "",//
        "",//任务数
        stock.FSupercargo,//
        stock.FSupercargo,
        keyid
         );

                print.Append(@"<table  border='1' cellspacing='0' width='550'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='50'>码表值</td>
            <td width='120'>客户名称/单号</td>
            <td width='120'>配送要求</td>
            <td width='80'>备注</td>
        </tr>");


                print.AppendFormat(@"<tr align='center'>
                                        <td height='21px'></td>
                                        <td align='left'>出发时间：{0}</td>
                                        <td align='left'></td>
                                        <td></td>
                                    </tr>",//


stock.FBeginDate + " " + stock.FBeginTime
                    );

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@FCompanyId", Company.id);
                parms.Add("@KeyId", keyid);
                var details = SqlService.ExecuteProcedureCommand("rpt_DispathLog", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td height='21px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td align='left'>{2}</td>
                                        <td>{3}</td>
                                    </tr>",//
                                          i + 1,//
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FAddress"],
                                          ""
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();
                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td height='21px'></td>
                                        <td align='left'>结束时间：</td>
                                        <td align='left'></td>
                                        <td></td>
                                    </tr>" //

                    );

                print.Append(bottomDetails);

            }

            _response = print.ToString();
        }

        private void AjaxPrintLeaseB(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = LeaseService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:400px;border:0px solid red' >
<h1 style='text-align:center;'><font face='隶书' size='6'>{0}押金收款单</font></h1>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='720'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{1}</td><td width='64' align='right'>单号</td>
<td width='120'>{2}</td></tr><tr height='22'><td>联系人</td><td colspan='3' style='width: 224px'>{3}</td><td align='right'>日期</td>
<td>{4}</td></tr></table>",//
                                 Company.com_name,//
                                 stock.FName,//
                                 stock.KeyId,//
                                 stock.FLinkman,//
                                 Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));

                print.Append(@"<table  border='1' cellspacing='0' width='720'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='40'  height='30px'>序号</td>
            <td width='100'>钢瓶名称</td>
            <td width='80' >规格</td>
            <td width='60' >单位</td>
            <td width='60' >数量</td>
            <td width='60' >押金单价</td>
            <td width='60' >金额</td>
            <td width='80' >日租金/瓶</td>
            <td width='90' >开始时间</td>
        </tr> ");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintLease", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>

                                    </tr>", i + 1,//序号
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FBottleQty"],//数量
                                          details.Rows[i]["FPrice"],
                                          details.Rows[i]["FAmount"],
                                          details.Rows[i]["FRentDay"],
                                          Convert.ToDateTime(details.Rows[i]["FDate"]).ToString("yyyy-MM-dd")
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr>
            <td colspan='9'  height='30px'><div align='left'>合计：{0}大写金额：{1}</div></td>
        </tr>
<tr><td colspan='9' height='30px'><div align='center'>收款人员签名（盖章）</div></td></tr>
    </table>", details.Compute("sum(FAmount)", "true"),//
             RmbHelper.Convert(Convert.ToDecimal(details.Compute("sum(FAmount)", "true"))));

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='720' cellspacing='0' border='0'>
        
        <tr>
            <td height='21' colspan='9'>1、凡顾客将租用空瓶交回我公司，应无损坏，瓶租依章付清，押金如数退回。</td>
        </tr>
        <tr>
            <td height='21' colspan='9'>2、凡顾客退瓶后，未按章付清瓶租者，由我公司在此押金内抵扣。</td>
        </tr>
        <tr>
            <td height='21' colspan='9'>3、本单无加盖我公司财务专用章无效，凡客户需退此押金时，需携回红色押金联，无此单我公司一律拒绝退款。</td>
        </tr>
        <tr>
            <td height='21' colspan='9'>4、三年内不退瓶视同售出，需要继续使用的更换押金单。</td>
        </tr>
        <tr>
            <td height='21' colspan='9' style='font-size:12px;'>第一联为白色：销方财务;第二联为红色：销方仓库；第三联为蓝色：客户留存；第四联为绿色：客户留存；第五联为黄色：销方门卫留存</td>            
        </tr>
<tr>
            <td width='60' height='30px'>租瓶人：</td>
            <td width='68'></td>
            <td width='60'></td>
            <td width='68'></td>
            <td width='60'></td>
            <td width='68'>还瓶人：</td>
            <td width='60'></td>
            <td width='60'></td>
            <td width='80'></td>
            <td width='40'></td>
        </tr>
    </table>
</div>
</body>
</html>");

                print.Append(bottom);
            }

            _response = print.ToString();
        }

        private void AjaxPrintBlank(HttpContext context)
        {
            //var keyid = context.Request["keyid"];
            //var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            //if (stock != null)
            //{
            //    var client = CustomerService.FirstOrDefault(p => p.FCode == stock.FCode && p.FCompanyId == Company.id);

            print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>上海浦江特种气体有限公司发货单</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='780'>
<tr height='22'><td width='100' >客户代码：</td><td colspan='3' style='width: 120px'>{0}</td><td width='260' align='Left'>发货单号：</td>
<td>{1}</td></tr>
<tr height='22'><td width='100' >客户名称：</td><td colspan='3' style='width: 120px'>{2}</td><td width='260' align='Left'>日期：</td>
<td>{3}</td></tr>
</table>", "", "", "", "");
            print.Append(@"<table  border='1' cellspacing='0' width='750'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='60' height='30px'>订单编号</td>
            <td width='60'>商品名称</td>
            <td width='50'>规格</td>
            <td width='50'>单位</td>
			<td width='60'>实送数量</td>
            <td width='60'>实送重量</td>
			<td width='50'>单价</td>
			<td width='50'>金额</td>
			<td width='60'>回收空瓶</td>
            <td width='70'>备注</td>
        </tr>");//<td width='60'>欠瓶数量</td>

            //var parms = new Dictionary<string, object>();
            //parms.Clear();

            //parms.Add("@companyId", Company.id);
            //parms.Add("@KeyId", keyid);
            //parms.Add("@begin", MonthFirstDay(ServiceDateTime));
            //parms.Add("@end", MonthEnd(ServiceDateTime));

            //var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

            var salesDetails = new StringBuilder();


            for (int i = 0; i < 5; i++)
            {
                salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
                                        <td>{9}</td>
										</tr>", //
                                      "",//details.Rows[i]["FNum"],//订单号码//i + 1,//<td>{10}</td>
                                      "",//details.Rows[i]["FName"],
                                      "",//details.Rows[i]["FSpec"],
                                      "",//details.Rows[i]["FUnit"],
                                         //Convert.ToDecimal(details.Rows[i]["FQty1"]).ToString("#.##").Equals("0.00") ? "" : //
                                         //Convert.ToDecimal(details.Rows[i]["FQty1"].ToString()//
                                         //.Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty1"]) ://
                                         //details.Rows[i]["FQty1"],

                                      "",

                                      //(Convert.ToDecimal(details.Rows[i]["FQty2"]).ToString("#.##").Equals("0.00") || Convert.ToInt32(details.Rows[i]["FQty2"]).Equals(0)) ? "" : //
                                      //Convert.ToDecimal(details.Rows[i]["FQty2"].ToString()//
                                      //.Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty2"]) ://
                                      //details.Rows[i]["FQty2"],

                                      "",//
                                         //details.Rows[i]["FQty2"].ToString().Equals("0") ? "" : details.Rows[i]["FQty2"],
                                         //"",//欠瓶数
                                         //(client.FPushFlag == null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FPrice"],//,//单价//
                                         //(client.FPushFlag == null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FAmount"],
                                      "",
                                      "",//回收空瓶//Convert.ToDecimal(details.Rows[i]["AddUpBottleQty"]) == 0 ? "" : details.Rows[i]["AddUpBottleQty"],
                                      "", ""//details.Rows[i]["FMemo"]
                                      );
            }

            int printCount = 7;//- details.Rows.Count;

            for (int i = 0; i < printCount; i++)
            {
                salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
            }

            print.Append(salesDetails);

            var bottomDetails = new StringBuilder();

            bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>合计</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
										<td></td>
                                        <td></td></tr></table>", //
                                                           "",//details.Compute("SUM(FQty1)", "true"),//
                                                           "",//details.Compute("SUM(FQty2)", "true"),//
                                                           "",//
                                                           "",//
                                                           "");

            print.Append(bottomDetails);

            var bottom =
                string.Format(@"<br/><table style='font-size:13px;' align='center' width='780' cellspacing='0' border='0'>
        <tr>
            <td height='25' width='780'>司机：{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;押运员：{1} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;车牌号：{2} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;送货人：{3}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;收货人：</td>            
        </tr>
        <tr>
            <td height='21' style='font-size:12px;'>第一联为白色：销方财务;第二联为红色：销方仓库；第三联为蓝色：客户留存；第四联为绿色：客户留存；第五联为黄色：销方门卫留存</td>            
        </tr>
    </table>
</div>
</body>
</html>", "", "", "", "");////stock.FDriver, stock.FSupercargo, stock.FVehicleNum, "");

            print.Append(bottom);
            //}

            _response = print.ToString();
        }

        private void AjaxPrintSwap(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                var client = CustomerService.FirstOrDefault(p => p.FCode == stock.FCode && p.FCompanyId == Company.id);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>上海浦江特种气体有限公司换货单</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='780'>
<tr height='22'><td width='100' >客户代码：</td><td colspan='3' style='width: 220px'>{0}</td><td width='100' align='right'>发货单号：</td>
<td>{1}</td></tr>
<tr height='22'><td width='100' >客户名称：</td><td colspan='3' style='width: 220px'>{2}</td><td width='100' align='right'>日期：</td>
<td>{3}</td></tr>
</table>", stock.FCode, stock.KeyId, stock.FName, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));
                print.Append(@"<table  border='1' cellspacing='0' width='750'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='60' height='30px'>订单编号</td>
            <td width='60'>商品名称</td>
            <td width='50'>规格</td>
            <td width='50'>单位</td>
			<td width='60'>实送数量</td>
            <td width='60'>实送重量</td>
			<td width='50'>单价</td>
			<td width='50'>金额</td>
			<td width='60'>回收空瓶</td>
            <td width='70'>备注</td>
        </tr>");//<td width='60'>欠瓶数量</td>

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

                var salesDetails = new StringBuilder();


                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
                                        <td>{9}</td>
										</tr>", //
                                          details.Rows[i]["FNum"],//订单号码//i + 1,//<td>{10}</td>
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          Convert.ToDecimal(details.Rows[i]["FQty1"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty1"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty1"]) ://
                                          details.Rows[i]["FQty1"],

                                          (Convert.ToDecimal(details.Rows[i]["FQty2"]).ToString("#.##").Equals("0.00") || Convert.ToInt32(details.Rows[i]["FQty2"]).Equals(0)) ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty2"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty2"]) ://
                                          details.Rows[i]["FQty2"],
                                          //details.Rows[i]["FQty2"].ToString().Equals("0") ? "" : details.Rows[i]["FQty2"],
                                          //"",//欠瓶数
                                          (client.FPushFlag == null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FPrice"],//,//单价//
                                          (client.FPushFlag == null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FAmount"],
                                          "",//回收空瓶//Convert.ToDecimal(details.Rows[i]["AddUpBottleQty"]) == 0 ? "" : details.Rows[i]["AddUpBottleQty"],
                                          details.Rows[i]["FMemo"]
                                          );
                }

                int printCount = 7 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td   height='30px'>合计</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
										<td></td>
                                        <td></td></tr></table>", //
                                                               details.Compute("SUM(FQty1)", "true"),//
                                                               "",//details.Compute("SUM(FQty2)", "true"),//
                                                               "",//
                                                               "",//
                                                               "");

                print.Append(bottomDetails);

                var bottom = string.Format(@"<br/><table style='font-size:13px;' align='center' width='780' cellspacing='0' border='0'>
        <tr>
            <td height='25' width='780'>司机：{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;押运员：{1} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;车牌号：{2} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;送货人：{3}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;收货人：</td>            
        </tr>
        <tr>
            <td height='21' style='font-size:12px;'>第一联为白色：销方财务;第二联为红色：销方仓库；第三联为蓝色：客户留存；第四联为绿色：客户留存；第五联为黄色：销方门卫留存</td>            
        </tr>
    </table>
</div>
</body>
</html>", stock.FDriver, stock.FSupercargo, stock.FVehicleNum, "");

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        #region 调度
        private void AjaxPrintVehicle(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = DispathCenterService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {

                var logistic = new ProjectItemsService().FirstOrDefault(p => p.FSParent == "1028" && p.FValue == stock.FLogisticsCode);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:560px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>货车出门单
</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='550'>
<tr height='22'><td align='left'>承运方:</td><td colspan=3>{0}</td><td align='left'>码表初值:</td><td>{1}</td></tr>
<tr height='22'><td align='left'>出发站:</td><td>{2}</td><td align='left'>车辆号码:</td><td>{3}</td><td align='left'>驾驶员:</td><td>{4}</td></tr>
<tr height='22'><td align='left'>出发日期:</td><td>{5}</td><td align='left'>出发时间:</td><td>{6}</td><td align='left'>押运员:</td><td>{7}</td></tr>
</table>", //

        //物流公司全称
        logistic != null ? logistic.FEx03 : "上海坦申物流有限公司",
        "",//码表初值
        stock.FFrom,//
        stock.FVehicleNum,//
        stock.FDriver,//
        Convert.ToDateTime(stock.FBeginDate).ToString("yyyy-MM-dd"),
        stock.FBeginTime,//
        stock.FSupercargo
         );

                print.Append(@"<table  border='1' cellspacing='0' width='550'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='100'>货物名称</td>
            <td width='50'>规格</td>
            <td width='50'>计量单位</td>
            <td width='50'>商品数量</td>
            <td width='50'>容器数量</td>
            <td width='80'>备注</td>
        </tr>");

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@FCompanyId", Company.id);
                parms.Add("@KeyId", keyid);
                var details = SqlService.ExecuteProcedureCommand("rpt_DispathList", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td height='21px' align='left'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td></td>
                                    </tr>",//
                                          details.Rows[i]["FItemName"].ToString().Length < 40 ? details.Rows[i]["FItemName"] : details.Rows[i]["FItemName"].ToString().Insert((details.Rows[i]["FItemName"].ToString().Length/2)-1, "\n"),
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],

                                          Convert.ToDecimal(details.Rows[i]["FQty"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty"]) ://
                                          details.Rows[i]["FQty"],

                                          details.Rows[i]["FBottleQty"]//

                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();
                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td height='21px'>合计</td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td></td>
                                        </tr>", //
                    details.Compute("SUM(FQty)", "true"),
                    details.Compute("SUM(FBottleQty)", "true")
                    );
                //);

                print.Append(bottomDetails);

                var list = new StringBuilder();
                list.Append(@"<br/><table style='font-size:13px;' align='center' width='550' cellspacing='0' border='0'>");

                var units = SqlService.ExecuteProcedureCommand("rpt_DispathList", parms).Tables[1];
                for (int i = 0; i < units.Rows.Count; i++)
                {
                    list.AppendFormat(@"<tr align='center'>
                                            <td width='80' align='left'>{0}</td>
                                            <td width='200' align='left'>{1}</td>
                                            <td width='30'>{2}</td>
                                            <td width='60' align='left'>{3}</td>
                                        </tr>",//
                                          units.Rows[i]["FCode"],//
                                          units.Rows[i]["FName"],//
                                          "",
                                          units.Rows[i]["FBottleQty"]//
                                          );
                }

                print.Append(list);

                print.Append("<tr><td colspan=4 height='21px'></td></tr>");
                print.Append("<tr><td colspan=4>注：本单据当班有效。</td></tr>");
                print.AppendFormat("<tr><td colspan=4>车辆号牌:{0}&nbsp;&nbsp;&nbsp;&nbsp;送货人签字：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;当班门卫签字</td></tr>", stock.FVehicleNum);
                print.AppendFormat("<tr><td colspan=4>No:{0}&nbsp;&nbsp;&nbsp;&nbsp;制单:{1}&nbsp;&nbsp;&nbsp;&nbsp; 上海浦江特种气体有限公司</td></tr>", stock.KeyId, stock.CreateBy);

                print.Append("</table>");
            }

            _response = print.ToString();

        }

        /// <summary>
        ///     排管车出门证
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintTubeVehicle(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = DispathCenterService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {

                var logistic = new ProjectItemsService().FirstOrDefault(p => p.FSParent == "1028" && p.FValue == stock.FLogisticsCode);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:560px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>货车出门单
</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='550'>
<tr height='22'><td align='left'>承运方:</td><td colspan=3>{0}</td><td align='left'>码表初值:</td><td>{1}</td></tr>
<tr height='22'><td align='left'>出发站:</td><td>{2}</td><td align='left'>车辆号码:</td><td>{3}</td><td align='left'>驾驶员:</td><td>{4}</td></tr>
<tr height='22'><td align='left'>出发日期:</td><td>{5}</td><td align='left'>出发时间:</td><td>{6}</td><td align='left'>押运员:</td><td>{7}</td></tr>
</table>", //

        //物流公司全称
        logistic != null ? logistic.FEx03 : "上海坦申物流有限公司",
        "",//码表初值
        stock.FFrom,//
        stock.FVehicleNum,//
        stock.FDriver,//
        Convert.ToDateTime(stock.FBeginDate).ToString("yyyy-MM-dd"),
        stock.FBeginTime,//
        stock.FSupercargo
         );

                print.Append(@"<table  border='1' cellspacing='0' width='550'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='100'>货物名称</td>
            <td width='50'>规格</td>
            <td width='50'>计量单位</td>
            <td width='50'>交付温度（°C）</td>
            <td width='50'>交付压力（MPa）</td>
            <td width='80'>备注</td>
        </tr>");

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@FCompanyId", Company.id);
                parms.Add("@KeyId", keyid);
                var details = SqlService.ExecuteProcedureCommand("rpt_DispathList2", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td height='21px' align='left'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                    </tr>",//
                                          details.Rows[i]["FItemName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],

                                          Convert.ToDecimal(details.Rows[i]["FPayTemperature"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FPayTemperature"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FPayTemperature"]) ://
                                          details.Rows[i]["FPayTemperature"],

                                          details.Rows[i]["FPayPressure"],

                                          details.Rows[i]["FMemo"]

                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();
                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td height='21px'>合计</td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td></td>
                                        </tr>", //
                    details.Compute("SUM(FPayTemperature)", "true"),
                    details.Compute("SUM(FPayPressure)", "true")
                    );
                //);

                print.Append(bottomDetails);

                var list = new StringBuilder();
                list.Append(@"<br/><table style='font-size:13px;' align='center' width='550' cellspacing='0' border='0'>");

                var units = SqlService.ExecuteProcedureCommand("rpt_DispathList2", parms).Tables[1];
                for (int i = 0; i < units.Rows.Count; i++)
                {
                    list.AppendFormat(@"<tr align='center'>
                                            <td width='80' align='left'>{0}</td>
                                            <td width='200' align='left'>{1}</td>
                                            <td width='30'>{2}</td>
                                            <td width='60' align='left'>{3}</td>
                                        </tr>",//
                                          units.Rows[i]["FCode"],//
                                          units.Rows[i]["FName"],//
                                          "",
                                          ""//
                                          );
                }

                print.Append(list);

                print.Append("<tr><td colspan=4 height='21px'></td></tr>");
                print.Append("<tr><td colspan=4>注：本单据当班有效。</td></tr>");
                print.AppendFormat("<tr><td colspan=4>车辆号牌:{0}&nbsp;&nbsp;&nbsp;&nbsp;送货人签字：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;当班门卫签字</td></tr>", stock.FVehicleNum);
                print.AppendFormat("<tr><td colspan=4>No:{0}&nbsp;&nbsp;&nbsp;&nbsp;制单:{1}&nbsp;&nbsp;&nbsp;&nbsp; 上海浦江特种气体有限公司</td></tr>", stock.KeyId, stock.CreateBy);

                print.Append("</table>");
            }

            _response = print.ToString();

        }

        private void AjaxPrintVehiclePass(HttpContext context)
        {

            var keyid = context.Request["keyid"];
            var stock = DispathCenterService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>上海浦江特种气体有限公司&nbsp;&nbsp;放行条</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td align='left'>调度单号:</td><td>{0}</td><td></td><td></td><td align='left'>日期:</td>
<td>{1}</td></tr>
<tr height='22'><td align='left'>车牌号:</td><td >{2}</td><td align='left'>司机:</td><td>{3}</td><td align='left'>押运员:</td><td>{4}</td></tr>
</table>", stock.KeyId, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FVehicleNum, stock.FDriver, stock.FSupercargo);//, Company.com_name
                print.Append(@"<table  border='1' cellspacing='0' width='710'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='46'>序号</td>
            <td width='80'>客户名称</td>
            <td width='64'>商品名称</td>
            <td width='37'>规格</td>
            <td width='37'>数量</td>
<td width='37'>空瓶</td>
            <td width='100'>备注</td>
        </tr>");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                var details = SqlService.ExecuteProcedureCommand("rpt_DispathCenterList", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td  align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
<td></td>
                                        <td>{5}</td>                                        
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FItemName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FBottleQty"],
                                          details.Rows[i]["FMemo"]
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();
                bottomDetails.AppendFormat(@"<tr><td  height='21></td>  <td></td><td></td><td></td><td></td><td></td><td></td> </tr>");
                bottomDetails.AppendFormat(@"<tr><td  height='21></td>  <td></td><td></td><td></td><td></td><td></td><td></td> </tr>");

                bottomDetails.AppendFormat(@"<tr><td  height='21></td>  <td></td><td></td><td></td><td></td><td></td><td></td> </tr>");
                bottomDetails.AppendFormat(@"<tr><td  height='21></td>  <td></td><td></td><td></td><td></td><td></td><td></td> </tr>");
                bottomDetails.AppendFormat(@"<tr><td  height='21></td>  <td></td><td></td><td></td><td></td><td></td><td></td> </tr>");
                bottomDetails.AppendFormat(@"<tr><td  height='21></td>  <td></td><td></td><td></td><td></td><td></td><td></td> </tr>");

                bottomDetails.AppendFormat(@"</table><p/>");

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='714' cellspacing='0' border='0'> 
        <tr>
            <td height='21'>保安员</td><td >仓管员</td><td >营业科</td>        
        </tr>
    </table>
</div>
</body>
</html>");
                print.Append(bottom);
            }

            _response = print.ToString();

        }

        #endregion

        private void AjaxPrintStockOut(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                //var client = CustomerService.FirstOrDefault(p => p.FCode == stock.FCode && p.FCompanyId == Company.id);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>上海浦江特种气体有限公司出仓单</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='780'>
<tr height='22'><td width='100' >客户代码：</td><td colspan='3' style='width: 220px'>{0}</td><td width='100' align='right'>单号：</td>
<td>{1}</td></tr>
<tr height='22'><td width='100' >客户名称：</td><td colspan='3' style='width: 220px'>{2}</td><td width='100' align='right'>日期：</td>
<td>{3}</td></tr>
</table>", stock.FCode, stock.KeyId, stock.FName, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));
                print.Append(@"<table  border='1' cellspacing='0' width='750'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='60' height='30px'>订单编号</td>
            <td width='60'>商品名称</td>
            <td width='50'>规格</td>
            <td width='50'>单位</td>
			<td width='60'>实送数量</td>
            <td width='60'>实送重量</td>
			<td width='50'>单价</td>
			<td width='50'>金额</td>
			<td width='60'>回收空瓶</td>
            <td width='70'>备注</td>
        </tr>");//<td width='60'>欠瓶数量</td>

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

                var salesDetails = new StringBuilder();


                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
                                        <td>{9}</td>
										</tr>", //
                                          details.Rows[i]["FNum"],//订单号码//i + 1,//<td>{10}</td>
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          Convert.ToDecimal(details.Rows[i]["FQty1"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty1"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty1"]) ://
                                          details.Rows[i]["FQty1"],

                                          (Convert.ToDecimal(details.Rows[i]["FQty2"]).ToString("#.##").Equals("0.00") || Convert.ToInt32(details.Rows[i]["FQty2"]).Equals(0)) ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty2"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty2"]) ://
                                          details.Rows[i]["FQty2"],
                                          //details.Rows[i]["FQty2"].ToString().Equals("0") ? "" : details.Rows[i]["FQty2"],
                                          //"",//欠瓶数
                                          "",//(client.FPushFlag == null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FPrice"],//,//单价//
                                          "",//(client.FPushFlag == null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FAmount"],
                                          "",//回收空瓶//Convert.ToDecimal(details.Rows[i]["AddUpBottleQty"]) == 0 ? "" : details.Rows[i]["AddUpBottleQty"],
                                          details.Rows[i]["FMemo"]
                                          );
                }

                int printCount = 7 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td   height='30px'>合计</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
										<td></td>
                                        <td></td></tr></table>", //
                                                               details.Compute("SUM(FQty1)", "true"),//
                                                               "",//details.Compute("SUM(FQty2)", "true"),//
                                                               "",//
                                                               "",//
                                                               "");

                print.Append(bottomDetails);

                var bottom = string.Format(@"<br/><table style='font-size:13px;' align='center' width='780' cellspacing='0' border='0'>
        <tr>
            <td height='25' width='780'>领料人：{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{1} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{2} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{3}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;发货人：</td>            
        </tr>
        <tr>
            <td height='21' style='font-size:12px;'>第一联为白色：仓库;第二联为红色：领料人；第三联为蓝色：财务留存；</td>            
        </tr>
    </table>
</div>
</body>
</html>", "", "", "", "");

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        #region 销售
        private void AjaxPrintSales(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                var client = CustomerService.FirstOrDefault(p => p.FCode == stock.FCode && p.FCompanyId == Company.id);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'>
<head>
    <title>打印</title>
</head>
<body>
    <div style='margin:2px auto;width:750px;height:220px;border:0px solid red'>
        <table style='font-size:13px;' border='0' cellspacing='0' align='center' width='750'>
        	<tr height='22'>
                <td width='270'>配送点 {0} 电话 {1}</td>
                <td colspan='3'><center><font face='隶书' size='6'>销售出库-入库单&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></center></td>
                <td width = '60' ><div align = 'right'>
                  白：账务
                  红：销销
                </div></td>
            </tr>
            <tr>
            <td> 交货日期：{2}</td>
                <td> 单据号：{5}</ td >
                    <td colspan = '2'> 业务类型 普通销售 </td>
                         <td><div align = 'right' > 蓝：仓库 </div ></td>
                                </tr>
                                <tr>
                                <td> 客户名称：{3}</td>
                                <td colspan = '2' > 交货方式 {4}
                付款方式 转账</td>
<td></td>
<td> <div align = 'right' > 蓝：客户 </div ></td>
</tr>
</table> ", stock.FDistributionPoint,//配送点
"021-67121825", //电话
Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"),//
stock.FName + string.Format("({0})", stock.FOrgName).Replace("()", ""),//客户名称
stock.FDeliveryMethod,//交货方式
stock.KeyId//
);
                print.Append(@"<table  border='1' cellspacing='0' width='750'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='42' height='30px'>商品代码</td>
            <td width='60'>商品名称</td>
            <td width='40'>规格</td>
            <td width='22'>计量单位</td>
			<td width='42'>商品发出数</td>
            <td width='42'>容器发出数</td>
			<td width='42'>商品收入数</td>
			<td width='42'>容器收入数</td>
			<td width='42'>无气瓶数</td>
            <td width='40'>备注</td>
        </tr>");//

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{9}</td>
                                        <td>{8}</td>
										</tr>", //
                                          details.Rows[i]["FINum"],//产品代码
                                          details.Rows[i]["FName"].ToString().Replace("()", ""),
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],

                                          //商器发出数
                                          Convert.ToDecimal(details.Rows[i]["FQty"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty"]) ://
                                          details.Rows[i]["FQty"],//

                                          //容器发出数
                                          details.Rows[i]["FBottleQty"],//容器发出数

                                          //商品收入数
                                          Convert.ToDecimal(details.Rows[i]["FReturnQty"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FReturnQty"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FReturnQty"]) ://
                                          details.Rows[i]["FReturnQty"],

                                          //容器收入数
                                          Convert.ToInt32(details.Rows[i]["FRecycleQty"]).ToString().Equals("0") ? "" : //
                                          details.Rows[i]["FRecycleQty"],

                                          details.Rows[i]["FMemo"], details.Rows[i]["FEmptyBottle"]
                                          );
                }

                int printCount = 5 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center' style='border:0px solid'>
                                        <td  height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails); 

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'  style='border:0px solid'>
                                        <td height='30px'>合计数量</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
										<td></td></tr></table>", //
                                                               details.Compute("SUM(FQty)", "true"),//
                                                               details.Compute("SUM(FBottleQty)", "true"),//
                                                               details.Compute("SUM(FReturnQty)", "true"),//
                                                               details.Compute("SUM(FRecycleQty)", "true"),//
                                                               details.Compute("SUM(FEmptyBottle)", "true")//
                                                               );

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='750' cellspacing='0' border='0'>
            <tr><td colspan='10'>送货地址：{5} {6} {7} {8}</td></tr>
            <tr>
              <td width='60' height='25'>业务员</td>
              <td width='60'>{0}</td>
              <td width='60'>出库人</td>
              <td width='60'>{2}</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>入库人</td>
              <td width='60'></td>
            </tr>
            <tr>
              <td>制单人</td>
              <td>{1}</td>
              <td>出库时间</td>
              <td>{3}</td>
              <td>送货人</td>
              <td>&nbsp;</td>
              <td>客户签收</td>
              <td>&nbsp;</td>
              <td>入库时间</td>
                <td height='25'></td>
            </tr>
            <tr>
              <td colspan='7' style='font-size:12px;'>*注：商品发出与收入数之差用于费用结算，容器项用于记录包装物流转。两者分别计数</td>
              <td height = '21' style = 'font-size:16px;' colspan = '3' > 上海浦江特种气体有限公司 </td >
            </tr>
            <tr>
              <td colspan = '7' style = 'font-size:12px;' > 打印时间：{4}（单据共1页，第1页）</td >
              <td height = '21' style = 'font-size:12px;' colspan = '3' > 地址：上海市化学工业区才华路10号</td>
            </tr>
        </table>
</div>
</body>
</html> ", stock.FSalesman, stock.CreateBy, stock.FShipper, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
stock.FAddress, stock.FLinkman, stock.FPhone, stock.FMemo
);

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        /// <summary>
        ///     调拨申请单
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintAllotPlan(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = PassCardService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                var client = CustomerService.FirstOrDefault(p => p.FCode == stock.FCode && p.FCompanyId == Company.id);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'>
<head>
    <title>打印</title>
</head>
<body>
    <div style='margin:2px auto;width:750px;height:440px;border:0px solid red'>
        <table style='font-size:13px;' border='0' cellspacing='0' align='center' width='750'>
        	<tr height='22'>
                <td width='270'>配送点 {0} 电话 {1}</td>
                <td colspan='3'><center><font face='隶书' size='6'>调拨申请单&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></center></td>
                <td width = '60' ><div align = 'right'>
                  白：账务
                  红：销销
                </div></td>
            </tr>
            <tr>
            <td> 交货日期：{2}</td>
                <td> 单据号：{5}</ td >
                    <td colspan = '2'> 业务类型 普通销售 </td>
                         <td><div align = 'right' > 蓝：仓库 </div ></td>
                                </tr>
                                <tr>
                                <td> 客户名称：{3}</td>
                                <td colspan = '2' > 交货方式 {4}
                付款方式 转账</td>
<td></td>
<td> <div align = 'right' > 蓝：客户 </div ></td>
</tr>
</table> ", stock.FDistributionPoint,//配送点
"021-67121825", //电话
Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"),//
stock.FName,//客户名称
stock.FDeliveryMethod,//交货方式
stock.KeyId//
);
                print.Append(@"<table  border='1' cellspacing='0' width='750'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='60' height='30px'>商品代码</td>
            <td width='60'>商品名称</td>
            <td width='50'>规格</td>
            <td width='50'>计量单位</td>
			<td width='60'>商品发出数</td>
            <td width='60'>空器发出数</td>
			<td width='50'>商品收入数</td>
			<td width='50'>容器收入</td>
            <td width='70'>备注</td>
        </tr>");//

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintAllotPlan", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
										</tr>", //
                                          details.Rows[i]["FINum"],//产品代码
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          Convert.ToDecimal(details.Rows[i]["FQty"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty"]) ://
                                          details.Rows[i]["FQty"],//商品发出数

                                          //(Convert.ToDecimal(details.Rows[i]["FBottleQty"]).ToString("#.##").Equals("0.00") //
                                          //|| Convert.ToInt32(details.Rows[i]["FBottleQty"]).Equals(0)) ? "" : //
                                          //Convert.ToDecimal(details.Rows[i]["FBottleQty"].ToString()//
                                          //.Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FBottleQty"]) ://
                                          details.Rows[i]["FBottleQty"],//容器发出数

                                          //details.Rows[i]["FQty2"].ToString().Equals("0") ? "" : details.Rows[i]["FQty2"],
                                          //"",//欠瓶数
                                          //(client.FPushFlag==null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FPrice"],//,//单价//
                                          //(client.FPushFlag==null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FAmount"],
                                          "",//回收空瓶//Convert.ToDecimal(details.Rows[i]["AddUpBottleQty"]) == 0 ? "" : details.Rows[i]["AddUpBottleQty"],

                                          details.Rows[i]["FRecycleQty"],//商品收入数

                                          details.Rows[i]["FMemo"]
                                          );
                }

                int printCount = 5 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>合计数量</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
										<td></td></tr></table>", //
                                                               details.Compute("SUM(FQty)", "true"),//
                                                               details.Compute("SUM(FBottleQty)", "true"),//
                                                               "",//
                                                               details.Compute("SUM(FRecycleQty)", "true")//
                                                               );

                print.Append(bottomDetails);

                var bottom = string.Format(@"<br/><table style='font-size:13px;' align='center' width='750' cellspacing='0' border='0'>
            <tr>
              <td width='60' height='25'>业务员</td>
              <td width='60'>{0}</td>
              <td width='60'>出库人</td>
              <td width='60'>{2}</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>入库人</td>
              <td width='60'></td>
            </tr>
            <tr>
              <td>制单人</td>
              <td>{1}</td>
              <td>出库时间</td>
              <td>{3}</td>
              <td>送货人</td>
              <td>&nbsp;</td>
              <td>客户签收</td>
              <td>&nbsp;</td>
              <td>入库时间</td>
                <td height='25'></td>
            </tr>
            <tr>
              <td colspan='7' style='font-size:12px;'>*注：商品发出与收入数之差用于费用结算，容器项用于记录包装物流转。两者分别计数</td>
              <td height = '21' style = 'font-size:16px;' colspan = '3' > 上海浦江特种气体有限公司 </td >
            </tr>
            <tr>
              <td colspan = '7' style = 'font-size:12px;' > 打印时间：{4}（单据共1页，第1页）</td >
              <td height = '21' style = 'font-size:12px;' colspan = '3' > 地址：上海市化学工业区才华路10号</td>
            </tr>
        </table>
</div>
</body>
</html> ", stock.FSalesman, stock.CreateBy, stock.FShipper, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"), DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        private void AjaxPrintAllotTrans(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                var client = CustomerService.FirstOrDefault(p => p.FCode == stock.FCode && p.FCompanyId == Company.id);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'>
<head>
    <title>打印</title>
</head>
<body>
    <div style='margin:2px auto;width:750px;height:440px;border:0px solid red'>
        <table style='font-size:13px;' border='0' cellspacing='0' align='center' width='750'>
        	<tr height='22'>
                <td width='270'>配送点 {0} 电话 {1}</td>
                <td colspan='3'><center><font face='隶书' size='6'>调拨出库-入库单&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></center></td>
                <td width = '60' ><div align = 'right'>
                  白：账务
                  红：销销
                </div></td>
            </tr>
            <tr>
            <td> 交货日期：{2}</td>
                <td> 单据号：{5}</ td >
                    <td colspan = '2'> 业务类型 普通销售 </td>
                         <td><div align = 'right' > 蓝：仓库 </div ></td>
                                </tr>
                                <tr>
                                <td> 客户名称：{3}</td>
                                <td colspan = '2' > 交货方式 {4}
                付款方式 转账</td>
<td></td>
<td> <div align = 'right' > 蓝：客户 </div ></td>
</tr>
</table> ", stock.FDistributionPoint,//配送点
"021-67121825", //电话
Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"),//
stock.FName,//客户名称
stock.FDeliveryMethod,//交货方式
stock.KeyId//
);
                print.Append(@"<table  border='1' cellspacing='0' width='750'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='60' height='30px'>商品代码</td>
            <td width='60'>商品名称</td>
            <td width='50'>规格</td>
            <td width='50'>计量单位</td>
			<td width='60'>商品数</td>
            <td width='60'>容器数</td>
            <td width='70'>备注</td>
        </tr>");//

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintAllotTrans", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
										</tr>", //
                                          details.Rows[i]["FINum"],//产品代码
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          Convert.ToDecimal(details.Rows[i]["FQty"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty"]) ://
                                          details.Rows[i]["FQty"],//商品发出数

                                          //(Convert.ToDecimal(details.Rows[i]["FBottleQty"]).ToString("#.##").Equals("0.00") //
                                          //|| Convert.ToInt32(details.Rows[i]["FBottleQty"]).Equals(0)) ? "" : //
                                          //Convert.ToDecimal(details.Rows[i]["FBottleQty"].ToString()//
                                          //.Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FBottleQty"]) ://
                                          details.Rows[i]["FBottleQty"],//容器发出数

                                          //details.Rows[i]["FQty2"].ToString().Equals("0") ? "" : details.Rows[i]["FQty2"],
                                          //"",//欠瓶数
                                          //(client.FPushFlag==null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FPrice"],//,//单价//
                                          //(client.FPushFlag==null || client.FPushFlag.Equals("否")) ? "" : details.Rows[i]["FAmount"],
                                          //"",//回收空瓶//Convert.ToDecimal(details.Rows[i]["AddUpBottleQty"]) == 0 ? "" : details.Rows[i]["AddUpBottleQty"],

                                          //details.Rows[i]["FRecycleQty"],//商品收入数

                                          details.Rows[i]["FMemo"]
                                          );
                }

                int printCount = 5 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>合计数量</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
										<td></td></tr></table>", //
                                                               details.Compute("SUM(FQty)", "true"),//
                                                               details.Compute("SUM(FBottleQty)", "true"),//
                                                               "",//
                                                               details.Compute("SUM(FRecycleQty)", "true")//
                                                               );

                print.Append(bottomDetails);

                var bottom = string.Format(@"<br/><table style='font-size:13px;' align='center' width='750' cellspacing='0' border='0'>
            <tr>
              <td width='60' height='25'>业务员</td>
              <td width='60'>{0}</td>
              <td width='60'>出库人</td>
              <td width='60'>{2}</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>入库人</td>
              <td width='60'></td>
            </tr>
            <tr>
              <td>制单人</td>
              <td>{1}</td>
              <td>出库时间</td>
              <td>{3}</td>
              <td>送货人</td>
              <td>&nbsp;</td>
              <td>客户签收</td>
              <td>&nbsp;</td>
              <td>入库时间</td>
                <td height='25'></td>
            </tr>
            <tr>
              <td colspan='7' style='font-size:12px;'>*注：商品发出与收入数之差用于费用结算，容器项用于记录包装物流转。两者分别计数</td>
              <td height = '21' style = 'font-size:16px;' colspan = '3' > 上海浦江特种气体有限公司 </td >
            </tr>
            <tr>
              <td colspan = '7' style = 'font-size:12px;' > 打印时间：{4}（单据共1页，第1页）</td >
              <td height = '21' style = 'font-size:12px;' colspan = '3' > 地址：上海市化学工业区才华路10号</td>
            </tr>
        </table>
</div>
</body>
</html> ", stock.FSalesman, stock.CreateBy, stock.FShipper, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"), DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        /// <summary>
        ///     氢气发货单
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintTubeSales(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                var client = CustomerService.FirstOrDefault(p => p.FCode == stock.FCode && p.FCompanyId == Company.id);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'>
<head>
    <title>打印</title>
</head>
<body>
    <div style='margin:2px auto;width:750px;height:440px;border:0px solid red'>
        <table style='font-size:13px;' border='0' cellspacing='0' align='center' width='750'>
        	<tr height='22'>
                <td width='270'>配送点 {0} 电话 {1}</td>
                <td colspan='3'><center><font face='隶书' size='6'>销售出库-入库单&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></center></td>
                <td width = '60' ><div align = 'right'>
                  白：账务
                  红：销销
                </div></td>
            </tr>
            <tr>
            <td> 交货日期：{2}</td>
                <td> 单据号：{5}</ td >
                    <td colspan = '2'> 业务类型 普通销售 </td>
                         <td><div align = 'right' > 蓝：仓库 </div ></td>
                                </tr>
                                <tr>
                                <td> 客户名称：{3}</td>
                                <td colspan = '2' > 交货方式 {4}
                付款方式 转账</td>
<td></td>
<td> <div align = 'right' > 蓝：客户 </div ></td>
</tr>
</table> ", stock.FDistributionPoint,//配送点
"021-67121825", //电话
Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"),//
stock.FName + string.Format("({0})", stock.FOrgName).Replace("()", ""),//客户名称
stock.FDeliveryMethod,//交货方式
stock.KeyId//
);
                print.Append(@"<table  border='1' cellspacing='0' width='750'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='40' height='30px'>商品代码</td>
            <td width='60'>商品名称</td>
            <td width='50'>规格</td>
            <td width='22'>计量单位</td>
			<td width='60'>交付温度(°C)</td>
            <td width='60'>交付压力(MPa)</td>
			<td width='60'>回收温度(°C)</td>
			<td width='60'>回收压力(MPa)</td>
            <td width='80'>备注</td>
        </tr>");//

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintTubeSales", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
										</tr>", //
                                          details.Rows[i]["FINum"],//产品代码
                                          details.Rows[i]["FName"].ToString().Replace("()", ""),
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],

                                          details.Rows[i]["FPayTemperature"].ToString().Replace(".00", ""),//
                                          details.Rows[i]["FPayPressure"].ToString().Replace(".00", ""),//

                                          details.Rows[i]["FReceiveTemperature"].ToString().Replace(".00", ""),//
                                          details.Rows[i]["FReceivePressure"].ToString().Replace(".00", ""),//

                                          details.Rows[i]["FMemo"]
                                          );
                }

                int printCount = 5 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails);

                //      var bottomDetails = new StringBuilder();

                //      bottomDetails.AppendFormat(@"<tr align='center'>
                //                              <td height='30px'>合计数量</td>
                //                              <td></td>
                //                              <td></td>
                //                              <td></td>
                //                              <td>{0}</td>
                //                              <td>{1}</td>
                //                              <td>{2}</td>
                //                              <td>{3}</td>
                //<td></td></tr></table>", //
                //                                                     details.Compute("SUM(FQty)", "true"),//
                //                                                     details.Compute("SUM(FBottleQty)", "true"),//
                //                                                     "",//
                //                                                     details.Compute("SUM(FRecycleQty)", "true")//
                //                                                     );

                //      print.Append(bottomDetails);

                var bottom = string.Format(@"<br/><table style='font-size:13px;' align='center' width='750' cellspacing='0' border='0'>
            <tr><td colspan='10'>送货地址：{5} {6} {7} {8}</td></tr>
            <tr>
              <td width='60' height='25'>业务员</td>
              <td width='60'>{0}</td>
              <td width='60'>出库人</td>
              <td width='60'>{2}</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>入库人</td>
              <td width='60'></td>
            </tr>
            <tr>
              <td>制单人</td>
              <td>{1}</td>
              <td>出库时间</td>
              <td>{3}</td>
              <td>送货人</td>
              <td>&nbsp;</td>
              <td>客户签收</td>
              <td>&nbsp;</td>
              <td>入库时间</td>
                <td height='25'></td>
            </tr>
            <tr>
              <td colspan='7' style='font-size:12px;'>*注：商品发出与收入数之差用于费用结算，容器项用于记录包装物流转。两者分别计数</td>
              <td height = '21' style = 'font-size:16px;' colspan = '3' > 上海浦江特种气体有限公司 </td >
            </tr>
            <tr>
              <td colspan = '7' style = 'font-size:12px;' > 打印时间：{4}（单据共1页，第1页）</td >
              <td height = '21' style = 'font-size:12px;' colspan = '3' > 地址：上海市化学工业区才华路10号</td>
            </tr>
        </table>
</div>
</body>
</html> ", stock.FSalesman, stock.CreateBy, stock.FShipper,//
DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"), DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
stock.FAddress, stock.FLinkman, stock.FPhone, stock.FMemo);

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        /// <summary>
        ///     销售
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintSales2(HttpContext context)
        {

            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>华东气体配送单</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='100' >制单日期：</td><td colspan='3' style='width: 200px'>{0}</td><td width='100' align='right'>发货单号：</td>
<td>{1}</td></tr>
<tr height='22'><td width='100' >客户名称：</td><td colspan='3' style='width: 200px'>{2}</td><td width='100' align='right'>备注说明：</td>
<td>白：存根&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;红：财务&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;黄：客户</td></tr>
</table>", Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.KeyId, stock.FName);

                print.Append(@"<table  border='1' cellspacing='0' width='710'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='40'  rowspan='2'>序号</td>
            <td width='138' rowspan='2'>商品名称</td>
            <td width='35'  rowspan='2'>单位</td>
            <td width='50'  rowspan='2'>规格</td>
            <td colspan='4' width='240'>散瓶配送</td>
	        <td colspan='4' width='240'>集装格配送</td>
	        <td width='110' rowspan='2'>备注</td>
        </tr>
        <tr>
            <td width='50'>计划</td>
            <td width='50'>实收</td>
            <td width='50'>回空</td>
	        <td width='50'>欠瓶数</td>
            <td width='50'>满箱重</td>
            <td width='50'>满箱号</td>
	        <td width='50'>空箱重</td>
            <td width='50'>空箱号</td>
        </td>");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
                                        <td>{9}</td>
                                        <td>{10}</td>
                                        <td>{11}</td><td>{12}</td></tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FSpec"],
                                          Convert.ToDecimal(details.Rows[i]["FPlanQty"]) == 0 ? "" : details.Rows[i]["FPlanQty"],
                                          "",
                                          "",
                                          Convert.ToDecimal(details.Rows[i]["AddUpBottleQty"]) == 0 ? "" : details.Rows[i]["AddUpBottleQty"],
                                          "",//details.Rows[i]["FQty1"],
                                          "",//details.Rows[i]["FBottleNum"],
                                          "",//details.Rows[i]["FQty2"],
                                          "",//details.Rows[i]["FRecycleQtyNum"],
                                          "");
                }

                int printCount = 10 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center' height='22'>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails);

                //var bottomDetails = new StringBuilder();

                //                bottomDetails.AppendFormat(@"<tr align='center'><td>合计</td>
                //                                        <td></td>
                //                                        <td></td>
                //                                        <td></td>
                //                                        <td></td>
                //                                        <td>{0}</td>
                //                                        <td>{1}</td>
                //                                        <td>{2}</td>
                //                                        <td>{3}</td>
                //                                        <td>{4}</td>
                //										<td>{5}</td>
                //										<td></td></tr></table>", //
                //                                                               details.Compute("SUM(FAmount)", "true"),//
                //                                                               details.Compute("SUM(FQty)", "true"),//
                //                                                               details.Compute("SUM(FRecycleQty)", "true"),//
                //                                                               "", "", "");

                //print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='714' cellspacing='0' border='0'>
        <tr>
            <td height='25' width='714'>驾驶员：{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;押运员：{1} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;车牌号：{2} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;制单人：{3}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;签收人：</td>            
        </tr>
        <tr>
            <td height='21'>地址：无锡市梅村锡达路508-1号&nbsp;&nbsp;&nbsp;&nbsp;电话：0510-88156080，88156026&nbsp;&nbsp;&nbsp;&nbsp;传真：0510-81151076</td>            
        </tr>
    </table>
</div>
</body>
</html>", stock.FDriver, stock.FSupercargo, stock.FVehicleNum, stock.CreateBy);

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        #endregion

        private void AjaxPrintPassCard(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = PassCardService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {//<center><b><font face='隶书' size='5' ></font></b></center>
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='5'>{3}销售订单</font></b></center>

<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td  >单号:</td><td colspan='3' style='width: 200px'>{4}</td><td align='left'>制单日期:</td><td>{5}</td></tr>
<tr height='22'><td  >司机:</td><td colspan='3' style='width: 200px'>{6}</td><td align='left'>押运员:</td><td>{7}</td></tr>
<tr height='22'><td >车牌号:</td><td >{0}</td><td align='right'></td><td>{1}</td><td align='left'>制单人:</td><td>{2}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;送货日期：&nbsp;&nbsp;&nbsp;&nbsp;年&nbsp;&nbsp;&nbsp;&nbsp;月&nbsp;&nbsp;&nbsp;&nbsp;日</td></tr>
</table>",
                    stock.FVehicleNum, "", stock.CreateBy, Company.com_name, stock.KeyId, stock.FDate, stock.FDriver, stock.FSupercargo);

                print.Append(@"<table  border='1' cellspacing='0' width='710'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='40' rowspan='2'>序号</td>
            <td width='130' rowspan='2'>商品名称</td>
            <td width='60' rowspan='2'>规格</td>
            <td width='35' rowspan='2'>单位</td>
            <td colspan='3' width='220'>气体信息</td>
            <td width='50' rowspan='2'>单价</td>
            <td width='50' rowspan='2'>金额</td>
            <td width='110' rowspan='2'>备注</td>
        </tr>
        <tr align='center'>
            <td width='50'>公斤</td>
            <td width='50'>实瓶</td>
            <td width='50'>空瓶</td>
        </tr>");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintPassCard", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
                                        <td>{9}</td>
                                    </tr>", i + 1,
                        details.Rows[i]["FName"],
                        details.Rows[i]["FSpec"],
                        details.Rows[i]["FUnit"],
                        "", //details.Rows[i]["FQty"],
                            //details.Rows[i]["FBottleQty"],

                        details.Rows[i]["FBottleQty"].ToString().Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FBottleQty"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FBottleQty"]) ://
                                          details.Rows[i]["FBottleQty"],

                        "",//details.Rows[i]["FRecycleQty"],
                        "", //details.Rows[i]["AddUpBottleQty"],
                        "", //details.Rows[i]["FPrice"],
                        details.Rows[i]["FMemo"]
                        );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
            <td  height='21'></td>
<td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
        </tr>
        <tr>
            <td  height='21'></td>
<td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
        </tr>
    </table>");

                print.Append(bottomDetails);

                var bottom =
                    string.Format(@"<table style='font-size:13px;' align='center' width='714' cellspacing='0' border='0'>
       
        
        <tr>
            <td height='21' colspan='2' width='714'>备注：第一联：销方留存，第二联：客户留存，第三联：财务记账，第四联：仓库留存，第五联：门卫留存</td>            
        </tr>
        <tr>
            <td height='10' colspan='2' width='714'>&nbsp;&nbsp;</td>            
        </tr>
        <tr>
            <td height='21' width='400'>收货人签字:</td>
            <td  align='left' width='200'>仓库员签字：</td>            
        </tr>
    </table>
</div>
</body>
</html>", stock.FShipper, stock.FSupercargo, stock.FDriver, stock.FVehicleNum, DateTime.Now, stock.FMemo, stock.CreateBy);

                print.Append(bottom);
            }


            _response = print.ToString();
        }


        #region 销售
        private void AjaxPrintSalesRec(HttpContext context)
        {

            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6' >收&nbsp;&nbsp;款&nbsp;&nbsp;收&nbsp;&nbsp;据</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td>客户名称:{0}</td><td>单据号:{1}</td><td align='left'>日期:{2}</td></tr>
</table>", stock.FName, stock.KeyId, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));

                print.Append(@"<table  border='1' cellspacing='0' width='710'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='46' >序号</td>
            <td width='110' >商品名称</td>
            <td width='64' >规格</td>
            <td width='37' >单位</td>
            <td width='60' >数量</td>
            <td width='60' >单价</td>
            <td width='60' >金额</td>
            <td width='100' >备注</td>
        </tr>");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FBottleQty"],
                                          details.Rows[i]["FPrice"],
                                          details.Rows[i]["FAmount"],
                                          details.Rows[i]["FMemo"]
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
            <td><div align='right'>合计</div></td>
            <td colspan='4'>{0}</td>
            <td>小写</td>
            <td colspan='2'>{1}</td>
        </tr>
    </table>",
             //
             RmbHelper.Convert(Convert.ToDecimal(details.Compute("sum(FAmount)", "true"))),
                details.Compute("sum(FAmount)", "true"));


                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='714' cellspacing='0' border='0'>
        <tr>
            <td height='21' colspan='2' width='714'>备注：第一联（白色）：记账联     第二联（红色）：客户留存     第三联（黄色）：财务留存</td>            
        </tr>
        <tr>
            <td height='21' width='400'>收款人签名:</td>
            <td  align='left' width='200'>制单人：{0}</td>            
        </tr>
    </table>
</div>
</body>
</html>", stock.CreateBy);



                print.Append(bottom);
            }

            _response = print.ToString();

        }
        #endregion

        private void AjaxPrintBottleReturn(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockInService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>{7}</font></b></center>
<center><b><font face='隶书' size='6' >回收气瓶单</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='80' >客户代码:</td><td colspan='3' style='width: 200px'>{0}</td><td width='80' align='left'>客户代码:</td>
<td>{1}</td></tr>
<tr height='22'><td  >单号:</td><td colspan='3' style='width: 200px'>{2}</td><td align='left'>日期:</td>
<td>{3}</td></tr>
<tr height='22'><td >车牌号:</td><td >{4}</td><td align='right'></td><td>{5}</td><td align='left'>制单人:</td><td>{6}</td></tr>
</table>", stock.FCode, stock.FName, stock.KeyId, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FVehicleNum, "", stock.CreateBy, Company.com_name);

                print.Append(@"<table  border='1' cellspacing='0' width='710'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='46' rowspan='2'>序号</td>
            <td width='110' rowspan='2'>商品名称</td>
            <td width='64' rowspan='2'>规格</td>
            <td width='37' rowspan='2'>单位</td>
            <td colspan='4' width='240'>气体信息</td>
            
            <td width='80' colspan='3'>备注</td>
        </tr>
        <tr align='center'>
            <td width='61'>公斤</td>
            <td width='55'>实瓶</td>
            <td width='61'>空瓶</td>
            <td width='61'>空瓶</td>

<td width='80' colspan='3'></td>
        </tr>");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintPurchase", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td  colspan=3>{8}</td>
                                        
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          "",
                                          "",
                                          details.Rows[i]["FBottleQty"],
                                          "",
                                          details.Rows[i]["FMemo"]
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
            <td  height='21'></td>
<td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td  colspan=3></td>
        </tr>
        <tr>
            <td  height='21'></td>
<td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td  colspan=3></td>
        </tr>
    </table>");

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='714' cellspacing='0' border='0'>
       
        
        <tr>
            <td height='21' colspan='2' width='714'>备注：第一联(白色):办公室存；第二联(红色):值班门卫存；第三联(蓝色)：购方留存;第四联(黄色)：财务记账</td>            
        </tr>
        <tr>
            <td height='21' width='400'>制单人签字:</td>
            <td  align='left' width='200'></td>            
        </tr>
    </table>
</div>
</body>
</html>");
                print.Append(bottom);
            }

            _response = print.ToString();
        }

        private void AjaxPrintFK(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var order = FkOrderService.FirstOrDefault(p => p.keyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (order != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>{7}付款单</font></b></center>
<center><font face='隶书' size='3' >{8}</font></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{0}</td><td width='64' align='right'>单号</td>
<td width='120'>{1}</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>{2}</td><td align='right'>日期</td>
<td>{3}</td></tr><tr height='22'><td>联系人</td><td>{4}</td><td>联系电话</td><td>{5}</td><td>发票类型</td>
<td>{6}</td></tr></table>", order.FName, order.keyId, "", Convert.ToDateTime(order.FDate).ToString("yyyy-MM-dd"), "", "", "", Company.com_name, string.Format("地址：{0}  电话：{1}   ", Company.FAddress, Company.com_tel));

                print.AppendFormat(@"<table  border='1' cellspacing='0' width='840'  align='center' style='font-size:13px;'>
        <tr align='center' height='150'>
            <td width='120'>款项内容</td><td colspan='3'></td>
        </tr>
        <tr align='center'  height='30'>
            <td width='120'>大写 </td><td>{0}</td><td width='120'>小写 </td><td>{1}</td>
        </tr>
</table>", RmbHelper.Convert(Convert.ToDecimal(order.FAmount)), order.FAmount);

                var bottomDetails = new StringBuilder();

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='774' cellspacing='0' border='0'>
        <tr>
            <td width='80' height='23'>制单人：</td>
            <td width='81'>{0}</td>
            <td width='57'></td>
            <td width='57'></td>
            <td width='120'>经手人签字：</td>
            <td width='35'></td>
            <td width='51'></td>
            <td width='68'></td>
            <td width='75'></td>
            <td width='144'></td>
        </tr>
<tr>
            <td height='21' colspan='10'>备注：{2}</td>
        </tr>
        <tr>
            <td height='21' colspan='7'>注：白色联—财务 红色联—顾客 绿色联—存根</td>
<td height='21'>打印时间</td>
            <td height='21' colspan='2'>{1}</td>
        </tr>
    </table>
</div>
</body>
</html>", order.CreateBy, DateTime.Now, order.FMemo);

                print.Append(bottom);
            }

            _response = print.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintSK(HttpContext context)
        {

            var keyid = context.Request["keyid"];
            var order = SkOrderService.FirstOrDefault(p => p.keyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (order != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>{7}收款单</font></b></center>
<center><font face='隶书' size='3' >{8}</font></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{0}</td><td width='64' align='right'>单号</td>
<td width='120'>{1}</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>{2}</td><td align='right'>日期</td>
<td>{3}</td></tr><tr height='22'><td>联系人</td><td>{4}</td><td>联系电话</td><td>{5}</td><td>发票类型</td>
<td>{6}</td></tr></table>", order.FName, order.keyId, "", Convert.ToDateTime(order.FDate).ToString("yyyy-MM-dd"), "", "", "", Company.com_name, string.Format("地址：{0}  电话：{1}   ", Company.FAddress, Company.com_tel));

                print.AppendFormat(@"<table  border='1' cellspacing='0' width='840'  align='center' style='font-size:13px;'>
        <tr align='center' height='150'>
            <td width='120'>款项内容</td><td colspan='3'></td>
        </tr>
        <tr align='center'  height='30'>
            <td width='120'>大写 </td><td>{0}</td><td width='120'>小写 </td><td>{1}</td>
        </tr>
</table>", RmbHelper.Convert(Convert.ToDecimal(order.FAmount)), order.FAmount);

                var bottomDetails = new StringBuilder();

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='774' cellspacing='0' border='0'>
        <tr>
            <td width='80' height='23'>制单人：</td>
            <td width='81'>{0}</td>
            <td width='57'></td>
            <td width='57'></td>
            <td width='120'>经手人签字：</td>
            <td width='35'></td>
            <td width='51'></td>
            <td width='68'></td>
            <td width='75'></td>
            <td width='144'></td>
        </tr>
<tr>
            <td height='21' colspan='10'>备注：{2}</td>
        </tr>
        <tr>
            <td height='21' colspan='7'>注：白色联—财务 红色联—顾客 绿色联—存根</td>
<td height='21'>打印时间</td>
            <td height='21' colspan='2'>{1}</td>
        </tr>
    </table>
</div>
</body>
</html>", order.CreateBy, DateTime.Now, order.FMemo);

                print.Append(bottom);
            }

            _response = print.ToString();
        }

        private void AjaxPrintProfit(HttpContext context)
        {

            var keyid = context.Request["keyid"];
            var stock = StockInService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>{7}盘盈单</font></b></center>
<center><font face='隶书' size='3' >{8}</font></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{0}</td><td width='64' align='right'>单号</td>
<td width='120'>{1}</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>{2}</td><td align='right'>日期</td>
<td>{3}</td></tr><tr height='22'><td>联系人</td><td>{4}</td><td>联系电话</td><td>{5}</td><td>发票类型</td>
<td>{6}</td></tr></table>", stock.FName, stock.KeyId, stock.FAddress, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FLinkman, stock.FPhone, "", Company.com_name, string.Format("地址：{0}  电话：{1}   ", Company.FAddress, Company.com_tel));

                print.Append(@"<table  border='1' cellspacing='0' width='840' align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='46' >序号</td>
            <td width='120'>品名</td>
            <td width='74'>规格</td>
            <td width='37'>单位</td>
            <td width='70'>数量</td>
        </tr>");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintProfit", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FQty"]);
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"</table>");

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='left' width='774' cellspacing='0' border='0'>
        <tr>
            <td width='60' height='23'>制单人：</td>
            <td width='80'>{0}</td>
            <td width='60'>配送员:</td>
            <td width='80'>{1}</td>
            <td width='60'>司机：</td>
            <td width='80'>{2}</td>
            <td width='60'>车号：</td>
            <td width='80'>{3}</td>
            <td width='80'>客户签字：</td>
            <td width='60'></td>
        </tr>
        <tr>
            <td height='21' colspan='10'>备注：{5}</td>
        </tr>
        <tr>
            <td height='21' colspan='6'>注：白色联—财务 红色联—顾客 绿色联—存根</td>
            <td height='21' >打印时间</td><td height='21' colspan='3'>{4}</td>
        </tr>
    </table>
</div>
</body>
</html>", stock.CreateBy, stock.FSupercargo, stock.FDriver, stock.FVehicleNum, DateTime.Now, stock.FMemo);

                print.Append(bottom);
            }

            _response = print.ToString();
        }

        private void AjaxPrintLosses(HttpContext context)
        {

            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>{7}盘亏单</font></b></center>
<center><font face='隶书' size='3' >{8}</font></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{0}</td><td width='64' align='right'>单号</td>
<td width='120'>{1}</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>{2}</td><td align='right'>日期</td>
<td>{3}</td></tr><tr height='22'><td>联系人</td><td>{4}</td><td>联系电话</td><td>{5}</td><td>发票类型</td>
<td>{6}</td></tr></table>", stock.FName, stock.KeyId, stock.FAddress, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FLinkman, stock.FPhone, "", Company.com_name, string.Format("地址：{0}  电话：{1}   ", Company.FAddress, Company.com_tel));

                print.Append(@"<table  border='1' cellspacing='0' width='840'  align='center' style='font-size:13px;'>
        <tr align='center' height='26'>
            <td width='46' >序号</td>
            <td width='120'>品名</td>
            <td width='74'>规格</td>
            <td width='37'>单位</td>
            <td width='70'>数量</td>
        </tr>");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintLosses", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center' height='26'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FQty"]);
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"</table>");

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='left' width='774' cellspacing='0' border='0'>
        <tr>
            <td width='60' height='23'>制单人：</td>
            <td width='80'>{0}</td>
            <td width='60'>配送员:</td>
            <td width='80'>{1}</td>
            <td width='60'>司机：</td>
            <td width='80'>{2}</td>
            <td width='60'>车号：</td>
            <td width='80'>{3}</td>
            <td width='80'>客户签字：</td>
            <td width='60'></td>
        </tr>
        <tr>
            <td height='21' colspan='10'>备注：{5}</td>
        </tr>
        <tr>
            <td height='21' colspan='6'>注：白色联—财务 红色联—顾客 绿色联—存根</td>
            <td height='21' >打印时间</td><td height='21' colspan='3'>{4}</td>
        </tr>
    </table>
</div>
</body>
</html>", stock.CreateBy, stock.FSupercargo, stock.FDriver, stock.FVehicleNum, DateTime.Now, stock.FMemo);
                print.Append(bottom);
            }

            _response = print.ToString();
        }

        private void AjaxPrintLeaseReturn(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = LeaseReturnService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<h1 style='text-align:center;'><font face='隶书' size='6'>{7}气瓶租金收款单</font></h1>
<center><font face='隶书' size='3' >{8}</font></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='710'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{0}</td><td width='64' align='right'>单号</td>
<td width='120'>{1}</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>{2}</td><td align='right'>日期</td>
<td>{3}</td></tr><tr height='22'><td>联系人</td><td>{4}</td><td>联系电话</td><td>{5}</td><td>发票类型</td>
<td>{6}</td></tr></table>", stock.FName, stock.KeyId, stock.FAddress, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FLinkman, stock.FPhone, "", Company.com_name, string.Format("地址：{0}  电话：{1}   ", Company.FAddress, Company.com_tel));

                print.Append(@"<table  border='1' cellspacing='0' width='710'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='46'>序号</td>
            <td width='80'>品名</td>
            <td width='60' >规格</td>
            <td width='60' >单位</td>
            <td width='60'>数量</td>
            <td width='70'>日租金/瓶</td>
            <td width='60'>押金/个</td>
        </tr> ");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintLeaseReturn", parms).Tables[0];

                var salesDetails = new StringBuilder();

                decimal sum = 0.00M;

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FBottleQty"],
                                          details.Rows[i]["FRentDay"],
                                          details.Rows[i]["FPrice"]
                                          );

                    sum += Convert.ToDecimal(details.Rows[i]["FPrice"]) * Convert.ToDecimal(details.Rows[i]["FBottleQty"]);

                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr><td colspan='7'>收租金：{0}  大写金额：{1}&nbsp; &nbsp; &nbsp; &nbsp; 退押金：{2}  大写金额：{3}</td></tr></table>"
                    , details.Compute("sum(FPayMentRentals)", "true"),//
                RmbHelper.Convert(Convert.ToDecimal(details.Compute("sum(FPayMentRentals)", "true"))), sum, RmbHelper.Convert(Convert.ToDecimal(sum)));
                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='700' cellspacing='0' border='0'>
        <tr>
            <td width='55' height='23'>制单人：</td>
            <td width='81'>{0}</td>
            <td width='57'>配送员:</td>
            <td width='87'>{1}</td>
            <td width='47'>司机：</td>
            <td width='95'>{2}</td>
            <td width='51'>车号：</td>
            <td width='68'>{3}</td>
            <td width='75'>客户签字：</td>
            <td width='144'></td>
        </tr>
        <tr>
            <td height='21' colspan='8'>备注：白色联—财务 红色联—顾客 绿色联—存根</td>
<td height='21'>打印时间</td>
            <td height='21'>{4}</td>
        </tr>
    </table>
</div>
</body>
</html>", stock.CreateBy, stock.FSupercargo, stock.FDriver, stock.FVehicleNum, DateTime.Now);

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintLease(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = LeaseService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:400px;border:0px solid red' >
<h1 style='text-align:center;'><font face='隶书' size='6'>{0}押金收款单</font></h1>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='720'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{1}</td><td width='64' align='right'>单号</td>
<td width='120'>{2}</td></tr><tr height='22'><td>联系人</td><td colspan='3' style='width: 224px'>{3}</td><td align='right'>日期</td>
<td>{4}</td></tr></table>",//
                                 Company.com_name,//
                                 stock.FName,//
                                 stock.KeyId,//
                                 stock.FLinkman,//
                                 Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));

                print.Append(@"<table  border='1' cellspacing='0' width='720'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='40'  height='30px'>序号</td>
            <td width='100'>钢瓶名称</td>
            <td width='80' >规格</td>
            <td width='60' >单位</td>
            <td width='60' >数量</td>
            <td width='60' >押金单价</td>
            <td width='60' >金额</td>
            <td width='80' >日租金/瓶</td>
            <td width='90' >开始时间</td>
        </tr> ");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintLease", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>

                                    </tr>", i + 1,//序号
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FBottleQty"],//数量
                                          details.Rows[i]["FPrice"],
                                          details.Rows[i]["FAmount"],
                                          details.Rows[i]["FRentDay"],
                                          Convert.ToDateTime(details.Rows[i]["FDate"]).ToString("yyyy-MM-dd")
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr>
            <td colspan='9'  height='30px'><div align='left'>合计：{0}大写金额：{1}</div></td>
        </tr>
<tr><td colspan='9' height='30px'><div align='center'>收款人员签名（盖章）</div></td></tr>
    </table>", details.Compute("sum(FAmount)", "true"),//
             RmbHelper.Convert(Convert.ToDecimal(details.Compute("sum(FAmount)", "true"))));

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='center' width='720' cellspacing='0' border='0'>
        
        <tr>
            <td height='21' colspan='9'>1、凡顾客将租用空瓶交回我公司，应无损坏，瓶租依章付清，押金如数退回。</td>
        </tr>
        <tr>
            <td height='21' colspan='9'>2、凡顾客退瓶后，未按章付清瓶租者，由我公司在此押金内抵扣。</td>
        </tr>
        <tr>
            <td height='21' colspan='9'>3、本单无加盖我公司财务专用章无效，凡客户需退此押金时，需携回红色押金联，无此单我公司一律拒绝退款。</td>
        </tr>
        <tr>
            <td height='21' colspan='9'>4、三年内不退瓶视同售出，需要继续使用的更换押金单。</td>
        </tr>
        <tr>
            <td height='21' colspan='9'>5、不使用的瓶请及时归还，对于一个月以上未用我公司的将按1元/天（40L瓶），10元/天（杜瓦罐、液氨罐、生物容器）收取租金。</td>
        </tr>
        <tr>
            <td height='21' colspan='9' style='font-size:12px;'>第一联为白色：销方财务;第二联为红色：销方仓库；第三联为蓝色：客户留存；第四联为绿色：客户留存；第五联为黄色：销方门卫留存</td>            
        </tr>
<tr>
            <td width='60' height='30px'>租瓶人：</td>
            <td width='68'></td>
            <td width='60'></td>
            <td width='68'></td>
            <td width='60'></td>
            <td width='68'>还瓶人：</td>
            <td width='60'></td>
            <td width='60'></td>
            <td width='80'></td>
            <td width='40'></td>
        </tr>
    </table>
</div>
</body>
</html>");

                print.Append(bottom);
            }

            _response = print.ToString();
        }

        #region 充气入库单

        private void AjaxPrintProduction(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockInService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>{7}充气入库单</font></b></center>
<center><font face='隶书' size='3' >{8}</font></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='68'>客户名称</td><td colspan='3' style='width: 224px'>{0}</td><td width='64' align='right'>单号</td>
<td width='120'>{1}</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>{2}</td><td align='right'>日期</td>
<td>{3}</td></tr><tr height='22'><td>联系人</td><td>{4}</td><td>联系电话</td><td>{5}</td><td>发票类型</td>
<td>{6}</td></tr></table>", stock.FName, stock.KeyId, stock.FAddress, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FLinkman, stock.FPhone, "", Company.com_name, string.Format("地址：{0}  电话：{1}   ", Company.FAddress, Company.com_tel));

                print.Append(@"<table  border='1' cellspacing='0' width='840'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='40' >序号</td>
            <td width='120'>品名</td>
            <td width='74' >规格</td>
            <td width='50' >单位</td>
            <td width='80' >数量</td>
        </tr>");

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintProduction", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FQty"]
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"</tr>
    </table>");

                print.Append(bottomDetails);


                var bottom = string.Format(@"<table style='font-size:13px;' align='left' width='774' cellspacing='0' border='0'>
        <tr>
            <td width='60' height='23'>制单人：</td>
            <td width='80'>{0}</td>
            <td width='60'>配送员:</td>
            <td width='80'>{1}</td>
            <td width='60'>司机：</td>
            <td width='80'>{2}</td>
            <td width='60'>车号：</td>
            <td width='80'>{3}</td>
            <td width='80'>客户签字：</td>
            <td width='60'></td>
        </tr>
        <tr>
            <td height='21' colspan='10'>备注：{5}</td>
        </tr>
        <tr>
            <td height='21' colspan='6'>注：白色联—财务 红色联—顾客 绿色联—存根</td>
            <td height='21' >打印时间</td><td height='21' colspan='3'>{4}</td>
        </tr>
    </table>
</div>
</body>
</html>", stock.CreateBy, stock.FSupercargo, stock.FDriver, stock.FVehicleNum, DateTime.Now, stock.FMemo);

                print.Append(bottom);
            }

            _response = print.ToString();
        }
        #endregion

        private void AjaxPrintAllotDept(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>{7}物资调拨单（内）</font></b></center>
<center><font face='隶书' size='3' >{8}</font></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{0}</td><td width='64' align='right'>单号</td>
<td width='120'>{1}</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>{2}</td><td align='right'>日期</td>
<td>{3}</td></tr><tr height='22'><td>联系人</td><td>{4}</td><td>联系电话</td><td>{5}</td><td>发票类型</td>
<td>{6}</td></tr></table>", stock.FName, stock.KeyId, stock.FAddress, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FLinkman, stock.FPhone, "", Company.com_name, string.Format("地址：{0}  电话：{1}   ", Company.FAddress, Company.com_tel, DateTime.Now));

                print.Append(@"<table  border='1' cellspacing='0' width='840'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='46' rowspan='2'>序号</td>
            <td width='120' rowspan='2'>品名</td>
            <td width='74' rowspan='2'>规格</td>
            <td width='37' rowspan='2'>单位</td>
            <td colspan='4' width='290'>气体信息</td>
            <td width='70' rowspan='2'>单价</td>
            <td width='70' rowspan='2'>金额</td>
            <td width='80' rowspan='2'>备注</td>
        </tr>
        <tr align='center'>
            <td width='61'>公斤</td>
            <td width='61'>空瓶</td>
            <td width='55'>实瓶</td>
            <td width='71'>累计欠瓶</td>
        </tr>");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
                                        <td>{9}</td>
                                        <td>{10}</td>
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FQty"],
                                          details.Rows[i]["FRecycleQty"],
                                          details.Rows[i]["FBottleQty"],
                                          details.Rows[i]["AddUpBottleQty"],
                                          "",
                                          "",
                                          ""
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
            <td colspan='4'><div align='right'>合计</div></td>
            <td>{5}</td>
            <td>{0}</td>
            <td>{1}</td>
            <td>{2}</td>
            <td colspan='2'><div align='left'>金额合计：{3}</div></td>
        </tr>
        <tr>
            <td colspan='11'>大写金额：{4}</td>
        </tr>
    </table>", details.Compute("sum(FRecycleQty)", "true"),//
             details.Compute("sum(FBottleQty)", "true"), //
             details.Compute("sum(AddUpBottleQty)", "true"), //
             details.Compute("sum(FAmount)", "true"),//
             RmbHelper.Convert(Convert.ToDecimal(details.Compute("sum(FAmount)", "true"))),
             details.Compute("sum(FQty)", "true"));

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='left' width='774' cellspacing='0' border='0'>
        <tr>
            <td width='60' height='23'>制单人：</td>
            <td width='80'>{0}</td>
            <td width='60'>配送员:</td>
            <td width='80'>{1}</td>
            <td width='60'>司机：</td>
            <td width='80'>{2}</td>
            <td width='60'>车号：</td>
            <td width='80'>{3}</td>
            <td width='80'>客户签字：</td>
            <td width='60'></td>
        </tr>
        <tr>
            <td height='21' colspan='10'>备注：{5}</td>
        </tr>
        <tr>
            <td height='21' colspan='6'>注：白色联—财务 红色联—顾客 绿色联—存根</td>
            <td height='21' >打印时间</td><td height='21' colspan='3'>{4}</td>
        </tr>
    </table>
</div>
</body>
</html>", stock.CreateBy, stock.FSupercargo, stock.FDriver, stock.FVehicleNum, DateTime.Now, stock.FMemo);
                print.Append(bottom);
            }

            _response = print.ToString();

        }

        private void AjaxPrintAllot(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>{7}物资调拨单（外）</font></b></center>
<center><font face='隶书' size='3' >{8}</font></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{0}</td><td width='64' align='right'>单号</td>
<td width='120'>{1}</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>{2}</td><td align='right'>日期</td>
<td>{3}</td></tr><tr height='22'><td>联系人</td><td>{4}</td><td>联系电话</td><td>{5}</td><td>发票类型</td>
<td>{6}</td></tr></table>", stock.FName, stock.KeyId, stock.FAddress, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FLinkman, stock.FPhone, "", Company.com_name, string.Format("地址：{0}  电话：{1}   ", Company.FAddress, Company.com_tel, DateTime.Now));

                print.Append(@"<table  border='1' cellspacing='0' width='840'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='46' rowspan='2'>序号</td>
            <td width='120' rowspan='2'>品名</td>
            <td width='74' rowspan='2'>规格</td>
            <td width='37' rowspan='2'>单位</td>
            <td colspan='4' width='290'>气体信息</td>
            <td width='70' rowspan='2'>单价</td>
            <td width='70' rowspan='2'>金额</td>
            <td width='80' rowspan='2'>备注</td>
        </tr>
        <tr align='center'>
            <td width='61'>公斤</td>
            <td width='61'>空瓶</td>
            <td width='55'>实瓶</td>
            <td width='71'>累计欠瓶</td>
        </tr>");
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
                                        <td>{9}</td>
                                        <td>{10}</td>
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FQty"],
                                          details.Rows[i]["FRecycleQty"],
                                          details.Rows[i]["FBottleQty"],
                                          details.Rows[i]["AddUpBottleQty"],
                                          "",
                                          "",
                                          ""
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
            <td colspan='4'><div align='right'>合计</div></td>
            <td>{5}</td>
            <td>{0}</td>
            <td>{1}</td>
            <td>{2}</td>
            <td colspan='2'><div align='left'>金额合计：{3}</div></td>
        </tr>
        <tr>
            <td colspan='11'>大写金额：{4}</td>
        </tr>
    </table>", details.Compute("sum(FRecycleQty)", "true"),//
             details.Compute("sum(FBottleQty)", "true"), //
             details.Compute("sum(AddUpBottleQty)", "true"), //
             details.Compute("sum(FAmount)", "true"),//
             RmbHelper.Convert(Convert.ToDecimal(details.Compute("sum(FAmount)", "true"))),
             details.Compute("sum(FQty)", "true"));

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='left' width='774' cellspacing='0' border='0'>
        <tr>
            <td width='60' height='23'>制单人：</td>
            <td width='80'>{0}</td>
            <td width='60'>配送员:</td>
            <td width='80'>{1}</td>
            <td width='60'>司机：</td>
            <td width='80'>{2}</td>
            <td width='60'>车号：</td>
            <td width='80'>{3}</td>
            <td width='80'>客户签字：</td>
            <td width='60'></td>
        </tr>
        <tr>
            <td height='21' colspan='10'>备注：{5}</td>
        </tr>
        <tr>
            <td height='21' colspan='6'>注：白色联—财务 红色联—顾客 绿色联—存根</td>
            <td height='21' >打印时间</td><td height='21' colspan='3'>{4}</td>
        </tr>
    </table>
</div>
</body>
</html>", stock.CreateBy, stock.FSupercargo, stock.FDriver, stock.FVehicleNum, DateTime.Now, stock.FMemo);

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        #region 采购单
        /// <summary>
        ///     采购单
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintBottleTo(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                //var client = U.FirstOrDefault(p => p.FCode == stock.FCode && p.FCompanyId == Company.id);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>上海浦江特种气体有限公司气瓶出库单</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='780'>
<tr height='22'><td width='100' >客户代码：</td><td colspan='3' style='width: 220px'>{0}</td><td width='100' align='right'>单号：</td>
<td>{1}</td></tr>
<tr height='22'><td width='100' >客户名称：</td><td colspan='3' style='width: 220px'>{2}</td><td width='100' align='right'>日期：</td>
<td>{3}</td></tr>
</table>", stock.FCode, stock.KeyId, stock.FName, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));
                print.Append(@"<table  border='1' cellspacing='0' width='750'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='60' height='30px'>订单编号</td>
            <td width='60'>商品名称</td>
            <td width='50'>规格</td>
            <td width='50'>单位</td>
			<td width='60'>容器出库</td>
            <td width='70'>备注</td>
        </tr>");

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

                var salesDetails = new StringBuilder();


                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
										</tr>", //
                                          details.Rows[i]["FNum"],//订单号码//i + 1,//<td>{10}</td>
                                          details.Rows[i]["FName"].ToString().Replace("()",""),
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          Convert.ToDecimal(details.Rows[i]["FQty"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty"]) ://
                                          details.Rows[i]["FQty"],

                                          details.Rows[i]["FMemo"]
                                          );
                }

                int printCount = 7 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>合计</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td></td>
										</tr></table>", //
                                             details.Compute("SUM(FQty)", "true"));

                print.Append(bottomDetails);

                var bottom = string.Format(@"<br/><table style='font-size:13px;' align='center' width='780' cellspacing='0' border='0'>
        <tr>
            <td height='25' width='780'>司机：{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;押运员：{1} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;车牌号：{2} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;送货人：{3}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;收货人：</td>            
        </tr>
        <tr>
            <td height='21' style='font-size:12px;'>第一联为白色：销方财务;第二联为红色：销方仓库；第三联为蓝色：客户留存；第四联为绿色：客户留存；第五联为黄色：销方门卫留存</td>            
        </tr>
    </table>
</div>
</body>
</html>", stock.FDriver, stock.FSupercargo, stock.FVehicleNum, "");

                print.Append(bottom);
            }

            _response = print.ToString();
        }
        #endregion

        private void AjaxPrintStockIn(HttpContext context)
        {

        }

        #region 采购退货单
        private void AjaxPrintPurchaseReturn(HttpContext context)
        {

            var keyid = context.Request["keyid"];
            var stock = StockOutService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6' style='letter-spacing: 3px'>{7}退货出仓单</font></b></center>
<center><font face='隶书' size='3' >{8}</font></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>{0}</td><td width='64' align='right'>单号</td>
<td width='120'>{1}</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>{2}</td><td align='right'>日期</td>
<td>{3}</td></tr><tr height='22'><td>联系人</td><td>{4}</td><td>联系电话</td><td>{5}</td><td>发票类型</td>
<td>{6}</td></tr></table>", stock.FName, stock.KeyId, stock.FAddress, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FLinkman, stock.FPhone, "", Company.com_name, string.Format("地址：{0}  电话：{1}   ", Company.FAddress, Company.com_tel));

                print.Append(@"<table  border='1' cellspacing='0' width='840'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='46' rowspan='2'>序号</td>
            <td width='120' rowspan='2'>品名</td>
            <td width='74' rowspan='2'>规格</td>
            <td width='37' rowspan='2'>单位</td>
            <td colspan='4' width='290'>气体信息</td>
            <td width='70' rowspan='2'>单价</td>
            <td width='70' rowspan='2'>金额</td>
            <td width='80' rowspan='2'>备注</td>
        </tr>
        <tr align='center'>
            <td width='61'>公斤</td>
            <td width='61'>空瓶</td>
            <td width='55'>实瓶</td>
            <td width='71'>累计欠瓶</td>
        </tr>");


                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSales", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
                                        <td>{9}</td>
                                        <td>{10}</td>
                                    </tr>", i + 1,
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FQty"],
                                          details.Rows[i]["FRecycleQty"],
                                          details.Rows[i]["FBottleQty"],
                                          details.Rows[i]["AddUpBottleQty"],
                                          "",//details.Rows[i]["FPrice"],
                                          "",//details.Rows[i]["FAmount"],
                                          details.Rows[i]["FMemo"]
                                          );
                }

                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
            <td colspan='4'><div align='right'>合计</div></td>
            <td>{5}</td>
            <td>{0}</td>
            <td>{1}</td>
            <td>{2}</td>
            <td colspan='2'><div align='left'>金额合计：{3}</div></td>
        </tr>
        <tr>
            <td colspan='11'>大写金额：{4}</td>
        </tr>
    </table>", details.Compute("sum(FRecycleQty)", "true"),//
             details.Compute("sum(FBottleQty)", "true"), //
             details.Compute("sum(AddUpBottleQty)", "true"), //
             details.Compute("sum(FAmount)", "true"),//
             RmbHelper.Convert(Convert.ToDecimal(details.Compute("sum(FAmount)", "true"))),
             details.Compute("sum(FQty)", "true")
                );

                print.Append(bottomDetails);

                var bottom = string.Format(@"<table style='font-size:13px;' align='left' width='774' cellspacing='0' border='0'>
        <tr>
            <td width='60' height='23'>制单人：</td>
            <td width='80'>{0}</td>
            <td width='60'>配送员:</td>
            <td width='80'>{1}</td>
            <td width='60'>司机：</td>
            <td width='80'>{2}</td>
            <td width='60'>车号：</td>
            <td width='80'>{3}</td>
            <td width='80'>客户签字：</td>
            <td width='60'></td>
        </tr>
        <tr>
            <td height='21' colspan='10'>备注：{5}</td>
        </tr>
        <tr>
            <td height='21' colspan='6'>注：白色联—财务 红色联—顾客 绿色联—存根</td>
            <td height='21' >打印时间</td><td height='21' colspan='3'>{4}</td>
        </tr>
    </table>
</div>
</body>
</html>", stock.CreateBy, stock.FSupercargo, stock.FDriver, stock.FVehicleNum, DateTime.Now, stock.FMemo);

                // <tr>
                //    <td height='21' colspan='10'>第一联：存根（白） 第二联：提货（红） 第三联：结算：（黄） 第四联：回单（绿）</td>
                //</tr>

                print.Append(bottom);
            }

            _response = print.ToString();


        }
        #endregion

        #region 采购单
        /// <summary>
        ///     采购单
        /// </summary>
        /// <param name="context"></param>
        private void AjaxPrintPurchase(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockInService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                //var client = SupplierService.FirstOrDefault(p => p.FCode == stock.FCode && p.FCompanyId == Company.id);

                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'>
<head>
    <title>打印</title>
</head>
<body>
    <div style='margin:2px auto;width:750px;height:220px;border:0px solid red'>
        <table style='font-size:13px;' border='0' cellspacing='0' align='center' width='750'>
        	<tr height='22'>
                <td width='270'>配送点 {0} 电话 {1}</td>
                <td colspan='3'><center><font face='隶书' size='6'>采购出库-入库单&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></center></td>
                <td width = '60' ><div align = 'right'>
                  白：账务
                  红：销销
                </div></td>
            </tr>
            <tr>
            <td> 交货日期：{2}</td>
                <td> 单据号：{5}</ td >
                    <td colspan = '2'> 业务类型 普通采购 </td>
                         <td><div align = 'right' > 蓝：仓库 </div ></td>
                                </tr>
                                <tr>
                                <td> 客户名称：{3}</td>
                                <td colspan = '2' > 交货方式 {4}
                付款方式 转账</td>
<td></td>
<td> <div align = 'right' > 蓝：客户 </div ></td>
</tr>
</table> ", stock.FDistributionPoint,//配送点
"021-67121825", //电话
Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"),//
stock.FName,//客户名称
stock.FDeliveryMethod,//交货方式
stock.KeyId//
);
                print.Append(@"<table  border='1' cellspacing='0' width='750'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='50' height='30px'>序号</td>
            <td width='60'>商品名称</td>
            <td width='46'>规格</td>
            <td width='32'>计量单位</td>
			<td width='60'>商品入库数</td>
            <td width='60'>容器入库数</td>
			<td width='60'>商品出库数</td>
			<td width='60'>容器出库数</td>
            <td width='70'>备注</td>
        </tr>");//

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintPurchase", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
										</tr>", //
                                          i + 1,//details.Rows[i]["FINum"],//产品代码
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],

                                          //商器发出数
                                          Convert.ToDecimal(details.Rows[i]["FQty"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FQty"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty"]) ://
                                          details.Rows[i]["FQty"],//

                                          //容器发出数
                                          details.Rows[i]["FBottleQty"],//容器发出数

                                          //商品收入数
                                          Convert.ToDecimal(details.Rows[i]["FReturnQty"]).ToString("#.##").Equals("0.00") ? "" : //
                                          Convert.ToDecimal(details.Rows[i]["FReturnQty"].ToString()//
                                          .Split('.')[1]) == 0 ? Convert.ToDecimal(details.Rows[i]["FReturnQty"]) ://
                                          details.Rows[i]["FReturnQty"],

                                          //容器收入数
                                          Convert.ToInt32(details.Rows[i]["FRecycleQty"]).ToString().Equals("0") ? "" : //
                                          details.Rows[i]["FRecycleQty"],

                                          details.Rows[i]["FMemo"]
                                          );
                }

                int printCount = 5 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td height='30px'>合计数量</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
										<td></td></tr></table>", //
                                                               details.Compute("SUM(FQty)", "true"),//
                                                               details.Compute("SUM(FBottleQty)", "true"),//
                                                               details.Compute("SUM(FReturnQty)", "true"),//
                                                               details.Compute("SUM(FRecycleQty)", "true")//
                                                               );

                print.Append(bottomDetails);

                var bottom = string.Format(@"<br/><table style='font-size:13px;' align='center' width='750' cellspacing='0' border='0'>
            <tr><td colspan='10'>备注：{5} {6} {7} {8}</td></tr>          
            <tr>
              <td width='60' height='25'>业务员</td>
              <td width='60'>{0}</td>
              <td width='60'>出库人</td>
              <td width='60'>{2}</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>&nbsp;</td>
              <td width='60'>入库人</td>
              <td width='60'></td>
            </tr>
            <tr>
              <td>制单人</td>
              <td>{1}</td>
              <td>出库时间</td>
              <td>{3}</td>
              <td>送货人</td>
              <td>&nbsp;</td>
              <td>客户签收</td>
              <td>&nbsp;</td>
              <td>入库时间</td>
                <td height='25'></td>
            </tr>
            <tr>
              <td colspan='7' style='font-size:12px;'>*注：商品发出与收入数之差用于费用结算，容器项用于记录包装物流转。两者分别计数</td>
              <td height = '21' style = 'font-size:16px;' colspan = '3' > 上海浦江特种气体有限公司 </td >
            </tr>
            <tr>
              <td colspan = '7' style = 'font-size:12px;' > 打印时间：{4}（单据共1页，第1页）</td >
              <td height = '21' style = 'font-size:12px;' colspan = '3' > 地址：上海市化学工业区才华路10号</td>
            </tr>
        </table>
</div>
</body>
</html> ", stock.FSalesman, stock.CreateBy, stock.FShipper, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
stock.FAddress, stock.FLinkman, stock.FPhone ,stock.FMemo
);

                print.Append(bottom);
            }

            _response = print.ToString();

        }

        ////        private void AjaxPrintPurchase(HttpContext context)
        ////        {
        ////            var keyid = context.Request["keyid"];
        ////            var stock = StockInService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
        ////            var print = new StringBuilder();


        ////            if (stock != null)
        ////            {
        ////                print.AppendFormat(@"<!DOCTYPE html>
        ////<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
        ////<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
        ////<center><b><font face='隶书' size='6'>{7}采购入库单</font></b></center>
        ////<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='685'>
        ////<tr height='22'><td width='80' >供应商编码:</td><td colspan='3' style='width: 200px'>{0}</td><td width='80' align='left'>供应商:</td>
        ////<td>{1}</td></tr>
        ////<tr height='22'><td  >单号:</td><td colspan='3' style='width: 200px'>{2}</td><td align='left'>日期:</td>
        ////<td>{3}</td></tr></table>", stock.FCode, stock.FName, stock.KeyId, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"), stock.FVehicleNum, "", stock.CreateBy, Company.com_name);

        ////                print.Append(@"<table  border='1' cellspacing='0' width='710'  align='center' style='font-size:13px;'>
        ////        <tr align='center'>
        ////            <td width='46'>序号</td>
        ////            <td width='80'>商品名称</td>
        ////            <td width='64'>规格</td>
        ////            <td width='40'>单位</td>
        ////            <td>数量</td>
        ////<td>重量</td>
        ////            <td>单价</td>
        ////            <td>金额</td>
        ////            <td>备注</td>
        ////        </tr>");
        ////                var parms = new Dictionary<string, object>();
        ////                parms.Clear();

        ////                parms.Add("@companyId", Company.id);
        ////                parms.Add("@KeyId", keyid);
        ////                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
        ////                parms.Add("@end", MonthEnd(ServiceDateTime));

        ////                var details = SqlService.ExecuteProcedureCommand("proc_PrintPurchase", parms).Tables[0];

        ////                var salesDetails = new StringBuilder();

        ////                for (int i = 0; i < details.Rows.Count; i++)
        ////                {
        ////                    salesDetails.AppendFormat(@"<tr align='center'>
        ////                                        <td>{0}</td>
        ////                                        <td>{1}</td>
        ////                                        <td>{2}</td>
        ////                                        <td>{3}</td>
        ////                                        <td>{4}</td>
        ////                                        <td></td>
        ////                                        <td>{5}</td>
        ////                                        <td>{6}</td>
        ////                                        <td>{7}</td>
        ////                                    </tr>", i + 1,
        ////                                          details.Rows[i]["FName"],
        ////                                          details.Rows[i]["FSpec"],
        ////                                          details.Rows[i]["FUnit"],

        ////                                          Convert.ToDecimal(details.Rows[i]["FQty"]).ToString("f2").Equals("0.00") ? "" : //
        ////                                          Convert.ToDecimal(details.Rows[i]["FQty"].ToString()//
        ////                                          .Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FQty"]) ://
        ////                                          details.Rows[i]["FQty"],

        ////                                          //Convert.ToDecimal(details.Rows[i]["FPrice"]).ToString("f2").Equals("0.00") ? "" : //
        ////                                          //Convert.ToDecimal(details.Rows[i]["FPrice"].ToString()//
        ////                                          //.Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FPrice"]) ://
        ////                                          //details.Rows[i]["FPrice"],

        ////                                          "",

        ////                                          //Convert.ToDecimal(details.Rows[i]["FAmount"]).ToString("f2").Equals("0.00") ? "" : //
        ////                                          //Convert.ToDecimal(details.Rows[i]["FAmount"].ToString()//
        ////                                          //.Split('.')[1]) == 0 ? Convert.ToInt32(details.Rows[i]["FAmount"]) ://
        ////                                          //details.Rows[i]["FAmount"],

        ////                                          "",

        ////                                          details.Rows[i]["FMemo"]
        ////                                          );
        ////                }

        ////                int printCount = 7 - details.Rows.Count;

        ////                for (int i = 0; i < printCount; i++)
        ////                {
        ////                    salesDetails.AppendFormat(@"<tr align='center'>
        ////                                        <td  height='30px'></td>
        ////                                        <td></td>
        ////                                        <td></td>
        ////                                        <td></td>
        ////                                        <td></td>
        ////                                        <td></td>
        ////                                        <td></td>
        ////                                        <td></td>
        ////                                        <td></td></tr>");
        ////                }


        ////                print.Append(salesDetails);

        ////                var bottomDetails = new StringBuilder();

        ////                bottomDetails.AppendFormat(@"<tr align='center'>
        ////                                        <td   height='30px'>合计</td>
        ////                                        <td></td>
        ////                                        <td></td><td></td>
        ////                                        <td>{0}</td>
        ////                                        <td></td>
        ////                                        <td>{1}</td>
        ////                                        <td></td></tr></table>", //
        ////                                                               "",//details.Compute("SUM(FQty)", "true"),//
        ////                                                               "",//details.Compute("SUM(FAmount)", "true"),//
        ////                                                               "",//
        ////                                                               "",//
        ////                                                               "");

        ////                print.Append(bottomDetails);

        ////                var bottom = string.Format(@"<br/><table style='font-size:13px;' align='center' width='780' cellspacing='0' border='0'>
        ////        <tr>
        ////            <td height='25' width='780'>送货单位及经手人：{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;入库单位及经手人：{1} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>            
        ////        </tr>
        ////        <tr>
        ////            <td height='21' style='font-size:12px;'>第一联为白色：销方财务;第二联为红色：销方仓库；第三联为蓝色：客户留存；第四联为绿色：客户留存；第五联为黄色：销方门卫留存</td>            
        ////        </tr>
        ////    </table>
        ////</div>
        ////</body>
        ////</html>", "","");

        ////                print.Append(bottom);
        ////            }

        ////            _response = print.ToString();
        ////        }
        #endregion

        #region 销售退货
        private void AjaxPrintSalesReturn(HttpContext context)
        {
            var keyid = context.Request["keyid"];
            var stock = StockInService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
            var print = new StringBuilder();

            if (stock != null)
            {
                print.AppendFormat(@"<!DOCTYPE html>
<html lang='en'><head><title>打印</title></head>
<body><div style='margin:2px auto;width:750px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='6'>上海浦江特种气体有限公司退货单</font></b></center>
<table style='font-size:13px;' border='0' cellspacing='0' align='center' width='780'>
<tr height='22'><td width='100' >客户代码：</td><td colspan='3' style='width: 220px'>{0}</td><td width='100' align='right'>发货单号：</td>
<td>{1}</td></tr>
<tr height='22'><td width='100' >客户名称：</td><td colspan='3' style='width: 220px'>{2}</td><td width='100' align='right'>日期：</td>
<td>{3}</td></tr>
</table>", stock.FCode, stock.KeyId, stock.FName, Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));
                print.Append(@"<table  border='1' cellspacing='0' width='780'  align='center' style='font-size:13px;'>
        <tr align='center'>
            <td width='60' height='30px'>订单编号</td>
            <td width='60'>商品名称</td>
            <td width='50'>规格</td>
            <td width='50'>单位</td>
			<td width='60'>退货数量</td>
            <td width='60'>实送重量</td>
			<td width='50'>单价</td>
			<td width='50'>金额</td>
			<td width='60'>回收空瓶</td>
            <td width='70'>备注</td>
        </tr>");

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@companyId", Company.id);
                parms.Add("@KeyId", keyid);
                parms.Add("@begin", MonthFirstDay(ServiceDateTime));
                parms.Add("@end", MonthEnd(ServiceDateTime));

                var details = SqlService.ExecuteProcedureCommand("proc_PrintSalesReturn", parms).Tables[0];

                var salesDetails = new StringBuilder();

                for (int i = 0; i < details.Rows.Count; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'>{0}</td>
                                        <td align='left'>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        <td>{8}</td>
                                        <td>{9}</td>
										</tr>", //
                                          details.Rows[i]["FNum"],//订单号码//i + 1,//<td>{10}</td>
                                          details.Rows[i]["FName"],
                                          details.Rows[i]["FSpec"],
                                          details.Rows[i]["FUnit"],
                                          details.Rows[i]["FQty1"].ToString().Equals("0.00") ? "" : details.Rows[i]["FQty1"],
                                          details.Rows[i]["FQty2"].ToString().Equals("0.00") ? "" : details.Rows[i]["FQty2"],
                                          //"",//欠瓶数
                                          "",//单价//Convert.ToDecimal(details.Rows[i]["FPrice"]) == 0 ? "" : details.Rows[i]["FPrice"],
                                          "",//金额//Convert.ToDecimal(details.Rows[i]["FAmount"]) == 0 ? "" : details.Rows[i]["FAmount"],
                                          "",//回收空瓶//Convert.ToDecimal(details.Rows[i]["AddUpBottleQty"]) == 0 ? "" : details.Rows[i]["AddUpBottleQty"],
                                          details.Rows[i]["FMemo"]
                                          );
                }

                int printCount = 9 - details.Rows.Count;

                for (int i = 0; i < printCount; i++)
                {
                    salesDetails.AppendFormat(@"<tr align='center'>
                                        <td  height='30px'></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
										</tr>");
                }


                print.Append(salesDetails);

                var bottomDetails = new StringBuilder();

                bottomDetails.AppendFormat(@"<tr align='center'>
                                        <td   height='30px'>合计</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
										<td></td>
                                        <td></td></tr></table>", //
                                                               details.Compute("SUM(FQty1)", "true"),//
                                                               details.Compute("SUM(FQty2)", "true"),//
                                                               "",//
                                                               "",//
                                                               "");

                print.Append(bottomDetails);

                var bottom = string.Format(@"<br/><table style='font-size:13px;' align='center' width='780' cellspacing='0' border='0'>
        <tr>
            <td height='25' width='780'>司机：{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;押运员：{1} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;车牌号：{2} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;送货人：{3}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;收货人：</td>            
        </tr>
        <tr>
            <td height='21' style='font-size:12px;'>第一联为白色：销方财务;第二联为红色：销方仓库；第三联为蓝色：客户留存；第四联为绿色：客户留存；第五联为黄色：销方门卫留存</td>            
        </tr>
    </table>
</div>
</body>
</html>", stock.FDriver, stock.FSupercargo, stock.FVehicleNum, "");

                print.Append(bottom);
            }

            _response = print.ToString();
        }
        #endregion

        #region 财务日期
        protected string MonthFirstDay(DateTime dateTime)
        {
            if (Company != null)
            {
                if (Company.FMonthly.Contains("是"))
                {
                    var parms = new Dictionary<string, object>();
                    parms.Clear();
                    parms.Add("@Fdate", Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd"));
                    var date = SqlService.ExecuteProcedureCommand("proc_MonthDay", parms);
                    return Convert.ToDateTime(date.Tables[0].Rows[0][0]).ToString("yyyy-MM-dd");
                }
                else
                {
                    return Convert.ToDateTime(Convert.ToDateTime(dateTime).AddMonths(-1).ToString("yyyy-MM") //
                        + "-" + Company.FMonthlyDay).AddDays(1).ToString("yyyy-MM-dd");
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        protected string MonthEnd(DateTime dateTime)
        {

            if (Company != null)
            {
                if (Company.FMonthly.Contains("是"))
                {
                    var parms = new Dictionary<string, object>();
                    parms.Clear();
                    parms.Add("@Fdate", Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd"));
                    var date = SqlService.ExecuteProcedureCommand("proc_MonthDay", parms);
                    return Convert.ToDateTime(date.Tables[1].Rows[0][0]).ToString("yyyy-MM-dd");
                }
                else
                {
                    return Convert.ToDateTime(Convert.ToDateTime(dateTime).ToString("yyyy-MM")//
                        + "-" + Company.FMonthlyDay).ToString("yyyy-MM-dd");
                }
            }
            return null;
        }

        #endregion

        /// <summary>
        ///     数据服务
        /// </summary>
        private SqlService _sqlSever;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SqlService SqlService
        {
            get { return _sqlSever ?? (_sqlSever = new SqlService()); }
            set { _sqlSever = value; }
        }

        /// <summary>
        ///     服务器时间
        /// </summary>
        protected DateTime ServiceDateTime
        {
            get
            {
                return Convert.ToDateTime(SqlService.ExecuteProcedureCommand("proc_ServiceDateTime").Tables[0].Rows[0][0]);
            }
        }
        /// <summary>
        ///     企业
        /// </summary>
        protected base_company Company
        {
            get
            {
                var companydId = -1;
                var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
                if (formsPrincipal != null)
                {
                    companydId = formsPrincipal.UserInfo.AccountComId;
                }
                return new CompanyService().FirstOrDefault(p => p.id == companydId);
            }
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