using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewMonthBottleService: EntityRepositoryBase<DbContext, vm_MonthBottle>
    {
        public ViewMonthBottleService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewMonthBottleService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}