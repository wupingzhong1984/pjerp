using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     设置供应商对应产品采购价
    /// </summary>
    public class SupplierPriceService: EntityRepositoryBase<DbContext, LHSupplierPrice>
    {
        public SupplierPriceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SupplierPriceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}