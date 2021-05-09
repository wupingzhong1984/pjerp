using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewItemsPriceService: EntityRepositoryBase<DbContext, vm_ItemsPrice>
    {
        public ViewItemsPriceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewItemsPriceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}