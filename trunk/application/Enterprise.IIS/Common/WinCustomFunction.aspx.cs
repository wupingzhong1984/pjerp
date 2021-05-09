using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Data;
using FineUI;

namespace Enterprise.IIS.Common
{
    public partial class WinCustomFunction : PageBase
    {

        private CustomFunctionService _customFunctionService;
        protected CustomFunctionService CustomFunctionService
        {
            get { return _customFunctionService ?? (_customFunctionService = new CustomFunctionService()); }
            set { _customFunctionService = value; }
        }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

                InitTreeMenu();
            }
        }

        protected void btnResult_Click(object sender, EventArgs e) 
        {
            if (CustomFunctionService.Delete(p => p.FAccountId == CurrentUser.Id)) 
            {
                Alert.Show("已全部清空定义常用功能，请重新定义。", MessageBoxIcon.Information);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                foreach (var pnode in leftMenuTree.Nodes)
                {
                    foreach (var node in pnode.Nodes)
                    {
                        if (node.Checked)
                        {
                            int id = Convert.ToInt32(node.NodeID);

                            var customerFun = CustomFunctionService.Where(p => p.FMenuId == id).FirstOrDefault();

                            if (customerFun == null)
                            {
                                var fun = new base_custom_function
                                {
                                    FAccountId = CurrentUser.Id,
                                    FMenuId = Convert.ToInt32(node.NodeID)
                                };

                                CustomFunctionService.AddEntity(fun);
                            };
                        }
                    }
                }

                CustomFunctionService.SaveChanges();

                isSucceed = true;
            }
            catch (Exception)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        private void InitTreeMenu()
        {
            if (CurrentUser == null)
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("~/login.html");
            }

            if (CurrentUser == null)
                return;

            var roleId = Int32.Parse(CurrentUser.AccountRoleId);

            var role = RoleService.Single(p => p.id == roleId);

            if (role == null)
                return;

            var menuArr = Array.ConvertAll(
                (role.role_menu_code_p + role.role_menu_code.TrimEnd(',')).Split(',').ToArray(),
                Int32.Parse);

            IList<base_menu> menus = MenuService.Where(p => menuArr.Contains(p.id) && p.deleteflag == 0 && p.menu_is_view == 1).ToList();

            foreach (base_menu menu in menus.Where(p => p.menu_parent_id == 0).OrderBy(p => p.menu_sort))
            {
                var innerTree = new TreeNode
                {
                    Text = menu.menu_name,
                };

                leftMenuTree.Nodes.Add(innerTree);

                int pId = menu.id;
                foreach (var menu1 in menus.Where(p => p.menu_parent_id == pId).OrderBy(p => p.menu_sort))
                {
                    var node = new TreeNode
                    {
                        Text = menu1.menu_name,
                        NodeID = menu1.id.ToString(),
                        IconUrl = menu1.menu_class,
                        EnableCheckBox = true
                    };

                    innerTree.Nodes.Add(node);
                }
            }
        }
    }
}