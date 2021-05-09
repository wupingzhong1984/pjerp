using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
   public class StockoutDispatchServices: EntityRepositoryBase<DbContext, LHStockOutDispatch>
    {
        public StockoutDispatchServices(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockoutDispatchServices()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}