using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class ProjectItemsService: EntityRepositoryBase<DbContext, LHProjectItems>
    {
        public ProjectItemsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ProjectItemsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}