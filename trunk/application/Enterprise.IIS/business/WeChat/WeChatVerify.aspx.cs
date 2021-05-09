using Enterprise.IIS.Common;
using Enterprise.IIS.WeChatModel;
using FineUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Enterprise.IIS.business.WeChat
{
    public partial class WeChatVerify : System.Web.UI.Page
    {/// <summary>
     ///     选中菜单ID
     /// </summary>
        protected int MenuId
        {
            get { return int.Parse(trMenu.SelectedNodeID); }
        }

        protected WeChatService weChatService;
        protected WeChatService WeChatServices
        {
            get
            {
                if (weChatService == null)
                {
                    weChatService = new WeChatService();
                }

                return weChatService;
            }
        }

        protected string access_token;

        protected string Access_token
        {
            get
            {
                return WeChatServices.Acces_token();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTreeSource();
            }
        }

        protected void trMenu_NodeCommand(object sender, FineUI.TreeCommandEventArgs e)
        {
            Alert.Show("aprghjaeoriheoiu");
        }

        private void LoadTreeSource()
        {

            trMenu.Nodes.Clear();

            TreeNode rootNode = new TreeNode
            {
                Text = "通讯录",
                NodeID = "0",
                Expanded = true,
                //  EnablePostBack = true,
                CommandName = "0",
                EnableClickEvent = true
            };

            trMenu.Nodes.Add(rootNode);

            //加载子部门信息
            LoadChildNodes(rootNode, 1);

            //设置默认选择项
            trMenu.SelectedNodeID = rootNode.NodeID;
            trMenu_NodeCommand(null, null);
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="level"></param>
        private void LoadChildNodes(TreeNode node, int level)
        {
            string depart = WeChatServices.WeChatRequest("/department/list?access_token=" + Access_token, "", "get");
            WeChatDepartMent departMent = JsonConvert.DeserializeObject<WeChatDepartMent>(depart);

            LoadDepartmentNodes(node, departMent.department, level, 0);
        }

        private void LoadDepartmentNodes(TreeNode node, List<WeChatDepartMents> departMents, int level, int parentid)
        {
            if (parentid != 0)
            {
                level ++;
            }
            IEnumerable<TreeNode> nods = (from x in departMents
                                          where x.parentid == parentid
                                          select new TreeNode
                                          {
                                              Text = x.name,
                                              NodeID = x.id.ToString(),
                                              IconUrl = "",
                                              CommandName = level.ToString(CultureInfo.InvariantCulture),
                                              EnableClickEvent = true,
                                          });
            foreach (TreeNode item in nods)
            {
                int id = int.Parse(item.NodeID);
                string man = WeChatServices.WeChatRequest("/user/simplelist?access_token=" + Access_token + "& department_id=" + item.NodeID + "&fetch_child=0", "", "get");
                WeChatEmployee employee = JsonConvert.DeserializeObject<WeChatEmployee>(man);
                foreach (Employee us in employee.userlist)
                {
                    TreeNode tree = new TreeNode
                    {
                        Text = us.name,
                        NodeID = us.userid.ToString(),
                        IconUrl = "",
                        CommandName = (level + 1).ToString(CultureInfo.InvariantCulture),
                        EnableClickEvent = true,
                    };
                    item.Nodes.Add(tree);
                }
                //加载子部门信息
                node.Nodes.Add(item);
                //递归加载部门组织
                LoadDepartmentNodes(item, departMents, level, int.Parse(item.NodeID));
            }

        }

    }
}