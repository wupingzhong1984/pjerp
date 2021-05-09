using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     客户对应产品销售价
    /// </summary>
    public class ViewCustomerPriceService : EntityRepositoryBase<DbContext, vm_SetCustomerPrice>
    {
        public ViewCustomerPriceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewCustomerPriceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}