using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class FKOrderCheckedDetailsService: EntityRepositoryBase<DbContext, LHFKOrderCheckedDetails>
    {
        public FKOrderCheckedDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public FKOrderCheckedDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}