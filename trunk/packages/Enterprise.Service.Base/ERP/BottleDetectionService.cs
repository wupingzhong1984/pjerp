using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     单据操作状态
    /// </summary>
    public class BottleDetectionService : EntityRepositoryBase<DbContext, LHBottleDetection>
    {
        public BottleDetectionService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public BottleDetectionService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}