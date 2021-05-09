using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     收款单
    /// </summary>
    public class SKOrderService : EntityRepositoryBase<DbContext, LHSKOrder>
    {
        public SKOrderService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SKOrderService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}