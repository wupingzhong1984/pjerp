using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class OrgnizationService : EntityRepositoryBase<DbContext, base_orgnization>
    {
        public OrgnizationService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }

        public OrgnizationService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            base.IsOwnContext = true;
        }
    }
}