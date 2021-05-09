using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     入库操作
    /// </summary>
    public class StockInService: EntityRepositoryBase<DbContext, LHStockIn>
    {
        public StockInService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockInService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}