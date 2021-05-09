using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
   public class TubePlanService: EntityRepositoryBase<DbContext, LHTubePlan>
    {
        public TubePlanService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public TubePlanService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}