using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     变更日志
    /// </summary>
    public class StockOutDetailsLogService: EntityRepositoryBase<DbContext, LHStockOutDetails_Log>
    {
        public StockOutDetailsLogService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockOutDetailsLogService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}