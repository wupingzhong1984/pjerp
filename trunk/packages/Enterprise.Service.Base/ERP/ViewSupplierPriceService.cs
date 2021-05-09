using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     配置供应商采购价
    /// </summary>
    public class ViewSupplierPriceService : EntityRepositoryBase<DbContext, vm_SetSupplierPrice>
    {
        public ViewSupplierPriceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewSupplierPriceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}