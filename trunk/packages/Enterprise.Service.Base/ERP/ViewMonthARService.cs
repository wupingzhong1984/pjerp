using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewMonthARService : EntityRepositoryBase<DbContext, vm_MonthAR>
    {
        public ViewMonthARService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewMonthARService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}