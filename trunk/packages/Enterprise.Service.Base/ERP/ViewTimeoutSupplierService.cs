using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewTimeoutSupplierService: EntityRepositoryBase<DbContext, vm_TimeoutSupplier>
    {
        public ViewTimeoutSupplierService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewTimeoutSupplierService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}