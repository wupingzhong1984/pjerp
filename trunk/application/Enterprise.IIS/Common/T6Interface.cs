using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using Enterprise.DataAccess.SQLServer;
using Enterprise.IIS.U8.ServiceReference;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;

namespace Enterprise.IIS.Common
{
    public class T6Interface
    {

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
        private SqlService _service;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SqlService SqlService
        {
            get { return _service ?? (_service = new SqlService()); }
            set { _service = value; }
        }

        #region 数据接口

        /// <summary>
        ///     XML转换DataSet
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataSet GetDataToXml(string data)
        {
            try
            {
                var ds = new DataSet();

                using (var xml = new StringReader(data))
                {
                    ds.ReadXml(xml, XmlReadMode.Auto);
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     数据对接接口
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataSet GetRequestData(string xml)
        {
            string url = T6Account.Url;
            var webRequest = (HttpWebRequest) WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            var document = new System.Xml.XmlDocument();
            document.LoadXml(xml);
            var requestStream = new StreamWriter(webRequest.GetRequestStream());
            requestStream.Write(document.OuterXml);
            requestStream.Close();

            // ReSharper disable once AssignNullToNotNullAttribute
            var responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
            string responseData = responseReader.ReadToEnd();
            document.LoadXml(responseData);

            return GetDataToXml(document.OuterXml);
        }

        #endregion

        /// <summary>
        ///     同步发货单
        /// </summary>
        /// <param name="keyId">发货单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitSales(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_Sales", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml =
                    string.Format(@"<?xml version='1.0' encoding='utf-8'?><vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'>
	<vouch type ='05' bdel='0'><head  单据号='{3}' 业务类型='普通销售' 单据日期='{4}' 客户编码='{5}' 销售类型编码='{6}'
			 部门编码='{7}' 业务员编码='{8}'  备注='{9}' 税率='13' 制单人='{10}'
		币种='{11}' 汇率='{12}'  最后修改时间='' 外部唯一id='' />
		<body>", T6Account.Sender, //
                        //DateTime.Now.ToString("yyyy-MM-dd"),
                        Convert.ToDateTime(data.Rows[0]["FDate"]).ToString("yyyy-MM-dd"),
                        //DateTime.Now.Year, //T6Account.Dynamicdate,//数据标识
                        Convert.ToDateTime(data.Rows[0]["FDate"]).Year,
                        keyId, //单据号
                               //DateTime.Now.ToString("yyyy-MM-dd"),
                        Convert.ToDateTime(data.Rows[0]["FDate"]).ToString("yyyy-MM-dd"),//data.Rows[0]["FDate"], //日期
                        data.Rows[0]["FCode"], //客户编码
                        data.Rows[0]["FT6SaleTypeNum"], //销售类型编码
                        data.Rows[0]["FIOrgNum"], //部门编码
                        data.Rows[0]["FSalesmanNum"], //业务员编码
                        data.Rows[0]["FMemo"], //备注
                        data.Rows[0]["FShipperNum"], //制单人
                        data.Rows[0]["FT6Currency"], //币种
                        data.Rows[0]["FT6ExchangeRate"] //汇率
                    );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_SalesList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    if (Convert.ToDecimal(list.Rows[i]["FPrice"]) == 0)
                    {
                        result = "发货明细中有价格为零的，不能上传T6";
                        break;
                    }
                    
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 仓库编码='{1}' 存货编码='{2}' 数量='{3}' 税率='13' 含税单价='{4}'  无税单价='{5}' 无税金额='{6}'  税额='{7}' 价税合计='{8}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //序号
                            list.Rows[i]["FT6WarehouseNum"], //仓库编码
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"], //数量
                            Convert.ToDecimal(list.Rows[i]["FPrice"]), //含税单价
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) / 1.17M,
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]) / 1.17M,
                            //6
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]) / 1.17M *
                            0.17M,
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]));
                }

                if (!string.IsNullOrEmpty(result)&&result.Length>1)
                {
                    return result;
                }

                xml += @"</body></vouch></vouchs>";

                //-------------------------------------------
                //var logservice = new LogsService();
                //var log = new t_logs
                //{
                //    account_name = keyId,
                //    action_desc = xml,
                //    action_on = DateTime.Now
                //};
                //logservice.Add(log);
                //-------------------------------------------

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();

                        //单据已经存在!
                        if (result.Contains("单据已经存在"))
                        {
                            StockOutService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockOut
                            {
                                FT6Status = "已同步", //
                            });
                        }
                    }
                    else
                    {
                        StockOutService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockOut
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     同步销售退货单
        /// </summary>
        /// <param name="keyId">发货单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitSalesReturn(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_SalesReturn", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml =
                    string.Format(@"<?xmlhydrogen version='1.0' encoding='utf-8'?><vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'>
	<vouch type ='05' bdel='0'><head  单据号='{3}' 业务类型='普通销售' 单据日期='{4}' 客户编码='{5}' 销售类型编码='{6}'
			 部门编码='{7}' 业务员编码='{8}'  备注='{9}' 税率='13' 制单人='{10}'
		币种='{11}' 汇率='{12}'  最后修改时间='' 外部唯一id='' />
		<body>", T6Account.Sender, //
                        data.Rows[0]["FDate"], DateTime.Now.Year, //数据标识
                        keyId, //单据号
                        data.Rows[0]["FDate"], //日期
                        data.Rows[0]["FCode"], //客户编码
                        data.Rows[0]["FT6SaleTypeNum"], //销售类型编码
                        data.Rows[0]["FIOrgNum"], //部门编码
                        data.Rows[0]["FSalesmanNum"], //业务员编码
                        data.Rows[0]["FMemo"], //备注
                        data.Rows[0]["FShipperNum"], //制单人
                        data.Rows[0]["FT6Currency"], //币种
                        data.Rows[0]["FT6ExchangeRate"] //汇率
                    );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_SalesReturnList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 仓库编码='{1}' 存货编码='{2}' 数量='{3}' 税率='13' 含税单价='{4}'  无税单价='{5}' 无税金额='{6}'  税额='{7}' 价税合计='{8}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //mxb[i].cwhcode, mxb[i].cpid, mxb[i].sjsl,
                            list.Rows[i]["FT6WarehouseNum"], //仓库编码
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"], //数量
                            Convert.ToDecimal(list.Rows[i]["FPrice"]), //含税单价
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) / 1.17M,
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]) / 1.17M,
                            //6
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]) / 1.17M *
                            0.17M,
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]));
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockInService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockIn
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     同步采购单
        /// </summary>
        /// <param name="keyId">发货单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitPurchase(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_Purchase", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml =
                    string.Format(@"<?xmlhydrogen version='1.0' encoding='utf-8'?><vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'>
	<vouch type ='01' bdel='0'><head  单据号='{3}' 业务类型='普通采购' 单据日期='{4}' 供应商编码='{5}' 采购类型编码='{6}'
			 部门编码='{7}' 业务员编码='{8}'  备注='{9}' 税率='13' 制单人='{10}'
		币种='{11}' 汇率='{12}'  最后修改时间='' 外部唯一id='' 仓库编码='{13}'/>
		<body>", T6Account.Sender, //
                        data.Rows[0]["FDate"],
                        Convert.ToDateTime(data.Rows[0]["FDate"]).Year,
                        //DateTime.Now.Year, //T6Account.Dynamicdate,//数据标识
                        keyId, //单据号
                        data.Rows[0]["FDate"], //日期
                        data.Rows[0]["FCode"], //客户编码
                        data.Rows[0]["FT6PurchaseTypeNum"], //销售类型编码
                        data.Rows[0]["FIOrgNum"], //部门编码
                        data.Rows[0]["FSalesmanNum"], //业务员编码
                        data.Rows[0]["FMemo"], //备注
                        data.Rows[0]["FShipperNum"], //制单人
                        data.Rows[0]["FT6Currency"], //币种
                        data.Rows[0]["FT6ExchangeRate"], //汇率
                        data.Rows[0]["FT6WarehouseNum"]
                    );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_PurchaseList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 仓库编码='{1}' 存货编码='{2}' 数量='{3}' 税率='13' 含税单价='{4}'  无税单价='{5}' 无税金额='{6}'  税额='{7}' 价税合计='{8}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //mxb[i].cwhcode, mxb[i].cpid, mxb[i].sjsl,
                            list.Rows[i]["FT6WarehouseNum"], //仓库编码
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"], //数量
                            Convert.ToDecimal(list.Rows[i]["FPrice"]), //含税单价
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) / 1.17M,
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]) / 1.17M,
                            //6
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]) / 1.17M *
                            0.17M,
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]));
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockInService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockIn
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     同步采购退货单
        /// </summary>
        /// <param name="keyId">发货单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitPurchaseReturn(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_PurchaseReturn", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml =
                    string.Format(@"<?xmlhydrogen version='1.0' encoding='utf-8'?><vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'>
	<vouch type ='01' bdel='0'><head  单据号='{3}' 业务类型='普通采购' 单据日期='{4}' 供应商编码='{5}' 采购类型编码='{6}'
			 部门编码='{7}' 业务员编码='{8}'  备注='{9}' 税率='13' 制单人='{10}'
		币种='{11}' 汇率='{12}'  最后修改时间='' 外部唯一id='' 仓库编码='{13}'/>
		<body>", T6Account.Sender, //
                        data.Rows[0]["FDate"], DateTime.Now.Year, //数据标识
                        keyId, //单据号
                        data.Rows[0]["FDate"], //日期
                        data.Rows[0]["FCode"], //客户编码
                        data.Rows[0]["FT6PurchaseTypeNum"], //销售类型编码
                        data.Rows[0]["FIOrgNum"], //部门编码
                        data.Rows[0]["FSalesmanNum"], //业务员编码
                        data.Rows[0]["FMemo"], //备注
                        data.Rows[0]["FShipperNum"], //制单人
                        data.Rows[0]["FT6Currency"], //币种
                        data.Rows[0]["FT6ExchangeRate"], //汇率
                        data.Rows[0]["FT6WarehouseNum"]
                    );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_PurchaseReturnList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 仓库编码='{1}' 存货编码='{2}' 数量='{3}' 税率='13' 含税单价='{4}'  无税单价='{5}' 无税金额='{6}'  税额='{7}' 价税合计='{8}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //mxb[i].cwhcode, mxb[i].cpid, mxb[i].sjsl,
                            data.Rows[0]["FT6WarehouseNum"], //list.Rows[i]["FT6WarehouseNum"],//仓库编码
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"], //数量
                            Convert.ToDecimal(list.Rows[i]["FPrice"]), //含税单价
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) / 1.17M,
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]) / 1.17M,
                            //6
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]) / 1.17M *
                            0.17M,
                            Convert.ToDecimal(list.Rows[i]["FPrice"]) * Convert.ToDecimal(list.Rows[i]["FQty"]));
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockOutService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockOut
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     同步出仓单
        /// </summary>
        /// <param name="keyId">发货单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitStockOut(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_StockOut", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
<vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'><vouch type ='09' bdel='0' >
<head  单据号='{3}' 业务类型='其他出库' 单据日期='{4}' 仓库编码='{5}' 
收发类别编码='{6}' 部门编码='{7}' 业务员编码='{8}'  备注='{10}' 最后修改时间='' 外部唯一id=''  制单人='{9}' /><body>",
                    T6Account.Sender, //
                    data.Rows[0]["FDate"], //
                                           //DateTime.Now.Year, //数据标识
                    Convert.ToDateTime(data.Rows[0]["FDate"]).Year,
                    keyId, //单据号
                    data.Rows[0]["FDate"], //日期
                    data.Rows[0]["FT6WarehouseNum"],
                    data.Rows[0]["FT6ReceiveSendTypeNum"],
                    data.Rows[0]["FIOrgNum"], //部门编码
                    data.Rows[0]["FSalesmanNum"], //业务员编码
                    data.Rows[0]["FMemo"], //备注
                    data.Rows[0]["FShipperNum"] //制单人
                );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_StockOutList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 存货编码='{1}' 数量='{2}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"] //数量
                        );
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockOutService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockOut
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     同步进仓单
        /// </summary>
        /// <param name="keyId">发货单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitStockIn(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_StockIn", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
<vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'><vouch type ='08' bdel='0' >
<head  单据号='{3}' 业务类型='其他入库' 单据日期='{4}' 仓库编码='{5}' 
收发类别编码='{6}' 部门编码='{7}' 业务员编码='{8}'  备注='{10}' 最后修改时间='' 外部唯一id=''  制单人='{9}' /><body>",
                    T6Account.Sender, //
                    data.Rows[0]["FDate"], //
                    //DateTime.Now.Year, //数据标识
                    Convert.ToDateTime(data.Rows[0]["FDate"]).Year,
                    keyId, //单据号
                    data.Rows[0]["FDate"], //日期
                    data.Rows[0]["FT6WarehouseNum"],
                    data.Rows[0]["FT6ReceiveSendTypeNum"], //收发类型
                    data.Rows[0]["FIOrgNum"], //部门编码
                    data.Rows[0]["FSalesmanNum"], //业务员编码
                    data.Rows[0]["FMemo"], //备注
                    data.Rows[0]["FShipperNum"] //制单人
                );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_StockInList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 存货编码='{1}' 数量='{2}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"] //数量
                        );
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockInService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockIn
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     同步调拨单
        /// </summary>
        /// <param name="keyId">同步调拨单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitAllot(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_Allot", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
<vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'><vouch type ='12' bdel='0' >
<head 单据号='{3}' 业务类型='调拨单' 单据日期='{4}' 转出仓库编码='{5}' 转入仓库编码='{6}' 入库类别编码='0113' 
出库类别编码='0212' 转入部门编码='{7}' 转出部门编码='{8}' 业务员编码='{9}' 备注='{10}' 最后修改时间='' 外部唯一id=''  制单人='{11}' /><body>",
                    T6Account.Sender, //
                    Convert.ToDateTime(data.Rows[0]["FDate"]).ToString("yyyy-MM-dd"), //
                    //DateTime.Now.Year, //数据标识
                    Convert.ToDateTime(data.Rows[0]["FDate"]).Year,
                    keyId, //单据号
                    Convert.ToDateTime(data.Rows[0]["FDate"]).ToString("yyyy-MM-dd"), //T6Account.Dynamicdate,//数据标识
                    data.Rows[0]["FT6HOut"],//6
                    data.Rows[0]["FT6HIn"],//5
                    data.Rows[0]["FOrgIn"], //转入部门编码
                    data.Rows[0]["FOrgOut"], //转出部门编码8
                    data.Rows[0]["FShipperNum"], //业务员编码
                    data.Rows[0]["FMemo"], //备注
                    data.Rows[0]["FShipperNum"] //制单人
                );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_AllotList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 存货编码='{1}' 数量='{2}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"] //数量
                        );
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockOutService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockOut
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     成品入库
        /// </summary>
        /// <param name="keyId">成品入库</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitProductStockIn(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_ProductStockIn", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
<vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'>
	<vouch type ='10'  bdel='0'>
<head 单据号='{3}' 业务类型='成品入库' 单据日期='{4}' 仓库编码='{5}' 
收发类别编码='{6}' 部门编码='{7}' 业务员编码='{8}' 备注='{9}' 最后修改时间='' 外部唯一id=''  制单人='{8}' /><body>",
                    T6Account.Sender, //
                                      data.Rows[0]["FDate"], //
                    Convert.ToDateTime(data.Rows[0]["FDate"]).Year,
                    //DateTime.Now.Year, //数据标识
                    keyId, //单据号
                    data.Rows[0]["FDate"], //日期
                    data.Rows[0]["FT6WarehouseNum"],
                    data.Rows[0]["FT6ReceiveSendTypeNum"], //收发类型
                    data.Rows[0]["FIOrgNum"], //部门编码
                    data.Rows[0]["FSalesmanNum"], //业务员编码
                    data.Rows[0]["FMemo"] //备注
                );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_ProductStockInList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 仓库编码='{1}' 存货编码='{2}' 数量='{3}' 单价='{4}'  金额='{5}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //
                            list.Rows[i]["FT6WarehouseNum"], //仓库编码
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"], //数量
                            Convert.ToDecimal(list.Rows[i]["FPrice"]), //含税单价
                            Convert.ToDecimal(list.Rows[i]["FAmount"]) //金额
                        );
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockInService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockIn
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     领料单
        /// </summary>
        /// <param name="keyId">领料单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitMaterialStockOut(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_MaterialStockOut", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
<vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'>
	<vouch type ='11' bdel='0' >
<head  单据号='{3}' 业务类型='领料' 单据日期='{4}' 仓库编码='{5}' 
收发类别编码='{6}' 部门编码='{7}' 业务员编码='{8}'  备注='{9}' 
最后修改时间='' 外部唯一id=''  制单人='{8}' /><body>",
                    T6Account.Sender, //
                    data.Rows[0]["FDate"], //
                                           //DateTime.Now.Year, //数据标识
                    Convert.ToDateTime(data.Rows[0]["FDate"]).Year,
                    keyId, //单据号
                    data.Rows[0]["FDate"], //日期
                    data.Rows[0]["FT6WarehouseNum"],
                    data.Rows[0]["FT6ReceiveSendTypeNum"], //收发类型
                    data.Rows[0]["FIOrgNum"], //部门编码
                    data.Rows[0]["FSalesmanNum"], //业务员编码
                    data.Rows[0]["FMemo"] //备注
                );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_MaterialStockOutList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 仓库编码='{1}' 存货编码='{2}' 数量='{3}' 单价='{4}' 金额='{5}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //序号
                            list.Rows[i]["FT6WarehouseNum"], //仓库编码
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"], //数量
                            Convert.ToDecimal(list.Rows[i]["FPrice"]), //含税单价
                            Convert.ToDecimal(list.Rows[i]["FAmount"]));
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockOutService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockOut
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     盘盈单
        /// </summary>
        /// <param name="keyId">盘盈单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitProfit(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_Lossess", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
<vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'>
	<vouch type ='18'  bdel='0'>
		<head  单据号='{3}' 账面日期='{4}' 盘点日期='{5}' 仓库编码='{6}' 入库类别编码='{7}' 出库类别编码='{8}'
			 部门编码='{9}' 业务员编码='{10}' 盘点类型='普通仓库盘点'  备注='{11}' 制单人='{10}'
		 最后修改时间='' 外部唯一id='' />
		<body>",
                    T6Account.Sender, //
                    data.Rows[0]["FDate"], //
                                           //DateTime.Now.Year, //数据标识
                    Convert.ToDateTime(data.Rows[0]["FDate"]).Year,
                    keyId, //单据号
                    data.Rows[0]["FDate"], //日期
                    data.Rows[0]["FDate"], //日期
                    data.Rows[0]["FT6WarehouseNum"],
                    data.Rows[0]["FT6PurchaseTypeNum"], //入库类别编码
                    data.Rows[0]["FT6SaleTypeNum"], //出库类别编码
                    data.Rows[0]["FIOrgNum"], //部门编码
                    data.Rows[0]["FSalesmanNum"], //业务员编码
                    data.Rows[0]["FMemo"] //备注
                );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_LossessList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 存货编码='{1}' 账面数量='{2}' 盘点数量='{3}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"], //账面数量
                            list.Rows[i]["FQty"] //盘点数量
                        );
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockOutService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockOut
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     盘亏单
        /// </summary>
        /// <param name="keyId">盘亏单</param>
        /// <param name="companyId">公司Id</param>
        public string SubmitLossess(string keyId, int companyId)
        {
            string result = string.Empty;

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", companyId);
            parms.Add("@KeyId", keyId);

            var data = SqlService.ExecuteProcedureCommand("u8_Lossess", parms).Tables[0];

            if (data != null && data.Rows.Count > 0)
            {
                string xml = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
<vouchs cacc_id='{0}' dregdate='{1}' iyear='{2}'>
	<vouch type ='18'  bdel='0'>
		<head  单据号='{3}' 账面日期='{4}' 盘点日期='{5}' 仓库编码='{6}' 入库类别编码='{7}' 出库类别编码='{8}'
			 部门编码='{9}' 业务员编码='{10}' 盘点类型='普通仓库盘点'  备注='{11}' 制单人='{10}'
		 最后修改时间='' 外部唯一id='' />
		<body>",
                    T6Account.Sender, //
                    data.Rows[0]["FDate"], //
                                           //DateTime.Now.Year, //数据标识
                    Convert.ToDateTime(data.Rows[0]["FDate"]).Year,
                    keyId, //单据号
                    data.Rows[0]["FDate"], //日期
                    data.Rows[0]["FDate"], //日期
                    data.Rows[0]["FT6WarehouseNum"],
                    data.Rows[0]["FT6PurchaseTypeNum"], //入库类别编码
                    data.Rows[0]["FT6SaleTypeNum"], //出库类别编码
                    data.Rows[0]["FIOrgNum"], //部门编码
                    data.Rows[0]["FSalesmanNum"], //业务员编码
                    data.Rows[0]["FMemo"] //备注
                );

                var parm = new Dictionary<string, object>();
                parm.Clear();

                parm.Add("@FCompanyId", companyId);
                parm.Add("@KeyId", keyId);

                var list = SqlService.ExecuteProcedureCommand("u8_LossessList", parms).Tables[0];
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    xml +=
                        string.Format(
                            @"<row 行号='{0}' 存货编码='{1}' 账面数量='{2}' 盘点数量='{3}' 批次='' 自由项1='' 自由项2='' 自由项3='' 自由项4='' 自由项5='' 自由项6='' 自由项7='' 自由项8='' 自由项9='' 自由项10='' />"
                            , i + 1, //
                            list.Rows[i]["FNum"], //存货编码
                            list.Rows[i]["FQty"], //账面数量
                            list.Rows[i]["FQty"] //盘点数量
                        );
                }

                xml += @"</body></vouch></vouchs>";

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = GetDataToXml(document.OuterXml);

                if (ds.Tables.Count > 0 && ds.Tables.Contains("vouch") && ds.Tables["vouch"].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["vouch"].Rows[0]["err"].ToString()))
                    {
                        result = ds.Tables["vouch"].Rows[0]["err"].ToString();
                    }
                    else
                    {
                        StockOutService.Update(p => p.FCompanyId == companyId && p.KeyId == keyId, p => new LHStockOut
                        {
                            FT6Status = "已同步", //
                        });

                        result = "上传完成。";
                    }
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Contains("ret") && ds.Tables["ret"].Rows.Count > 0)
                    {
                        result = ds.Tables["ret"].Rows[0]["err"].ToString();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     同步已上传T6的发货单，验证是否已开票
        /// </summary>
        public void DateT6Bill()
        {
            var list = StockOutService.Where(p => p.FT6Status == "已同步" && p.FT6BillStatus == "未开票").ToList();

            foreach (var stock in list)
            {
                var xml =
                    string.Format(
                        @"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[select COUNT(1) SaleBillStatus from SaleBillVouch where cDLCode like '%{0}%']]></sql></getrs>",
                        stock.KeyId);

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = T6Interface.GetDataToXml(document.OuterXml);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables["ret"].Rows.Count > 0)
                {
                    //正常
                    if (ds.Tables["ret"].Rows[0]["bsuccess"].ToString().Equals("1"))
                    {
                        var rs = T6Interface.GetDataToXml(ds.Tables["ret"].Rows[0]["rs"].ToString());

                        DataTable dt = rs.Tables["row"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Columns.Contains("SaleBillStatus"))
                            {
                                var status = dt.Rows[i]["SaleBillStatus"].ToString();

                                if (status.Equals("1"))
                                {
                                    stock.FT6BillStatus = "已开票";

                                    StockOutService.SaveChanges();
                                }

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     同步已开票的单据，是否已收款
        /// </summary>
        public void DateT6Payment()
        {
            var list =
                StockOutService.Where(
                    p => p.FT6Status == "已同步" && p.FT6BillStatus == "已开票" && p.FT6PaymentStatus == "未收款").ToList();

            foreach (var stock in list)
            {
                var xml =
                    string.Format(
                        @"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[select distinct cDLCode from salebillvouch a inner join salebillvouchs b on a.SBVID=b.sbvid where isnull(iExchSum,0) = isum and  cDLCode like '%{0}%']]></sql></getrs>",
                        stock.KeyId);

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = T6Interface.GetDataToXml(document.OuterXml);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables["ret"].Rows.Count > 0)
                {
                    //正常
                    if (ds.Tables["ret"].Rows[0]["bsuccess"].ToString().Equals("1"))
                    {
                        var rs = T6Interface.GetDataToXml(ds.Tables["ret"].Rows[0]["rs"].ToString());

                        DataTable dt = rs.Tables["row"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Columns.Contains("cDLCode"))
                            {
                                stock.FT6PaymentStatus = "已收款";

                                StockOutService.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public decimal DateT6Customer(string code)
        {
            var customer = new CustomerService().Where(p => p.FCode == code).FirstOrDefault();

            var days = customer.FTipsDay;

            if (days > 0)
            {
                var xml =
                    string.Format(
                        @"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[select SUM(ISNULL(iSum,0)-ISNULL(iExchSum,0))AR from salebillvouch a inner join salebillvouchs b on a.SBVID=b.sbvid where isnull(iExchSum,0) <> isum and cCusCode='{0}'
AND dateadd(day,-{1},getdate()) >dDate ]]></sql></getrs>",
                        code,days);

                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = T6Interface.GetDataToXml(document.OuterXml);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables["ret"].Rows.Count > 0)
                {
                    //正常
                    if (ds.Tables["ret"].Rows[0]["bsuccess"].ToString().Equals("1"))
                    {
                        var rs = T6Interface.GetDataToXml(ds.Tables["ret"].Rows[0]["rs"].ToString());

                        DataTable dt = rs.Tables["row"];

                        return  Convert.ToInt32(dt.Rows[0]["ar"]);
                    }
                }
            }
            return 0M;
        }
    }
}