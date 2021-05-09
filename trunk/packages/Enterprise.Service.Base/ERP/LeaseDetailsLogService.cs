using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LeaseDetailsLogService: EntityRepositoryBase<DbContext, LHLeaseDetails_Log>
    {
        public LeaseDetailsLogService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LeaseDetailsLogService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}