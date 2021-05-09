using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class FKOrderService: EntityRepositoryBase<DbContext, LHFKOrder>
    {
        public FKOrderService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public FKOrderService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}