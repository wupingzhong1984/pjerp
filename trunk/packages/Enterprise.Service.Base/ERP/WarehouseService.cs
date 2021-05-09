using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class WarehouseService: EntityRepositoryBase<DbContext, LHWarehouse>
    {
        public WarehouseService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public WarehouseService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}