using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LeaseDetailsService: EntityRepositoryBase<DbContext, LHLeaseDetails>
    {
        public LeaseDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LeaseDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}