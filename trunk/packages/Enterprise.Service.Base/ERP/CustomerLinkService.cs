using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;


namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     客户联络方式
    /// </summary>
    public class CustomerLinkService: EntityRepositoryBase<DbContext, LHCustomerLink>
    {
        public CustomerLinkService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public CustomerLinkService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}