using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class SupplierService: EntityRepositoryBase<DbContext, LHSupplier>
    {
        public SupplierService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SupplierService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}