using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     初始客户应收账款
    /// </summary>
    public class InitCustomerARService: EntityRepositoryBase<DbContext, LHInitCustomerAR>
    {
        public InitCustomerARService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public InitCustomerARService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}