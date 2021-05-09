using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;
using System.Data.Entity;

namespace Enterprise.Service.Base.ERP
{
    public class CustomerItemPriceService : EntityRepositoryBase<DbContext, V_CustomerItemPrice>
    {
        public CustomerItemPriceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public CustomerItemPriceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");

            IsOwnContext = true;
        }
    }
}
