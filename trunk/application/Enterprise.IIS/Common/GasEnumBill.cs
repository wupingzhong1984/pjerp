using Enterprise.Framework.Enum;

namespace Enterprise.IIS.Common
{
    public enum GasEnumBill
    {
        /// <summary>
        ///     发货单
        /// </summary>
        [EnumDescription("发货单")]
        Sales=1,

        /// <summary>
        ///     销售退货单
        /// </summary>
        [EnumDescription("销售退货单")]
        SalesReturn=2,
        
        /// <summary>
        ///    物资调拨单 
        /// </summary>
        [EnumDescription("物资调拨单")]
        Allot = 3,

        /// <summary>
        ///     采购单
        /// </summary>
        [EnumDescription("采购单")]
        Purchase=4,

        /// <summary>
        ///     采购退货单
        /// </summary>
        [EnumDescription("采购退货单")]
        PurchaseReturn=5,

        /// <summary>
        ///     生产单
        /// </summary>
        [EnumDescription("生产单")]
        Production=6,

        /// <summary>
        ///     租赁单
        /// </summary>
        [EnumDescription("租赁单")]
        Lease=7,

        /// <summary>
        ///     租赁还瓶单
        /// </summary>
        [EnumDescription("退租单")]
        LeaseReturn=8,

        /// <summary>
        ///     盘亏单
        /// </summary>
        [EnumDescription("盘亏单")]
        Lossess=9,

        /// <summary>
        ///     盘盈单
        /// </summary>
        [EnumDescription("盘盈单")]
        Profit = 10,

        /// <summary>
        ///     付款单
        /// </summary>
        [EnumDescription("付款单")]
        Payment = 11,

        /// <summary>
        ///     收款单
        /// </summary>
        [EnumDescription("收款单")]
        Receipt = 12,

        /// <summary>
        ///     空瓶回收单
        /// </summary>
        [EnumDescription("空瓶回收单")]
        BottleReturn=13,

        /// <summary>
        ///     送货提成单
        /// </summary>
        [EnumDescription("送货提成单")]
        DispatchCommission = 14,

        /// <summary>
        ///    物资调拨单 
        /// </summary>
        [EnumDescription("物资调拨单")]
        AllotCheck = 15,

        /// <summary>
        ///     空瓶出库单
        /// </summary>
        [EnumDescription("空瓶出库单")]
        BottleTo = 16,

        /// <summary>
        ///     核销收款单
        /// </summary>
        [EnumDescription("核销收款单")]
        VerificationReceipt = 17,

        /// <summary>
        ///     核销收款单
        /// </summary>
        [EnumDescription("核销付款单")]
        FOrderChecked = 18,

        /// <summary>
        ///     现金费用单
        /// </summary>
        [EnumDescription("现金费用单")]
        InternalExpense = 19,

        /// <summary>
        ///     出仓单
        /// </summary>
        [EnumDescription("出仓单")]
        ProductOut=20,

        /// <summary>
        ///     入仓单
        /// </summary>
        [EnumDescription("入仓单")]
        ProductIn = 21,

        /// <summary>
        ///     现金费用单
        /// </summary>
        [EnumDescription("一般费用单")]
        GeneralExpense = 22,

        /// <summary>
        ///     调整客户代码期初占用钢瓶单
        /// </summary>
        [EnumDescription("调客户代码占用钢瓶单")]
        ChangeBottle=23,

        /// <summary>
        ///     调整客户代码期初应收应付单
        /// </summary>
        [EnumDescription("调整客户代码应收应付单")]
        ChangeAccount = 24,

        /// <summary>
        ///     银行存取单据
        /// </summary>
        [EnumDescription("银行存取单据")]
        TransferAccounts = 25,

        /// <summary>
        ///     液体进销计划
        /// </summary>
        [EnumDescription("液体进销计划")]
        LiquidPlan = 26,

        /// <summary>
        ///     液体采购单
        /// </summary>
        [EnumDescription("液体采购单")]
        LiquidPurchase = 27,

        /// <summary>
        ///     液体采购单
        /// </summary>
        [EnumDescription("液体发货单")]
        LiquidSales = 28,

        /// <summary>
        ///     液体采购订单
        /// </summary>
        [EnumDescription("液体采购订单")]
        LiquidPurchasePlan = 29,

        /// <summary>
        ///     液体销售订单
        /// </summary>
        [EnumDescription("液体销售订单")]
        LiquidSalesPlan = 30,

        /// <summary>
        ///     钢瓶检测
        /// </summary>
        [EnumDescription("钢瓶检测")]
        BottleDetection = 31,

        /// <summary>
        ///    销售订单
        /// </summary>
        [EnumDescription("销售订单")]
        PassCard = 32,

        /// <summary>
        ///     其它收入单
        /// </summary>
        [EnumDescription("其它收入单")]
        FinanceRevenue = 40,

        /// <summary>
        ///     销售开票
        /// </summary>
        [EnumDescription("销售发货")]
        InvoiceIV = 41,

        /// <summary>
        ///     采购收货
        /// </summary>
        [EnumDescription("采购收货")]
        InvoiceIR = 42,

        /// <summary>
        ///     收发票43
        /// </summary>
        [EnumDescription("收发票")]
        ReceiptRI = 43,

        /// <summary>
        ///     开发票44
        /// </summary>
        [EnumDescription("开发票")]
        ReceiptRP = 44,

        /// <summary>
        ///     采购单请示单
        /// </summary>
        [EnumDescription("采购单请示单")]
        PurchaseApp = 45,

        /// <summary>
        ///     车辆维修
        /// </summary>
        [EnumDescription("车辆维修")]
        VehicleRepare=46,

        /// <summary>
        ///     加油登记
        /// </summary>
        [EnumDescription("加油登记")]
        VehicleFuel=47,

        /// <summary>
        ///     轮胎登记
        /// </summary>
        [EnumDescription("轮胎登记")]
        VehicleTire=48,

        /// <summary>
        ///     氢气进销计划
        /// </summary>
        [EnumDescription("氢气进销计划")]
        TubePlan = 49,

        /// <summary>
        ///     氢气采购单
        /// </summary>
        [EnumDescription("氢气采购单")]
        TubePurchase = 50,

        /// <summary>
        ///     氢气发货单
        /// </summary>
        [EnumDescription("氢气发货单")]
        TubeSales = 51,

        /// <summary>
        ///     调度日志
        /// </summary>
        [EnumDescription("调度日志")]
        Dispatch =52,

        /// <summary>
        ///     调拨申请单
        /// </summary>
        [EnumDescription("调拨申请单")]
        AllotPlan = 53,

        /// <summary>
        ///     气体分析
        /// </summary>
        [EnumDescription("气体分析")]
        Analyse = 54,

        /// <summary>
        ///     回收气瓶处理单
        /// </summary>
        [EnumDescription("回收气瓶处理单")]
        BottleReturnExpenses = 55,

        /// <summary>
        ///     生产领料
        /// </summary>
        [EnumDescription("生产领料")]
        picking = 56,

        /// <summary>
        ///     生产产量
        /// </summary>
        [EnumDescription("生产产量")]
        output = 57,
    }
}