using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     车辆
    /// </summary>
    public class VehicleService: EntityRepositoryBase<DbContext, LHVehicle>
    {
        public VehicleService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public VehicleService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}