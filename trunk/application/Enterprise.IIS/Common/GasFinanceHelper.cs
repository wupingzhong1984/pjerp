using System.Linq;
using Enterprise.Service.Base.ERP;

namespace Enterprise.IIS.Common
{
    /// <summary>
    ///     财务Helper
    /// </summary>
    public class GasFinanceHelper
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
        ///     批量更新会计科目金额
        /// </summary>
        /// <param name="code">科目编码</param>
        /// <param name="value">科目值</param>
        public void BatchUpdateSubjectByTyCode(string code, decimal value)
        {
            var subject = SubjectService.Where(p => p.FCode == code).FirstOrDefault();

            if (subject != null)
            {
                subject.FAmount += value;
                SubjectService.SaveChanges();
            }

            if (subject != null) 
                UpdateSubjectByParentCode(subject.FParentCode,value);
        }

        /// <summary>
        ///     递归更新处理
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="value"></param>
        private void UpdateSubjectByParentCode(string parentCode, decimal value)
        {
            var subject = SubjectService.Where(p => p.FCode == parentCode).FirstOrDefault();

            if (subject != null)
            {
                subject.FAmount += value;
                SubjectService.SaveChanges();
            }

            if (subject != null) 
                UpdateSubjectByParentCode(subject.FParentCode, value);
        }



    }
}