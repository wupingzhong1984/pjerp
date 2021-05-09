using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LeaseReturnService: EntityRepositoryBase<DbContext, LHLeaseReturn>
    {
        public LeaseReturnService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LeaseReturnService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}