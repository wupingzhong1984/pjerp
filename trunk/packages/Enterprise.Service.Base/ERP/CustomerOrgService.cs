using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;


namespace Enterprise.Service.Base.ERP
{
    public class CustomerOrgService : EntityRepositoryBase<DbContext, LHCustomerOrg>
    {
        public CustomerOrgService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public CustomerOrgService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}