using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LeaseReturnDetailsService: EntityRepositoryBase<DbContext, LHLeaseReturnDetails>
    {
        public LeaseReturnDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LeaseReturnDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}