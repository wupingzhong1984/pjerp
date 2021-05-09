using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class BulletinItemService : EntityRepositoryBase<DbContext, base_bulletin_items>
    {
        public BulletinItemService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public BulletinItemService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}