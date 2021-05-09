using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LiquidPlanTaskService: EntityRepositoryBase<DbContext, LHLiquidPlanTask>
    {
        public LiquidPlanTaskService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LiquidPlanTaskService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}