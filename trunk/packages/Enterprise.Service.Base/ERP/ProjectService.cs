using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class ProjectService: EntityRepositoryBase<DbContext, LHProject>
    {
        public ProjectService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ProjectService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}