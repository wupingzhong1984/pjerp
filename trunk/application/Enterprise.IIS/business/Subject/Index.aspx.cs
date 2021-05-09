using System;
using System.Collections.Generic;
using System.Linq;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Enterprise.Data;
using FineUI;

namespace Enterprise.IIS.business.Subject
{
    public partial class Index : PageBase
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

        private string _month;

        protected string FMonth
        {
            get { return _month ?? DateTime.Now.ToString("yyyy-MM"); }
            set { _month = value; }
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
                    && p.FDate == FMonth).ToList());
            }
            set { _subjects = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

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
        ///     编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int index = TabStrip1.ActiveTabIndex;

                string sid = String.Empty;

                switch (index)
                {
                    case 0://【资产类】

                        sid = trAsset.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }

                        break;
                    case 1://【负债类】

                        sid = trLiabilities.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 2://【损益类】

                        sid = trLoss.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 3://【成本类】

                        sid = trCost.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 4://【所有者权益类】

                        sid = trEquity.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                }

                PageContext.RegisterStartupScript(
                    Window1.GetShowReference(string.Format("./Edit.aspx?FCode={0}&action=1",
                        sid), "添加会计科目"));
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int index = TabStrip1.ActiveTabIndex;

                string sid = String.Empty;

                switch (index)
                {
                    case 0://【资产类】

                        sid = trAsset.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }

                        break;
                    case 1://【负债类】

                        sid = trLiabilities.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 2://【损益类】

                        sid = trLoss.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 3://【成本类】

                        sid = trCost.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 4://【所有者权益类】

                        sid = trEquity.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                }

                PageContext.RegisterStartupScript(
                    Window1.GetShowReference(string.Format("./Edit.aspx?FCode={0}&action=2",
                        sid), "编辑会计科目"));
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     作废
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInvalid_Click(object sender, EventArgs e)
        {
            try
            {
                int index = TabStrip1.ActiveTabIndex;

                string sid = String.Empty;

                switch (index)
                {
                    case 0://【资产类】

                        sid = trAsset.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要作废的科目！", MessageBoxIcon.Information);
                            return;
                        }

                        break;
                    case 1://【负债类】

                        sid = trLiabilities.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要作废的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 2://【损益类】

                        sid = trLoss.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要编辑的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 3://【成本类】

                        sid = trCost.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要作废的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case 4://【所有者权益类】

                        sid = trEquity.SelectedNodeID;

                        if (string.IsNullOrEmpty(sid))
                        {
                            Alert.Show("请选择要作废的科目！", MessageBoxIcon.Information);
                            return;
                        }
                        break;
                }

                SubjectService.Update(
                    p => p.FDate == FMonth //
                        && p.FCompanyId == CurrentUser.AccountComId //
                        && p.FCode == sid //
                        ,p => new LHSubject { FFlag = 0});

                Alert.Show("作废成功！", MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                Alert.Show("作废失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
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
            var list = Subjects.Where(p => p.FParentCode == "00000" //
                && p.FCompanyId == CurrentUser.AccountComId //
                && p.FDate == FMonth).ToList();

            foreach (var subject in list)
            {
                var rootNode = new TreeNode
                {
                    Text = string.Format("{0}-{1}",subject.FUserCode, subject.FName),
                    NodeID = subject.FCode,
                    EnableClickEvent = true,
                    Expanded = true
                };

                if (subject.FFlag != 1)
                {
                    rootNode.CssClass = "important";
                }


                var code = subject.FCode;

                switch (code)
                {
                    case "00001"://【01资产类】
                        trAsset.Nodes.Add(rootNode);
                        LoadChildNodes(rootNode);
                        break;
                    case "00002"://【02负债类】
                        trLiabilities.Nodes.Add(rootNode);
                        LoadChildNodes(rootNode);
                        break;
                    case "00003"://【03损益类】
                        trLoss.Nodes.Add(rootNode);
                        LoadChildNodes(rootNode);
                        break;
                    case "00004"://【04成本类】
                        trCost.Nodes.Add(rootNode);
                        LoadChildNodes(rootNode);
                        break;
                    case "00005"://【05所有者权益类】
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

            if (Subjects.Count(p => p.FParentCode == nodeId //
                //&& p.FFlag == 1 //
                && p.FCompanyId == CurrentUser.AccountComId//
                && p.FDate == FMonth) == 0)
            {
                node.Leaf = true;
            }
            else
            {
                //node.Expanded = true;
                node.Nodes.Clear();
                foreach (
                    var subject in
                        Subjects.Where(p => p.FParentCode == nodeId //
                            //&& p.FFlag == 1 //
                            && p.FCompanyId == CurrentUser.AccountComId//
                            && p.FDate == FMonth))
                {
                    var cNode = new TreeNode
                    {
                        Text = string.Format("{0}-{1}", subject.FUserCode, subject.FName),
                        NodeID = subject.FCode,
                        EnableClickEvent = true,
                        Expanded = false
                    };

                    if (subject.FFlag != 1)
                    {
                        cNode.CssClass = "important";
                    }

                    node.Nodes.Add(cNode);

                    LoadChildNodes(cNode);
                }
            }
        }

        #endregion
    }
}