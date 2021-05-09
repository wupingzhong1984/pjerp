using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     车辆
    /// </summary>
    public class VehicleTireService: EntityRepositoryBase<DbContext, LHVehicleTire>
    {
        public VehicleTireService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public VehicleTireService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}