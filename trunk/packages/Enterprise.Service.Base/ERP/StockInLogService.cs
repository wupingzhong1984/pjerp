using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public  class StockInLogService: EntityRepositoryBase<DbContext, LHStockIn_Log>
    {
        public StockInLogService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockInLogService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}