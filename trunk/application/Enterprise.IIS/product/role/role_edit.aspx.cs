using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FineUI;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using AspNet = System.Web.UI.WebControls;
using Enterprise.Data;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.role
{
// ReSharper disable once InconsistentNaming
    public partial class role_edit : PageBase
    {

        /// <summary>
        ///     角色信息
        /// </summary>
        private base_role _baseRole;

        /// <summary>
        ///     菜单列表
        /// </summary>
        private IList<base_menu> _menus;

        /// <summary>
        ///     菜单数据服务
        /// </summary>
        private MenuService _menuService;

        /// <summary>
        ///     角色数据服务
        /// </summary>
        private RoleService _roleService;

        /// <summary>
        ///     菜单数据服务
        /// </summary>
        protected MenuService MenuService
        {
            get { return _menuService ?? (_menuService = new MenuService()); }
            set { _menuService = value; }
        }

        /// <summary>
        ///     角色数据服务
        /// </summary>
        protected RoleService RoleService
        {
            get { return _roleService ?? (_roleService = new RoleService()); }
            set { _roleService = value; }
        }

        /// <summary>
        ///     角色信息    
        /// </summary>
        protected base_role BaseRole
        {
            get { return _baseRole ?? (_baseRole = RoleService.FirstOrDefault(p => p.id == RoleId)); }
            set { _baseRole = value; }
        }

        /// <summary>
        ///     当前角色对象ID
        /// </summary>
        protected Int32 RoleId
        {
            get { return Int32.Parse(Request["id"]); }
        }

        /// <summary>
        ///     当前画面操作项
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction)int.Parse(s);
            }
        }

        /// <summary>
        ///     菜单列表
        /// </summary>
        protected IList<base_menu> Menus
        {
            get
            {
                return _menus ?? (_menus = MenuService.All().ToList());
            }
            set { _menus = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //设置控件属性状态
                SetControlAttributeValue();

                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
        }

        private void SetControlAttributeValue()
        {
            txtrole_name.Focus();
        }

        /// <summary>
        ///     初始化数据
        /// </summary>
        private void InitData()
        {
            IList<base_menu> menus = new List<base_menu>();
            foreach (var menu1 in Menus.Where(p => p.menu_parent_id == 0).OrderBy(p=>p.menu_sort))
            {
                int pId = menu1.id;
                menus.Add(menu1);
                foreach (var menu2 in Menus.Where(p => p.menu_parent_id == pId).OrderBy(p => p.menu_sort))
                {
                    menus.Add(menu2);
                }
            }
            // 加载所有业务
            Grid1.DataSource = menus;
            Grid1.DataBind();

            // 加载功能项
            var list = new AcitonService().Where(p => p.deleteflag == 0).ToList();
            cboxListActions.DataSource = list;
            cboxListActions.DataBind();
        }

        /// <summary>
        ///     加载所有数据列表
        /// </summary>
        private void LoadData()
        {
            //取前权限信息
            if (BaseRole != null)
            {
                txtrole_name.Text = BaseRole.role_name;
                txtrole_desc.Text = BaseRole.role_desc;
            }
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            var cboxActions = (AspNet.CheckBoxList)Grid1.Rows[e.RowIndex].FindControl("cbxlActions");

            if (cboxActions != null)
            {
                var menu = (base_menu)e.DataItem;

                if (!string.IsNullOrEmpty(menu.menu_actions))
                {
                    foreach (string action in menu.menu_actions.TrimEnd(',').Split(','))
                    {
                        // 菜单功能为：功能Name|功能En|功能ID
                        string[] actionArr = action.Split('|');
                        
                        string actionName = actionArr[0];
                        string actionEn = actionArr[1];
                        string actionId = actionArr[2];
                        // 创建项， 显示名称为：功能Name，值为：功能En^功能IDM菜单ID
                        var item = new AspNet.ListItem(actionName, string.Format("{0}^{1}M{2}", actionEn, actionId, menu.id));

                        if (BaseRole != null)
                        {
                            //验证是否有相应的权限
                            if (BaseRole.role_action.Contains(string.Format("^{0}M{1}", actionId, menu.id)))
                                item.Selected = true;
                        }

                        cboxActions.Items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        ///     提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSava_Click(object sender, EventArgs e)
        {
            // 选中项
            var sbRoleAction = new StringBuilder();
            // 二级菜单
            var sbRoleMenuCode = new StringBuilder();
            // 一级菜单
            var sbRoleMenuCodeP = new StringBuilder();

            foreach (GridRow row in Grid1.Rows)
            {
                var cboxActions = row.FindControl("cbxlActions") as AspNet.CheckBoxList;

                //如果找到该控件，取上面数据
                if (cboxActions != null)
                {
                    foreach (AspNet.ListItem item in cboxActions.Items)
                    {
                        if (!item.Selected)
                            continue;
                        //功能En^功能IDM菜单ID
                        string[] actionidMenuid = item.Value.Split('M');

                        sbRoleAction.AppendFormat("{0}|{1},", item.Text, item.Value);

                        if (!sbRoleMenuCode.ToString().Contains(actionidMenuid[1]))
                        {
                            sbRoleMenuCode.AppendFormat("{0},", actionidMenuid[1]);
                        }

                        int id = Int32.Parse(actionidMenuid[1]);
                        int? menuParentId = Menus.Single(p => p.id == id).menu_parent_id;
                        if (menuParentId != null)
                        {
                            string pId = menuParentId.Value.ToString(CultureInfo.InvariantCulture);

                            if (!sbRoleMenuCodeP.ToString().Contains(pId))
                            {
                                sbRoleMenuCodeP.AppendFormat("{0},", pId);
                            }
                        }
                    }
                }

            }

            // 保存角色
            switch (Actions)
            {
                case WebAction.Add:
                    BaseRole = new base_role
                    {
                        role_menu_code = sbRoleMenuCode.ToString(),
                        role_action = sbRoleAction.ToString(),
                        role_menu_code_p = sbRoleMenuCodeP.ToString(),
                        role_name = txtrole_name.Text.Trim(),
                        role_desc = txtrole_desc.Text.Trim(),
                        deleteflag = 0,
                        role_flag = 1,
                        FCompanyId = CurrentUser.AccountComId,
                        FFlag = 1
                    };
                    if (RoleService.Add(BaseRole))
                    {
                        Alert.Show("提交成功。", MessageBoxIcon.Information);
                    }
                    else
                    {
                        Alert.Show("提交失败！", MessageBoxIcon.Error);
                    }
                    break;
                case WebAction.Edit:
                    BaseRole.role_menu_code = sbRoleMenuCode.ToString();
                    BaseRole.role_action = sbRoleAction.ToString();
                    BaseRole.role_menu_code_p = sbRoleMenuCodeP.ToString();
                    BaseRole.role_name = txtrole_name.Text.Trim();
                    BaseRole.role_desc = txtrole_desc.Text.Trim();
                    if (RoleService.SaveChanges() >= 0)
                    {
                        Alert.Show("提交成功。", MessageBoxIcon.Information);
                    }
                    else
                    {
                        Alert.Show("提交失败！", MessageBoxIcon.Error);
                    }
                    break;
                case WebAction.Details:
                    break;
            }
        }

        /// <summary>
        ///     返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request["ReturnUrl"] ?? "role_list.aspx");
        }
    }
}