using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class ItemsService: EntityRepositoryBase<DbContext, LHItems>
    {
        public ItemsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ItemsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}