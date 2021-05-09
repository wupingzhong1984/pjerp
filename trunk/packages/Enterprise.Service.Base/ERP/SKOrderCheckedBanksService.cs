using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     单据操作状态
    /// </summary>
    public class SKOrderCheckedBanksService : EntityRepositoryBase<DbContext, LHSKOrderCheckedBanks>
    {
        public SKOrderCheckedBanksService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SKOrderCheckedBanksService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}