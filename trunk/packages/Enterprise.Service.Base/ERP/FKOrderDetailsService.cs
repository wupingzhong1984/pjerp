using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class FKOrderDetailsService: EntityRepositoryBase<DbContext, LHFKOrderDetails>
    {
        public FKOrderDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public FKOrderDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}