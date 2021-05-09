using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;


namespace Enterprise.Service.Base.ERP
{
    public class DispatchDetailsService : EntityRepositoryBase<DbContext, LHDispatchDetails>
    {
        public DispatchDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public DispatchDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}