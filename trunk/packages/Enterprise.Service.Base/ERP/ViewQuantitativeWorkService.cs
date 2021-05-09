using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewQuantitativeWorkService: EntityRepositoryBase<DbContext, vm_QuantitativeWork>
    {
        public ViewQuantitativeWorkService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewQuantitativeWorkService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}