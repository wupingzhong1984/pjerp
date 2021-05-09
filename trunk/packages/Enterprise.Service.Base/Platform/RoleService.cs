using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class RoleService : EntityRepositoryBase<DbContext, base_role>
    {
        public RoleService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }

        public RoleService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            base.IsOwnContext = true;
        }
    }
}