using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     车辆
    /// </summary>
    public class ViewStockOutService: EntityRepositoryBase<DbContext, vm_StockOut>
    {
        public ViewStockOutService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewStockOutService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}