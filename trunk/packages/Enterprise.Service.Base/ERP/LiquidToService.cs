using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LiquidToService: EntityRepositoryBase<DbContext, LHLiquidTo>
    {
        public LiquidToService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LiquidToService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}