using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class ConfigsetService : EntityRepositoryBase<DbContext, base_configset>
    {
        public ConfigsetService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }

        public ConfigsetService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            base.IsOwnContext = true;
        }
    }
}