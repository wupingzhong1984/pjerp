using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     单据操作状态
    /// </summary>
    public class BottleDetectionDetailsService : EntityRepositoryBase<DbContext, LHBottleDetectionDetails>
    {
        public BottleDetectionDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public BottleDetectionDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}