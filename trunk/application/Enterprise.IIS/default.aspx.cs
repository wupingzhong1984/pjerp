using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Security;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Charts.Charts.Column;
using Enterprise.Charts.Charts.Doughnut;
using Enterprise.Charts.Charts.Line;
using Enterprise.Charts.Charts.Pie;
using TreeNode = FineUI.TreeNode;
using FineUI;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS
{
    // ReSharper disable once InconsistentNaming
    public partial class _default : PageBase
    {
        private BulletinItemService _bulletinItemService;
        /// <summary>
        /// 
        /// </summary>
        protected BulletinItemService BulletinItemService
        {
            get { return _bulletinItemService ?? (_bulletinItemService = new BulletinItemService()); }
            set { _bulletinItemService = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private BulletinService _bulletinService;
        protected BulletinService BulletinService
        {
            get { return _bulletinService ?? (_bulletinService = new BulletinService()); }
            set { _bulletinService = value; }
        }

        public string SystemName;
        /// <summary>
        ///     
        /// </summary>
        private base_configset _baseConfigset;

        /// <summary>
        ///     
        /// </summary>
        private ConfigsetService _configsetService;

        /// <summary>
        ///     
        /// </summary>
        protected ConfigsetService ConfigsetService
        {
            get { return _configsetService ?? (_configsetService = new ConfigsetService()); }
            set { _configsetService = value; }
        }

        /// <summary>
        ///     
        /// </summary>

        protected int Key
        {
            get { return 1; }
        }

        /// <summary>
        ///      字典信息    
        /// </summary>
        protected base_configset BaseConfigset
        {
            get { return _baseConfigset ?? (_baseConfigset = ConfigsetService.FirstOrDefault(p => p.id == Key)); }
            set { _baseConfigset = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private MenuService _menuService;

        /// <summary>
        ///     
        /// </summary>
        private RoleService _roleService;

        private string _menuType = "menu";

        /// <summary>
        ///     
        /// </summary>
        protected MenuService MenuService
        {
            get { return _menuService ?? (_menuService = new MenuService()); }
            set { _menuService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        protected RoleService RoleService
        {
            get { return _roleService ?? (_roleService = new RoleService()); }
            set { _roleService = value; }
        }

        private CustomFunctionService _customFunctionService;
        protected CustomFunctionService CustomFunctionService
        {
            get { return _customFunctionService ?? (_customFunctionService = new CustomFunctionService()); }
            set { _customFunctionService = value; }
        }


        private CompanyService _companyService;
        protected CompanyService CompanyService
        {
            get { return _companyService ?? (_companyService = new CompanyService()); }
            set { _companyService = value; }
        }

        #region Page_Init

        protected void Page_Init(object sender, EventArgs e)
        {
            HttpCookie menuCookie = Request.Cookies["MenuStyle_v4"];

            if (menuCookie != null)
            {
                _menuType = menuCookie.Value;
            }

            if (_menuType == "accordion")
            {
                InitAccordionMenu();
            }
            else
            {
                InitTreeMenu();
            }

            UserFunction();

        }

        /// <summary>
        ///     用户常用功能
        /// </summary>
        private void UserFunction()
        {
            var functions = CustomFunctionService.Where(p => p.FAccountId == CurrentUser.Id).ToList();
            int i = 0;
            foreach (var fun in functions)
            {
                if (i > 18)
                {
                    break;
                }
                var menu = MenuService.Where(p => p.id == fun.FMenuId).FirstOrDefault();

                var btn = new Button();

                btn.CssClass = "bgbtn2";
                btn.ID = "btn" + fun.FId;
                btn.MenuID = "btn" + fun.FId;
                btn.Text = menu.menu_name;

                string url = string.Empty;
                var urls = menu.menu_url.Split(',');

                if (urls.Count() > 1)
                {
                    url = urls[1];
                }
                else
                {
                    url = urls[0];
                }
                btn.OnClientClick = string.Format("openWindow('{0}?action=1', '{1}')", url, menu.menu_name);

                if (!string.IsNullOrEmpty(menu.menu_class_icon))
                {
                    btn.IconUrl = menu.menu_class_icon;//
                }
                else
                {
                    btn.IconUrl = "~/images/System/LH-160621-0020.png";
                }

                btn.IconAlign = IconAlign.Top;

                if (i == 11 || i == 0)
                {
                    Toolbar bar = null;
                    if (Panel6.Toolbars.Count > 2)
                    {
                        break;
                    }
                    bar = new Toolbar();
                    Panel6.Toolbars.Add(bar);
                    Panel6.Toolbars[0].Items.Add(btn);
                }
                else
                {
                    if (i <= 10)
                        Panel6.Toolbars[0].Items.Add(btn);
                    else
                    {
                        Panel6.Toolbars[1].Items.Add(btn);
                    }
                }
                i++;
            }

            var btnAdd = new Button();
            btnAdd.Click += btnAdd_Click;
            btnAdd.CssClass = "bgbtn2";
            btnAdd.Text = "常用功能";
            btnAdd.IconUrl = "~/images/AddIcon.png";
            btnAdd.IconAlign = IconAlign.Top;
            if (Panel6.Toolbars.Count != 0)
            {
                if (Panel6.Toolbars.Count >= 2)
                {
                    Panel6.Toolbars[1].Items.Add(btnAdd);
                    Panel6.Height = 206;
                }
                else
                {
                    Panel6.Toolbars[0].Items.Add(btnAdd);
                    Panel6.Height = 103;
                }
            }
            else
            {
                Panel6.Height = 103;
                var bar = new Toolbar();
                Panel6.Toolbars.Add(bar);
                Panel6.Toolbars[0].Items.Add(btnAdd);
            }
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            //重新登录系统
            if (!Page.User.Identity.IsAuthenticated)
            {
                //清楚缓存
                Response.Expires = 0;
                Response.AddHeader("pragma", "no-cache");
                Response.AddHeader("cache-control", "private");
                Response.CacheControl = "no-cache";
                FormsAuthentication.RedirectToLoginPage();
            }

            if (!IsPostBack)
            {

                

                // ---------------------------
                //lblIpaddress.Text = XAccount.account_ip_addres;
                //lblLastLogin.Text = XAccount.account_last_date.ToString();
                //lblLogin.Text = XAccount.account_number;
                //lblRegDate.Text = XAccount.createdon.ToString();
                //lblRoles.Text = XAccount.account_role_name;
                //lblUsername.Text = XAccount.account_name;

                //Timer1.Enabled = true;

                BindDataGrid();

                BindDataAuditGrid();

                btnPassword.OnClientClick =
                    window1.GetShowReference("~/product/account/changepassword.aspx", "修改密码");

                ltlUser.Text = String.Format("{0}", CurrentUser.AccountName);

                var company = CompanyService.FirstOrDefault(p => p.id == CurrentUser.AccountComId);
                if (company != null)
                {
                    lblSystemName.Text = company.com_name;
                }

                ltlVersion.Text = BaseConfigset.version;

                ltlOnline.Text = string.Format("{0}", XAccount.account_ip_addres);
            }
        }

        /// <summary>
        ///     密码修改后重新登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
        }

        private void InitTreeMenu()
        {
            //根据角色查询权限
            if (CurrentUser == null)
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("~/login.html");
            }

            if (CurrentUser == null)
                return;

            var roleId = Int32.Parse(CurrentUser.AccountRoleId);

            //根据用户角色加载相应的菜单项
            var role = RoleService.FirstOrDefault(p => p.id == roleId);

            if (role == null)
                return;

            var menuArr = Array.ConvertAll(
                (role.role_menu_code_p + role.role_menu_code.TrimEnd(',')).Split(',').ToArray(),
                Int32.Parse);

            IList<base_menu> menus = MenuService.Where(p => menuArr.Contains(p.id) && p.deleteflag == 0 && p.menu_is_view == 1).ToList();


            var leftMenuTree = new Tree
            {
                ShowBorder = false,
                EnableSingleClickExpand = true,
                ShowHeader = false,
            };

            leftPanel.Items.Add(leftMenuTree);

            foreach (base_menu menu in menus.Where(p => p.menu_parent_id == 0).OrderBy(p => p.menu_sort))
            {
                var innerTree = new TreeNode
                {
                    Text = menu.menu_name,
                    IconUrl = menu.menu_class,
                };

                leftMenuTree.Nodes.Add(innerTree);

                int pId = menu.id;
                foreach (var menu1 in menus.Where(p => p.menu_parent_id == pId).OrderBy(p => p.menu_sort))
                {
                    var node = new TreeNode
                    {
                        Text = menu1.menu_name,
                        NavigateUrl = menu1.menu_url.Split(',')[0],
                        IconUrl = menu1.menu_class
                    };

                    innerTree.Nodes.Add(node);
                }
            }
        }

        private void InitAccordionMenu()
        {
            var accordionMenu = new Accordion
            {
                ID = "accordionMenu",
                EnableFill = false,
                ShowBorder = false,
                ShowHeader = false,
                AutoScroll = true,
            };

            //根据角色查询权限
            if (CurrentUser == null)
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("~/login.html");
            }

            if (CurrentUser == null) return;

            var roleId = Int32.Parse(CurrentUser.AccountRoleId);

            //根据用户角色加载相应的菜单项
            var role = RoleService.Single(p => p.id == roleId);

            if (role == null) return;

            var menuArr = Array.ConvertAll(
                (role.role_menu_code_p + role.role_menu_code.TrimEnd(',')).Split(',').ToArray(),
                Int32.Parse);

            IList<base_menu> menus = MenuService.Where(p => menuArr.Contains(p.id) && p.deleteflag == 0 && p.menu_is_view == 1).ToList();

            foreach (base_menu menu in menus.Where(p => p.menu_parent_id == 0).OrderBy(p => p.menu_sort))
            {
                var accordionPane = new AccordionPane
                {
                    Title = menu.menu_name,
                    Layout = Layout.Fit,
                    ShowBorder = false,
                    BodyPadding = "2px 0 0 0",
                    IconUrl = menu.menu_class,
                };
                accordionMenu.Items.Add(accordionPane);

                var innerTree = new Tree
                {
                    EnableArrows = true,
                    ShowBorder = false,
                    ShowHeader = false,
                    EnableIcons = false,
                    AutoScroll = true
                };

                int pId = menu.id;
                foreach (var menu1 in menus.Where(p => p.menu_parent_id == pId).OrderBy(p => p.menu_sort))
                {
                    var node = new TreeNode
                    {
                        Text = menu1.menu_name,
                        Target = menu1.menu_is_frame_view == 1 ? "main" : "_blank",
                        NavigateUrl = menu1.menu_url.Split(',')[0],
                        IconUrl = menu1.menu_class
                    };

                    innerTree.Nodes.Add(node);
                }

                accordionPane.Items.Add(innerTree);
            }

            //return accordionMenu;
        }

        #endregion

        #region Event

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                PageContext.RegisterStartupScript(
                    window3.GetShowReference(string.Format("./Common/WinCustomFunction.aspx"), "常用功能"));


            }
            catch (Exception)
            {
                Alert.Show("添加常用功能失败！", MessageBoxIcon.Warning);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Response.Redirect("~/login.html");

            //清楚缓存
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");
            Response.AddHeader("cache-control", "private");
            Response.CacheControl = "no-cache";
        }

        /// <summary>
        ///     下载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownloadPrinter_Click(object sender, EventArgs e)
        {

            string printer = "lodop32.zip";
            string filePath = Server.MapPath("res/CLodop_Setup_for_Win32NT_https_2.068.zip");
            //var fs = new FileStream(filePath, FileMode.Open);
            //var bytes = new byte[(int)fs.Length];
            //fs.Read(bytes, 0, bytes.Length);
            //fs.Close();
            //Response.ContentType = "application/octet-stream";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" //
            //    + HttpUtility.UrlEncode(printer, System.Text.Encoding.UTF8));
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();
            //--------------------------
            var fileInfo = new FileInfo(filePath);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + printer);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            Response.End();

        }

        #endregion

        //---------------------------
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();

                    if (e.CommandName == "actView")
                    {
                        PageContext.RegisterStartupScript(string.Format("openAddFineUI('./product/bulletin/view.aspx?action=6&sid={0}');", sid));
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("查看失败！", MessageBoxIcon.Warning);
            }
        }
        //---------------------------
        private void BindDataGrid()
        {
            int output;

            dynamic orderingSelector;
            Expression<Func<base_bulletin, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<base_bulletin> list = BulletinService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            Grid1.RecordCount = output;

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        /// <summary>
        ///     创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<base_bulletin, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_bulletin));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            
            int sid = Convert.ToInt32(CurrentUser.Id);

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "receiver"), Expression.Constant(sid, typeof(int?))));

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "deleteflag"), Expression.Constant(0, typeof(int?))));

            Expression<Func<base_bulletin, bool>> predicate = Expression.Lambda<Func<base_bulletin, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "id";
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
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "DESC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        #region 审核


        /// <summary>
        ///     审核
        /// </summary>
        private void BindDataAuditGrid()
        {
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FCompanyId", CurrentUser.AccountComId);
            parms.Add("@FNum", CurrentUser.AccountName);

            var list = SqlService.ExecuteProcedureCommand("proc_AuditDetails", parms);

            //绑定数据源
            Grid2.DataSource = list;
            Grid2.DataBind();
        }


        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (Grid2.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string KeyId = Grid2.DataKeys[Grid2.SelectedRowIndexArray[0]][0].ToString();

                    string url = Grid2.DataKeys[Grid2.SelectedRowIndexArray[0]][1].ToString();

                    if (e.CommandName == "actView")
                    {
                        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}','{1}');", url, KeyId));
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("查看失败！", MessageBoxIcon.Warning);
            }
        }


        #endregion

        protected void Timer1_OnTickOnTick(object sender, EventArgs e)
        {
            BindDataGrid();

            BindDataAuditGrid();
        }
    }
}