using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class DeviceService: EntityRepositoryBase<DbContext, LHDevice>
    {
        public DeviceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public DeviceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}