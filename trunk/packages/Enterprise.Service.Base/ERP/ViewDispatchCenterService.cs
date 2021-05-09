using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     调度中心
    /// </summary>
    public class ViewDispatchCenterService: EntityRepositoryBase<DbContext, vm_DispatchCenter>
    {
        public ViewDispatchCenterService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewDispatchCenterService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}