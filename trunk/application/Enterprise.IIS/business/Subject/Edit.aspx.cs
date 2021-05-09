using System;
using System.Collections.Generic;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;

namespace Enterprise.IIS.business.Subject
{

    /// <summary>
    ///     会计科目
    /// </summary>
    public partial class Edit : PageBase
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

        private string _month;

        protected string FMonth
        {
            get { return _month ?? DateTime.Now.ToString("yyyy-MM"); }
            set { _month = value; }
        }

        /// <summary>
        ///     科目内码
        /// </summary>
        protected string FCode
        {
            get { return Request["FCode"]; }
        }

        private LHSubject _subject;

        protected LHSubject Subject
        {
            get { return _subject ?? (_subject = SubjectService.FirstOrDefault(p => p.FCode == FCode
                &&p.FCompanyId==CurrentUser.AccountComId //
                &&p.FDate==FMonth  )); }

            set { _subject = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
                BindData();
            }
        }

        private void BindData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    ddlLeavl.Readonly = false;
                    ddlLeavl.Enabled = true;
                    break;
                case WebAction.Edit:
                    if (Subject != null)
                    {
                        ddlLeavl.Readonly = true;
                        ddlLeavl.Enabled = false;

                        txtFComment.Text = Subject.FComment;
                        txtFName.Text = Subject.FName;
                        txtUserCode.Text = Subject.FUserCode;
                    }
                    break;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSuessed = false;
            try
            {
                switch (Actions)
                {
                    case WebAction.Add:
                        isSuessed = SubmintAdd();
                        break;
                    case WebAction.Edit:
                        isSuessed = SubmintEdit();
                        break;
                }
            }
            catch (Exception)
            {
                isSuessed = false;
            }
            finally
            {
                if (isSuessed)
                {
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            //当前节点Code

            var parms= new Dictionary<string,object>();
            parms.Clear();

            parms.Add("@FName", txtFName.Text.Trim());
            parms.Add("@FComment", txtFComment.Text.Trim());
            parms.Add("@FUserCode", txtUserCode.Text.Trim());
            parms.Add("@FCode", FCode);
            parms.Add("@Leavl",ddlLeavl.SelectedValue);
            parms.Add("@companyId", CurrentUser.AccountComId);

            SqlService.ExecuteProcedureNonQuery("proc_Subject", parms);

            return true;
        }

        private bool SubmintEdit()
        {
            if (Subject != null)
            {
                Subject.FName = txtFName.Text.Trim();
                Subject.FComment = txtFComment.Text.Trim();
                Subject.FUserCode = txtUserCode.Text.Trim();

                return SubjectService.SaveChanges() >= 0;
            }
            return false;
        }
    }
}