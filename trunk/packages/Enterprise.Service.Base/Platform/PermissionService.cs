using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class PermissionService : EntityRepositoryBase<DbContext, base_permission>
    {
        public PermissionService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }

        public PermissionService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            base.IsOwnContext = true;
        }
    }
}