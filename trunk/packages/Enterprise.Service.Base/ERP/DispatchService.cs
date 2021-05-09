using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;


namespace Enterprise.Service.Base.ERP
{
    public class DispatchService : EntityRepositoryBase<DbContext, LHDispatch>
    {
        public DispatchService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public DispatchService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}