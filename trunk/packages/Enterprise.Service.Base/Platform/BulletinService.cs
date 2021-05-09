using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class BulletinService: EntityRepositoryBase<DbContext, base_bulletin>
    {
        public BulletinService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public BulletinService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}