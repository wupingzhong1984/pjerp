using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using Enterprise.Data;
using Enterprise.Framework.Extension;
using Enterprise.IIS.Common;
using Enterprise.IIS.U8.ServiceReference;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;
using FineUI;
using NPOI.SS.Formula.Functions;

namespace Enterprise.IIS
{
    public partial class SynergicData : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private WarehouseService _warehouseService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected WarehouseService WarehouseService
        {
            get { return _warehouseService ?? (_warehouseService = new WarehouseService()); }
            set { _warehouseService = value; }
        }

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
        ///     部门组织数据服务
        /// </summary>
        private OrgnizationService _orgnizationService;
        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        protected OrgnizationService OrgnizationService
        {
            get { return _orgnizationService ?? (_orgnizationService = new OrgnizationService()); }
            set { _orgnizationService = value; }
        }

        private EmployeeService _employeeService;
        /// <summary>
        /// 
        /// </summary>
        protected EmployeeService EmployeeService
        {
            get { return _employeeService ?? (_employeeService = new EmployeeService()); }
            set { _employeeService = value; }
        }

        /// <summary>
        ///     数据字典
        /// </summary>
        private ProjectItemsService _projectItemsService;
        /// <summary>
        ///     数据字典
        /// </summary>
        protected ProjectItemsService ProjectItemsService
        {
            get { return _projectItemsService ?? (_projectItemsService = new ProjectItemsService()); }
            set { _projectItemsService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private SupplierService _supplierService;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected SupplierService SupplierService
        {
            get { return _supplierService ?? (_supplierService = new SupplierService()); }
            set { _supplierService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ItemsService _itemsService;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        protected ItemsService ItemsService
        {
            get { return _itemsService ?? (_itemsService = new ItemsService()); }
            set { _itemsService = value; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        /// <summary>
        ///     部门档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDepartment_OnClick(object sender, EventArgs e)
        {
            string xml = string.Format(@"<ufinterface sender='{0}' receiver='{1}' roottag='department' docid='112431824' 
proc='query' codeexchanged='N' exportneedexch='N' paginate='0' display='部门档案' 
family='基础档案' dynamicdate='{3}' maxdataitems='20000' bignoreextenduserdefines='y' 
needpage='1' totalpagenum='1' needpaginate='y' timestamp='' lastquerydate='{2}'/>", T6Account.Sender, T6Account.Receiver, DateTime.Now, T6Account.Dynamicdate);//12/14/2015

            DataSet ds = T6Interface.GetRequestData(xml);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("department") && ds.Tables["department"].Rows.Count > 0)
            {
                //写入
                DataTable dt = ds.Tables["department"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var orgnization = new base_orgnization
                    {
                        FCompanyId = CurrentUser.AccountComId,
                        FFlag = 1,
                        deleteflag = 0,
                        createdon = DateTime.Now,
                        code_cn = ChineseSpell.MakeSpellCode(dt.Rows[i]["name"].ToString(), "",
                            SpellOptions.FirstLetterOnly).ToUpper(),
                        org_type = 1,
                        org_name = dt.Rows[i]["name"].ToString(),
                        code = dt.Rows[i]["code"].ToString(),
                        org_sort = 1
                    };
                }

                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            else
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                    ds.Tables["item"].Rows.Count > 0)
                {
                    Alert.Show(ds.Tables["item"].Rows[0]["dsc"].ToString(), MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        ///     人事档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEmployee_OnClick(object sender, EventArgs e)
        {
            string xml = string.Format(@"<ufinterface sender='{0}' receiver='{1}' roottag='person' docid='386608600' 
proc='query' codeexchanged='N' exportneedexch='N' paginate='0' display='职员档案' 
family='基础档案' dynamicdate='{3}' maxdataitems='20000' bignoreextenduserdefines='y' 
needpage='1' totalpagenum='1' needpaginate='y' timestamp='' lastquerydate='{2}'/>", T6Account.Sender, T6Account.Receiver, DateTime.Now, T6Account.Dynamicdate);//12/14/2015

            DataSet ds = T6Interface.GetRequestData(xml);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["person"] != null && ds.Tables["person"].Rows.Count > 0)
            {
                //写入
                DataTable dt = ds.Tables["person"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var name = dt.Rows[i]["name"].ToString();
                    var code = dt.Rows[i]["code"].ToString();
                    var orgNum = dt.Rows[i]["dept_code"].ToString();

                    var emp = EmployeeService.FirstOrDefault(p => p.name == name);

                    if (emp != null)
                    {
                        emp.FINum = code;
                        emp.FIOrgNum = orgNum;
                        emp.job_number = emp.number;
                        emp.sex = "男";
                        EmployeeService.SaveChanges();
                    }
                    else
                    {
                        //生成工号
                        var sequence = new SequenceService().GH("GH", CurrentUser.AccountComId);
                        var numberAuto = sequence.Split('-')[2];

                        var addEmp = new base_employee
                        {
                            FCompanyId = CurrentUser.AccountComId,
                            FFlag = 1,
                            FINum = code,
                            job_number = numberAuto,
                            FIOrgNum = orgNum,
                            deleteflag = 0,
                            number = numberAuto,
                            name = name,
                            code_cn = ChineseSpell.MakeSpellCode(name, "",
                                SpellOptions.FirstLetterOnly).ToUpper()
                        };

                        EmployeeService.Add(addEmp);
                    }
                }
                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            else
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                    ds.Tables["item"].Rows.Count > 0)
                {
                    Alert.Show(ds.Tables["item"].Rows[0]["dsc"].ToString(), MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        ///     供货商分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSupplierType_OnClick(object sender, EventArgs e)
        {
            try
            {
                string xml = string.Format(@"<ufinterface sender='{0}' receiver='{1}' roottag='vendorclass' docid='112431824' 
proc='query' codeexchanged='N' exportneedexch='N' paginate='0' display='供应商分类' 
family='基础档案' dynamicdate='{3}' maxdataitems='20000' bignoreextenduserdefines='y' 
needpage='1' totalpagenum='1' needpaginate='y' timestamp='' lastquerydate='{2}'/>", T6Account.Sender, T6Account.Receiver, DateTime.Now, T6Account.Dynamicdate);//12/14/2015

                DataSet ds = T6Interface.GetRequestData(xml);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables["vendorclass"].Rows.Count > 0)
                {
                    //删除分类
                    ProjectItemsService.Delete(p => p.FSParent == "1003" && p.FCompanyId == 1 && p.FParentId == "2078");

                    DataTable dt = ds.Tables["vendorclass"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var items = new LHProjectItems
                        {
                            FId = "2078" + dt.Rows[i]["code"],
                            FCompanyId = 1,
                            FFlag = 1,
                            FParentId = "2078",
                            FName = dt.Rows[i]["name"].ToString(),
                            FKey = "",
                            FValue = "",
                            FSParent = "1003"
                        };

                        ProjectItemsService.Add(items);
                    }

                    Alert.Show("更新完成！", MessageBoxIcon.Information);
                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                        ds.Tables["item"].Rows.Count > 0)
                    {
                        Alert.Show(ds.Tables["item"].Rows[0]["dsc"].ToString(), MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     供应商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSupplier_OnClick(object sender, EventArgs e)
        {
            try
            {
//                string xml = String.Format(@"<ufinterface sender='{0}' receiver='{1}' roottag='vendor' docid='589005231' 
//proc='query' codeexchanged='N' exportneedexch='N' paginate='0' display='供应商档案' 
//family='基础档案' dynamicdate='{3}' maxdataitems='20000' bignoreextenduserdefines='y' 
//needpage='1' totalpagenum='1' needpaginate='y' timestamp='' lastquerydate='{2}'/>", T6Account.Sender, T6Account.Receiver, DateTime.Now, T6Account.Dynamicdate);
                var xml =
                    string.Format(@"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[SELECT * FROM vendor ]]></sql></getrs>");
                var client = new U2ImportSoapClient();
                var resxml = client.Importvouch(xml);

                var document = new System.Xml.XmlDocument();
                document.LoadXml(resxml);

                DataSet ds = T6Interface.GetDataToXml(document.OuterXml);


                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["ret"].Rows.Count > 0)
                {
                    //删除
                    //SupplierService.Delete(p => p.FCompanyId == CurrentUser.AccountComId);
                    if (ds.Tables["ret"].Rows[0]["bsuccess"].ToString().Equals("1"))
                    {
                        var rs = T6Interface.GetDataToXml(ds.Tables["ret"].Rows[0]["rs"].ToString());

                        DataTable dt = rs.Tables["row"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string salesperson = string.Empty;
                            if (dt.Columns.Contains("cVenPPerson"))
                            {
                                var num = dt.Rows[i]["cVenPPerson"].ToString();

                                var emp = EmployeeService.FirstOrDefault(p => p.FINum == num);
                                if (emp != null)
                                {
                                    salesperson = emp.name;
                                }
                            }

                            var code = dt.Rows[i]["cVencode"].ToString();
                            var supplier = SupplierService.Where(p => p.FCode == code).FirstOrDefault();
                            if (supplier != null)
                            {
                                supplier.FName = dt.Rows[i]["cVenname"].ToString();
                                supplier.FAddress = dt.Columns.Contains("cVenAddress") ? dt.Rows[i]["cVenAddress"].ToString() : "";
                                supplier.FPhome = dt.Columns.Contains("cVenphone") ? dt.Rows[i]["cVenphone"].ToString() : "";
                                supplier.FMoile = dt.Columns.Contains("cVenfax") ? dt.Rows[i]["cVenfax"].ToString() : "";
                                supplier.FSpell = ChineseSpell.MakeSpellCode(dt.Rows[i]["cVenName"].ToString(), "",
                                    SpellOptions.FirstLetterOnly).ToUpper();

                                SupplierService.SaveChanges();

                            }
                            else
                            {
                                var member = new LHSupplier
                                {
                                    FCode = dt.Rows[i]["cVencode"].ToString(),
                                    FName = dt.Rows[i]["cVenname"].ToString(),
                                    FAddress = dt.Columns.Contains("cVenAddress") ? dt.Rows[i]["cVenAddress"].ToString() : "",
                                    FPhome = dt.Columns.Contains("cVenphone") ? dt.Rows[i]["cVenphone"].ToString() : "",
                                    FMoile = dt.Columns.Contains("cVenfax") ? dt.Rows[i]["cVenfax"].ToString() : "",
                                    FSpell = ChineseSpell.MakeSpellCode(dt.Rows[i]["cVenName"].ToString(), "",
                                        SpellOptions.FirstLetterOnly).ToUpper(),
                                    FFlag = 1,
                                    FCompanyId = CurrentUser.AccountComId,
                                    FCateId = "2078",
                                    FIsAllot = 0,
                                    FSubCateId = "2078" + (dt.Columns.Contains("sort_code") ? dt.Rows[i]["sort_code"].ToString() : ""),
                                    FSalesman = salesperson,
                                    FFreight = 0,
                                    FCredit = 0,
                                };

                                SupplierService.Add(member);
                            }

                        }

                        Alert.Show("更新完成！", MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                        ds.Tables["item"].Rows.Count > 0)
                    {
                        Alert.Show(ds.Tables["item"].Rows[0]["dsc"].ToString(), MessageBoxIcon.Warning);
                    }
                }

            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     客户分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCustomerType_OnClick(object sender, EventArgs e)
        {
            try
            {
                string xml = string.Format(@"<ufinterface sender='{0}' receiver='{1}' roottag='customerclass' docid='459723174' 
proc='query' codeexchanged='N' exportneedexch='N' paginate='0' display='客户分类' 
family='基础档案' dynamicdate='{3}' maxdataitems='20000' bignoreextenduserdefines='y' 
needpage='1' totalpagenum='1' needpaginate='y' timestamp='' lastquerydate='{2}'/>", T6Account.Sender, T6Account.Receiver, DateTime.Now, T6Account.Dynamicdate);//12/14/2015

                DataSet ds = T6Interface.GetRequestData(xml);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables["customerclass"].Rows.Count > 0)
                {
                    //删除分类
                    ProjectItemsService.Delete(p => p.FSParent == "1002" && p.FCompanyId == 1 && p.FParentId == "2077");

                    DataTable dt = ds.Tables["customerclass"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var items = new LHProjectItems
                        {
                            FId = "2077" + dt.Rows[i]["code"],
                            FCompanyId = 1,
                            FFlag = 1,
                            FParentId = "2077",
                            FName = "客户分类",
                            FKey = dt.Rows[i]["name"].ToString(),
                            FValue = "2077" + dt.Rows[i]["code"],
                            FSParent = "1002"
                        };

                        ProjectItemsService.Add(items);
                    }

                    Alert.Show("更新完成！", MessageBoxIcon.Information);
                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                        ds.Tables["item"].Rows.Count > 0)
                    {
                        Alert.Show(ds.Tables["item"].Rows[0]["dsc"].ToString(), MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     客户档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCustomer_OnClick(object sender, EventArgs e)
        {
            try
            {
                //CustomerService.Delete(p => p.FCompanyId == CurrentUser.AccountComId);

                string where = string.Empty;

                while (true)
                {
                    where = " and  convert(varchar(10),dModifyDate,20)='"+DateTime.Now.ToString("yyyy-MM-dd")+"'";
                    var xml =
                    string.Format(@"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[SELECT TOP 300 * FROM dbo.Customer WHERE 1=1 {0}  ORDER BY cCusCode]]></sql></getrs>", where);

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
                                string salesperson = string.Empty;
                                if (dt.Columns.Contains("cCusPerson"))
                                {
                                    var num = dt.Rows[i]["cCusPerson"].ToString();

                                    var emp = EmployeeService.FirstOrDefault(p => p.FINum == num);
                                    if (emp != null)
                                    {
                                        salesperson = emp.name;
                                    }
                                }

                                var iARMoney = dt.Columns.Contains("iARMoney") ? dt.Rows[i]["iARMoney"].ToString() : "0";//dt.Rows[i]["iARMoney"] ?? "0";

                                if (string.IsNullOrEmpty(iARMoney.ToString()))
                                {
                                    iARMoney = "0";
                                }

                                var code = dt.Rows[i]["cCusCode"].ToString();
                                var customer = CustomerService.Where(p => p.FCode == code).FirstOrDefault();
                                if (customer != null)
                                {
                                    customer.FName = dt.Rows[i]["cCusName"].ToString();
                                    customer.FAddress = dt.Columns.Contains("cCusAddress") ? dt.Rows[i]["cCusAddress"].ToString() : "";
                                    //FLinkman = dt.Columns.Contains("cCusPerson") ? dt.Rows[i]["cCusPerson"].ToString() : "",
                                    customer.FPhome = dt.Columns.Contains("cCusPhone")
                                        ? dt.Rows[i]["cCusPhone"].ToString()
                                        : "";
                                    customer.FMoile = dt.Columns.Contains("cCusFax")
                                        ? dt.Rows[i]["cCusFax"].ToString()
                                        : "";
                                    customer.FSpell = ChineseSpell.MakeSpellCode(dt.Rows[i]["cCusName"].ToString(), "",
                                        SpellOptions.FirstLetterOnly).ToUpper();

                                    customer.FCreLine = dt.Columns.Contains("iCusCreLine")
                                        ? Convert.ToDecimal(dt.Rows[i]["iCusCreLine"])
                                        : 0;
                                    customer.FARMoney = iARMoney;//dt.Columns.Contains("iARMoney") ? Convert.ToDecimal(dt.Rows[i]["iARMoney"]) : 0,
                                    customer.FFrequency = dt.Columns.Contains("iFrequency")
                                        ? Convert.ToDecimal(dt.Rows[i]["iFrequency"])
                                        : 0;
                                    customer.FDistric = dt.Columns.Contains("cDCCode")
                                        ? dt.Rows[i]["cDCCode"].ToString()
                                        : "";
                                    CustomerService.SaveChanges();
                                }
                                else
                                {
                                    var member = new LHCustomer
                                    {
                                        FCode = dt.Rows[i]["cCusCode"].ToString(),
                                        FName = dt.Rows[i]["cCusName"].ToString(),
                                        FAddress = dt.Columns.Contains("cCusAddress") ? dt.Rows[i]["cCusAddress"].ToString() : "",
                                        //FLinkman = dt.Columns.Contains("cCusPerson") ? dt.Rows[i]["cCusPerson"].ToString() : "",
                                        FPhome = dt.Columns.Contains("cCusPhone") ? dt.Rows[i]["cCusPhone"].ToString() : "",
                                        FMoile = dt.Columns.Contains("cCusFax") ? dt.Rows[i]["cCusFax"].ToString() : "",
                                        FSpell = ChineseSpell.MakeSpellCode(dt.Rows[i]["cCusName"].ToString(), "",
                                            SpellOptions.FirstLetterOnly).ToUpper(),
                                        FFlag = 1,
                                        FCompanyId = CurrentUser.AccountComId,
                                        FCateId = "2077",
                                        FIsAllot = 0,
                                        FSubCateId = "2077" + (dt.Columns.Contains("cCCCode") ? dt.Rows[i]["cCCCode"].ToString() : ""),
                                        FSalesman = salesperson,
                                        FFreight = 0,
                                        FCredit = 0,
                                        FCreLine = dt.Columns.Contains("iCusCreLine") ? Convert.ToDecimal(dt.Rows[i]["iCusCreLine"]) : 0,
                                        FARMoney = iARMoney,//dt.Columns.Contains("iARMoney") ? Convert.ToDecimal(dt.Rows[i]["iARMoney"]) : 0,
                                        FCreditFlag = "通过",
                                        FGroupNo = dt.Rows[i]["cCusCode"].ToString(),
                                        FGroupNoFlag = "是",
                                        FFrequency = dt.Columns.Contains("iFrequency") ? Convert.ToDecimal(dt.Rows[i]["iFrequency"]) : 0,
                                        FDistric = dt.Columns.Contains("cDCCode") ? dt.Rows[i]["cDCCode"].ToString() : "",
                                    };

                                    CustomerService.Add(member);
                                }
                            }

                            var rowLast = dt.AsEnumerable().Last<DataRow>();
                            where = string.Format(@" and cCusCode>'{0}'", rowLast["cCusCode"].ToString());// rowLast["cCusCode"].ToString();

                            if (dt.Rows.Count < 300)
                            {
                                break;
                            }
                        }
                    }
                }

                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     存货分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnItemType_OnClick(object sender, EventArgs e)
        {

        }
        /// <summary>
        ///     存货档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnItem_OnClick(object sender, EventArgs e)
        {
            try
            {
                string where = string.Empty;

                while (true)
                {
                    var xml =
                    string.Format(@"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[SELECT TOP 300 cInvCCode,cInvCode,cInvName,cInvStd,iTaxRate FROM Inventory WHERE 1=1 {0} and cInvName not like '%停用%' ORDER BY cInvCode]]></sql></getrs>", where);

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
                                var cInvCode = dt.Rows[i]["cInvCode"].ToString();
                                var cInvName = dt.Rows[i]["cInvName"].ToString();
                                var cInvStd = dt.Rows[i]["cInvStd"].ToString();
                                var iTaxRate = Convert.ToInt32(dt.Rows[i]["iTaxRate"]);

                                var items =
                                    ItemsService.Where(p => p.FName == cInvName && p.FSpec == cInvStd).FirstOrDefault();

                                if (items != null)
                                {
                                    items.FINum = cInvCode;
                                    items.FITaxRate = iTaxRate;
                                    //items.FName = cInvName;
                                    //items.FSpec = cInvStd;

                                    ItemsService.SaveChanges();
                                }
                                else
                                {
                                    //分类
                                    string code = string.Empty;
                                    string cateSub = string.Empty;
                                    if (dt.Columns.Contains("cInvCCode"))
                                    {
                                        var sub = dt.Rows[i]["cInvCCode"].ToString();
                                        var parms = new Dictionary<string, object>();
                                        parms.Clear();
                                        parms.Add("@companyid", CurrentUser.AccountComId);

                                        if (sub.StartsWith("01")) //原料
                                        {
                                            parms.Add("@type", "2002");//                 
                                            var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                            code = list.Rows[0][0].ToString();

                                            cateSub = "2002";
                                        }
                                        else if (sub.StartsWith("07")) //服务
                                        {
                                            parms.Add("@type", "2002");//                 
                                            var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                            code = list.Rows[0][0].ToString();

                                            cateSub = "2002";
                                        }
                                        else if (sub.StartsWith("11")) //产成品
                                        {
                                            parms.Add("@type", "2000");//                 
                                            var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                            code = list.Rows[0][0].ToString();

                                            cateSub = "2000";
                                        }
                                        else if (sub.StartsWith("12")) //包装物
                                        {
                                            parms.Add("@type", "2001");//                 
                                            var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                            code = list.Rows[0][0].ToString();

                                            cateSub = "2001";
                                        }
                                        else if (sub.StartsWith("13")) //外购产品
                                        {
                                            parms.Add("@type", "2000");//                 
                                            var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                            code = list.Rows[0][0].ToString();

                                            cateSub = "2000";
                                        }
                                        else if (sub.StartsWith("15")) //备品备件
                                        {
                                            parms.Add("@type", "2002");//                 
                                            var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                            code = list.Rows[0][0].ToString();
                                            cateSub = "2002";
                                        }
                                        else if (sub.StartsWith("99")) //停用编码
                                        {
                                            parms.Add("@type", "2002");//                 
                                            var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                            code = list.Rows[0][0].ToString();
                                            cateSub = "2002";
                                        }
                                    }

                                    var its = new LHItems
                                    {
                                        FCode = code,
                                        FName = cInvName,
                                        FSpec = cInvStd,
                                        FSpell = ChineseSpell.MakeSpellCode(cInvName, "",
                                            SpellOptions.FirstLetterOnly).ToUpper(),
                                        FFlag = 1,

                                        //分类
                                        FCateId = cateSub,
                                        FSubCateId = cateSub,

                                        FCompanyId = CurrentUser.AccountComId,
                                        FGroupNum = code,
                                        FMemo = "",

                                        //单位
                                        //FUnit = ddlUnit.SelectedValue,
                                        FPurchasePrice = 0,
                                        FSalesPrice = 0,
                                        FPieceWork1 = 0,
                                        FPieceWork2 = 0,
                                        FPieceWork3 = 0,
                                        FPieceWork4 = 0,
                                        FPieceWork5 = 0,

                                        FINum = cInvCode,
                                        FITaxRate = iTaxRate

                                    };

                                    ItemsService.Add(its);
                                }
                            }

                            //提交
                            //ItemsService.SaveChanges();

                            var rowLast = dt.AsEnumerable().Last<DataRow>();
                            where = string.Format(@" and cInvCode>'{0}'", rowLast["cInvCode"].ToString());// rowLast["cCusCode"].ToString();

                            if (dt.Rows.Count < 300)
                            {
                                break;
                            }
                        }
                    }
                }

                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        ///     计量单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUnit_OnClick(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnWarehouse_OnClick(object sender, EventArgs e)
        {
            string xml = string.Format(@"<ufinterface sender='{0}' receiver='{1}' roottag='warehouse' docid='547700107' 
proc='query' codeexchanged='N' exportneedexch='N' paginate='0' display='仓库档案' 
family='基础档案' dynamicdate='{3}' maxdataitems='20000' bignoreextenduserdefines='y' 
needpage='1' totalpagenum='1' needpaginate='y' timestamp='' lastquerydate='{2}'/>", T6Account.Sender, T6Account.Receiver, DateTime.Now, T6Account.Dynamicdate);//12/14/2015

            DataSet ds = T6Interface.GetRequestData(xml);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["warehouse"] != null && ds.Tables["warehouse"].Rows.Count > 0)
            {
                //写入
                DataTable dt = ds.Tables["warehouse"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    var name = dt.Rows[i]["name"].ToString();

                    if (name.Equals("停用"))
                    {
                        continue;
                    }

                    var code = dt.Rows[i]["code"].ToString();
                    var address = dt.Rows[i]["address"].ToString();
                    var itemType = dt.Rows[i]["valuestyle"].ToString();//计价方式

                    var emp = WarehouseService.FirstOrDefault(p => p.FName == name);

                    if (emp != null)
                    {
                        emp.FAddress = address;
                        //emp.FCode = code;
                        emp.FItemType = itemType;
                        //emp.FCompanyId = CurrentUser.AccountComId;
                        emp.FFlag = 1;
                        emp.FINum = code;

                        WarehouseService.SaveChanges();
                    }
                    else
                    {
                        //仓库内码
                        var parms = new Dictionary<string, object>();
                        parms.Clear();

                        parms.Add("@companyid", CurrentUser.AccountComId);
                        parms.Add("@type", "Warehouse");//仓库                  
                        var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                        var FCode = list.Rows[0][0].ToString();

                        var addEmp = new LHWarehouse
                        {
                            FCompanyId = CurrentUser.AccountComId,
                            FFlag = 1,
                            FINum = code,
                            FCode = FCode,
                            FName = name,
                            FItemType = itemType,
                            FAddress = address,
                            FLinkman = "",
                            FSpell = ChineseSpell.MakeSpellCode(name, "",
                                SpellOptions.FirstLetterOnly).ToUpper()
                        };

                        WarehouseService.Add(addEmp);
                    }
                }
                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            else
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                    ds.Tables["item"].Rows.Count > 0)
                {
                    Alert.Show(ds.Tables["item"].Rows[0]["dsc"].ToString(), MessageBoxIcon.Warning);
                }
            }
        }
        /// <summary>
        /// 收发类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReceive_OnClick(object sender, EventArgs e)
        {
            try
            {
                var xml =
                    string.Format(@"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[SELECT cRdCode,cRdName,iRdGrade FROM Rd_Style order by cRdCode]]></sql></getrs>");

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
                            #region Add/Update
                            //收发类型
                            var name = dt.Rows[i]["cRdName"].ToString();
                            var code = dt.Rows[i]["cRdCode"].ToString();
                            var grade = dt.Rows[i]["iRdGrade"].ToString();//iRdGrade

                            var its = ProjectItemsService.FirstOrDefault(p => p.FKey == name && p.FSParent == "1024");
                            if (its != null)
                            {
                                its.FParentId = "1024";
                                its.FName = "收发类型";
                                its.FKey = name;
                                its.FValue = name;
                                its.FEx01 = code;
                                its.FEx02 = grade;

                                ProjectItemsService.SaveChanges();

                            }
                            else
                            {
                                var items = new LHProjectItems
                                {
                                    FId = "1024" + code,
                                    FCompanyId = 1,
                                    FFlag = 1,
                                    FParentId = "1024",
                                    FName = "收发类型",
                                    FKey = name,
                                    FValue = name,
                                    FSParent = "1024",
                                    FEx01 = code,
                                    FEx02 = grade
                                };

                                ProjectItemsService.Add(items);
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                        ds.Tables["item"].Rows.Count > 0)
                    {
                        throw new Exception(ds.Tables["item"].Rows[0]["dsc"].ToString());
                    }
                }
                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// 采购类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPurchase_OnClick(object sender, EventArgs e)
        {
            try
            {
                var xml =
                    string.Format(@"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[SELECT cPTCode,cPTName,cRdCode FROM PurchaseType]]></sql></getrs>");

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
                            #region Add/Update
                            //收发类型
                            var name = dt.Rows[i]["cPTName"].ToString();
                            var code = dt.Rows[i]["cPTCode"].ToString();
                            var grade = dt.Rows[i]["cRdCode"].ToString();//iRdGrade

                            var its = ProjectItemsService.FirstOrDefault(p => p.FKey == name && p.FSParent == "1022");
                            if (its != null)
                            {
                                its.FParentId = "1022";
                                its.FName = "采购类型";
                                its.FKey = name;
                                its.FValue = name;
                                its.FEx01 = code;
                                its.FEx02 = grade;

                                ProjectItemsService.SaveChanges();

                            }
                            else
                            {
                                var items = new LHProjectItems
                                {
                                    FId = "1022" + code,
                                    FCompanyId = 1,
                                    FFlag = 1,
                                    FParentId = "1022",
                                    FName = "采购类型",
                                    FKey = name,
                                    FValue = name,
                                    FSParent = "1022",
                                    FEx01 = code,
                                    FEx02 = grade
                                };

                                ProjectItemsService.Add(items);
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                        ds.Tables["item"].Rows.Count > 0)
                    {
                        throw new Exception(ds.Tables["item"].Rows[0]["dsc"].ToString());
                    }
                }
                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        ///     销售类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSales_OnClick(object sender, EventArgs e)
        {
            try
            {
                var xml =
                    string.Format(@"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[SELECT cSTCode,cSTName,cRdCode FROM saletype]]></sql></getrs>");

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
                            #region Add/Update
                            //收发类型
                            var name = dt.Rows[i]["cSTName"].ToString();
                            var code = dt.Rows[i]["cSTCode"].ToString();
                            var grade = dt.Rows[i]["cRdCode"].ToString();//iRdGrade

                            var its = ProjectItemsService.FirstOrDefault(p => p.FKey == name && p.FSParent == "1023");
                            if (its != null)
                            {
                                its.FParentId = "1023";
                                its.FName = "销售类型";
                                its.FKey = name;
                                its.FValue = name;
                                its.FEx01 = code;
                                its.FEx02 = grade;

                                ProjectItemsService.SaveChanges();

                            }
                            else
                            {
                                var items = new LHProjectItems
                                {
                                    FId = "1023" + code,
                                    FCompanyId = 1,
                                    FFlag = 1,
                                    FParentId = "1023",
                                    FName = "销售类型",
                                    FKey = name,
                                    FValue = name,
                                    FSParent = "1023",
                                    FEx01 = code,
                                    FEx02 = grade
                                };

                                ProjectItemsService.Add(items);
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                        ds.Tables["item"].Rows.Count > 0)
                    {
                        throw new Exception(ds.Tables["item"].Rows[0]["dsc"].ToString());
                    }
                }
                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        ///     销售订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalesOrder_OnClick(object sender, EventArgs e)
        {
        }
        /// <summary>
        ///     销售发货单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalesNote_OnClick(object sender, EventArgs e)
        {
        }
        /// <summary>
        ///     销售退货单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalesReturn_OnClick(object sender, EventArgs e)
        {

        }
        /// <summary>
        ///     采购订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPurchaseOrder_OnClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     采购单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPurchaseNote_OnClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     采购退货单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnbtnPurchaseReturn_OnClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     查看应收账款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAR_OnClick(object sender, EventArgs e)
        {

        }
        /// <summary>
        ///     查看应付账款 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAP_OnClick(object sender, EventArgs e)
        {

        }

        protected void btnCurrency_OnClick(object sender, EventArgs e)
        {
            try
            {
                var xml =
                    string.Format(@"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[SELECT i_id,cexch_code,cexch_name FROM foreigncurrency]]></sql></getrs>");

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
                            #region Add/Update
                            //收发类型
                            var name = dt.Rows[i]["cexch_name"].ToString();
                            var code = dt.Rows[i]["i_id"].ToString();
                            var grade = dt.Rows[i]["cexch_code"].ToString();//iRdGrade

                            var its = ProjectItemsService.FirstOrDefault(p => p.FKey == name && p.FSParent == "1025");
                            if (its != null)
                            {
                                its.FParentId = "1025";
                                its.FName = "币种";
                                its.FKey = name;
                                its.FValue = name;
                                its.FEx01 = code;
                                its.FEx02 = grade;

                                ProjectItemsService.SaveChanges();
                            }
                            else
                            {
                                var items = new LHProjectItems
                                {
                                    FId = "1025" + code,
                                    FCompanyId = 1,
                                    FFlag = 1,
                                    FParentId = "1025",
                                    FName = "币种",
                                    FKey = name,
                                    FValue = name,
                                    FSParent = "1025",
                                    FEx01 = code,
                                    FEx02 = grade
                                };

                                ProjectItemsService.Add(items);
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                        ds.Tables["item"].Rows.Count > 0)
                    {
                        throw new Exception(ds.Tables["item"].Rows[0]["dsc"].ToString());
                    }
                }
                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }


        protected void btnFDataCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string where = string.Empty;


                where = string.Format(@" and cCuscode ='{0}' ", txtFCustomer.Text.Trim());


                while (true)
                {
                    var xml =
                    string.Format(@"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[SELECT TOP 1 * FROM dbo.Customer WHERE 1=1 {0}  ORDER BY cCusCode]]></sql></getrs>", where);

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
                                string salesperson = string.Empty;
                                if (dt.Columns.Contains("cCusPerson"))
                                {
                                    var num = dt.Rows[i]["cCusPerson"].ToString();

                                    var emp = EmployeeService.FirstOrDefault(p => p.FINum == num);
                                    if (emp != null)
                                    {
                                        salesperson = emp.name;
                                    }
                                }

                                var iARMoney = dt.Columns.Contains("iARMoney") ? dt.Rows[i]["iARMoney"].ToString() : "0";//dt.Rows[i]["iARMoney"] ?? "0";

                                if (string.IsNullOrEmpty(iARMoney.ToString()))
                                {
                                    iARMoney = "0";
                                }

                                var code = dt.Rows[i]["cCusCode"].ToString();
                                var customer = CustomerService.Where(p => p.FCode == code).FirstOrDefault();
                                if (customer != null)
                                {
                                    customer.FName = dt.Rows[i]["cCusName"].ToString();
                                    customer.FAddress = dt.Columns.Contains("cCusAddress") ? dt.Rows[i]["cCusAddress"].ToString() : "";
                                    //FLinkman = dt.Columns.Contains("cCusPerson") ? dt.Rows[i]["cCusPerson"].ToString() : "",
                                    customer.FPhome = dt.Columns.Contains("cCusPhone")
                                        ? dt.Rows[i]["cCusPhone"].ToString()
                                        : "";
                                    customer.FMoile = dt.Columns.Contains("cCusFax")
                                        ? dt.Rows[i]["cCusFax"].ToString()
                                        : "";
                                    customer.FSpell = ChineseSpell.MakeSpellCode(dt.Rows[i]["cCusName"].ToString(), "",
                                        SpellOptions.FirstLetterOnly).ToUpper();

                                    customer.FCreLine = dt.Columns.Contains("iCusCreLine")
                                        ? Convert.ToDecimal(dt.Rows[i]["iCusCreLine"])
                                        : 0;
                                    customer.FARMoney = iARMoney;//dt.Columns.Contains("iARMoney") ? Convert.ToDecimal(dt.Rows[i]["iARMoney"]) : 0,
                                    customer.FFrequency = dt.Columns.Contains("iFrequency")
                                        ? Convert.ToDecimal(dt.Rows[i]["iFrequency"])
                                        : 0;
                                    customer.FDistric = dt.Columns.Contains("cDCCode")
                                        ? dt.Rows[i]["cDCCode"].ToString()
                                        : "";
                                    CustomerService.SaveChanges();
                                }
                                else
                                {
                                    var member = new LHCustomer
                                    {
                                        FCode = dt.Rows[i]["cCusCode"].ToString(),
                                        FName = dt.Rows[i]["cCusName"].ToString(),
                                        FAddress = dt.Columns.Contains("cCusAddress") ? dt.Rows[i]["cCusAddress"].ToString() : "",
                                        //FLinkman = dt.Columns.Contains("cCusPerson") ? dt.Rows[i]["cCusPerson"].ToString() : "",
                                        FPhome = dt.Columns.Contains("cCusPhone") ? dt.Rows[i]["cCusPhone"].ToString() : "",
                                        FMoile = dt.Columns.Contains("cCusFax") ? dt.Rows[i]["cCusFax"].ToString() : "",
                                        FSpell = ChineseSpell.MakeSpellCode(dt.Rows[i]["cCusName"].ToString(), "",
                                            SpellOptions.FirstLetterOnly).ToUpper(),
                                        FFlag = 1,
                                        FCompanyId = CurrentUser.AccountComId,
                                        FCateId = "2077",
                                        FIsAllot = 0,
                                        FSubCateId = "2077" + (dt.Columns.Contains("cCCCode") ? dt.Rows[i]["cCCCode"].ToString() : ""),
                                        FSalesman = salesperson,
                                        FFreight = 0,
                                        FCredit = 0,
                                        FCreLine = dt.Columns.Contains("iCusCreLine") ? Convert.ToDecimal(dt.Rows[i]["iCusCreLine"]) : 0,
                                        FARMoney = iARMoney,//dt.Columns.Contains("iARMoney") ? Convert.ToDecimal(dt.Rows[i]["iARMoney"]) : 0,

                                        FGroupNo = dt.Rows[i]["cCusCode"].ToString(),
                                        FGroupNoFlag = "是",
                                        FFrequency = dt.Columns.Contains("iFrequency") ? Convert.ToDecimal(dt.Rows[i]["iFrequency"]) : 0,
                                        FDistric = dt.Columns.Contains("cDCCode") ? dt.Rows[i]["cDCCode"].ToString() : "",
                                    };

                                    CustomerService.Add(member);
                                }
                            }

                            var rowLast = dt.AsEnumerable().Last<DataRow>();
                            where = string.Format(@" and cCusCode>'{0}'", rowLast["cCusCode"].ToString());// rowLast["cCusCode"].ToString();

                            if (dt.Rows.Count < 300)
                            {
                                break;
                            }
                        }
                    }
                }

                Alert.Show("更新完成！", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }

        protected void btnDataFSupplier_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     查看应收发票
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnARInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                var list = StockOutService.Where(p => p.FT6BillStatus == "未开票" && p.FType == 1 && p.FFlag == 1 && p.FDeleteFlag == 0).ToList();

                foreach (var item in list)
                {
                    string xmlDepartment = String.Format(@"<?xml version='1.0' encoding='utf-8' ?> 
<ufinterface sender='{0}' receiver='{1}'
 	roottag='SQLEXE' proc='vendor'
 	codeexchanged='n' dynamicdate='{2}' >
<sql value='select DISTINCT cDLCode from DispatchList a left join DispatchLists b on a.DLID=b.DLID where b.iSettleQuantity>0 and a.cDLCode={3} '/>
</ufinterface>", //U8Account.Sender, U8Account.Receiver, U8Account.Dynamicdate, u8Code);
    T6Account.Sender, T6Account.Receiver, DateTime.Now, T6Account.Dynamicdate, item.KeyId);//12/14/2015

                    DataSet ds = T6Interface.GetRequestData(xmlDepartment);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("vendor") && ds.Tables["vendor"].Rows.Count > 0)
                    {
                        //_bllCommon.ExecSql(string.Format(@"update kpjl set zdr=1 where u8_dh='{0}';", u8Code));

                        item.FT6BillStatus = "已开票";
                    }
                    else
                    {
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("item") &&
                            ds.Tables["item"].Rows.Count > 0)
                        {
                            throw new Exception(ds.Tables["item"].Rows[0]["dsc"].ToString());
                        }
                    }

                }

                StockOutService.SaveChanges();
            }
            catch
            { }
        }

        protected void btnItemName_Click(object sender, EventArgs e)
        {
            try
            {
                string where = string.Empty;

                if (string.IsNullOrEmpty(txtFItemCode.Text.Trim()) //
                    || string.IsNullOrEmpty(txtFItemName.Text.Trim()))
                {
                    Alert.Show("请输入要更新T6系统中要变更的存货代码和存货名称，同步更新后ERP同时也会更新！", MessageBoxIcon.Information);
                    return;
                }

                where = string.Format(@" and cInvCode ='{0}' ", txtFItemCode.Text.Trim());

                string update = string.Format(@" set nocount on UPDATE  Inventory  SET cInvName='{1}' WHERE cInvCode='{0}' ",//
                    txtFItemCode.Text.Trim(), txtFItemName.Text.Trim());

                var xml =
                string.Format(@"<?xml version='1.0' encoding='utf-8' ?><getrs cacc_id='008'><sql><![CDATA[{1} SELECT TOP 1 cInvCCode,cInvCode,cInvName,cInvStd,iTaxRate FROM Inventory WHERE 1=1 {0} and cInvName not like '%停用%' ORDER BY cInvCode]]></sql></getrs>", where, update);

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
                            var cInvCode = dt.Rows[i]["cInvCode"].ToString();
                            var cInvName = dt.Rows[i]["cInvName"].ToString();
                            var cInvStd = dt.Columns.IndexOf("cInvStd") > -1 ? dt.Rows[i]["cInvStd"].ToString() : "";
                            var iTaxRate = Convert.ToInt32(dt.Rows[i]["iTaxRate"]);

                            var items =
                                ItemsService.Where(p => p.FName == cInvName && p.FSpec == cInvStd).FirstOrDefault();

                            if (items != null)
                            {
                                items.FINum = cInvCode;
                                items.FITaxRate = iTaxRate;

                                ItemsService.SaveChanges();
                            }
                            else
                            {
                                //分类
                                string code = string.Empty;
                                string cateSub = string.Empty;
                                if (dt.Columns.Contains("cInvCCode"))
                                {
                                    var sub = dt.Rows[i]["cInvCCode"].ToString();
                                    var parms = new Dictionary<string, object>();
                                    parms.Clear();
                                    parms.Add("@companyid", CurrentUser.AccountComId);

                                    if (sub.StartsWith("01")) //原料
                                    {
                                        parms.Add("@type", "2002");//                 
                                        var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                        code = list.Rows[0][0].ToString();

                                        cateSub = "2002";
                                    }
                                    else if (sub.StartsWith("07")) //服务
                                    {
                                        parms.Add("@type", "2002");//                 
                                        var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                        code = list.Rows[0][0].ToString();

                                        cateSub = "2002";
                                    }
                                    else if (sub.StartsWith("11")) //产成品
                                    {
                                        parms.Add("@type", "2000");//                 
                                        var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                        code = list.Rows[0][0].ToString();

                                        cateSub = "2000";
                                    }
                                    else if (sub.StartsWith("12")) //包装物
                                    {
                                        parms.Add("@type", "2001");//                 
                                        var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                        code = list.Rows[0][0].ToString();

                                        cateSub = "2001";
                                    }
                                    else if (sub.StartsWith("13")) //外购产品
                                    {
                                        parms.Add("@type", "2000");//                 
                                        var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                        code = list.Rows[0][0].ToString();

                                        cateSub = "2000";
                                    }
                                    else if (sub.StartsWith("15")) //备品备件
                                    {
                                        parms.Add("@type", "2002");//                 
                                        var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                        code = list.Rows[0][0].ToString();
                                        cateSub = "2002";
                                    }
                                    else if (sub.StartsWith("99")) //停用编码
                                    {
                                        parms.Add("@type", "2002");//                 
                                        var list = SqlService.ExecuteProcedureCommand("proc_GetCode", parms).Tables[0];
                                        code = list.Rows[0][0].ToString();
                                        cateSub = "2002";
                                    }
                                }

                                var its = new LHItems
                                {
                                    FCode = code,
                                    FName = cInvName,
                                    FSpec = cInvStd,
                                    FSpell = ChineseSpell.MakeSpellCode(cInvName, "",
                                        SpellOptions.FirstLetterOnly).ToUpper(),
                                    FFlag = 1,

                                    //分类
                                    FCateId = cateSub,
                                    FSubCateId = cateSub,

                                    FCompanyId = CurrentUser.AccountComId,
                                    FGroupNum = code,
                                    FMemo = "",

                                    //单位
                                    //FUnit = ddlUnit.SelectedValue,
                                    FPurchasePrice = 0,
                                    FSalesPrice = 0,
                                    FPieceWork1 = 0,
                                    FPieceWork2 = 0,
                                    FPieceWork3 = 0,
                                    FPieceWork4 = 0,
                                    FPieceWork5 = 0,

                                    FINum = cInvCode,
                                    FITaxRate = iTaxRate

                                };

                                ItemsService.Add(its);
                            }
                        }

                        //提交
                        //ItemsService.SaveChanges();

                        var rowLast = dt.AsEnumerable().Last<DataRow>();
                        where = string.Format(@" and cInvCode>'{0}'", rowLast["cInvCode"].ToString());// rowLast["cCusCode"].ToString();
                    }
                    else
                    {
                        Alert.Show("更新失败！", MessageBoxIcon.Information);
                    }
                }

                Alert.Show("更新完成！", MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, MessageBoxIcon.Warning);
            }
        }
    }
}