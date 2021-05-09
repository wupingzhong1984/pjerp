using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class IconService : EntityRepositoryBase<DbContext, base_icon>
    {
        public IconService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }

        public IconService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            base.IsOwnContext = true;
        }
    }
}
