using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewTimeoutCustomerService: EntityRepositoryBase<DbContext, vm_TimeoutCustomer>
    {
        public ViewTimeoutCustomerService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewTimeoutCustomerService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}