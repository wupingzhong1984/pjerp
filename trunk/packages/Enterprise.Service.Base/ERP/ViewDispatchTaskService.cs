using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class ViewDispatchTaskService : EntityRepositoryBase<DbContext, vm_DispatchTask>
    {
        public ViewDispatchTaskService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewDispatchTaskService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}