using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public  class StockInDetailsLogService: EntityRepositoryBase<DbContext, LHStockInDetails_Log>
    {
        public StockInDetailsLogService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockInDetailsLogService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}