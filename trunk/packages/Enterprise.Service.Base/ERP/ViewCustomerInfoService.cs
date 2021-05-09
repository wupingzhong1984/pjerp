using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class ViewCustomerInfoService : EntityRepositoryBase<DbContext, vm_CustomerInfo>
    {
        public ViewCustomerInfoService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewCustomerInfoService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}