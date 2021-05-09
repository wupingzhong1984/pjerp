using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LiquidPlanService: EntityRepositoryBase<DbContext, LHLiquidPlan>
    {
        public LiquidPlanService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LiquidPlanService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}