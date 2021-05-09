using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LeaseService: EntityRepositoryBase<DbContext, LHLease>
    {
        public LeaseService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LeaseService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}