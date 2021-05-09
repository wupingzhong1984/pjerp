using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     设置产品销售价格
    /// </summary>
    public class CustomerPriceLogService: EntityRepositoryBase<DbContext, LHCustomerPrice_Log>
    {
        public CustomerPriceLogService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public CustomerPriceLogService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}