using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewMonthItemsService: EntityRepositoryBase<DbContext, vm_MonthItems>
    {
        public ViewMonthItemsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewMonthItemsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}