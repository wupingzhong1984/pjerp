using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     车辆
    /// </summary>
    public class VeiwPriceHistoryService: EntityRepositoryBase<DbContext, vm_CustomerPriceHistory>
    {
        public VeiwPriceHistoryService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public VeiwPriceHistoryService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}