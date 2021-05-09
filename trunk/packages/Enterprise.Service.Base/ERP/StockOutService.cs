using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class StockOutService: EntityRepositoryBase<DbContext, LHStockOut>
    {
        public StockOutService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockOutService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}