using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;
namespace Enterprise.Service.Base.Platform
{
    public class ProvinceService : EntityRepositoryBase<DbContext, base_province>
    {
        public ProvinceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ProvinceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}