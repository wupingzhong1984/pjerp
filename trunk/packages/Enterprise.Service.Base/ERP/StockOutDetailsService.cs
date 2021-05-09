using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class StockOutDetailsService: EntityRepositoryBase<DbContext, LHStockOutDetails>
    {
        public StockOutDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockOutDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}