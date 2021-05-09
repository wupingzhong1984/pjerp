using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;


namespace Enterprise.Service.Base.ERP
{
    public class StockOutLogService: EntityRepositoryBase<DbContext, LHStockOut_Log>
    {
        public StockOutLogService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockOutLogService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}