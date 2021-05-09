using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LiquidPlanTaskDetailsService: EntityRepositoryBase<DbContext, LHLiquidPlanTaskDetails>
    {
        public LiquidPlanTaskDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LiquidPlanTaskDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}