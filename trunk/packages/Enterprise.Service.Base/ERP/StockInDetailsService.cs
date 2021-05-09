using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class StockInDetailsService: EntityRepositoryBase<DbContext, LHStockInDetails>
    {
        public StockInDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockInDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}