using System;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class SequenceService : EntityRepositoryBase<DbContext, base_sequence>
    {
        public SequenceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SequenceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }

        /// <summary>
        /// 创建流水号
        /// </summary>
        /// <param name="fix">业务类型前缀</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string CreateSequence(string fix,int companyId)
        {
            var parameters = new ObjectParameter[3];
            parameters[0] = new ObjectParameter("type", fix);
            parameters[1] = new ObjectParameter("Date", DateTime.Now.ToString("yyyyMMdd"));
            parameters[2] = new ObjectParameter("companyId", companyId);

            return base.ObjContext.ExecuteFunction<String>("proc_Sequence", parameters).First();
        }

        /// <summary>
        /// 创建流水号
        /// </summary>
        /// <param name="fix">业务类型前缀</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string GH(string fix, int companyId)
        {
            var parameters = new ObjectParameter[3];
            parameters[0] = new ObjectParameter("type", fix);
            parameters[1] = new ObjectParameter("Date", "20100101");
            parameters[2] = new ObjectParameter("companyId", 1);

            return base.ObjContext.ExecuteFunction<String>("proc_Sequence", parameters).First();
        }


        /// <summary>
        /// 创建流水号
        /// </summary>
        /// <param name="date">业务类型前缀</param>
        /// <param name="fix">前缀</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string CreateSequence(DateTime date, string fix, int companyId)
        {
            var parameters = new ObjectParameter[3];
            parameters[0] = new ObjectParameter("type", fix);
            parameters[1] = new ObjectParameter("Date", date.ToString("yyyyMMdd"));
            parameters[2] = new ObjectParameter("companyId", companyId);

            return base.ObjContext.ExecuteFunction<String>("proc_Sequence", parameters).First();
        }
    }
}