using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewMonthAPService : EntityRepositoryBase<DbContext, vm_MonthAP>
    {
        public ViewMonthAPService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewMonthAPService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}