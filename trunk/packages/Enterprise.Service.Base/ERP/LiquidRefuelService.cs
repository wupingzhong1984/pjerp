using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LiquidRefuelService: EntityRepositoryBase<DbContext, LHLiquidRefuel>
    {
        public LiquidRefuelService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LiquidRefuelService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}