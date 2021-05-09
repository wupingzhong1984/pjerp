using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class DispatchCenterService: EntityRepositoryBase<DbContext, LHDispatchCenter>
    {
        public DispatchCenterService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public DispatchCenterService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}