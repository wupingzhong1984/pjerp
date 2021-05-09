using System;
using System.Collections.Generic;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;

namespace Enterprise.IIS.business.Bank
{
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
            get
            {
                string date = DateTime.Now.ToString("yyyy-MM");
                
                return _subject ?? (_subject = SubjectService.FirstOrDefault(p => p.FCode == FCode
                && p.FCompanyId == CurrentUser.AccountComId && p.FDate == date));
            }

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
                    break;
                case WebAction.Edit:
                    if (Subject != null)
                    {

                        txtFComment.Text = Subject.FComment;
                        txtFName.Text = Subject.FName;
                        txtUserCode.Text = Subject.FUserCode;
                        ddlFAbstract.SelectedValue = Subject.FAbstract;

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
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FName", txtFName.Text.Trim());
            parms.Add("@FComment", txtFComment.Text.Trim());
            parms.Add("@FUserCode", txtUserCode.Text.Trim());
            parms.Add("@FCode", "0000100004");
            parms.Add("@Leavl", "添加到同级目录");
            parms.Add("@companyId", CurrentUser.AccountComId);
            parms.Add("@FAbstract",ddlFAbstract.SelectedValue);

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
                Subject.FAbstract = ddlFAbstract.SelectedValue;

                return SubjectService.SaveChanges() >= 0;
            }
            return false;
        }
    }
}