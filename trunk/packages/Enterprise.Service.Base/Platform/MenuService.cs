using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class MenuService : EntityRepositoryBase<DbContext, base_menu>
    {
        public MenuService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }

        public MenuService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            base.IsOwnContext = true;
        }
    }
}