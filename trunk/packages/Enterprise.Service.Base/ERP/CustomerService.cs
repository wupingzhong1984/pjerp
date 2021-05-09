using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class CustomerService: EntityRepositoryBase<DbContext, LHCustomer>
    {
        public CustomerService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public CustomerService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}