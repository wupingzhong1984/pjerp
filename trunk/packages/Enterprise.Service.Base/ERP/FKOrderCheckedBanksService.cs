using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class FKOrderCheckedBanksService: EntityRepositoryBase<DbContext, LHFKOrderCheckedBanks>
    {
        public FKOrderCheckedBanksService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public FKOrderCheckedBanksService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}