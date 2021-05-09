using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class CompanyService : EntityRepositoryBase<DbContext, base_company>
    {
        public CompanyService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public CompanyService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}