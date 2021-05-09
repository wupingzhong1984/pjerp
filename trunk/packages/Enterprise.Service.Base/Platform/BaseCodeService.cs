using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class BaseCodeService : EntityRepositoryBase<DbContext, base_code>
    {
        public BaseCodeService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public BaseCodeService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}