using Enterprise.Framework.Enum;

namespace Enterprise.IIS.Common
{
    /// <summary>
    ///     单据状态
    /// </summary>
    public enum GasEnumBillStauts
    {
        /// <summary>
        ///     预单
        /// </summary>
        [EnumDescription("预单")]
        Pre = 9,

        /// <summary>
        ///     已制单
        /// </summary>
        [EnumDescription("已制单")]
        Add=10,

        /// <summary>
        ///     已开票
        /// </summary>
        [EnumDescription("已开票")]
        Print=20,

        /// <summary>
        ///     已审核
        /// </summary>
        [EnumDescription("已审核")]
        Audit=30,

        /// <summary>
        ///     反审核
        /// </summary>
        [EnumDescription("反审核")]
        UnAudit=40,

        /// <summary>
        ///     已配送
        /// </summary>
        [EnumDescription("已配送")]
        Delivery=50,

        /// <summary>
        ///     客户已确认
        /// </summary>
        [EnumDescription("客户已确认")]
        Confirm=60,

        /// <summary>
        ///     已作费
        /// </summary>
        [EnumDescription("已作废")]
        Voided = 0,

        /// <summary>
        ///     已完成
        /// </summary>
        [EnumDescription("已完成")]
        End=100,

        [EnumDescription("预派车")]
        wating=0,

        [EnumDescription("正式派车")]
        suport=1

    }

    /// <summary>
    ///     审核状态
    /// </summary>
    public enum GasEnumAuditFlag
    {
        /// <summary>
        ///    未审核
        /// </summary>
        [EnumDescription("未审核")]
        Unaudited =0,

        /// <summary>
        ///    审核通过
        /// </summary>
        [EnumDescription("审核通过")]
        Yes = 1,

        /// <summary>
        ///    审核未同过
        /// </summary>
        [EnumDescription("审核不通过")]
        No = 2


    }

    //public enum GasAuditStatus



}