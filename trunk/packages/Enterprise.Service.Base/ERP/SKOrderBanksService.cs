using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     收款单
    /// </summary>
    public class SKOrderBanksService : EntityRepositoryBase<DbContext, LHSKOrderBanks>
    {
        public SKOrderBanksService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SKOrderBanksService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}