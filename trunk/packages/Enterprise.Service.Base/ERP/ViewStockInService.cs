using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class ViewStockInService  : EntityRepositoryBase<DbContext, vm_StockIn>
    {
        public ViewStockInService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewStockInService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}