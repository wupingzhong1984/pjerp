using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Enterprise.DataAccess.SQLServer;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Service.Base.Platform;
using Enterprise.Framework.FormsAuth;
using FineUI;

namespace Enterprise.IIS.Common
{
    /// <summary>
    ///     常用工具类
    /// </summary>
    public class GasHelper
    {
        #region 下接框选择

        #region 仓库
        /// <summary>
        ///     仓库
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetWarehouseByName(string name)
        {
            if (name.Equals("-1"))
                return string.Empty;

            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            var service = new SqlService();
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@companyId", companydId);
            parms.Add("@FName", name);

            return service.ExecuteProcedureCommand("proc_GetWarehouseByName", parms).Tables[0].Rows[0]["FCode"].ToString();

        }
        #endregion

        #region 收发类型
        /// <summary>
        ///     收发类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDataBindReceiveSendType(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1024" && p.FCompanyId == companydId && p.FFlag==1);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FEx01";
            ddl.DataSimulateTreeLevelField = "FEx02";

            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }
        #endregion

        #region 销售类型
        /// <summary>
        ///     销售类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDataBindSaleType(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1023" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FEx01";
            //ddl.DataSimulateTreeLevelField = "FEx02";

            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }
        #endregion

        #region 采购类型
        /// <summary>
        ///     采购类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDataBindPurchaseType(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1022" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FEx01";
            //ddl.DataSimulateTreeLevelField = "FEx02";

            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }
        #endregion

        #region 币种
        /// <summary>
        ///     币种
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDataBindCurrencyType(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1025" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FKey"; //"FEx01";
            //ddl.DataSimulateTreeLevelField = "FEx02";

            ddl.DataBind();

            //ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }
        #endregion

        /// <summary>
        ///     存运设备FKey/FId
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDataBindDevice(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1030" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FId"; //"FEx01";
            //ddl.DataSimulateTreeLevelField = "FEx02";

            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        #region 组织架构
        /// <summary>
        ///     组织架构
        /// </summary>
        /// <param name="ddl">DropDownList</param>
        public static void DropDownListDataBindOrgnization(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new OrgnizationService();

            ddl.DataSource = service.Where(p => p.deleteflag==0 && p.FCompanyId == companydId);
            ddl.DataTextField = "org_name";
            ddl.DataValueField = "id";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }
        #endregion

        #region 仓库档案
        /// <summary>
        ///     仓库档案
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListWarehouseDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var warehouse = new WarehouseService().Where(p => p.FFlag == 1 && p.FCompanyId == companydId);

            //钢瓶集合
            ddl.DataSource = warehouse.OrderBy(p=>p.FName);
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FName";
            ddl.DataBind();

            //ddl.Items.Insert(0, new ListItem(@"", "-1"));

            ddl.Items[0].Selected = true;
        }
        #endregion

        #region 仓库档案
        /// <summary>
        ///     仓库档案
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListWarehouseInfoDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var warehouse = new WarehouseService().Where(p => p.FFlag == 1 && p.FCompanyId == companydId);

            //钢瓶集合
            ddl.DataSource = warehouse.OrderBy(p => p.FName);
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FCode";
            ddl.DataBind();

            //ddl.Items.Insert(0, new ListItem(@"", "-1"));

            ddl.Items[0].Selected = true;
        }
        #endregion

        #region 根据编码<Key/Value>
        /// <summary>
        ///     根据编码
        /// </summary>
        /// <param name="ddl">DropDownList</param>
        /// <param name="code">字典编码</param>
        public static void DropDownListDataBind(DropDownList ddl, string code)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == code&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }
        #endregion

        #region 所属区域
        /// <summary>
        ///     所属区域（1004）
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDistricDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FParentId == "1004" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FKey";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }
        #endregion

        #region 客户类型
        /// <summary>
        ///     客户类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListCustomerCateDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1002" && p.FCompanyId == companydId);
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FId";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }
        #endregion

        #region 供应商分类
        /// <summary>
        ///     供应商分类
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListSupplierCateDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1003" && p.FCompanyId == companydId);
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FId";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }
        #endregion

        #region 组织机构
        /// <summary>
        ///     组织机构
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListOrgnizationDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new OrgnizationService();

            ddl.DataSource = service.Where(p => p.deleteflag == 0 && p.FCompanyId == companydId);
            ddl.DataTextField = "org_name";
            ddl.DataValueField = "id";//"id";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }

        #endregion

        /// <summary>
        ///     结算方案
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="code">客户代码</param>
        /// <param name="type">业务类型</param>
        public static void DropDownListTubePriceDataBind(DropDownList ddl,string code,string type)
        {
            //var companydId = -1;
            //var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            //if (formsPrincipal != null)
            //{
            //    companydId = formsPrincipal.UserInfo.AccountComId;
            //}

            ddl.Items.Clear();

            var service = new TubePriceService();

            ddl.DataSource = service.Where(p => p.FCode == code && p.FBill == type);
            ddl.DataTextField = "FMemo";
            ddl.DataValueField = "FId";//"id";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }

        /// <summary>
        ///     省
        /// </summary>
        /// <param name="ddlFProvince"></param>
        public static void DropDownListProvinceDataBind(DropDownList ddl)
        {
            ddl.Items.Clear();

            var service = new ProvinceService();

            ddl.DataSource = service.Where(p => p.p_code==0);
            ddl.DataTextField = "city_name";//省
            ddl.DataValueField = "id";//"id";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        #region 生产车间
        /// <summary>
        ///     生产车间
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListWorkshopDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new OrgnizationService();

            ddl.DataSource = service.Where(p => p.deleteflag == 0 && p.FCompanyId == companydId);
            ddl.DataTextField = "org_name";
            ddl.DataValueField = "org_name";//"id";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }
        #endregion

        #region 产品分类
        /// <summary>
        ///     产品分类
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListItemsCateDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1000" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FId";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }
        #endregion

        #region 作业区
        /// <summary>
        ///     作业区
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDistributionPointDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1026" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FKey";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }
        #endregion

        /// <summary>
        ///     客户分部门
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="code"></param>
        public static void DropDownListOrgDataBind(DropDownList ddl,string code)
        {
            //var companydId = -1;
            //var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            //if (formsPrincipal != null)
            //{
            //    companydId = formsPrincipal.UserInfo.AccountComId;
            //}

            ddl.Items.Clear();

            var service = new CustomerOrgService();

            ddl.DataSource = service.Where(p => p.FCode == code);
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FId";
            ddl.DataBind();

            ddl.SelectedIndex = 0;

            //ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }

        /// <summary>
        ///     客户多址
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="code"></param>
        public static void DropDownListAddressDataBind(DropDownList ddl, string code)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new CustomerLinkService();

            ddl.DataSource = service.Where(p => p.FCode == code&&p.FCompanyId== companydId);
            ddl.DataTextField = "FAddress";
            ddl.DataValueField = "FAddress";
            ddl.DataBind();

            ddl.SelectedIndex = 0;

            //ddl.Items.Insert(0, new ListItem(@"", "-1"));

        }

        #region 通过钢瓶名称查钢瓶代码
        /// <summary>
        ///     通过钢瓶名称查钢瓶代码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetBottleCodeByName(string name)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            var service = new SqlService();
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@companyId", companydId);
            parms.Add("@FName", name);

            return service.ExecuteProcedureCommand("proc_GetBottle", parms).Tables[0].Rows[0]["FCode"].ToString();

        }
        #endregion

        /// <summary>
        ///     绑定气瓶编码组合集合
        /// </summary>
        /// <param name="ddl">DropDownList</param>
        public static void DropDownListBottleDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();
            //var service = new ItemsService();

            var service= new SqlService();
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@companyId",companydId);
            parms.Add("@FCateId",2001);

            //钢瓶集合
            ddl.DataSource = service.ExecuteProcedureCommand("proc_SelectorBottle", parms).Tables[0].DefaultView;//service.Where(p => p.FCateId == 2001 && p.FFlag == 1&&p.FCompanyId==companydId);
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FCode";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     绑定气瓶编码组合集合
        /// </summary>
        /// <param name="ddl">DropDownList</param>
        public static void DropDownListBottleAllDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();
            var service = new ItemsService();

            //钢瓶集合
            ddl.DataSource = service.Where(p => p.FCateId == "2001" && p.FFlag == 1 && p.FCompanyId == companydId);
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FCode";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     绑定业务员集合
        /// </summary>
        /// <param name="ddl">DropDownList</param>
        public static void DropDownListSalesmanDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();
            //业务员
            var salesmans = new EmployeeService().Where(p => p.deleteflag == 0&&p.FCompanyId==companydId && p.professional.Contains("业务员"));

            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "name";
            ddl.DataValueField = "name";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     押运员
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListSupercargoDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new EmployeeService().Where(p => p.deleteflag == 0&&p.FCompanyId==companydId && p.professional.Contains("押运员"));

            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "name";
            ddl.DataValueField = "name";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     仓库管理员
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListGodownKeeperDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new EmployeeService().Where(p => p.deleteflag == 0 && p.FCompanyId == companydId//
                && p.professional.Contains("仓库管理员"));

            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "name";
            ddl.DataValueField = "name";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     司机
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDriverDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new EmployeeService().Where(p => p.deleteflag == 0&&p.FCompanyId==companydId//
                && p.professional.Contains("司机"));

            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "name";
            ddl.DataValueField = "name";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }


        /// <summary>
        ///     调度员（）
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDispatcherDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new EmployeeService().Where(p => p.deleteflag == 0 && p.FCompanyId == companydId//
                && p.professional.Contains("调度"));

            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "name";
            ddl.DataValueField = "name";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }


        /// <summary>
        ///     发货人
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListShipperDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new EmployeeService().Where(p => p.deleteflag == 0&&p.FCompanyId==companydId);

            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "name";
            ddl.DataValueField = "name";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     生产者
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListProducerDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new EmployeeService().Where(p => p.deleteflag == 0&&p.FCompanyId==companydId && p.professional.Contains("充装工"));
            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "name";
            ddl.DataValueField = "name";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        public static void DropDownListGroupDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1031" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FKey"; //"FEx01";
            //ddl.DataSimulateTreeLevelField = "FEx02";

            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     检测员
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListSurveyorDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }


            ddl.Items.Clear();

            //业务员
            var salesmans = new EmployeeService().Where(p => p.deleteflag == 0&&p.FCompanyId==companydId);

            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "name";
            ddl.DataValueField = "name";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     绑定客户档案
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListCustomerDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new CustomerService().Where(p => p.FFlag == 1&&p.FCompanyId==companydId);

            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FCode";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }
        /// <summary>
        ///     供应商列表
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListSupplierDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new SupplierService().Where(p => p.FFlag == 1&&p.FCompanyId==companydId);

            //钢瓶集合
            ddl.DataSource = salesmans;
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FCode";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     产品集合
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListItemDataBind(DropDownList ddl)
        {
            ddl.Items.Clear();

            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            var data =
                new SqlService().Where(
                    string.Format(
                        @"SELECT FCode,FName+'/'+FSpec+'/'+FUnit AS FName FROM dbo.LHItems WHERE FFlag=1 AND FCompanyId={0}",
                        companydId));

            ddl.DataSource = data.Tables[0].AsDataView();
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FCode";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        public static void DropDownListLiquidDataBind(DropDownList ddl)
        {
            ddl.Items.Clear();

            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            var data =
                new SqlService().Where(
                    string.Format(
                        @"SELECT FCode,FName+'/'+FSpec+'/'+FUnit AS FName FROM dbo.LHItems WHERE FFlag=1 AND FIsLiquid='槽车液体' AND FCompanyId={0}",
                        companydId));

            ddl.DataSource = data.Tables[0].AsDataView();
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FCode";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        public static void DropDownListTubeDataBind(DropDownList ddl)
        {
            ddl.Items.Clear();

            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            var data =
                new SqlService().Where(
                    string.Format(
                        @"SELECT FCode,FName+'/'+FSpec+'/'+FUnit AS FName FROM dbo.LHItems WHERE FFlag=1 AND FIsLiquid='排管车' AND FCompanyId={0}",
                        companydId));

            ddl.DataSource = data.Tables[0].AsDataView();
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FCode";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     车牌号
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListVehicleNumDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new VehicleService().Where(p => p.FFlag == 1&&p.FCompanyId==companydId);
            ddl.DataSource = salesmans;
            ddl.DataTextField = "FNum";
            ddl.DataValueField = "FNum";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     物流公司
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListLogisticsDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1028" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            //ddl.Items.Insert(0, new ListItem(@"", "-1"));

            ddl.SelectedValue = "1009";
        }

        /// <summary>
        ///     单据状态
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListBillStatusDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1007"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        public static void DropDownListBillStatusDataBind(DropDownList ddl, string strCode)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == strCode&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     配送车辆类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDispatchVehicleClassDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }


            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1017"&& p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     车辆类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListVehicleClassDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1018"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     车辆状态
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListRunStatusDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1027" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     检验类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListCheckDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1019"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     订单类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListBillDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();

            ddl.DataSource = service.Where(p => p.FSParent == "1029" && p.FCompanyId == companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     维修类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListServiceClassDataBind(DropDownList ddl)
        {

            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1020"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     维修类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListServiceStatusDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1021"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     审核员
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListAuditorsDataBind(DropDownList ddl)
        {

            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            //业务员
            var salesmans = new AccountService().Where(p => p.deleteflag == 0 &&p.FCompanyId==companydId);// && p.professional == 6

            ddl.DataSource = salesmans;
            ddl.DataTextField =  "account_name";
            ddl.DataValueField = "account_number";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     区域
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListAreasDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1004"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     配送方式
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListDeliveryMethodDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1009"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

            ddl.SelectedValue = "配送";

        }

        /// <summary>
        ///     调拨种类
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListAllotWayDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1010"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        
        }
        /// <summary>
        ///     空瓶回收
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListReturnDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1016"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));

            ddl.SelectedValue = "正常回收";


        }

        /// <summary>
        ///     出仓类别
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListStockOutCateDataBind(DropDownList ddl)
        {

            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1011"&&p.FCompanyId== companydId && p.FFlag == 1);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }
        /// <summary>
        /// 产品类型
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownItemTypeDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();
            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1000" && p.FCompanyId == companydId && p.FId.Contains("2000"));
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FId";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        /// <summary>
        ///     入仓类别
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListStockInCateDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1012"&&p.FCompanyId==companydId &&p.FFlag==1 );
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }
        /// <summary>
        ///     银行账户
        /// </summary>
        /// <param name="ddl"></param>
        public static void DropDownListBankSubjectDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            string date = DateTime.Now.ToString("yyyy-MM");

            var service = new SubjectService();
            //ddl.DataSource = service.Where(p =>p.FCompanyId==companydId  && p.FFlag==1 && (p.FCode == "0000100003" || p.FCode == "0000100004" || p.FParentCode == "0000100004"));
            ddl.DataSource = service.Where(p => p.FCompanyId == companydId //
                && p.FFlag == 1 && p.FParentCode == "0000100004" && p.FDate==date);
            ddl.DataTextField = "FName";
            ddl.DataValueField = "FCode";
            ddl.DataBind();

            //不选择
            //ddl.Items[1].EnableSelect = false;
            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        public static void DropDownListLossessCateDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1013"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        public static void DropDownListProfitCateDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new ProjectItemsService();
            ddl.DataSource = service.Where(p => p.FSParent == "1014"&&p.FCompanyId==companydId);
            ddl.DataTextField = "FKey";
            ddl.DataValueField = "FValue";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        public static void DropDownListCompanyDataBind(DropDownList ddl)
        {
            var companydId = -1;
            var formsPrincipal = FormsPrincipal<LoginUser>.GetAccount(HttpContext.Current);
            if (formsPrincipal != null)
            {
                companydId = formsPrincipal.UserInfo.AccountComId;
            }

            ddl.Items.Clear();

            var service = new CompanyService();
            ddl.DataSource = service.Where(p => p.deleteflag==0);
            ddl.DataTextField = "com_name";
            ddl.DataValueField = "id";
            ddl.DataBind();

            ddl.SelectedValue = companydId.ToString(CultureInfo.InvariantCulture);
            ddl.Readonly = true;

            //ddl.Items.Insert(0, new ListItem(@"", "-1"));
        }

        #endregion

        /// <summary>
        ///     取销售产品
        /// </summary>
        /// <param name="item">商品代码/名称/助记码</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static DataSet GetSalesItem(string item,int companyId)
        {
            var sqlService= new SqlService();

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@item",item);
            parms.Add("@companyId", companyId);
            return sqlService.ExecuteProcedureCommand("proc_SalesItem", parms);
        }

        /// <summary>
        ///     采购产品
        /// </summary>
        /// <param name="item"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseItem(string item, int companyId)
        {
            var sqlService = new SqlService();

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@item", item);
            parms.Add("@companyId", companyId);
            return sqlService.ExecuteProcedureCommand("proc_PurchaseItem", parms);
        }

        /// <summary>
        ///     客户对应产品销售价
        /// </summary>
        /// <param name="code"></param>
        /// <param name="item"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public static Decimal GeCustomerPrice(string code, string item,int companyid)
        {
            var sqlService = new SqlService();

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@Code",code);
            parms.Add("@FItemCode", item);
            parms.Add("@companyid",companyid);

            return Convert.ToDecimal(sqlService.ExecuteProcedureCommand("proc_CustomerPrice", parms).Tables[0].Rows[0][0]);
        }

        /// <summary>
        ///     供应商采购价
        /// </summary>
        /// <param name="code"></param>
        /// <param name="item"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public static Decimal GeSupplierPrice(string code, string item, int companyid)
        {
            if (item == "-1")
                return 0;

            var sqlService = new SqlService();

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@Code",code);
            parms.Add("@FItemCode", item);
            parms.Add("@companyid", companyid);
            return Convert.ToDecimal(sqlService.ExecuteProcedureCommand("proc_SupplierPrice", parms).Tables[0].Rows[0][0]);
        }
        
        /// <summary>
        ///     单据状态流水
        /// </summary>
        /// <param name="status"></param>
        public static void AddBillStatus(LHBillStatus status)
        {
            var service= new BillStatusService();

            service.Add(status);
        }

        /// <summary>
        ///     单据审批流水
        /// </summary>
        /// <param name="flow"></param>
        public static void AddBillFlow(LHBillFlow flow)
        {
            BillFlowServie servie= new BillFlowServie();

            servie.Add(flow);
        }

        /// <summary>
        ///     取多选下拉框输入的值
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string GetDropDownListArrayString(ListItem[] array)
        {
            var sb = new StringBuilder();
            foreach (var item in array)
            {
                sb.Append(item.Text);
                sb.Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        ///     默认包装物
        /// </summary>
        /// <param name="code"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static LHItemsMapBottle BottleByCode(string code,int companyId)
        {
            var service = new ItemsMapBottleService();

            var bottle = service.FirstOrDefault(p => p.FCode == code && p.FCompanyId == companyId);

            if (bottle != null)
            {
                return bottle;
            }

            return null;
        }

        
    }
}