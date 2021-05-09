using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Data;
using FineUI;

namespace Enterprise.IIS.Common
{
    public partial class WinSubject : PageBase
    {

        /// <summary>
        ///     会计科目
        /// </summary>
        private SubjectService _subjectService;

        /// <summary>
        ///     会计科目
        /// </summary>
        protected SubjectService SubjectService
        {
            get { return _subjectService ?? (_subjectService = new SubjectService()); }
            set { _subjectService = value; }
        }

        private IList<LHSubject> _subjects;
        /// <summary>
        ///     部门列表
        /// </summary>
        protected IList<LHSubject> Subjects
        {
            get
            {
                return _subjects ?? (_subjects = SubjectService.Where(p => p.FCompanyId == CurrentUser.AccountComId
                    ).ToList());
            }
            set { _subjects = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
        }

        #region Protected Method

        /// <summary>
        ///     资产类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trAsset_NodeCommand(object sender, TreeCommandEventArgs e)
        {
        }

        /// <summary>
        ///     负债类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trLiabilities_NodeCommand(object sender, TreeCommandEventArgs e)
        {
        }
        /// <summary>
        ///     损益类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trLoss_NodeCommand(object sender, TreeCommandEventArgs e)
        {
        }

        /// <summary>
        ///     成本类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trCost_NodeCommand(object sender, TreeCommandEventArgs e)
        {
        }

        /// <summary>
        ///     所有者权益类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trEquity_NodeCommand(object sender, TreeCommandEventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Subjects = SubjectService.All().ToList();

            LoadData();
        }

        /// <summary>
        ///     确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int index = TabStrip1.ActiveTabIndex;

                TreeNode[] nodes = null;

                switch (index)
                {
                    case 0://【资产类】

                        nodes = trAsset.GetCheckedNodes();

                        if (!nodes.Any())
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }

                        break;
                    case 1://【负债类】

                        nodes = trLiabilities.GetCheckedNodes();

                        if (!nodes.Any())
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 2://【损益类】

                        nodes = trLoss.GetCheckedNodes();

                        if (!nodes.Any())
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 3://【成本类】

                        nodes = trCost.GetCheckedNodes();

                        if (!nodes.Any())
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 4://【所有者权益类】

                        nodes = trEquity.GetCheckedNodes();

                        if (!nodes.Any())
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                }

                var keys = new StringBuilder();
                if (nodes != null)
                { 
                    foreach (var node in nodes)
                    {
                        keys.AppendFormat("{0},", node.NodeID);
                    }

                    string values = keys.ToString().Substring(0, keys.Length - 1);

                    PageContext.RegisterStartupScript(//
                        string.Format(@"F.getActiveWindow().window.reloadTree('{0}');", //
                         values) + ActiveWindow.GetHidePostBackReference());

                }
                
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }
        #endregion

        #region Private Method


        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            var list = Subjects.Where(p => p.FParentCode == "00000" && p.FCompanyId == CurrentUser.AccountComId).ToList();

            foreach (var subject in list)
            {
                var rootNode = new TreeNode
                {
                    Text = subject.FName,
                    NodeID = subject.FCode,
                    EnableClickEvent = true,
                    Expanded = true
                };

                var code = subject.FCode;

                switch (code)
                {
                    case "00001"://【资产类】
                        trAsset.Nodes.Add(rootNode);
                        LoadChildNodes(rootNode);
                        break;
                    case "00002"://【负债类】
                        trLiabilities.Nodes.Add(rootNode);
                        LoadChildNodes(rootNode);
                        break;
                    case "00003"://【损益类】
                        trLoss.Nodes.Add(rootNode);
                        LoadChildNodes(rootNode);
                        break;
                    case "00004"://【成本类】
                        trCost.Nodes.Add(rootNode);
                        LoadChildNodes(rootNode);
                        break;
                    case "00005"://【所有者权益类】
                        trEquity.Nodes.Add(rootNode);
                        LoadChildNodes(rootNode);
                        break;
                }
            }
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(TreeNode node)
        {
            var nodeId = node.NodeID;

            if (Subjects.Count(p => p.FParentCode == nodeId && p.FFlag == 1 && p.FCompanyId == CurrentUser.AccountComId) == 0)
            {
                node.Leaf = true;
            }
            else
            {
                node.Expanded = true;
                node.Nodes.Clear();
                foreach (
                    var subject in
                        Subjects.Where(p => p.FParentCode == nodeId && p.FFlag == 1 && p.FCompanyId == CurrentUser.AccountComId))
                {
                    var cNode = new TreeNode
                    {
                        Text = subject.FName,
                        NodeID = subject.FCode,
                        EnableClickEvent = true,
                        Expanded = true,
                        EnableCheckBox = true
                    };

                    node.Nodes.Add(cNode);

                    LoadChildNodes(cNode);
                }
            }
        }
        #endregion
    }
}