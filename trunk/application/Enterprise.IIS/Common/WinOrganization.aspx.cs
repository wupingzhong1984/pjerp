using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using FineUI;
using Enterprise.Framework.Web;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using AspNet = System.Web.UI.WebControls;

namespace Enterprise.IIS.Common
{
    public partial class WinOrganization : PageBase
    {
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
        ///     公司组织数据服务
        /// </summary>
        private CompanyService _companyService;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        private OrgnizationService _orgnizationService;

        /// <summary>
        ///     部门列表
        /// </summary>
        private IList<base_orgnization> _orgnizations;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        protected OrgnizationService OrgnizationService
        {
            get { return _orgnizationService ?? (_orgnizationService = new OrgnizationService()); }
            set { _orgnizationService = value; }
        }

        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        protected CompanyService CompanyService
        {
            get { return _companyService ?? (_companyService = new CompanyService()); }
            set { _companyService = value; }
        }

        /// <summary>
        ///     部门列表
        /// </summary>
        protected IList<base_orgnization> Orgnizations
        {
            get
            {
                return _orgnizations ?? (_orgnizations = OrgnizationService.Where(p => p.deleteflag == 0).ToList());
            }
            set { _orgnizations = value; }
        }

        /// <summary>
        /// Tree1_NodeCheck
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Tree1_NodeCheck(object sender, TreeCheckEventArgs e)
        {
            if (e.Checked)
            {
                trDept.CheckAllNodes(e.Node.Nodes);
            }
            else
            {
                trDept.UncheckAllNodes(e.Node.Nodes);
            }

        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ////设置控件属性状态
                //SetControlAttributeValue();

                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
        }

        /// <summary>
        ///     初始化数据
        /// </summary>
        private void InitData()
        {
            //加载组纪机构
            LoadOrgnizationTreeSource();
        }

        /// <summary>
        /// 选中当前树的节点IDS
        /// </summary>
        /// <returns></returns>
        private string GetSelectedNodeIds()
        {
            var nodes = trDept.GetCheckedNodes();
            if (nodes.Length > 0)
            {
                var ids = new StringBuilder();
                foreach (TreeNode node in nodes)
                {
                    ids.AppendFormat("{0},", node.NodeID);
                }
                return ids.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     树绑定
        /// </summary>
        private void LoadOrgnizationTreeSource()
        {
            trDept.Nodes.Clear();

            var list = CompanyService.Where(p => p.deleteflag == 0).OrderBy(p => p.prentid).ToList();

            foreach( var company in list)
            {
                var rootNode = new TreeNode();
                rootNode.Text = company.com_name;
                rootNode.NodeID = company.id.ToString(CultureInfo.InvariantCulture);
                rootNode.EnableCheckBox = true;
                //rootNode.Icon = Icon.BulletFeed;
                rootNode.Expanded = true;

                trDept.Nodes.Add(rootNode);

                //加载子部门信息
                //LoadChildNodes(rootNode);
            };
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(TreeNode node)
        {
            var nodeId = Int32.Parse(node.NodeID);

            if (OrgnizationService.Count(p => p.org_parent_id == nodeId && p.deleteflag == 0) == 0)
            {
                node.Leaf = true;
            }
            else
            {
                //node.Expanded = true;
                node.Nodes.Clear();
                foreach (
                    base_orgnization orgnization in
                        OrgnizationService.Where(p => p.org_parent_id == nodeId && p.deleteflag == 0))
                {
                    var cNode = new TreeNode
                    {
                        Text = string.Format(@"{0}-{1}", orgnization.code, orgnization.org_name),
                        NodeID = orgnization.id.ToString(CultureInfo.InvariantCulture),
                        EnableCheckBox = true,
                        Expanded = false,
                        //Icon = Icon.BulletFeed
                    };
                    //加载子部门信息
                    node.Nodes.Add(cNode);

                    //
                    LoadChildNodes(cNode);
                }
            }
        }

        /// <summary>
        ///     加载所有数据列表
        /// </summary>
        private void LoadData()
        {

        }

        protected void Tree1_NodeCommand(object sender, TreeCommandEventArgs e)
        {
            var code = e.NodeID;
            var name = e.Node.Text;

            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(code, name) + ActiveWindow.GetHideReference());
        }

        /// <summary>
        ///     提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSava_Click(object sender, EventArgs e)
        {
            var code = trDept.SelectedNodeID;
            var name = trDept.SelectedNode.Text;

            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(code, name) + ActiveWindow.GetHideReference());
        }

        /// <summary>
        ///     返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            
        }
    }
}