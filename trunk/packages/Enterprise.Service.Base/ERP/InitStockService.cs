using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class InitStockService: EntityRepositoryBase<DbContext, LHInitStock>
    {
        public InitStockService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public InitStockService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}