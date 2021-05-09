using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class FKOrderBanksService: EntityRepositoryBase<DbContext, LHFKOrderBanks>
    {
        public FKOrderBanksService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public FKOrderBanksService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}