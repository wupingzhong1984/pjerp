using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class AcitonService : EntityRepositoryBase<DbContext, base_aciton>
    {
        public AcitonService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public AcitonService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }

        /// <summary>
        /// 将满足指定的条件的元素对象标记为待逻辑删除；
        /// </summary>
        /// <param name="conditions">条件T-Sql语句</param>
        /// <returns>操作是否成功</returns>
        public bool LogicDelete(string conditions)
        {
            bool isSucceed = false;
            var query = Set.SqlQuery(conditions);
            foreach (var item in query)
            {
                item.deleteflag = 1;
            }

            if (IsOwnContext)
                isSucceed = Context.SaveChanges() > 0;
            else
                isSucceed = true;

            return isSucceed;
        }
    }
}