using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     瓶气对应钢瓶标识
    /// </summary>
    public class ItemsMapBottleService: EntityRepositoryBase<DbContext, LHItemsMapBottle>
    {
        public ItemsMapBottleService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ItemsMapBottleService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}